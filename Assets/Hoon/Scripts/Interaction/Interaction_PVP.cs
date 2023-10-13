using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_PVP : Interaction
{
    protected override void Start()
    {
        base.Start();
        interUI = GameObject.Find("PVP Interaction");
        interUI.SetActive(false);
    }

    protected override void Interact()
    {
        NetworkManager.Instance.PVPReady();
        //SceneManager.LoadScene("BattleScene");
    }
}
