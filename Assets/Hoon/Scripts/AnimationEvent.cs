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
        iTween.RotateTo(playerStat.gameObject, iTween.Hash("y", originDir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
    }
}
