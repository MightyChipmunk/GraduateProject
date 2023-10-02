using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest_Action : AnimationEvent
{
    protected override void Attack()
    {
        base.Attack();

        enemyStat.HP -= (playerStat.Strength - enemyStat.Defense);
    }

    protected override void Skill()
    {
        base.Skill();

        enemyStat.HP -= (playerStat.Strength - enemyStat.Defense);
    }
}
