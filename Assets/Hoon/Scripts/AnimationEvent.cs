using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    protected Stat playerStat;
    protected Vector3 originDir;
    public Stat enemyStat;

    protected virtual void Start()
    {
        playerStat = GetComponentInParent<Stat>();
        originDir = transform.eulerAngles;
    }

    protected virtual void ActionName(string act)
    {
        BattleUIManager.Instance.ActionName(act);
    }

    protected virtual void Attack()
    {
        BattleManager.Instance.EnemyInfoSet(enemyStat);
        enemyStat.HP -= (playerStat.Strength - enemyStat.Defense);
        enemyStat.GetHit(playerStat.Strength - enemyStat.Defense);
    }

    protected virtual void Skill()
    {
        BattleManager.Instance.EnemyInfoSet(enemyStat);
    }

    protected virtual void EndMotion()
    {
        BattleUIManager.Instance.ActionEnd();
        BattleManager.Instance.EndTurn();
        BattleManager.Instance.MoveCam();
        BattleManager.Instance.isAction = false;
        iTween.RotateTo(playerStat.gameObject, iTween.Hash("y", originDir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
    }
}
