using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    ClientInterface tcpInterface = new ClientInterface();

    [SerializeField]
    List<TeamMember> playerMembers;

    [HideInInspector]
    public Team playerTeam;
    [HideInInspector]
    public Team enemyTeam;
    [HideInInspector]
    public Reward reward;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(Instance);

        playerTeam = new Team(playerMembers);
    }

    private void Update()
    {
        string data = tcpInterface.RecvMessage();
        if (data != null)
        {
            // 만약 서버로부터 Command 정보를 받는다면
            try
            {
                // 역직렬화 후 커맨드 실행
                Command command = JsonUtility.FromJson<Command>(data);
                BattleManager.Instance.ExcuteCommand(command);
            }
            // 만약 서버로부터 팀 정보를 받는다면 
            catch (ArgumentException e)
            {
                // 역직렬화 후 상대팀의 정보 저장
                enemyTeam = JsonUtility.FromJson<Team>(data);
                // PVP 시작 (서버에 연결되어 있으니 자신은 PVP 준비 한 상태)
                PVPStart();
            }
        }
    }

    public void TCPStart()
    {
        tcpInterface.Start();
    }

    public bool IsConnected()
    {
        return tcpInterface.isReady ? true : false;
    }

    public void Send(string data)
    {
        byte[] StrByte = Encoding.UTF8.GetBytes(data);
        tcpInterface.SendMessage(StrByte);
    }

    // PVP가 준비됐다면 서버에 연결 후 자신의 팀 정보를 직렬화 하고 서버에 보냄
    public void PVPReady()
    {
        TCPStart();
        string data = JsonUtility.ToJson(playerTeam);
        byte[] StrByte = Encoding.UTF8.GetBytes(data);
        tcpInterface.SendMessage(StrByte);
    }

    public void PVPStart()
    {
        SceneManager.LoadScene("BattleScene");
    }

    public void CloseNet()
    {
        tcpInterface.CloseAll();
        tcpInterface = new ClientInterface();
    }
}
