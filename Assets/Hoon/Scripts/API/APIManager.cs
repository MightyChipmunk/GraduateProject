using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            UpdateUserInfo();
        }
    }

    public void UpdateUserInfo()
    {
        StartCoroutine(UserUpCo());
    }

    public void SkillUp(string modelNameLv)
    {
        if (NetworkManager.Instance.playerTeam.stones <= 0) {
            return;
        }

        string modelName = modelNameLv.Substring(0, modelNameLv.Length - 1);
        int lv = Int32.Parse(modelNameLv.Substring(modelNameLv.Length - 1, 1));

        foreach (TeamMember mem in NetworkManager.Instance.playerTeam.members)
        {
            if (mem.modelName == modelName && mem.skillLv < lv)
            {
                mem.skillLv = lv;
                StartCoroutine(StatUpCo(mem));
                NetworkManager.Instance.playerTeam.stones--;
                UpdateUserInfo();
            }
        }
    }

    public void EquipUp(string modelNameLv)
    {
        if (NetworkManager.Instance.playerTeam.gold <= 99) {
            return;
        }

        string modelName = modelNameLv.Substring(0, modelNameLv.Length - 1);
        int lv = Int32.Parse(modelNameLv.Substring(modelNameLv.Length - 1, 1));

        foreach (TeamMember mem in NetworkManager.Instance.playerTeam.members)
        {
            if (mem.modelName == modelName && mem.equipLv < lv)
            {
                mem.hp += 15 * (lv - mem.equipLv);
                mem.strength += 5 * (lv - mem.equipLv);
                mem.defence += 3 * (lv - mem.equipLv);
                mem.equipLv = lv;
                StartCoroutine(StatUpCo(mem));
                NetworkManager.Instance.playerTeam.gold -= 100;
                UpdateUserInfo();
            }
        }
    }

    public void StatUp(TeamMember mem)
    {
        StartCoroutine(StatUpCo(mem));
    }

    IEnumerator StatUpCo(TeamMember mem)
    {
        string data = JsonUtility.ToJson(mem);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("http://43.202.59.52:8081/updateCharacter", data))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Update complete!");
            }
        }
    }

    IEnumerator UserUpCo()
    {
        string data = JsonUtility.ToJson(NetworkManager.Instance.playerTeam);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("http://43.202.59.52:8081/updateUser", data))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Update complete!");
            }
        }
    }
}
