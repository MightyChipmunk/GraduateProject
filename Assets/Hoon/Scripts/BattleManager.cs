using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    GameObject player_Main;

    public static BattleManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        player_Main = GameObject.FindWithTag("Player");

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle(Transform field, List<GameObject> player, List<GameObject> enemy)
    {

    }
}
