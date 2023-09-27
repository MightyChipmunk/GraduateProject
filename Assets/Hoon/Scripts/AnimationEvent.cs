using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    Stat playerStat;
    public Stat enemyStat;

    private void Start()
    {
        playerStat = GetComponentInParent<Stat>();
    }

    public void Attack()
    {
        enemyStat.HP -= (playerStat.Strength - enemyStat.Defense);
        BattleManager.Instance.EnemyInfoSet(enemyStat);
    }

    public void EndMotion()
    {
        BattleManager.Instance.MoveCam();
    }
}
