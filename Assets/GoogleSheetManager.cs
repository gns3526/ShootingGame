using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value, makeNick, numOfPlayer, type;
}




public class GoogleSheetManager : MonoBehaviour
{
    // Start is called before the first frame update
    const string URL = "https://script.google.com/macros/s/AKfycbz57OWQfuLTrvT4mvNBEs5WoEQMyFgUZbCJ8QGWQLU-g2XppIeB9f584A/exec";
    [SerializeField] InputField idInput, PassInput;
    public GoogleData GD;
    string id, pass;
    [SerializeField] Text debuggingText;
    [Header("Managers")]
    [SerializeField] GameManager GM;
    [SerializeField] NetworkManager NM;

    [Header("Panel")]
    public GameObject loadingPanel;
    [SerializeField] GameObject registerPanel;

    [Header("Register")]
    bool once;

    [SerializeField] bool idCheck;
    [SerializeField] bool passwordCheck;
    [SerializeField] bool passwordReCheck;

    [SerializeField] InputField regIdInput;
    [SerializeField] InputField regPassInput;
    [SerializeField] InputField regPassReInput;

    [SerializeField] int cantIdReason;

    [SerializeField] Text idText;
    [SerializeField] Text passwordText;
    [SerializeField] Text passwordReText;

    [SerializeField] Button registerCompleteBtn;
    [SerializeField] Button idCheckBtn;

    [SerializeField] Image isCurrentPass;
    [SerializeField] Sprite[] currentSprites;
    
    [Header("Other")]
    public bool canMakeNick;
    [SerializeField] int playernum;
    public string playerNickName;
    [SerializeField] string playercolor;

    [SerializeField] bool movePlayerinfoComplete;
    [SerializeField] bool makeNickComplete;

    private void Update()
    {
        if(movePlayerinfoComplete && makeNickComplete)
        {
            movePlayerinfoComplete = false;
            makeNickComplete = false;

            NM.Connect();
        }
    }
    public bool CheckingSpecialText(string txt)
    {
        string str = @"[~!@\#$%^&*\(,.)\=+|\\/:;?""<>']";
        System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(str);
        return rex.IsMatch(txt);
    }
    public void CheckSpecialText()
    {
        if (!CheckingSpecialText(regIdInput.text))
        {
            idCheckBtn.interactable = true;
            idText.text = "";
        }

        else
        {
            idCheckBtn.interactable = false;
            idText.text = "특수문자가 포함되어 있습니다.";
        }

    }

    public void RegisterPanelOn(bool On)
    {
        if(On)
        registerPanel.SetActive(true);
        else
        {
            regIdInput.text = "";
            regPassInput.text = "";
            regPassReInput.text = "";
            registerPanel.SetActive(false);
        }
    }
    
    public void CanUseIdClick()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }

    public void CanRegist()
    {
        if (idCheck && passwordCheck && passwordReCheck) registerCompleteBtn.interactable = true;
        else registerCompleteBtn.interactable = false;
    }

    public void RegisterTextsUpdate()
    {
        if (idCheck)
        {
            idText.text = "사용할 수 있는 아이디 입니다.";
        }
        else
            idText.text = "중복된 아이디입니다.";

        if (passwordCheck) passwordText.text = "사용 가능한 비밀번호 입니다.";
        else passwordText.text = "특수문자가 포함되어 있습니다";

        if (passwordReCheck)
        {
            passwordReText.text = "올바른 비밀번호입니다.";
            isCurrentPass.sprite = currentSprites[0];
        }
        else
        {
            passwordReText.text = "틀린 비밀번호 입니다.";
            isCurrentPass.sprite = currentSprites[1];
        }
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
        
        loadingPanel.SetActive(true);
        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        //form.AddField("playerNum", playernum);
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));
    }
    public void CheckNickname()
    {

        WWWForm form = new WWWForm();
        form.AddField("order", "NicknameCheck");
        form.AddField("playerNum", playernum);
        form.AddField("index", 3);

        StartCoroutine(Post(form));
    }
    public void CheckNicknameSame()
    {

        WWWForm form = new WWWForm();
        form.AddField("order", "NicknameSameCheck");
        form.AddField("playerNum", playernum);
        form.AddField("playerNickName", NM.nickNameInput.text);

        StartCoroutine(Post(form));
    }
    public void SetValue(int index, string info)//upload info to spread seet
    {
        WWWForm form = new WWWForm();


        form.AddField("order", "setValue");
        form.AddField("playerNum", playernum);
        form.AddField("index", index);
        form.AddField("value", info);
               

        StartCoroutine(Post(form));
    }
    public void GetValue(int index)//bring info of spread seet
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "getValue");
        form.AddField("playerNum", playernum);
        form.AddField("index", index);

        StartCoroutine(Post(form));
    }

    public void MakeNickOnClick()
    {
        loadingPanel.SetActive(true);
        CheckNicknameSame();
    }

    public void SaveLvInfo()
    {
        WWWForm form = new WWWForm();

        form.AddField("order", "setLv");
        form.AddField("playerNum", playernum);
        form.AddField("playerLv", GM.playerLv + "." + GM.exp + "." + GM.maxExp);

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
            loadingPanel.SetActive(false);
            return;
        }

        print(GD.order + "을 실행했습니다" + GD.msg);
        debuggingText.text = (GD.order + "을 실행했습니다" + GD.msg);

        if (GD.order == "getValue")
        {
            if(GD.type == "3")
            {
                playerNickName = GD.value;
                NM.nickNameInput.text = playerNickName;

            }
            else if(GD.type == "5")
            {
                playercolor = GD.value;
                string[] result = playercolor.Split(new string[] { "," }, System.StringSplitOptions.None);
                for (int i = 0; i < result.Length; i++)
                {
                    GM.playerColors[i] = float.Parse(result[i]);
                }
                GetValue(6);
            }
            else if(GD.type == "6")
            {
                string[] result = GD.value.Split(new string[] { "." }, System.StringSplitOptions.None);
                GM.playerLv = int.Parse(result[0]);
                GM.exp = float.Parse(result[1]);
                GM.maxExp = float.Parse(result[2]);
                movePlayerinfoComplete = true;
            }
        }
        if(GD.order == "NicknameCheck")
        {
            if (GD.makeNick == "yes") canMakeNick = true;
            else canMakeNick = false;

            if (!canMakeNick)//이미닉이있다
            {
                GetValue(3);
                makeNickComplete = true;
                print("dkslslslqqqqqq");
                return;
            }
            //없다면 닉네임창에 놔둠;
            loadingPanel.SetActive(false);
        }
        if(GD.order == "login")
        {

            playernum = int.Parse(GD.numOfPlayer);
            GM.loginPanel.SetActive(false);
            CheckNickname();
            GetValue(5);

        }
        if (GD.order == "NicknameSameCheck")
        {
            if (GD.result == "OK") makeNickComplete = true;
            else print("닉네임 중복됨");
        }
        

    }

    private void OnApplicationQuit()
    {
        ColorSave();
        SaveLvInfo();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            ColorSave();
            SaveLvInfo();
        }

    }
    public void ColorSave()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "logout");
        form.AddField("playerNum", playernum);
        form.AddField("playerColor", GM.playerColors[0] + "," + GM.playerColors[1] + "," + GM.playerColors[2]);

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
