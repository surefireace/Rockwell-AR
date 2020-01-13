// by Donovan Colen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// docent interaction class.
/// </summary>
// TODO: not doing anything because docent was still a unanimated and untextured placeholder when this was created
public class CharacterContent : ContentBase
{
    [SerializeField] private GameObject m_model;

    // Start is called before the first frame update. use this for loading model, textures, and animations
    void Start()
    {
        //TODO: load model textures, and animations
    }

    // TODO: set model to a idle animation
    public override void PauseContent()
    {

    }

    // TODO: set model to a starting animation
    public override void ResetContent()
    {

    }

    // TODO: start model animations
    public override void StartContent()
    {

    }


}
