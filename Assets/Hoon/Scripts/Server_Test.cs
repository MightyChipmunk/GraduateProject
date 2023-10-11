using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server_Test : MonoBehaviour
{
    public static Server_Test Instance;

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
    }

    public void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}
