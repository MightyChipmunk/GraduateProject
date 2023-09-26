using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    public Slider hpBar;

    [SerializeField]
    int hp = 0;
    public int HP
    {
        get { return hp; }
        set 
        { 
            hp = value;
            if (hpBar != null)
                hpBar.value = hp;
        }
    }

    [SerializeField]
    string _name;
    public string Name 
    { 
        get { return _name; } 
        set { _name = value; }
    }

    [SerializeField]
    int index;
    public int GetIndex { get { return index; } }
    public int SetIndex { set { index = value; } }

    public int[] Near()
    {
        int[] near = new int[2];
        near[0] = index - 1;
        near[1] = index + 1;

        return near;
    }
}
