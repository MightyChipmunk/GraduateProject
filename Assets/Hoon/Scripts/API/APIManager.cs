using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    //public Button swSkill1;
    //public Button swSkill2;
    //public Button swSkill3;
    //public Button arSkill1;
    //public Button arSkill2;
    //public Button arSkill3;
    //public Button maSkill1;
    //public Button maSkill2;
    //public Button maSkill3;

    //public Button swEquip1;
    //public Button swEquip2;
    //public Button swEquip3;
    //public Button arEquip1;
    //public Button arEquip2;
    //public Button arEquip3;
    //public Button maEquip1;
    //public Button maEquip2;
    //public Button maEquip3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void StatUp()
    {
        //string modelName = modelNameLv.Substring(0, modelNameLv.Length - 1);
        //int lv = Int32.Parse(modelNameLv.Substring(modelNameLv.Length - 1, 1));

        NetworkManager.Instance.playerTeam.members[0].hp += 20;

        StartCoroutine(StatUpCo(NetworkManager.Instance.playerTeam.members[0]));
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
