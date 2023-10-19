using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_PVP : Interaction
{
    bool isConnected = false;

    protected override void Start()
    {
        base.Start();
        interUI = GameObject.Find("PVP Interaction");
        interUI.SetActive(false);
    }

    protected override void Interact()
    {
        if (isConnected)
            NetworkManager.Instance.PVPReady();
        else
        {
            NetworkManager.Instance.TCPStart();
            isConnected = true;
        }
    }
}
