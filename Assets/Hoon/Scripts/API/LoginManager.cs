using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UserLoginInfo
{
    public string userId;
    public string userPwd;
}

public class LoginManager : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField pwInput;
    public Button login;
    public Button signUpOpen;
    public Button signUp;
    string id;
    string pw;
    // Start is called before the first frame update
    void Start()
    {
        idInput.onEndEdit.AddListener((string s) => id = s);
        pwInput.onEndEdit.AddListener((string s) => pw = s);
        login.onClick.AddListener(() => StartCoroutine("Login"));
        signUpOpen.onClick.AddListener(() => signUp.transform.Find("SignUpUI").gameObject.SetActive(true));
        signUp.onClick.AddListener(() => StartCoroutine("SignUp"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Login()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://43.202.59.52:8081/login?" + "userId=" + id + "&userPwd=" + pw))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Login complete!");
                string data = www.downloadHandler.text;
                Debug.Log(data);
                Team team = JsonUtility.FromJson<Team>(data);
                team.userId = id;
                NetworkManager.Instance.playerTeam = team;
                NetworkManager.Instance.userId = id;
                SceneManager.LoadScene("Test_Hoon");
            }
        }
    }

    IEnumerator SignUp()
    {
        UserLoginInfo loginInfo = new UserLoginInfo();
        loginInfo.userId = id;
        loginInfo.userPwd = pw;
        string jsonLoginInfo = JsonUtility.ToJson(loginInfo);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm("http://43.202.59.52:8081/makeId", jsonLoginInfo))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonLoginInfo);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("SignUp complete!");
            }
        }
    }
}
