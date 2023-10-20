using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
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

    // �ӽ�
    public string userId;
    public TMP_InputField input;

    public int cnt = 0;
    public bool isReady = false;

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

        reward = new Reward();

        input.onEndEdit.AddListener((string s) =>
        {
            userId = s;
            playerTeam = new Team(playerMembers, userId);
        });
    }

    private void Update()
    {

    }
    
    public void Receive(string data)
    {
        // ���� �����κ��� Command ������ �޴´ٸ�
        if (data[2] == 'a')
        {
            // ������ȭ �� Ŀ�ǵ� ����
            Command command = JsonUtility.FromJson<Command>(data);
            BattleManager.Instance.ExcuteCommand(command);
        }
        // ���� �����κ��� �� ������ �޴´ٸ�
        else if (data[2] == 'u')
        {
            // ������ȭ �� ���� ���� ����
            Team team = JsonUtility.FromJson<Team>(data);
            cnt++;
            // ���� ���� ������ ������̶��
            if (team.userId != this.userId)
            {
                // PVP ���� 
                enemyTeam = team;
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

    public void Send(string data, int id)
    {
        ArraySegment<byte> segment = Write(data, id);
        byte[] packet = segment.ToArray();

        tcpInterface.SendMessage(packet);
    }

    // PVP�� �غ�ƴٸ� ������ ���� �� �ڽ��� �� ������ ����ȭ �ϰ� ������ ����
    public void PVPReady()
    {
        string data = JsonUtility.ToJson(playerTeam);

        Send(data, 1);
    }

    public ArraySegment<byte> Write(string data, int id)
    {
        byte[] tmp = new byte[4096];
        ushort size = (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, tmp, 0);
        byte[] bytes = new byte[size + 6];
        ArraySegment<byte> segment = new ArraySegment<byte>(bytes);
        ushort count = 0;
        bool success = true;

        Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);

        count += sizeof(ushort);
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), (ushort)id);
        count += sizeof(ushort);
        ushort contentLen = (ushort)Encoding.Unicode.GetBytes(data, 0, data.Length, segment.Array, segment.Offset + count + sizeof(ushort));
        success &= BitConverter.TryWriteBytes(s.Slice(count, s.Length - count), contentLen);
        count += sizeof(ushort);
        count += contentLen;
        success &= BitConverter.TryWriteBytes(s, count);
        if (success == false)
            return null;
        return segment;
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
