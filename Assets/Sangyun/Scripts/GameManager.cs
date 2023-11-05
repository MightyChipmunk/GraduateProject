using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public PlayerController player;

    public GameObject inventory;
    //public TMP_Text maxScoreTxt;
    public TMP_Text playerRockTxt;
    public TMP_Text playerCoinTxt;


    /*void Awake()
    {
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }*/

    void LateUpdate()
    {
        playerRockTxt.text = string.Format("{0:n0}", player.rock);
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
    }



}
