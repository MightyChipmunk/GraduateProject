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
        win.text = isWin ? "Ω¬∏Æ!" : "∆–πË..";
        gold.text = isWin ? "»πµÊ«— ∞ÒµÂ: " + reward.gold.ToString() : "»πµÊ«— ∞ÒµÂ: 0";
        exp.text = isWin ? "»πµÊ«— ∞Ê«Ëƒ°: " + reward.exp.ToString() : "»πµÊ«— ∞Ê«Ëƒ°: 0";
    }
}
