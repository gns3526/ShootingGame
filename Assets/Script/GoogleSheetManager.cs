using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg, value, makeNick, numOfPlayer, type;
}




public class GoogleSheetManager : MonoBehaviour
{
                        
    const string URL = "https://script.google.com/macros/s/AKfycbxaC1XjUAVUiawK-XB90Afy3yP3oGoixy-0XAmmzF6KZrVarwNhdcPOPSJLEKnkSU9z/exec";
    //const string URL = "https://script.google.com/macros/s/AKfycbxKbnF64Cg1qZtA4h8YbI7cDuY2BEXWA7evJuRZQhr7-Ym5ap9NsHAb49iwXNhkFT9P/exec"; // 테스트
    [SerializeField] InputField idInput, PassInput;
    public GoogleData GD;
    string id, pass;
    [SerializeField] Text debuggingText;
    [Header("Managers")]
    [SerializeField] GameManager GM;
    [SerializeField] NetworkManager NM;
    [SerializeField] AbilityManager AM;
    [SerializeField] ReinForceManager RM;
    [SerializeField] JobManager JM;
    [SerializeField] PlayerColorManager colorManager;
    [SerializeField] ChallengeManager challengeManager;

    [Header("Panel")]
    public GameObject loadingPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject logOutAskPanel;

    [Header("Register")]
    bool once;

    [SerializeField] bool idCheck;
    [SerializeField] bool passwordCheck;
    [SerializeField] bool passwordReCheck;

    [SerializeField] InputField regIdInput;
    [SerializeField] InputField regPassInput;
    [SerializeField] InputField regPassReInput;

    [SerializeField] Text idText;
    [SerializeField] Text passwordText;
    [SerializeField] Text passwordReText;

    [SerializeField] Button registerCompleteBtn;
    [SerializeField] Button idCheckBtn;

    [SerializeField] Image isCurrentId;
    [SerializeField] Image isCurrentPass;
    [SerializeField] Image isCurrentPassRe;
    [SerializeField] Sprite[] currentSprites;
    
    [Header("Other")]
    public bool canMakeNick;
    [SerializeField] int playernum;
    public string playerNickName;
    [SerializeField] string playercolor;

    [SerializeField] bool movePlayerinfoComplete;
    [SerializeField] bool makeNickComplete;
    [SerializeField] bool nickInputComplete;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            movePlayerinfoComplete = true;
            makeNickComplete = true;
            nickInputComplete = true;

            PhotonNetwork.OfflineMode = true;

            GM.loginPanel.SetActive(false);

            NM.OnJoinedLobby();
        }
    }

    void CompleteAllCheck()
    {
        if (movePlayerinfoComplete && makeNickComplete && nickInputComplete)
        {
            

            movePlayerinfoComplete = false;
            makeNickComplete = false;
            nickInputComplete = false;

            NM.Connect();
        }
    }

    public bool CheckingSpecialText(string txt)
    {
        string str = @"[~!@\#$%^&*\(,.)\=+|\\/:;?""<>']";
        System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(str);
        return rex.IsMatch(txt);
    }
    public void CheckWrongIdText()
    {
        idCheck = false;
        isCurrentId.sprite = currentSprites[1];
        if (!CheckingSpecialText(regIdInput.text) && regIdInput.text != "")
        {
            idCheckBtn.interactable = true;
            idText.text = "Corrent ID.";
        }
        else
        {
            idCheckBtn.interactable = false;
            idText.text = "Contains special characters.";
        }
    }
    public void CheckWrongPassText()
    {
        if (!CheckingSpecialText(regPassInput.text) && regPassInput.text != "")
        {
            passwordCheck = true;
            isCurrentPass.sprite = currentSprites[0];
            passwordText.text = "Corrent Password";
        }
        else
        {
            passwordCheck = false;
            isCurrentPass.sprite = currentSprites[1];
            passwordText.text = "Contains special characters.";
        }
    }
    public void CheckSamePassReText()
    {
        if(regPassReInput.text == regPassInput.text && regPassReInput.text != "")
        {
            passwordReCheck = true;
            passwordReText.text = "Password match.";
            isCurrentPassRe.sprite = currentSprites[0];
        }
        else
        {
            passwordReCheck = false;
            passwordReText.text = "Password mismatch.";
            isCurrentPassRe.sprite = currentSprites[1];
        }
    }

    bool isShow;
    [SerializeField] GameObject showImage;
    public void LoginPasswordShow()
    {
        isShow = !isShow;

        if (isShow)
        {
            PassInput.contentType = InputField.ContentType.Standard;
            PassInput.enabled = false;
            showImage.SetActive(true);
        }
        else
        {
            PassInput.contentType = InputField.ContentType.Password;
            PassInput.enabled = false;
            showImage.SetActive(false);
        }
        PassInput.enabled = true;

        SoundManager.Play("Btn_2");
    }

    public void RegisterPanelOn(bool On)
    {
        idCheck = false;
        passwordCheck = false;
        passwordReCheck = false;

        isCurrentId.sprite = currentSprites[1];
        isCurrentPass.sprite = currentSprites[1];
        isCurrentPassRe.sprite = currentSprites[1];

        idCheckBtn.interactable = false;
        if (On)
        registerPanel.SetActive(true);
        else
        {
            regIdInput.text = "";
            regPassInput.text = "";
            regPassReInput.text = "";
            idText.text = "";
            passwordText.text = "";
            passwordReText.text = "";
            registerPanel.SetActive(false);
        }

        SoundManager.Play("Btn_2");
    }

    public void CanRegist()
    {
        if (idCheck && passwordCheck && passwordReCheck) registerCompleteBtn.interactable = true;
        else registerCompleteBtn.interactable = false;
    }


    public void CheckSameId()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "checkSameId");
        form.AddField("id", regIdInput.text);
        regIdInput.interactable = false;

        StartCoroutine(Post(form));

        SoundManager.Play("Btn_2");
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
        if (idCheck && passwordCheck && passwordReCheck)
        {
            WWWForm form = new WWWForm();
            form.AddField("order", "register");
            form.AddField("id", regIdInput.text);
            form.AddField("pass", regPassInput.text);

            StartCoroutine(Post(form));
        }

        SoundManager.Play("Btn_2");
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
        form.AddField("id", id);
        form.AddField("pass", pass);

        StartCoroutine(Post(form));

        SoundManager.Play("Btn_2");
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

        SoundManager.Play("Btn_2");
    }

    public void SaveLvInfo()
    {
        WWWForm form = new WWWForm();

        form.AddField("order", "setLv");
        form.AddField("playerNum", playernum);
        form.AddField("playerLv", NM.playerIconCode + "." + GM.playerLv + "." + GM.exp + "." + GM.maxExp + "." + GM.money + "." + GM.reinPoint + "." + GM.codyMainCode + "." + GM.codyBodyCode + "." +
            GM.codyParticleCode + "." + GM.abilityCode[0] + "." + GM.abilityCode[1] + "." + GM.abilityCode[2] + "." +
            GM.abilityValue[0] + "." + GM.abilityValue[1] + "." + GM.abilityValue[2] + "." + AM.abilityGrade[0] + "." + AM.abilityGrade[1] + "." + AM.abilityGrade[2] + "." + GM.plainLv + "." + 
            RM.upgradeInfo[0] + "." + RM.upgradeInfo[1] + "." + RM.upgradeInfo[2] + "." + RM.upgradeInfo[3] + "." + RM.upgradeInfo[4] + "." + RM.upgradeInfo[5] + "." + JM.jobCode + "." +
            challengeManager.challenge[0] + "." + challengeManager.challenge[1] + "." + challengeManager.challenge[2] + "." + challengeManager.challenge[3]);
        
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

        if(GD.order == "register")
        {
            registerPanel.SetActive(false);
        }

        if (GD.order == "getValue")
        {
            if(GD.type == "3")
            {
                playerNickName = GD.value;
                NM.nickNameInput.text = GD.value;
                nickInputComplete = true;
                CompleteAllCheck();
            }
            else if(GD.type == "5")
            {
                playercolor = GD.value;
                string[] result = playercolor.Split(new string[] { "." }, System.StringSplitOptions.None);
                colorManager.playerMainColors[0] = int.Parse(result[0]);
                colorManager.playerMainColors[1] = int.Parse(result[1]);
                colorManager.playerMainColors[2] = int.Parse(result[2]);
                colorManager.playerBoosterColors[0] = int.Parse(result[3]);
                colorManager.playerBoosterColors[1] = int.Parse(result[4]);
                colorManager.playerBoosterColors[2] = int.Parse(result[5]);
                GetValue(6);
            }
            else if(GD.type == "6")
            {
                string[] result = GD.value.Split(new string[] { "." }, System.StringSplitOptions.None);

                NM.playerIconCode = int.Parse(result[0]);

                GM.playerLv = int.Parse(result[1]);
                GM.exp = float.Parse(result[2]);
                GM.maxExp = float.Parse(result[3]);

                GM.money = int.Parse(result[4]);
                GM.reinPoint = int.Parse(result[5]);

                GM.codyMainCode = int.Parse(result[6]);
                GM.codyBodyCode = int.Parse(result[7]);
                GM.codyParticleCode = int.Parse(result[8]);

                GM.abilityCode[0] = int.Parse(result[9]);
                GM.abilityCode[1] = int.Parse(result[10]);
                GM.abilityCode[2] = int.Parse(result[11]);

                GM.abilityValue[0] = int.Parse(result[12]);
                GM.abilityValue[1] = int.Parse(result[13]);
                GM.abilityValue[2] = int.Parse(result[14]);

                AM.abilityGrade[0] = int.Parse(result[15]);
                AM.abilityGrade[1] = int.Parse(result[16]);
                AM.abilityGrade[2] = int.Parse(result[17]);

                GM.plainLv = int.Parse(result[18]);

                RM.upgradeInfo[0] = int.Parse(result[19]);
                RM.upgradeInfo[1] = int.Parse(result[20]);
                RM.upgradeInfo[2] = int.Parse(result[21]);
                RM.upgradeInfo[3] = int.Parse(result[22]);
                RM.upgradeInfo[4] = int.Parse(result[23]);
                RM.upgradeInfo[5] = int.Parse(result[24]);

                JM.jobCode = int.Parse(result[25]);
                int index = 26;
                for (int i = 0; i < challengeManager.challenge.Length; i++)
                {
                    if (result[index] == "True")
                        challengeManager.challenge[i] = true;
                    else
                        challengeManager.challenge[i] = false;
                    index++;
                }

                NM.loginPlayerIconImage.sprite = NM.icons[NM.playerIconCode];
                movePlayerinfoComplete = true;
                CompleteAllCheck();
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
                CompleteAllCheck();
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
            if (GD.result == "OK")
            {
                makeNickComplete = true;
                nickInputComplete = true;
                CompleteAllCheck();
            }
            else print("닉네임 중복됨");
        }
        if(GD.order == "checkSameId")
        {
            regIdInput.interactable = true;
            if (GD.result == "OK")
            {
                idCheck = true;
                isCurrentId.sprite = currentSprites[0];
                idText.text = "Username is available.";
            }
            else
            {
                idCheck = false;
                isCurrentId.sprite = currentSprites[1];
                idText.text = "This is a duplicate ID.";
            }
        }

    }

    public void LogOutOpenOrClose(bool a)
    {
        logOutAskPanel.SetActive(a);
        SoundManager.Play("Btn_2");
    }

    public void LogOut()
    {
        logOutAskPanel.SetActive(false);
        NM.connectPanel.SetActive(true);
        GM.loginPanel.SetActive(true);

        for (int i = 0; i < GM.lobbyParticleDummy.Length; i++)
            GM.lobbyParticleDummy[i].Stop();

        NM.DisConnect();

        loadingPanel.SetActive(true);
        idInput.text = "";
        PassInput.text = "";
        ColorSave();
        SaveLvInfo();
        debuggingText.text = "Logout complete";
        loadingPanel.SetActive(false);

        SoundManager.Play("Btn_2");
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
        form.AddField("playerColor", colorManager.playerMainColors[0] + "." + colorManager.playerMainColors[1] + "." + colorManager.playerMainColors[2] + "." + colorManager.playerBoosterColors[0] + "." + colorManager.playerBoosterColors[1] + "." + colorManager.playerBoosterColors[2]);

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
