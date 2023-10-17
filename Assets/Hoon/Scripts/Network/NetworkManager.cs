using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (data[2] == 'a')
            {
                // 역직렬화 후 커맨드 실행
                Command command = JsonUtility.FromJson<Command>(data);
                BattleManager.Instance.ExcuteCommand(command);
            }
            // 만약 서버로부터 팀 정보를 받는다면
            else if (data[2] == 'm')
            {
                // 역직렬화 후 상대팀의 정보 저장
                enemyTeam = JsonUtility.FromJson<Team>(data);
                // PVP 시작 (서버에 연결되어 있으니 자신은 PVP 준비 한 상태)
                //PVPStart();
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

    // PVP가 준비됐다면 서버에 연결 후 자신의 팀 정보를 직렬화 하고 서버에 보냄
    public void PVPReady()
    {
        TCPStart();
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
