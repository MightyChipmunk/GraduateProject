using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kohaku_Action : AnimationEvent
{
    protected override void Attack()
    {
        base.Attack();
    }
    protected override void Skill()
    {
        base.Skill();

        int damage = (int)((float)playerStat.Strength * (2 + playerStat.skillLv * 0.5f) - enemyStat.Defense);
        damage = Mathf.Clamp(damage, 0, 999);
        enemyStat.HP -= damage;
        enemyStat.GetHit(damage);
    }
}
