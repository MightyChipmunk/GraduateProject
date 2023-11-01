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
        win.text = isWin ? "½Â¸®!" : "ÆÐ¹è..";
        gold.text = isWin ? reward.gold.ToString() : "0";
        exp.text = isWin ? reward.exp.ToString() : "0";
    }
}
