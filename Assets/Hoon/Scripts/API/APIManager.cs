using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SkillUp(string modelNameLv)
    {
        string modelName = modelNameLv.Substring(0, modelNameLv.Length - 1);
        int lv = Int32.Parse(modelNameLv.Substring(modelNameLv.Length - 1, 1));

        foreach (TeamMember mem in NetworkManager.Instance.playerTeam.members)
        {
            if (mem.modelName == modelName && mem.skillLv < lv)
            {
                mem.skillLv = lv;
                StartCoroutine(StatUpCo(mem));
            }
        }
    }

    public void EquipUp(string modelNameLv)
    {
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
            }
        }
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
}
