using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    public Slider hpBar;
    public TMP_Text chName;
    Animator anim;
    AnimationEvent animEvent;

    [SerializeField]
    int hp = 0;
    public int HP
    {
        get { return hp; }
        set 
        { 
            hp = value;
            if (hpBar != null)
            {
                hpBar.value = hp;
            }
        }
    }

    [SerializeField]
    string _name;
    public string Name 
    { 
        get { return _name; } 
        set 
        { 
            _name = value;
            if (chName != null)
                chName.text = _name;
        }
    }

    [SerializeField]
    int index;
    public int GetIndex { get { return index; } }
    public int SetIndex { set { index = value; } }

    [SerializeField]
    int strength = 10;
    public int Strength { get { return strength; } set { strength = value; } }

    [SerializeField]
    int defense = 3;
    public int Defense { get { return defense; } set { defense = value; } }

    public int[] Near()
    {
        int[] near = new int[2];
        near[0] = index - 1;
        near[1] = index + 1;

        return near;
    }

    public void Attack(Stat enemyStat)
    {
        anim.SetTrigger("Attack");
        animEvent.enemyStat = enemyStat;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        animEvent = GetComponentInChildren<AnimationEvent>();
    }
}
