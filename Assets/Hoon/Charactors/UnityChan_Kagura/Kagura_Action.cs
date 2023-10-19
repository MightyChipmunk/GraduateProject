using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kagura_Action : AnimationEvent
{
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
    }
}
