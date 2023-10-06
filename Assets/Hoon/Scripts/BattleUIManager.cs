using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public TMP_Text actionName;
    public GameObject startUI;
    public GameObject endUI;
    [HideInInspector]
    public Reward reward;

    public static BattleUIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    public void StartUI()
    {
        iTween.MoveTo(startUI, iTween.Hash("x", 960, "time", 1, "easetype", iTween.EaseType.easeOutQuint));
        iTween.MoveTo(startUI, iTween.Hash("x", 2880, "time", 1, "delay", 1.5f, "easetype", iTween.EaseType.easeInQuint));
    }

    public void EndUI(bool isWin)
    {
        if (endUI.activeSelf == true) return;

        endUI.SetActive(true);
        endUI.GetComponent<EndUI>().Init(reward, isWin);
    }

    public void ActionName(string act)
    {
        actionName.text = act;
        actionName.gameObject.SetActive(true);
    }

    public void ActionEnd()
    {
        actionName.gameObject.SetActive(false);
    }
}
