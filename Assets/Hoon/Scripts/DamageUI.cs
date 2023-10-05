using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    TMP_Text damageUI;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - GameObject.FindWithTag("UICamera").transform.position);
    }

    public void Init(int damage, float position)
    {
        damageUI = GetComponentInChildren<TMP_Text>();
        damageUI.text = damage.ToString();
        damageUI.transform.localScale = Vector3.zero;
        damageUI.transform.localPosition = new Vector3(0, position, 0);
        iTween.ScaleTo(damageUI.gameObject, iTween.Hash("x", 0.01, "y", 0.01, "z", 0.01, "time", 0.6f, "easetype", iTween.EaseType.easeOutElastic));
    }
}
