using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndUI : MonoBehaviour
{
    public TMP_Text win;
    public TMP_Text gold;
    public TMP_Text exp;

    public void Init(Reward reward, bool isWin)
    {
        win.text = isWin ? "�¸�!" : "�й�..";
        gold.text = isWin ? "ȹ���� ���: " + reward.gold.ToString() : "ȹ���� ���: 0";
        exp.text = isWin ? "ȹ���� ����ġ: " + reward.exp.ToString() : "ȹ���� ����ġ: 0";
    }
}
