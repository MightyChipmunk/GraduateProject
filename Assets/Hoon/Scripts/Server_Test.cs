using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server_Test : MonoBehaviour
{
    public static Server_Test Instance;

    public GameObject enemy;
    public List<TeamMember> playerMembers;
    public List<TeamMember> enemyMembers;

    public Team playerTeam;
    public Team enemyTeam;

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
        enemyTeam = new Team(enemyMembers);
    }

    public void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}
