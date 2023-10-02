using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Team
{
    List<TeamMember> members = new List<TeamMember>();
    public List<TeamMember> Members
    {
        get { return members; }
        set
        {
            members = value;
            int idx = 0;
            foreach (TeamMember member in members)
            {
                member.SetIndex = idx++;
            }
        }
    }

    public Team(List<TeamMember> list)
    {
        members = list;
        int idx = 0;
        foreach (TeamMember member in members)
        {
            member.SetIndex = idx++;
        }
    }

    public TeamMember GetIndexMember(int index)
    {
        return members[index];
    }
}


[Serializable]
public class TeamMember
{
    Team team;

    [SerializeField]
    int hp = 0;

    [SerializeField]
    string name;

    [SerializeField]
    int index;
    public int GetIndex { get { return index; } }
    public int SetIndex { set { index = value; } }

    [SerializeField]
    string modelName;

    [SerializeField]
    int strength;

    [SerializeField]
    int defence;

    [SerializeField]
    int speed;

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
        if (stat.hpBar != null)
            stat.hpBar.maxValue = hp;
        stat.HP = hp;
        stat.Name = name;
        stat.SetIndex = index;
        stat.Strength = strength;
        stat.Defense = defence;
        stat.Speed = speed;

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
        stat.SetIndex = index;
        stat.Strength= strength;
        stat.Defense = defence;
        stat.Speed = speed;

        return go;
    }
}