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

    List<Command> commands = new List<Command>();

    int selected = 0;
    public int Selected
    {
        get { return selected; }
        set 
        {
            if (selected != value && !IsGameOver())
            {
                if (value < 0)
                {
                    value = enemyList.Count - 1;
                }
                else if (value >= enemyList.Count)
                {
                    value = 0;
                }
                iTween.MoveTo(downArrow, iTween.Hash("x", enemyList[value].transform.position.x, "y", 4, "z", enemyList[value].transform.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
                Transform tr = turn[0].transform;
                Vector3 dir = Quaternion.LookRotation(enemyList[value].transform.position - tr.position).eulerAngles;
                iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", dir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            }
            selected = value;
            if (!IsGameOver())
                EnemyInfoSet(enemyList[selected].GetComponent<Stat>());
        }
    }
    List<GameObject> playerList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();
    List<GameObject> turn = new List<GameObject>();

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
        StartCoroutine(StartBattle(Server_Test.Instance.playerTeam, Server_Test.Instance.enemyTeam, Server_Test.Instance.reward));

        attack.onClick.AddListener(() =>
        {
            if (myTurn() && !isAction && !IsGameOver())
            {
                turn[0].GetComponent<Stat>().Attack(enemyList[selected].GetComponent<Stat>());
                commands.Add(new Command(0, turn[0].GetComponent<Stat>().GetIndex, enemyList[selected].GetComponent<Stat>().GetIndex, 0));
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
        if (!myTurn() && !isAction && !IsGameOver())
        {
            turn[0].GetComponent<Stat>().Attack(playerList[0].GetComponent<Stat>());
            commands.Add(new Command(0, turn[0].GetComponent<Stat>().GetIndex, playerList[selected].GetComponent<Stat>().GetIndex, 1));
        }

        downArrow.SetActive(myTurn() && !IsGameOver());
    }

    IEnumerator StartBattle(Team playerTeam, Team enemyTeam, Reward reward)
    {
        isAction = true;

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

        BattleUIManager.Instance.StartUI();
        BattleUIManager.Instance.reward = reward;
        yield return new WaitForSeconds(3f);
        isAction = false;
    }

    public void EnemyInfoSet(Stat stat)
    {
        if (!enemyList.Contains(stat.gameObject)) return;

        enemyInfo.GetComponentInChildren<Slider>().maxValue = stat.hpBar.maxValue;
        enemyInfo.GetComponentInChildren<Slider>().value = stat.hpBar.value;
        enemyInfo.GetComponentInChildren<TMP_Text>().text = stat.Name;
    }

    public void EndTurn()
    {
        if (IsGameOver()) return;

        TurnEnqueue(TurnDequeue());

        if (!myTurn())
        {
            EnemyInfoSet(turn[0].GetComponent<Stat>());
        }
        else
        {
            EnemyInfoSet(enemyList[selected].GetComponent<Stat>());
        }
    }

    public void MoveCam()
    {
        if (IsGameOver()) return;

        if (myTurn())
        {
            Transform tr = turn[0].transform;
            Vector3 dir = Quaternion.LookRotation(enemyList[selected].transform.position - tr.position).eulerAngles;
            iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", dir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", tr.position.x, "y", tr.position.y, "z", tr.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
        }
        else
        {
            Transform tr = turn[0].transform;
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", tr.position.x, "y", tr.position.y, "z", tr.position.z - 5.5, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            EnemyInfoSet(tr.GetComponent<Stat>());
        }
    }

    public void ShakeCam(float shakeRange = 0.03f)
    {
        StartCoroutine(ShakeCamCo(shakeRange));
    }

    IEnumerator ShakeCamCo(float shakeRange)
    {
        float currentTime = 0;
        float shakeTime = 0.3f;
        float shakePer = 0;
        Vector3 OrigPos = Camera.main.transform.localPosition;
        while (currentTime <= shakeTime)
        {
            shakePer += Time.deltaTime;
            if (shakePer >= 0.1f)
            {
                shakeRange *= -1;
            }

            yield return null;
            currentTime += Time.deltaTime;
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, OrigPos + new Vector3(1, 1, 0).normalized * shakeRange, 1);
        }
        Camera.main.transform.localPosition = OrigPos;
    }

    void InitTurn()
    {
        turn.Clear();
        foreach (GameObject go in turnInfoList)
        {
            Destroy(go);
        }
        turnInfoList.Clear();

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

    public bool myTurn()
    {
        return playerList.Contains(turn[0]);
    }

    List<GameObject> turnInfoList = new List<GameObject>();
    void TurnEnqueue(GameObject go)
    {
        turn.Add(go);

        GameObject info = Instantiate(turnInfo, canvas);
        info.GetComponent<TurnInfo>().Init(Resources.Load<Sprite>(go.GetComponent<Stat>().ModelName + "Icon"), go.GetComponent<Stat>().Name, go);
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

        GameObject dequeue = turn[0];
        turn.RemoveAt(0);
        return dequeue;
    }

    void TurnDelete(GameObject target)
    {
        int idx = 0;
        for (int i = 0; i < turnInfoList.Count; i++)
        {
            if (turnInfoList[i].GetComponent<TurnInfo>().target == target)
            {
                GameObject tmp = turnInfoList[i];
                turnInfoList.RemoveAt(i);
                Destroy(tmp);
                idx = i;
                break;
            }
        }

        for (int i = idx; i < turnInfoList.Count; i++)
        {
            iTween.MoveTo(turnInfoList[i], iTween.Hash("y", turnInfoList[i].transform.position.y + 100, "time", 0.3f, "easetype", iTween.EaseType.easeOutQuint));
        }

        turn.Remove(target);
    }

    public void CharDestroy(GameObject go)
    {
        if (playerList.Contains(go))
        {
            playerList.Remove(go);
            Destroy(go);
            TurnDelete(go);
        }
        else if (enemyList.Contains(go))
        {
            enemyList.Remove(go);
            Destroy(go);
            TurnDelete(go);

            Selected = 100;
        }
    }

    public bool IsGameOver()
    {
        if (enemyList.Count > 0 && playerList.Count > 0) return false;
        else
        {
            if (enemyList.Count <= 0)
                BattleUIManager.Instance.EndUI(true);
            else 
                BattleUIManager.Instance.EndUI(false);
            return true;
        }
    }

    void GetCommand(Command command)
    {
        Stat attacker;
        Stat deffender;

        if (command.team == 0)
        {
            attacker = playerList[command.attackerIdx].GetComponent<Stat>();
            deffender = enemyList[command.deffenderIdx].GetComponent<Stat>();
        }
        else
        {
            attacker = enemyList[command.attackerIdx].GetComponent<Stat>();
            deffender = playerList[command.deffenderIdx].GetComponent<Stat>();
        }


        switch (command.actionCategory)
        {
            case (0):
                attacker.Attack(deffender);
                break;
            case (1):
                attacker.Skill(deffender);
                break;
        }
    }
}
