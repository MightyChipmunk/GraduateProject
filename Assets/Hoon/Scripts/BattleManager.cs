using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Button skill;
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

                for (int i = 0; i < turnInfoList.Count; i++)
                {
                    GameObject tmp = turnInfoList[i];
                    Color col;
                    col = tmp.transform.Find("BG").GetComponent<Image>().color;
                    col.a = 0.5f;
                    tmp.transform.Find("BG").GetComponent<Image>().color = col;
                }

                iTween.MoveTo(downArrow, iTween.Hash("x", enemyList[value].transform.position.x, "y", 4, "z", enemyList[value].transform.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
                Transform tr = turn[0].transform;
                Vector3 dir = Quaternion.LookRotation(enemyList[value].transform.position - tr.position).eulerAngles;
                iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", dir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));

                TurnHilight(enemyList[value]);
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
        StartCoroutine(StartBattle(NetworkManager.Instance.playerTeam, NetworkManager.Instance.enemyTeam, NetworkManager.Instance.reward));

        attack.onClick.AddListener(() =>
        {
            if (myTurn() && !isAction && !IsGameOver())
            {
                Command command = new Command(0, playerList.IndexOf(turn[0]), selected, NetworkManager.Instance.userId);
                string data = JsonUtility.ToJson(command);
                if (NetworkManager.Instance.IsConnected())
                    NetworkManager.Instance.Send(data, 3);
                else
                    ExcuteCommand(command);
                commands.Add(command);
            }
            if (isAction)
            {
                isAction = false;
                MoveCam(false);
                TurnHilight(enemyList[selected]);
            }
        });
        skill.onClick.AddListener(() =>
        {
            if (myTurn() && !isAction && !IsGameOver())
            {
                Command command = new Command(1, playerList.IndexOf(turn[0]), selected, NetworkManager.Instance.userId);
                string data = JsonUtility.ToJson(command);
                if (NetworkManager.Instance.IsConnected())
                    NetworkManager.Instance.Send(data, 3);
                else
                    ExcuteCommand(command);
                commands.Add(command);
            }
            if (isAction)
            {
                isAction = false;
                MoveCam(false);
                TurnHilight(enemyList[selected]);
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

        // юс╫ц
        if (!myTurn() && !isAction && !IsGameOver() && !NetworkManager.Instance.IsConnected())
        {
            Command command = new Command(0, enemyList.IndexOf(turn[0]), 0);
            ExcuteCommand(command);
            commands.Add(command);
        }

        downArrow.SetActive(myTurn() && !IsGameOver() && !isAction);
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
        
        if (!myTurn()) isAction = false;
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
            isAction = false;
        }
        else
        {
            EnemyInfoSet(enemyList[selected].GetComponent<Stat>());
        }
    }

    public void MoveCam(bool turnEnd = true)
    {
        if (IsGameOver()) return;

        if (myTurn() && !turnEnd)
        {
            Transform tr = turn[0].transform;
            Vector3 dir = Quaternion.LookRotation(enemyList[selected].transform.position - tr.position).eulerAngles;
            iTween.RotateTo(Camera.main.transform.parent.gameObject, iTween.Hash("y", dir.y, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
            iTween.MoveTo(Camera.main.transform.parent.gameObject, iTween.Hash("x", tr.position.x, "y", tr.position.y, "z", tr.position.z, "time", 0.2f, "easetype", iTween.EaseType.easeOutQuint));
        }
        else if (myTurn() && turnEnd)
        {
            Transform tr = turn[0].transform;
            Vector3 dir = Quaternion.identity.eulerAngles;
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

        if (NetworkManager.Instance.cnt <= 1)
        {
            foreach (GameObject go in playerList)
            {
                allList.Add(go);
            }
            foreach (GameObject go in enemyList)
            {
                allList.Add(go);
            }
        }
        else if (NetworkManager.Instance.cnt >= 2)
        {
            foreach (GameObject go in enemyList)
            {
                allList.Add(go);
            }
            foreach (GameObject go in playerList)
            {
                allList.Add(go);
            }
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

        if (playerList.Contains(go))
        {
            Color col = Color.blue;
            col.a = 0.5f;
            info.transform.Find("BG").GetComponent<Image>().color = col;
        }
        else
        {
            Color col = Color.red;
            col.a = 0.5f;
            info.transform.Find("BG").GetComponent<Image>().color = col;
        }

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

    void TurnHilight(GameObject go)
    {
        for (int i = 0; i < turnInfoList.Count; i++)
        {
            if (turnInfoList[i].GetComponent<TurnInfo>().target == go)
            {
                GameObject tmp = turnInfoList[i];
                Color col;
                col = tmp.transform.Find("BG").GetComponent<Image>().color;
                col.a = 1;
                tmp.transform.Find("BG").GetComponent<Image>().color = col;
            }
        }
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

    public void ExcuteCommand(Command command)
    {
        Stat attacker;
        Stat deffender;

        if (command.id == NetworkManager.Instance.userId)
        {
            attacker = playerList[command.attackerIdx].GetComponent<Stat>();
            deffender = enemyList[command.deffenderIdx].GetComponent<Stat>();
        }
        else
        {
            for (int i = 0; i < turnInfoList.Count; i++)
            {
                GameObject tmp = turnInfoList[i];
                Color col;
                col = tmp.transform.Find("BG").GetComponent<Image>().color;
                col.a = 0.5f;
                tmp.transform.Find("BG").GetComponent<Image>().color = col;
            }

            attacker = enemyList[command.attackerIdx].GetComponent<Stat>();
            deffender = playerList[command.deffenderIdx].GetComponent<Stat>();

            TurnHilight(attacker.gameObject);
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

    private void OnDestroy()
    {
        if (NetworkManager.Instance.IsConnected())
        {
            NetworkManager.Instance.CloseNet();
            NetworkManager.Instance.cnt = 0;
        }
    }
}
