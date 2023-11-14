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
        int damage = playerStat.Strength - enemyStat.Defense;
        damage = Mathf.Clamp(damage, 0, 999);
        enemyStat.HP -= damage;
        enemyStat.GetHit(damage);
    }

    protected virtual void Skill()
    {
        BattleManager.Instance.EnemyInfoSet(enemyStat);
    }

    protected virtual void EndMotion()
    {
        BattleUIManager.Instance.ActionEnd();
        //BattleManager.Instance.isAction = false;
        BattleManager.Instance.EndTurn();
        BattleManager.Instance.MoveCam();
        iTween.RotateTo(playerStat.gameObject, iTween.Hash("y", originDir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
    }
}
