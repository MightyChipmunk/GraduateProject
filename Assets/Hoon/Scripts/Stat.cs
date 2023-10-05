using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [SerializeField]
    GameObject dieEffect;
    [SerializeField]
    GameObject hitEffect;

    public Slider hpBar;
    public TMP_Text chName;
    Animator anim;
    AnimationEvent animEvent;

    [SerializeField]
    int hp = 0;
    public int HP
    {
        get { return hp; }
        set 
        { 
            hp = value;
            if (hpBar != null)
            {
                hpBar.value = hp;
            }

            if (hp <= 0)
            {
                Died();
            }
        }
    }

    [SerializeField]
    string _name;
    public string Name 
    { 
        get { return _name; } 
        set 
        { 
            _name = value;
            if (chName != null)
                chName.text = _name;
        }
    }

    [SerializeField]
    int index;
    public int GetIndex { get { return index; } }
    public int SetIndex { set { index = value; } }

    [SerializeField]
    int strength = 10;
    public int Strength { get { return strength; } set { strength = value; } }

    [SerializeField]
    int defense = 3;
    public int Defense { get { return defense; } set { defense = value; } }

    [SerializeField]
    int speed = 5;
    public int Speed { get { return speed; } set { speed = value; } }

    [SerializeField]
    string modelName;
    public string ModelName { get { return modelName; } set { modelName = value; } }

    public int[] Near()
    {
        int[] near = new int[2];
        near[0] = index - 1;
        near[1] = index + 1;

        return near;
    }

    public void Attack(Stat enemyStat)
    {
        anim.SetTrigger("Attack");
        BattleManager.Instance.isAction = true;
        animEvent.enemyStat = enemyStat;

        if (!BattleManager.Instance.myTurn())
        {
            Vector3 camDir = Quaternion.LookRotation(transform.position - enemyStat.transform.position).eulerAngles;
            iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", camDir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", enemyStat.transform.position.x, "y", enemyStat.transform.position.y, "z", enemyStat.transform.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
        }
        Vector3 attackDir = Quaternion.LookRotation(enemyStat.transform.position - transform.position).eulerAngles;
        iTween.RotateTo(gameObject, iTween.Hash("y", attackDir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
    }

    public void GetHit(int damage)
    {
        anim.SetTrigger("GetHit");
        
        GameObject effect = Instantiate(hitEffect);
        effect.transform.position = transform.position + GetComponent<CharacterController>().center;

        GameObject damageUI = Instantiate(Resources.Load<GameObject>("DamageUI"));
        damageUI.transform.position = transform.position;
        damageUI.GetComponent<DamageUI>().Init(damage, GetComponent<CharacterController>().height);

        BattleManager.Instance.ShakeCam();
    }

    void Died()
    {
        GameObject go = Instantiate(dieEffect);
        go.transform.position = transform.position + GetComponent<CharacterController>().center;
        BattleManager.Instance.ShakeCam(0.05f);
        BattleManager.Instance.CharDestroy(gameObject);
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        animEvent = GetComponentInChildren<AnimationEvent>();
    }
}
