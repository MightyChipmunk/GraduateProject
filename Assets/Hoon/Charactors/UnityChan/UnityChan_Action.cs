using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChan_Action : AnimationEvent
{
    public GameObject effect1;
    public GameObject effect2;

    protected override void Attack()
    {
        base.Attack();
    }
    protected override void Skill()
    {
        base.Skill();

        int damage = (int)((float)playerStat.Strength * 2.5f - enemyStat.Defense);
        enemyStat.HP -= damage;
        enemyStat.GetHit(damage);

        effect1.SetActive(false);
        GameObject ef = Instantiate(effect2);
        ef.transform.eulerAngles = new Vector3(0, -90, 0);
        ef.transform.position = enemyStat.transform.position - Vector3.forward * 0.4f; 
    }
    protected override void ActionName(string act)
    {
        base.ActionName(act);

        if (act == "¸¶¹ý!")
        {
            effect1.SetActive(true);
        }
    }
}
