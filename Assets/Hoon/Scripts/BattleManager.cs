using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public GameObject downArrow;
    public GameObject charInfo;
    public GameObject enemyInfo;
    public Transform content;
    public Transform canvas;
    public Button attack;
    public static BattleManager Instance;

    int selected = 0;
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
                iTween.MoveTo(downArrow, iTween.Hash("x", enemyList[value].transform.position.x, "y", 4, "z", enemyList[value].transform.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
                iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", value * -10, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            }
            selected = value;
            EnemyInfoSet(enemyList[selected].GetComponent<Stat>());
        }
    }
    List<GameObject> playerList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();

    [SerializeField]
    Queue<GameObject> turn = new Queue<GameObject>();

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

        turn.Enqueue(playerList[0]);
        turn.Enqueue(playerList[1]);

        attack.onClick.AddListener(() =>
        {
            turn.Peek().GetComponent<Stat>().Attack(enemyList[selected].GetComponent<Stat>());
        });
        attack.onClick.AddListener(EndTurn);
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
            GameObject info = Instantiate(charInfo, content);
            GameObject newMem = mem.Instantiate(info);
            newMem.transform.position = new Vector3(-3 * mem.GetIndex, 0, 0);
            playerList.Add(newMem);
        }

        Selected = 0;
    }

    public void EnemyInfoSet(Stat stat)
    {
        enemyInfo.GetComponentInChildren<Slider>().maxValue = stat.hpBar.maxValue;
        enemyInfo.GetComponentInChildren<Slider>().value = stat.hpBar.value;
        enemyInfo.GetComponentInChildren<TMP_Text>().text = stat.Name;
    }

    void EndTurn()
    {
        turn.Enqueue(turn.Dequeue());
    }

    public void MoveCam()
    {
        if (playerList.Contains(turn.Peek()))
        {
            Transform tr = turn.Peek().transform;
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", tr.position.x, "y", tr.position.y, "z", tr.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
        }
    }
}
