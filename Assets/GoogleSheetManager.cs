using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
    // Start is called before the first frame update
    const string URL = "https://script.google.com/macros/s/AKfycbz57OWQfuLTrvT4mvNBEs5WoEQMyFgUZbCJ8QGWQLU-g2XppIeB9f584A/exec";
    [SerializeField] InputField idInput, PassInput;
    string id, pass;

    [SerializeField] int value;
    IEnumerator Start()
    {
        WWWForm form = new WWWForm();//데이터를 가져올때 도와줌
        form.AddField("value", "값");

        UnityWebRequest www = UnityWebRequest.Post(URL,form);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print(data);
    }

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

    private void OnApplicationQuit()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");

        StartCoroutine(Post(form));
    }

    public void SetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "setValue");
        form.AddField("value", value);
    }
    public void GetValue()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");

        StartCoroutine(Post(form));
    }

    //20분
    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();//wait for connect

            if (www.isDone) print(www.downloadHandler.text);
            else print("웹의 응답이 없습니다");
        }
    }
}
