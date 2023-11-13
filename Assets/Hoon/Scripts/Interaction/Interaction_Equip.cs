using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction_Equip : Interaction
{
    public GameObject EquipUI;

    protected override void Start()
    {
        base.Start();
        interUI = GameObject.Find("Equip Interaction");
        interUI.SetActive(false);
        EquipUI.SetActive(false);
        EquipUI.transform.Find("Quit").GetComponent<Button>().onClick.AddListener(() =>
        {
            EquipUI.SetActive(false);
            GameObject.Find("PlayerFollow").GetComponent<PlayerFollow>().canMove = true;
            GameObject.Find("Player").GetComponent<PlayerController>().canMove = true;
        });
    }

    protected override void Interact()
    {
        EquipUI.SetActive(true);
        GameObject.Find("PlayerFollow").GetComponent<PlayerFollow>().canMove = false;
        GameObject.Find("Player").GetComponent<PlayerController>().canMove = false;
    }
}
