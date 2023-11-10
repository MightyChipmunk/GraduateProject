using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction_Battle : Interaction
{
    public List<TeamMember> enemyMembers;
    Team enemyTeam;
    public Reward reward;

    void Awake()
    {
        base.Start();
        interUI = GameObject.Find("Battle Interaction");
        enemyTeam = new Team(enemyMembers);
    }

    protected override void Start()
    {
        base.Start();
        interUI.SetActive(false);
    }

    protected override void Interact()
    {
        NetworkManager.Instance.enemyTeam = this.enemyTeam;
        NetworkManager.Instance.reward = this.reward;
        SceneManager.LoadScene("BattleScene");
    }
}
