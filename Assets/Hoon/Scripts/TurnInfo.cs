using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnInfo : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    Image image;
    [SerializeField]
    TMP_Text nameText;

    public void Init(Sprite icon, string objName, GameObject go)
    {
        image.sprite = icon;
        nameText.text = objName;
        target = go;
    }
}
