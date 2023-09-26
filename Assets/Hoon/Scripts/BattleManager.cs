using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject downArrow;
    public static BattleManager Instance;

    int selected;
    public int Selected
    {
        get { return selected; }
        set 
        { 
            if (value < 0)
            {
                value = enemyList.Count - 1;
            }
            else if (value >= enemyList.Count)
            {
                value = 0;
            }
            if (selected != value)
            {
                iTween.MoveTo(downArrow, iTween.Hash("x", value * -3, "y", 4, "z", 6, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
                iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", value * -10, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            }
            selected = value; 
        }
    }
    List<GameObject> playerList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    private void Start()
    {
        StartBattle(Server_Test.Instance.playerTeam, Server_Test.Instance.enemyTeam);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Selected += 1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Selected -= 1;
        }
    }

    void StartBattle(Team playerTeam, Team enemyTeam)
    {
        foreach (TeamMember mem in enemyTeam.Members)
        {
            GameObject newMem = mem.Instantiate();
            newMem.transform.position = new Vector3(-3 * mem.GetIndex, 0, 6);
            enemyList.Add(newMem);
        }
        foreach (TeamMember mem in playerTeam.Members)
        {
            GameObject newMem = mem.Instantiate();
            newMem.transform.position = new Vector3(-3 * mem.GetIndex, 0, 0);
            playerList.Add(newMem);
        }

        selected = 0;
    }
}
