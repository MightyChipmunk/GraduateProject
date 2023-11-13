using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public static PopUp Instance;
    TMP_Text Text;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Text = transform.GetChild(0).GetComponentInChildren<TMP_Text>();    
        DontDestroyOnLoad(gameObject);
    }

    public void Pop(string text)
    {
        Text.text = text;
        iTween.ScaleTo(transform.GetChild(0).gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.5f, "easetype", iTween.EaseType.easeOutCirc));
        iTween.ScaleTo(transform.GetChild(0).gameObject, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 0.5f, "delay", 1.5f, "easetype", iTween.EaseType.easeOutCirc));
    }
}
