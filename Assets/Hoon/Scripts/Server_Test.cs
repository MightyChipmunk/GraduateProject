using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server_Test : MonoBehaviour
{
    public static Server_Test Instance;
    ClientInterface tcpInterface = new ClientInterface();

    [SerializeField]
    List<TeamMember> playerMembers;
    List<TeamMember> enemyMembers;

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
        tcpInterface.Start();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            string test = "Test";
            byte[] StrByte = Encoding.UTF8.GetBytes(test);
            tcpInterface.SendMessage(StrByte);
        }
    }
}
