using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableBtn : MonoBehaviour
{
    public int lv;
    public string modelName;
    public bool isSkill = false;

    public void DisableSkillBtn()
    {
        foreach (TeamMember mem in NetworkManager.Instance.playerTeam.members)
        {
            if (mem.modelName == modelName && mem.skillLv >= lv)
            {
                GetComponent<Button>().interactable = false;
            }
            else if (mem.modelName == modelName && mem.skillLv + 1 < lv)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }

    public void DisableEquipBtn()
    {
        foreach (TeamMember mem in NetworkManager.Instance.playerTeam.members)
        {
            if (mem.modelName == modelName && mem.equipLv >= lv)
            {
                GetComponent<Button>().interactable = false;
            }
            else if (mem.modelName == modelName && mem.equipLv + 1 < lv)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }

    private void Update()
    {
        if (isSkill)
        {
            DisableSkillBtn();
        }
        else
        {
            DisableEquipBtn();
        }
    }
}
