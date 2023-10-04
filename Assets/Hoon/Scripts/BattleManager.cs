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
    public GameObject turnInfo;
    public Transform content;
    public Transform canvas;
    public Button attack;
    public bool isAction = false;
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


        attack.onClick.AddListener(() =>
        {
            if (myTurn() && !isAction)
            {
                turn.Peek().GetComponent<Stat>().Attack(enemyList[selected].GetComponent<Stat>());
            }
        });
    }

    private void Update()
    {
        if (myTurn() && !isAction)
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

        //юс╫ц
        if (!myTurn() && !isAction)
        {
            turn.Peek().GetComponent<Stat>().Attack(playerList[0].GetComponent<Stat>());
        }

        downArrow.SetActive(myTurn());
    }

    void StartBattle(Team playerTeam, Team enemyTeam)
    {
        foreach (TeamMember mem in enemyTeam.Members)
        {
            GameObject newMem = mem.Instantiate();
            newMem.transform.position = new Vector3(-3 * mem.GetIndex, 0, 6);
            newMem.transform.rotation = Quaternion.LookRotation(Vector3.back);
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
        InitTurn();
        MoveCam();
    }

    public void EnemyInfoSet(Stat stat)
    {
        enemyInfo.GetComponentInChildren<Slider>().maxValue = stat.hpBar.maxValue;
        enemyInfo.GetComponentInChildren<Slider>().value = stat.hpBar.value;
        enemyInfo.GetComponentInChildren<TMP_Text>().text = stat.Name;
    }

    public void EndTurn()
    {
        TurnEnqueue(TurnDequeue());

        if (!myTurn())
        {
            EnemyInfoSet(turn.Peek().GetComponent<Stat>());
        }
        else
        {
            EnemyInfoSet(enemyList[selected].GetComponent<Stat>());
        }
    }

    public void MoveCam()
    {
        if (myTurn())
        {
            Transform tr = turn.Peek().transform;
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", tr.position.x, "y", tr.position.y, "z", tr.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
        }
        else
        {
            Transform tr = turn.Peek().transform;
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", tr.position.x, "y", tr.position.y, "z", tr.position.z - 5.5, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            EnemyInfoSet(tr.GetComponent<Stat>());
        }
    }

    void InitTurn()
    {
        List<GameObject> allList = new List<GameObject> ();
        foreach (GameObject go in playerList)
        {
            allList.Add(go);
        }
        foreach (GameObject go in enemyList)
        {
            allList.Add(go);
        }

        int cnt = allList.Count;

        for (int i = 0; i < cnt; i++)
        {
            int max = 0;
            int idx = 0;
            for (int j = 0; j < allList.Count; j++)
            {
                if (allList[j].GetComponent<Stat>().Speed >= max)
                {
                    max = allList[j].GetComponent<Stat>().Speed;
                    idx = j;
                }
            }
            TurnEnqueue(allList[idx]);
            allList.RemoveAt(idx);
        }
    }

    bool myTurn()
    {
        return playerList.Contains(turn.Peek());
    }

    List<GameObject> turnInfoList = new List<GameObject>();
    void TurnEnqueue(GameObject go)
    {
        turn.Enqueue(go);

        GameObject info = Instantiate(turnInfo, canvas);
        info.transform.Find("IconMask").transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(go.GetComponent<Stat>().Name + "Icon");
        info.transform.Find("Name").GetComponent<TMP_Text>().text = go.GetComponent<Stat>().Name;
        info.transform.position = new Vector3(150, 930 - 100 * turnInfoList.Count, 0);
        info.transform.localScale = Vector3.zero;
        iTween.ScaleTo(info, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        turnInfoList.Add(info);
    }

    GameObject TurnDequeue()
    {
        GameObject first = turnInfoList[0];
        turnInfoList.RemoveAt(0);
        Destroy(first);

        foreach (GameObject go in turnInfoList)
        {
            iTween.MoveTo(go, iTween.Hash("y", go.transform.position.y + 100, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        }

        return turn.Dequeue();
    }
}
