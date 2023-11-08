using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Team
{
    public int level;
    public int gold;
    public int exp;
    public string userId = "";

    public List<TeamMember> members = new List<TeamMember>();
    public List<TeamMember> Members
    {
        get { return members; }
    }

    public Team(List<TeamMember> list, string id = "")
    {
        userId = id;
        members = list;
    }

    public TeamMember GetIndexMember(int index)
    {
        return members[index];
    }
}


[Serializable]
public class TeamMember
{
    [SerializeField]
    int id = 0;
    public string userId = "";
    public int hp = 0;
    public string name;
    public string modelName;
    public int strength;
    public int defence;
    public int speed;
    public int skillLv;
    public int equipLv;

    public TeamMember(int hp, string name, string modelName)
    {
        this.hp = hp;
        this.name = name;
        this.modelName = modelName;
    }

    //Instantiate for Enemy
    public GameObject Instantiate()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(modelName));
        Stat stat = go.GetComponent<Stat>();
        stat.hpBar = go.transform.Find("E_Canvas").Find("HPBar").GetComponent<Slider>();
        stat.hpBar.maxValue = hp;
        stat.HP = hp;
        stat.Name = name;
        stat.Strength = strength;
        stat.Defense = defence;
        stat.Speed = speed;
        stat.ModelName = modelName;

        return go;
    }

    //Instantiate for Player
    public GameObject Instantiate(GameObject chInfo)
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(modelName));
        Stat stat = go.GetComponent<Stat>();
        stat.hpBar = chInfo.GetComponentInChildren<Slider>();
        stat.hpBar.maxValue = hp;
        stat.HP = hp;
        stat.chName = chInfo.GetComponentInChildren<TMP_Text>();
        stat.Name = name;
        stat.Strength= strength;
        stat.Defense = defence;
        stat.Speed = speed;
        stat.ModelName = modelName;

        go.transform.Find("E_Canvas").gameObject.SetActive(false);

        return go;
    }
}