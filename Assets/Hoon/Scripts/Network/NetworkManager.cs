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
            // ���� �����κ��� Command ������ �޴´ٸ�
            try
            {
                // ������ȭ �� Ŀ�ǵ� ����
                Command command = JsonUtility.FromJson<Command>(data);
                BattleManager.Instance.ExcuteCommand(command);
            }
            // ���� �����κ��� �� ������ �޴´ٸ� 
            catch (ArgumentException e)
            {
                // ������ȭ �� ������� ���� ����
                enemyTeam = JsonUtility.FromJson<Team>(data);
                // PVP ���� (������ ����Ǿ� ������ �ڽ��� PVP �غ� �� ����)
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

    // PVP�� �غ�ƴٸ� ������ ���� �� �ڽ��� �� ������ ����ȭ �ϰ� ������ ����
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
