using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStatus : MonoBehaviour
{
    public GameObject status;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            status.SetActive(!status.activeSelf);
        }
    }
}
