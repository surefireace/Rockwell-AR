// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
///  this class manages the lobby and the groups. based off the Launcher class in Photon's tutorial
/// </summary>
public class RockwellLobby : MonoBehaviourPunCallbacks
{
    // the name of the unity scene that the experiance will take place in
    [Tooltip("the name of the unity scene that the experiance will take place in")]
    [SerializeField]
    private string m_level = "Networktest";

    // the name of the lobby scene.
    [Tooltip("the name of the lobby scene")]
    [SerializeField]
    private string m_lobby = "UI_TS";

    // the name of the scene for clients as they wait to be assigned a group
    [Tooltip("the name of the scene for clients as they wait to be assigned a group")]
    [SerializeField]
    private string m_waitLevel = "SampleScene";

    private RoomOptions m_roomOpt = new RoomOptions { MaxPlayers = 0, PlayerTtl = 300000 }; // infinite players and a 5 min time out
    private TypedLobby m_lobbyOpt = new TypedLobby("Rockwell lobby", LobbyType.Default);    // the name of the lobby in Photon
    private string m_roomName = "lobby";    // the name of the room in Photon
    private List<int> m_groupLeaders = new List<int>(); // contains actor number of the players that lead the groups

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    private bool m_isConnecting;

    /// <summary>
    /// This client's version number. Users are separated from each other by m_gameVersion (which allows you to make breaking changes).
    /// </summary>
    private string m_gameVersion = "1";

    // the singleton instance for the class
    private static RockwellLobby m_instance;

    public static RockwellLobby Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<RockwellLobby>();
            }
            return m_instance;
        }
    }

    // called before Start
    void Awake()
    {
        if (Instance == null)
        {
            m_instance = this;
        }

        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        //for testing disconnects
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    PhotonNetwork.Disconnect();
        //}
    }

    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        m_isConnecting = true;

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Joining Room...");
            // #Critical we need at this point to attempt join the m_lobbyOpt. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinOrCreateRoom(m_roomName, m_roomOpt, m_lobbyOpt);
        }
        else
        {

            Debug.Log("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = this.m_gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// Called after the connection to the master is established and authenticated
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room. 
        // this case where m_isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (m_isConnecting)
        {
            Debug.Log("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a Lobby.");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinOrCreateRoom(m_roomName, m_roomOpt, m_lobbyOpt);
        }
    }

    /// <summary>
    /// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
    /// </summary>
    /// <remarks>
    /// Most likely all rooms are full or no rooms are available. <br/>
    /// </remarks>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(m_roomName, m_roomOpt, m_lobbyOpt);
    }


    /// <summary>
    /// Called after disconnecting from the Photon server.
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("<Color=Red>OnDisconnected</Color> " + cause);
        Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");
        
        if(cause != DisconnectCause.DisconnectByClientLogic || cause != DisconnectCause.DisconnectByServerLogic)
        {
            if(!PhotonNetwork.ReconnectAndRejoin())
            {
                Debug.LogError("Failed to reconnect and rejoin");
            }
        }

        m_isConnecting = false;

    }

    /// <summary>
    /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
    /// </summary>
    /// <remarks>
    /// This method is commonly used to instantiate player characters.
    /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
    ///
    /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
    /// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
    /// enough players are in the room to start playing.
    /// </remarks>
    public override void OnJoinedRoom()
    {
        Debug.Log("<Color=Green>OnJoinedLobby</Color> with " + PhotonNetwork.CountOfPlayers + " Player(s)");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        if(PhotonNetwork.CurrentRoom.Name != m_roomName)
        {
            PhotonNetwork.LoadLevel(m_lobby);
        }
        ClientManager.Instance.InitDeviceInfo();

        //Debug.Log("<Color=Cyan>" + Application.platform + "</Color>");
        if (Application.platform == RuntimePlatform.WSAPlayerX86 || Application.platform == RuntimePlatform.WSAPlayerARM)   // if platform is hololens
        {
            this.Invoke("LoadWaitLevel", 2);    // delay the loading so the Hololens 2 has time to load
        }

        //// #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        //if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        //{
        //    Debug.Log("We load " + m_level);

        //    // #Critical
        //    // Load the Room Level. 
        //    PhotonNetwork.LoadLevel(m_level);

        //}

    }

    /// <summary>
    /// to delay the loading of the scene so the headset can finish loading
    /// </summary>
    private void LoadWaitLevel()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "batt", ClientManager.Instance.Charge } });
        //Debug.Log("We load " + m_waitLevel);
        PhotonNetwork.LoadLevel(m_waitLevel);
    }

    /// <summary>
    /// Called when the server couldn't create a room (OpCreateRoom failed).
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError(returnCode + ": " + message);
    }

    /// <summary>
    /// Called after switching to a new MasterClient when the current one leaves.
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //[TODO] not working as intended
        //Debug.LogError("MasterClient disconnected");
        //if(PhotonNetwork.IsMasterClient)
        //{
        //    Debug.Log("was master trying to reconnect");
        //    PhotonNetwork.ReconnectAndRejoin();
        //}
        //else
        //{
        //    Debug.Log("loading starting scene");
        //    PhotonNetwork.LoadLevel(0);
        //}

    }

    /// <summary>
    /// Called when a remote player entered the room. This Player is already added to the playerlist.
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(AddButton(newPlayer));
        }
    }

    /// <summary>
    /// adds a button to the lobby menu, for the Hololens that just joined
    /// </summary>
    /// <param name="newPlayer"> the player's info</param>
    /// <returns></returns>
    private IEnumerator AddButton(Player newPlayer)
    {
        yield return new WaitForSeconds(2); // delay the creation so the client can update their nick name
        Debug.Log(newPlayer.NickName + " joined the room");
        DynamicButtons.Instance.CreateButton(newPlayer.NickName);
    }

    /// <summary>
    /// Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.
    /// </summary>
    public override void OnPlayerLeftRoom(Player player)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(player.NickName + " left the room");
            DynamicButtons.Instance.DestroyButton(player.NickName);
        }
    }

    /// <summary>
    /// creates a new "room" for the new group and tells the group leader they are the master
    /// </summary>
    /// <param name="groupName"> the name of the group</param>
    /// <param name="headsetName"> the name of the headset</param>
    public void CreateNewRoom(string groupName, string headsetName)
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player p in players)
        {
            Debug.Log("create " + p.NickName + " ::::: " + headsetName);
            if (p.NickName == headsetName)
            {
                p.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "group", groupName } });
                m_groupLeaders.Add(p.ActorNumber);
                photonView.RPC("CreateGroupsRoom", p, groupName);
            }
        }
        
    }

    /// <summary>
    /// adds the player to the existing group and tells the player what group they joined
    /// </summary>
    /// <param name="groupName"> the name of the group they are joining</param>
    /// <param name="headsetName"> the name of the headset</param>
    public void JoinGroupRoom(string groupName, string headsetName)
    {
        Player[] players = PhotonNetwork.PlayerList;
        Player groupLeader = null;

        foreach (int j in m_groupLeaders)
        {
            Player leader = PhotonNetwork.CurrentRoom.GetPlayer(j);
            if ((string)leader.CustomProperties["group"] == groupName)
            {
                groupLeader = leader;
            }
        }


        foreach (Player p in players)
        {
            Debug.Log("join " + p.NickName + " ::::: " + headsetName);

            if (p.NickName == headsetName && groupLeader != null)
            {
                p.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "group", groupName } });
                photonView.RPC("AddMember", groupLeader, p.ActorNumber);
                photonView.RPC("JoinGroup", p, groupName, groupLeader.ActorNumber);
            }
        }

    }

    /// <summary>
    /// removes the client from the group and tells that player that they were removed
    /// </summary>
    /// <param name="u"> the user that is being removed for the group</param>
    public void RemoveUserFromGroup(Proto.Sbee.User u)
    {
        foreach(var p in PhotonNetwork.CurrentRoom.Players)
        {
            if (u.headsetDevice.DeviceName == p.Value.NickName)
            {
                photonView.RPC("LeaveGroup", p.Value);
            }
        }
    }

    /// <summary>
    /// changes the leader of the group
    /// </summary>
    /// <param name="newActorNum"> the player's Actor number that is the new leader</param>
    /// <param name="oldActorNum"> the player's Actor number that is being replaced as the leader</param>
    public void ChangeLeader(int newActorNum, int oldActorNum)
    {
        if (m_groupLeaders.Contains(oldActorNum))
        {
            m_groupLeaders.Remove(oldActorNum);
        }

        m_groupLeaders.Add(newActorNum);
        Player p = PhotonNetwork.CurrentRoom.GetPlayer(newActorNum);

        Debug.Log("leader change in group " + (string)p.CustomProperties["group"] + " to " + p.NickName);

        GameObject o = GameObject.Find((string)p.CustomProperties["group"]);
        if(o != null)
        {
            Proto.Sbee.Group g = o.GetComponent<Proto.Sbee.Group>();

            if(g != null)
            {
                Proto.Sbee.User u = g.GetUser(p.NickName);

                if (u != null)
                {
                    u.userStatus = Proto.Sbee.User.UserStatus.Leader;
                }
            }
        }
    }

    /// <summary>
    /// to init the leader of the new group and have them load the new scene
    /// </summary>
    /// <param name="groupName"> the name of the group</param>
    [PunRPC]    
    private void CreateGroupsRoom(string groupName)
    {
        Debug.Log("createing new room named " + groupName);
        ClientManager c = ClientManager.Instance;
        c.IsLeader = true;
        c.Group = groupName;
        c.Leader = PhotonNetwork.LocalPlayer.ActorNumber;
        c.AddGroupMember(PhotonNetwork.LocalPlayer.ActorNumber);
        PhotonNetwork.LoadLevel(m_level);
        c.Invoke("EnableButtons", 2); // enable the buttons for the group leader but wait till level is loaded
    }

    /// <summary>
    /// to init the member and to load the scene
    /// </summary>
    /// <param name="groupName"> the name of the group they are joining</param>
    /// <param name="leaderActorNum"> the Actor number of the goups leader</param>
    [PunRPC]
    private void JoinGroup(string groupName, int leaderActorNum)
    {
        Debug.Log("joining room named " + groupName);
        ClientManager c = ClientManager.Instance;
        c.IsLeader = false;
        c.Group = groupName;
        c.Leader = leaderActorNum;
        PhotonNetwork.LoadLevel(m_level);

    }

    /// <summary>
    /// to de-init the player as they are no longer in a group
    /// </summary>
    [PunRPC]
    private void LeaveGroup()
    {
        ClientManager c = ClientManager.Instance;
        Debug.Log("leaving group named " + c.Group);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "group", "" } });
        c.Reset();
        PhotonNetwork.LoadLevel(m_waitLevel);
    }

    /// <summary>
    /// to tell the group leader a new member joined
    /// </summary>
    /// <param name="actorNum"> the Actor number of the new group member</param>
    [PunRPC]    
    private void AddMember(int actorNum)
    {
        Debug.Log("member " + PhotonNetwork.CurrentRoom.GetPlayer(actorNum).NickName + " added to group");
        ClientManager.Instance.AddGroupMember(actorNum);
    }



}
