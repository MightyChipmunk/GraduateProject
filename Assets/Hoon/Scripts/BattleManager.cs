using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{
    List<TeamMember> members = new List<TeamMember>();
    public List<TeamMember> Members
    {
        get { return members; }
        set
        {
            members = value;
            int idx = 0;
            foreach (TeamMember member in members)
            {
                member.SetIndex = idx++;
            }
        }
    }

    public Team(List<TeamMember> list)
    {
        members = list;
        int idx = 0;
        foreach (TeamMember member in members)
        {
            member.SetIndex = idx++;
        }
    }

    public TeamMember GetIndexMember(int index)
    {
        return members[index];
    }
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBattle(Team playerTeam, Team enemyTeam)
    {
        foreach (TeamMember mem in enemyTeam.Members)
        {
            GameObject newMem = mem.Instantiate();
            newMem.transform.position = new Vector3(-5 * mem.GetIndex, 0, 6);
        }
        foreach (TeamMember mem in playerTeam.Members)
        {
            GameObject newMem = mem.Instantiate();
            newMem.transform.position = new Vector3(-5 * mem.GetIndex, 0, 0);
        }
    }
}
