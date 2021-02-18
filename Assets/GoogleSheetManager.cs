using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value, makeNick;
}




public class GoogleSheetManager : MonoBehaviour
{
    // Start is called before the first frame update
    const string URL = "https://script.google.com/macros/s/AKfycbz57OWQfuLTrvT4mvNBEs5WoEQMyFgUZbCJ8QGWQLU-g2XppIeB9f584A/exec";
    [SerializeField] InputField idInput, PassInput;
    public GoogleData GD;
    string id, pass;
    [SerializeField] Text debuggingText;


    public int value;
    public string info;


    bool SetIdPass()
    {
        id = idInput.text.Trim();//delete between of id words
        pass = PassInput.text.Trim();

        if (id == "" || pass == "") return false;
        else return true;
    }
    public void Register()
    {
        if (!SetIdPass())
        {
            print("비어있다");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }

    public void Login()
    {
        if (!SetIdPass())
        {
            print("비어있다");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));

    }

    public void CheckNickname()
    {

        WWWForm form = new WWWForm();
        form.AddField("order", "NicknameCheck");
        form.AddField("index", 3);

        StartCoroutine(Post(form));


    }
    public void SetValue(int index)//upload info to spread seet
    {
        WWWForm form = new WWWForm();


        form.AddField("order", "setValue");
        form.AddField("index", index);
        form.AddField("value", info);
               

        StartCoroutine(Post(form));
    }
    public void GetValue(int index)//bring info of spread seet
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");
        form.AddField("index", index);

        StartCoroutine(Post(form));
    }

    void Debug(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        GD = JsonUtility.FromJson<GoogleData>(json);

        if(GD.result == "ERROR")
        {
            print(GD.order + "을 실행할수 없습니다" + GD.msg);
            debuggingText.text = (GD.order + "을 실행할수 없습니다" + GD.msg);
            return;
        }

        print(GD.order + "을 실행했습니다" + GD.msg);
        debuggingText.text = (GD.order + "을 실행했습니다" + GD.msg);

        if (GD.order == "getValue")
        {
            value = int.Parse(GD.value);
        }

    }

    private void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }
    IEnumerator Post(WWWForm form)//스프레드시트 실행
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();//wait for connect

            if (www.isDone) Debug(www.downloadHandler.text);
            else print("웹의 응답이 없습니다");
        }
    }
}
