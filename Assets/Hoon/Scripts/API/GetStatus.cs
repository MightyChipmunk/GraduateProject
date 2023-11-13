using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetStatus : MonoBehaviour
{
    public GameObject swordMan;
    public GameObject archer;
    public GameObject wizard;
    public TMP_Text lv;
    public TMP_Text exp;

    // Start is called before the first frame update
    void Awake()
    {
        transform.Find("Quit").GetComponent<Button>().onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        Init();
    }

    void Init()
    {
        lv.text = NetworkManager.Instance.playerTeam.level.ToString();
        exp.text = NetworkManager.Instance.playerTeam.exp.ToString();

        foreach (TeamMember mem in NetworkManager.Instance.playerTeam.members)
        {
            if (mem.modelName == "UnityChanKohaku")
            {
                swordMan.transform.Find("HP").GetComponent<TMP_Text>().text = mem.hp.ToString();
                swordMan.transform.Find("Strength").GetComponent<TMP_Text>().text = mem.strength.ToString();
                swordMan.transform.Find("Defense").GetComponent<TMP_Text>().text = mem.defence.ToString();
                swordMan.transform.Find("Speed").GetComponent<TMP_Text>().text = mem.speed.ToString();
                swordMan.transform.Find("SkillLv").GetComponent<TMP_Text>().text = mem.skillLv.ToString();
            }
            if (mem.modelName == "UnityChanKAGURA")
            {
                archer.transform.Find("HP").GetComponent<TMP_Text>().text = mem.hp.ToString();
                archer.transform.Find("Strength").GetComponent<TMP_Text>().text = mem.strength.ToString();
                archer.transform.Find("Defense").GetComponent<TMP_Text>().text = mem.defence.ToString();
                archer.transform.Find("Speed").GetComponent<TMP_Text>().text = mem.speed.ToString();
                archer.transform.Find("SkillLv").GetComponent<TMP_Text>().text = mem.skillLv.ToString();
            }
            if (mem.modelName == "UnityChan")
            {
                wizard.transform.Find("HP").GetComponent<TMP_Text>().text = mem.hp.ToString();
                wizard.transform.Find("Strength").GetComponent<TMP_Text>().text = mem.strength.ToString();
                wizard.transform.Find("Defense").GetComponent<TMP_Text>().text = mem.defence.ToString();
                wizard.transform.Find("Speed").GetComponent<TMP_Text>().text = mem.speed.ToString();
                wizard.transform.Find("SkillLv").GetComponent<TMP_Text>().text = mem.skillLv.ToString();
            }
        }
    }
}
