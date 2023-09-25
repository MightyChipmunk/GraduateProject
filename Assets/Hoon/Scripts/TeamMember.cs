using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeamMember
{
    Team team;

    [SerializeField]
    int hp = 0;
    public int HP
    {
        get { return hp; }
    }

    [SerializeField]
    string name;
    public string Name { get { return name; } }

    [SerializeField]
    int index;
    public int GetIndex { get { return index; } }
    public int SetIndex { set { index = value; } }

    [SerializeField]
    string modelName;
    public string ModelName { get { return modelName; } }

    public TeamMember(int hp, string name, string modelName)
    {
        this.hp = hp;
        this.name = name;
        this.modelName = modelName;
    }

    public GameObject Instantiate()
    {
        GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(modelName));
        Stat stat = go.AddComponent<Stat>();
        stat.HP = hp;
        stat.Name = name;
        stat.SetIndex = index;

        return go;
    }
}