using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kagura_Action : AnimationEvent
{
    public GameObject arrow;

    protected override void Attack()
    {
        base.Attack();

        Vector3 pos = enemyStat.transform.position;
        GameObject go = Instantiate(arrow);
        go.transform.position = transform.position + Vector3.up;
        StartCoroutine(DestroyCo(go));
        iTween.MoveTo(go, iTween.Hash("x", pos.x, "y", pos.y + 1, "z", pos.z, "time", 0.2f, "easetype", iTween.EaseType.linear));
    }
    protected override void Skill()
    {
        base.Skill();

        int damage = (int)((float)playerStat.Strength * 2.5f - enemyStat.Defense);
        enemyStat.HP -= damage;
        enemyStat.GetHit(damage);

        Vector3 pos = enemyStat.transform.position;
        GameObject go = Instantiate(arrow);
        go.transform.position = transform.position + Vector3.up; ;
        StartCoroutine(DestroyCo(go));
        iTween.MoveTo(go, iTween.Hash("x", pos.x, "y", pos.y + 1, "z", pos.z, "time", 0.2f, "easetype", iTween.EaseType.linear));
    }

    IEnumerator DestroyCo(GameObject go)
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(go);
    }
}
