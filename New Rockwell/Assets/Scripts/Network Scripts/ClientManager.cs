// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

#if !UNITY_EDITOR
    // used for getting remaining battery life. cant be used with the Unity editor
    using Windows.Devices.Power;
    using Windows.Devices.Enumeration;
#endif


/// <summary>
/// this is the local class the holds the headset info and manages the group
/// </summary>
public class ClientManager : MonoBehaviourPunCallbacks
{
    public bool IsLeader { get; set; } = false;     // to tell if is the leader of the group
    public float Charge { get; set; } = 0;     // the charge of the battery
    public string DeviceName { get; private set; } = "";    // the name of the headset the client is useing

    private float m_lastCharge = -1;    // the last charge the battery was at. only for Hololens

    // comment the timer because might be used in future
    //[SerializeField]
    //private float m_waitToReadyTime;    //how much time to wait before auto starting content in seconds

    //private float m_waitToReadyTimer = 0; // the timer to track how much time has passed

    //private bool m_startTimer = false;    // to tell if the timer has started

    private ContentManager m_contentManager;    // referance to the current content manager that controlls the interations

    private Dictionary<int, bool> m_othersReady = new Dictionary<int, bool>();  // player's actor number and bool to see if ready only contains those in the same group

    public string Group { get; set; } = "";     // the group the headset is assigned
    public int Leader { get; set; } = -1;   // the leader of the group actor number


    private static ClientManager m_instance;    // singleton

    public static ClientManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ClientManager>();
            }
            return m_instance;
        }
    }

    /// <summary>
    // sets the client back to its lobby state
    /// </summary>
    public void Reset()
    {
        if(IsLeader)
        {
            IsLeader = false;
            foreach(var t in m_othersReady)
            {
                if(PhotonNetwork.CurrentRoom.GetPlayer(t.Key).NickName != PhotonNetwork.NickName)
                {
                    Debug.Log("<Color=Cyan>assigning replacement leader</Color>");
                    photonView.RPC("AssignNewLeader", RpcTarget.MasterClient, t.Key, PhotonNetwork.LocalPlayer.ActorNumber);    // tell the lobby a new leader is being assigned
                    photonView.RPC("MakeLeader", PhotonNetwork.CurrentRoom.GetPlayer(t.Key));   // tell the group member it is the new leader

                    break;
                }
            }
            m_othersReady.Clear();
        }
        else
        {
            photonView.RPC("RemoveGroupMember", PhotonNetwork.CurrentRoom.GetPlayer(Leader), PhotonNetwork.LocalPlayer.ActorNumber); // remove the client from the leaders list
        }

        Group = "";
        Leader = -1;
        m_contentManager = null;

    }

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            m_instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        UpdateBattery();

        //if (m_startTimer)    // startTimer only ever set true on leader of the group
        {
            //m_waitToReadyTimer += Time.deltaTime;
            if (/*m_waitToReadyTimer >= m_waitToReadyTime ||*/ CheckIfAllReady())
            {
                if (IsLeader)
                {
                    foreach (var t in m_othersReady)
                    {
                        photonView.RPC("StartContent", PhotonNetwork.CurrentRoom.GetPlayer(t.Key), m_contentManager.ImgName);
                    }
                }
            }
        }
    }

    /// <summary>
    /// checks if everyone in the group is ready
    /// </summary>
    /// <returns> false when 1 group member is not ready</returns>
    private bool CheckIfAllReady()
    {
        foreach (KeyValuePair<int, bool> keyVal in m_othersReady)
        {
            //Debug.Log("actor number " + keyVal.Key + " is ready " + keyVal.Value);
            if (keyVal.Value == false)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// updates the battery charge
    /// </summary>
    private void UpdateBattery()
    {
#if !UNITY_EDITOR   // the following code is only viable outside of Unity. also will throw exception in emulators 
        if(Battery.AggregateBattery == null)
        {
            return;
        }
        var batt = Battery.AggregateBattery;
        if(batt != null)
        {
            var report = batt.GetReport();
            if (report != null)
            {
                Charge = ((float)report.RemainingCapacityInMilliwattHours / (float)report.FullChargeCapacityInMilliwattHours) * 100;
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
#endif
        Charge = (float)decimal.Round((decimal)Charge, 2);
        if(Charge != m_lastCharge)
        {
            m_lastCharge = Charge;
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "batt", ClientManager.Instance.Charge } });
            Debug.LogWarning("battery life remaining: " + Charge + "%");
        }
    }

    /// <summary>
    /// activates the buttons for controlling the interactions if the client is a group leader
    /// </summary>
    public void EnableButtons()
    {
        Debug.Log("enable buttons");
        GameObject ba = GameObject.FindGameObjectWithTag("Book Anchor");
        if(ba == null)
        {
            return;
        }

        GameObject bm = ba.transform.Find("ButttonGrid")?.gameObject;

        if (bm != null && IsLeader)
        {
            bm.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// initializes the client's player name and batttery life.
    /// </summary>
    public void InitDeviceInfo()
    {
        PhotonNetwork.NickName = DeviceName = System.Environment.MachineName + PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        Debug.Log("Name: " + PhotonNetwork.NickName);
        UpdateBattery();
    }

    /// <summary>
    /// readys the clients to start content, starts content if alone, or starts the timer if the master
    /// </summary>
    /// <param name="contMan"> the contentManager that is being activated</param>
    public void TriggerContent(ContentManager contMan)
    {
        if(contMan == null)
        {
            return;
        }

        if (m_contentManager == null)
        {
            m_contentManager = contMan;
        }
        else if(IsLeader == false && contMan != m_contentManager) // if already has a content manager and its not the same and is not the group leader
        {
            // stop showing and reset the new interaction. to stop the client from looking at other interactions while anouther one is showing
            RockwellVuforiaTrigger tvt = contMan.gameObject.GetComponentInParent<RockwellVuforiaTrigger>();
            if (tvt != null)
            {
                Debug.Log("already has interaction resetting img " + contMan.ImgName);
                tvt.StopInteraction();
            }
        }

        if (IsLeader)
        {
            if (m_contentManager != contMan)
            {
                StopContent();  // stop previous content
                m_contentManager = contMan; // assign the new content manager
            }

            if (PhotonNetwork.CurrentRoom == null)
            {
                Debug.LogError("not in room and vuforia was triggered");
                return;
            }
            if (m_othersReady.Count == 1)
            {
                m_contentManager.Invoke("Begin", 0.5f);    // just start the content stuff because we are alone no need to network. delayed to solve some timing issues
                ButtonClient.Instance.SwapPlayPause();

            }
            else if (m_othersReady.Count > 1)
            {

                ReadyContent(PhotonNetwork.LocalPlayer.ActorNumber);    // ready the leader

                if (CheckIfAllReady())  // if all are ready auto start it
                {
                    m_contentManager.Invoke("Begin", 0.5f); // play the content for self
                    foreach (var t in m_othersReady)
                    {
                        if (t.Key != PhotonNetwork.LocalPlayer.ActorNumber) //ignore self and no need to network a call we can do locally
                        {
                            photonView.RPC("StartContent", PhotonNetwork.CurrentRoom.GetPlayer(t.Key), m_contentManager.ImgName);
                        }
                    }
                }
                else
                {
                    //m_startTimer = true;
                }
            }
            else
            {
                // shouldnt get here. it would mean 0 or m_othersReady < 1 is in the group.
                Debug.LogAssertion(PhotonNetwork.CurrentRoom.PlayerCount);
            }
        }
        else
        {
            // tell the leader that this client is ready
            photonView.RPC("ReadyContent", PhotonNetwork.CurrentRoom.GetPlayer(Leader), PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    /// <summary>
    /// for stopping the content localy
    /// </summary>
    private void StopContent()
    {
        if(m_contentManager == null)
        {
            Debug.LogError("tried to stop null content");
            return;
        }

        // find and stop the previous interactions
        RockwellVuforiaTrigger tvt = m_contentManager.gameObject.GetComponentInParent<RockwellVuforiaTrigger>();
        if (tvt != null)
        {
            Debug.Log("Stopping last interactions on img " + m_contentManager.ImgName);
            tvt.StopInteraction();
        }
        else
        {
            Debug.LogError("couldn't find RockwellVuforiaTrigger");
            return; // dont contiue because content manager game object could be incorrectly set up
        }

        if (IsLeader)
        {
            ButtonClient.Instance.SwapPlayPause();
        }

        m_contentManager = null;    // clear the content manager

        // unready the group
        if (IsLeader == true)
        {
            var keys = new List<int>(m_othersReady.Keys);
            foreach (int key in keys)
            {
                m_othersReady[key] = false;
            }
        }
        Debug.Log("<Color=Cyan>finished stopping</Color>");

    }

    /// <summary>
    /// for the play/pause buttons for the group leader and tells the other group members to play/pause as well
    /// </summary>
    public void PlayPauseInteractions()
    {
        if (m_contentManager == null)
        {
            Debug.LogWarning("tried to resume/pause a null content");
            return;
        }

        if (IsLeader)
        {
            ButtonClient.Instance.SwapPlayPause();
        }

        if (m_contentManager.IsPaused || !m_contentManager.IsStarted)
        {
            m_contentManager.Invoke("Begin", 0.5f); // play the content for self
            foreach (var t in m_othersReady)
            {
                if (t.Key != PhotonNetwork.LocalPlayer.ActorNumber) //ignore self and no need to network a call we can do locally
                {
                    photonView.RPC("StartContent", PhotonNetwork.CurrentRoom.GetPlayer(t.Key), m_contentManager.ImgName);
                }
            }
        }
        else
        {
            m_contentManager.Invoke("Pause", 0.5f); // pause the content for self

            foreach (var t in m_othersReady)
            {
                if (t.Key != PhotonNetwork.LocalPlayer.ActorNumber) //ignore self and no need to network a call we can do locally
                {
                    photonView.RPC("PauseContent", PhotonNetwork.CurrentRoom.GetPlayer(t.Key));
                }
            }
        }
    }

    /// <summary>
    /// for the close buttons for the group leader and tells the other group members to stop interactions as well
    /// </summary>
    public void StopInteractions()
    {
        StopContent(); // stop content for self
        foreach (var t in m_othersReady)
        {
            if (t.Key != PhotonNetwork.LocalPlayer.ActorNumber) //ignore self and no need to network a call we can do locally
            {
                photonView.RPC("StopGroupInteractions", PhotonNetwork.CurrentRoom.GetPlayer(t.Key));
            }
        }
    }

    /// <summary>
    /// adds a member to the group
    /// </summary>
    /// <param name="actorNum"></param>
    public void AddGroupMember(int actorNum)
    {
        if (m_othersReady.ContainsKey(actorNum))    // could already be in dictionary and is joining back after disconnect
        {
            m_othersReady[actorNum] = false;
        }
        else
        {
            m_othersReady.Add(actorNum, false);
        }
    }

    /// <summary>
    /// to tell the group leader to remove a member
    /// </summary>
    /// <param name="actorNum"> the Actor number that is being removed from the group</param>
    [PunRPC]    
    private void RemoveGroupMember(int actorNum)
    {
        if (m_othersReady.ContainsKey(actorNum))
        {
            Debug.Log("removeing group member with Actor number: " + actorNum);
            m_othersReady.Remove(actorNum);

        }
    }

    /// <summary>
    /// begins the content stuff. if client hasnt looked at a img target try to find the same one
    /// </summary>
    /// <param name="imgName"> the name of the ImageTarget used by Vuforia</param>
    [PunRPC]
    private void StartContent(string imgName = "")
    {
        if(m_contentManager != null && m_contentManager.IsPaused)
        {
            m_contentManager.Invoke("Begin", 0.5f); // resume the content
            return;
        }

        //m_startTimer = false;
        //m_waitToReadyTimer = 0;

        if (m_contentManager != null && m_contentManager.ImgName == imgName)    // start if the client is looking at the same img
        {
            m_contentManager.Invoke("Begin", 0.5f); // start the content
        }
        else
        {
            if (imgName == "")
            {
                return; // no point in checking name if its default value;
            }

            if(m_contentManager != null)    //stop the content if contentmanager is of a different name
            {
                StopContent();
            }

            ContentManager[] cm = FindObjectsOfType<ContentManager>();
            foreach (ContentManager c in cm)
            {
                if (c.ImgName == imgName) // if the content manager has same name as trackable name
                {
                    m_contentManager = c;
                    c.Invoke("Begin", 0.5f);  // start it so the client that hasnt looked at the same target or any target, is synced
                    return;
                }
            }
        }
    }

    /// <summary>
    /// pause the content stuff locally.
    /// </summary>
    [PunRPC] 
    private void PauseContent()
    {
        if (m_contentManager != null)    //pause the content if the content manager isnt null
        {
            m_contentManager.Invoke("Pause", 0.5f); // pause the content
        }
        else
        {
            Debug.LogWarning("tried to pause a non existing content interaction");
        }
    }

    /// <summary>
    /// for stopping the content interactions for group members
    /// </summary>
    [PunRPC]    
    private void StopGroupInteractions()
    {
        Debug.Log("<Color=Cyan>stopping</Color>");

        StopContent();
    }

    /// <summary>
    /// update the array of who is ready on the leader
    /// </summary>
    /// <param name="actorNum"> the Actor number of the member that is now ready</param>
    [PunRPC]    
    private void ReadyContent(int actorNum)
    {
        if (m_othersReady.ContainsKey(actorNum))
        {
            m_othersReady[actorNum] = true;
        }
        else
        {
            m_othersReady.Add(actorNum, true);
        }
    }

    /// <summary>
    /// to tell the lobby master the leader changed
    /// </summary>
    /// <param name="newActorNum"> the Actor number of the new leader</param>
    /// <param name="oldActorNum"> the Actor number of the old leader</param>
    [PunRPC]    
    private void AssignNewLeader(int newActorNum, int oldActorNum)
    {
        RockwellLobby.Instance.ChangeLeader(newActorNum, oldActorNum);
    }

    /// <summary>
    /// to tell the new leader it is the leader and to update the other members
    /// </summary>
    [PunRPC]    
    private void MakeLeader()
    {
        Debug.Log("<Color=Cyan>assigned replacement leader</Color>");
        Leader = PhotonNetwork.LocalPlayer.ActorNumber;
        IsLeader = true;
        EnableButtons();
        if(m_othersReady.Count != 0)
        {
            m_othersReady.Clear();
        }

        foreach(var p in PhotonNetwork.CurrentRoom.Players)
        {
            if((string)p.Value.CustomProperties["group"] == Group)
            {
                Debug.Log("adding " + p.Value.NickName + " to group");
                m_othersReady.Add(p.Value.ActorNumber, false);
                photonView.RPC("UpdateLeader", p.Value, PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }

    }

    /// <summary>
    /// to update the group members of the leader change
    /// </summary>
    /// <param name="actorNumber"> the Actor number of the new leader</param>
    [PunRPC]    
    private void UpdateLeader(int actorNumber)
    {
        Leader = actorNumber;
    }

}
