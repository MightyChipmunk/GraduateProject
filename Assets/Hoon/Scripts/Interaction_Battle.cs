using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_Battle : Interaction
{
    public List<TeamMember> enemyMembers;
    Team enemyTeam;
    public Reward reward;

    protected override void Start()
    {
        base.Start();
        interUI = GameObject.Find("Battle Interaction");
        interUI.SetActive(false);
        enemyTeam = new Team(enemyMembers);
    }

    protected override void Interact()
    {
        Server_Test.Instance.enemyTeam = this.enemyTeam;
        Server_Test.Instance.reward = this.reward;
        SceneManager.LoadScene("BattleScene");
    }
}
