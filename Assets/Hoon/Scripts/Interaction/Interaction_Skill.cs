using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interaction_Skill : Interaction
{
    public GameObject SkillUpUI;

    protected override void Start()
    {
        base.Start();
        interUI = GameObject.Find("Skill Interaction");
        interUI.SetActive(false);
        SkillUpUI.SetActive(false);
        SkillUpUI.transform.Find("Quit").GetComponent<Button>().onClick.AddListener(() =>
        {
            SkillUpUI.SetActive(false);
            GameObject.Find("PlayerFollow").GetComponent<PlayerFollow>().canMove = true;
            GameObject.Find("Player").GetComponent<PlayerController>().canMove = true;
        });
    }

    protected override void Interact()
    {
        SkillUpUI.SetActive(true);
        GameObject.Find("PlayerFollow").GetComponent<PlayerFollow>().canMove = false;
        GameObject.Find("Player").GetComponent<PlayerController>().canMove = false;
    }
}
