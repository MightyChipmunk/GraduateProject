using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    protected Stat playerStat;
    public Stat enemyStat;

    protected virtual void Start()
    {
        playerStat = GetComponentInParent<Stat>();
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Skill()
    {

    }

    protected virtual void EndMotion()
    {
        BattleManager.Instance.EndTurn();
        BattleManager.Instance.MoveCam();
        BattleManager.Instance.isAction = false;
    }
}
