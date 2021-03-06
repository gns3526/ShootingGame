﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Pun;
using Photon.Realtime;


public class GameManager : MonoBehaviourPunCallbacks
{
    public bool isAndroid;

    [Header("Managers")]
    public static GameManager gm;

    public NetworkManager NM;
    public ObjectPooler OP;
    public OptionManager option;
    [SerializeField] GoogleSheetManager GSM;
    public Cards CM;
    [SerializeField] ReinForceManager RM;
    public JobManager jm;
    public DamageTextManager DTM;
    public PlayerColorManager colorManager;
    public ChallengeManager challengeManager;
    public PlayerState ps;

    [Header("GamePlayInfo")]
    public int stage;
    [SerializeField] int MaxStage;
    public bool isGameStart;
    public bool isPlaying;
    [SerializeField] Animator startAni;
    [SerializeField] Animator clearAni;
    public Animator fadeAni;

    [Header("MonsterSpawn")]
    [SerializeField] Transform[] enemySpawnPoint;
    [SerializeField] float nextSpawnDelay;
    [SerializeField] float curSpawnDelay;
    [SerializeField] List<Spawn> spawnList;
    [SerializeField] int spawnIndex;
    [SerializeField] bool spawnEnd;
    GameObject curSpawnEnemy;

    [Header("Score")]
    public int gameScore;
    public Text scoreText;
    [SerializeField] Image[] lifeImage;
    [SerializeField] Sprite[] lifeSprite;
    [SerializeField] Image[] boomImage;

    [SerializeField] Text clearScoreText;
    [SerializeField] Text clearLifeText;
    [SerializeField] Text clearTotalScore;

    [Header("Panels")]
    public GameObject gamePlayPanel;
    public GameObject gamePlayExpPanel;
    public Text getExpAmountText;
    public Text getGoldAmountText;

    [SerializeField] GameObject retryPanel;
    public GameObject mobileControlPanel;
    public GameObject deskTopControlPanel;
    public GameObject joyPadObject;
    [SerializeField] GameObject finalStageClearPanel;

    public GameObject codyPanel;
    public GameObject codyMainPanel;
    [SerializeField] GameObject codyBodyPanel;
    [SerializeField] GameObject codyParticlePanel;
    public GameObject codyIconPanel;
    [SerializeField] GameObject jobPanel;
    [SerializeField] GameObject reinForcePanel;

    [SerializeField] GameObject helpPanel;

    public GameObject abilityPanel;

    public GameObject loginPanel;
    public GameObject roomExpPanal;
    public GameObject lobbyExpPanel;
    public GameObject makeRoomPanel;
    [SerializeField] GameObject gameOverPanel;

    [SerializeField] GameObject leftGamePanel;
    [SerializeField] GameObject leftRoomPanel;

    [Header("PlayerInfo")]
    public GameObject playerInfoPanel;
    [SerializeField] Animator playerInfoAni;


    [Header("ControlPanel")]
    public GameObject normalShotBotton;
    public GameObject specialShotBotton_M;

    public GameObject border;

    public Text weaponBulletText_M;
    public Text weaponBulletText_D;
    public GameObject bulletMaxUi;
    public Image weaponShotButtonImage;

    [Header("CodyPanel")]
    public Image lobbyPlayer;
    public Sprite[] lobbyCodyDummy;
    public Sprite[] lobbyCodyMainDummy;
    public ParticleSystem[] lobbyParticleDummy;

    public CodySelectUpdate[] codySelectUpdate;



    [Header("Player")]
    [SerializeField] Transform playerPos;
    public GameObject myplayer;
    public Player myplayerScript;
    [SerializeField] float respawnCoolTIme;

    public bool canControll;

    public Player[] alivePlayers;
    public Player[] allPlayers;


    public int codyMainCode;
    public int codyBodyCode;
    public int codyParticleCode;

    public int playerLv;
    public float exp;
    public float maxExp;
    [SerializeField] Text playerLvText;
    [SerializeField] Text nickNameText;
    [SerializeField] Text expText;
    public Image playerIcon;
    [SerializeField] Image expImage;
    public Text goldAmountText;

    [SerializeField] Text playerLvText2;
    [SerializeField] Text nickNameText2;
    [SerializeField] Text expText2;
    [SerializeField] Image playerIcon2;
    [SerializeField] Image expImage2;

    [SerializeField] Text playerLvText3;
    [SerializeField] Text nickNameText3;
    [SerializeField] Text expText3;
    [SerializeField] Image playerIcon3;
    [SerializeField] Image expImage3;
    public Text goldAmountText3;

    [Header("PlayerStateInfo")]
    public int money;
    public int reinPoint;
    public int[] abilityCode;
    public int[] abilityValue;
    public int plainLv;

    [Header("Map")]
    public int mapCode;
    public Button selectMapButton;

    [SerializeField] GameObject mapPanel;
    [SerializeField] Animator mapInfoPanel;
    [SerializeField] Image mapSelectPanelImage;
    [SerializeField] Image mapThumnail;
    [SerializeField] Text mapNameText;
    [SerializeField] GameObject[] difficultStars;
    [SerializeField] Text mapNameinfoText;
    [SerializeField] Text mapCoinAmountinfoText;
    [SerializeField] Text mapExpAmountinfoText;

    [SerializeField] Text[] mapPointsText;
    [SerializeField] bool mapFocus;
    [SerializeField] RectTransform wordMap;
    [SerializeField] Vector3[] mapPos;
    [SerializeField] GameObject mapTouchBarrier;

    public Sprite[] mapThumnails;
    public string[] mapNames;
    [SerializeField] int[] mapDifficulty;
    public int[] mapCoinAmount;
    public int[] mapExpAmount;

    public int curMapCode;
    public Image roomMapThumnail;
    public Text roomMapName;

    [Header("Rader")]
    public GameObject raderOb;
    public RaderScript raderScript;

    [Header("Other")]
    public GameObject firstPetOb;

    public PhotonView pv;

    public bool stop;
    public float stopTime;

    public bool isGameEnd;
    public bool allBulletDelete;
    [SerializeField] bool setExpBarLerp;
    bool isLvUp;
    public bool expGIveOnce;
    float overExp;

    [SerializeField] bool once;
    bool stageEndOnce;


    private void Awake()
    {
        gm = this;
        if (SystemInfo.deviceType == DeviceType.Desktop) isAndroid = false;
        else if(SystemInfo.deviceType == DeviceType.Handheld) isAndroid = true;

        //isAndroid = true;

        
        if(isAndroid)
        Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);
        else if(!isAndroid)
        Screen.SetResolution(540, 960, false);

        spawnList = new List<Spawn>();

        for (int i = 0; i < mapPointsText.Length; i++)
            mapPointsText[i].text = mapNames[i];
    }

    public void SetExpPanel()
    {
        ExpPanelUpdate();
        setExpBarLerp = true;
    }
    void ExpPanelUpdate()
    {
        if (lobbyExpPanel.activeSelf)
        {
            expImage.fillAmount = 0;
            playerIcon.sprite = NM.icons[NM.playerIconCode];
            playerLvText.text = playerLv.ToString() + ".Lv";
            expText.text = exp.ToString() + "/" + maxExp.ToString();
            nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
        }
        if(roomExpPanal.activeSelf)
        {
            expImage2.fillAmount = 0;
            playerIcon2.sprite = NM.icons[NM.playerIconCode];
            playerLvText2.text = playerLv.ToString() + ".Lv";
            expText2.text = exp.ToString() + "/" + maxExp.ToString();
            nickNameText2.text = PhotonNetwork.LocalPlayer.NickName;
        }
        if (gamePlayPanel.activeSelf)
        {
            expImage3.fillAmount = 0;
            playerIcon3.sprite = NM.icons[NM.playerIconCode];
            playerLvText3.text = playerLv.ToString() + ".Lv";
            expText3.text = exp.ToString() + "/" + maxExp.ToString();
            nickNameText3.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    [PunRPC]
    void StageStart()
    {
        if (once)
        {
            StartCoroutine(PoolingLoading());
            scoreText.text = "Score:0";
            StartCoroutine(NM.SpawnDelay());
            nickNameText3.text = PhotonNetwork.NickName;

            OP.PrePoolInstantiate();

            once = false;
            isGameEnd = false;

            SoundManager.Stop("LobbyMusic_1");
            SoundManager.Stop("LobbyMusic_2");
            SoundManager.Play("LobbyMusic_3");
        }
        UpdateLifeIcon(ps.life);
        if(!myplayerScript.isDie)
        myplayerScript.godMode = false;

        stageEndOnce = true;

        isPlaying = true;

        pv.RPC("AlivePlayerSet", RpcTarget.All);
        pv.RPC("AllPlayerSet", RpcTarget.All);

        NM.roomPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        gamePlayExpPanel.SetActive(false);

        NM.chatPanel.SetActive(false);

        startAni.SetTrigger("Active");//스테이지Ui
        
        if(stage == MaxStage)
        {
            startAni.GetComponent<Text>().text = "Final" + stage.ToString() + "\nStart";
            clearAni.GetComponent<Text>().text = "Fianl" + stage.ToString() + "\nClear";
        }
        else
        {
            startAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nStart";
            clearAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nClear";
        }


        if (PhotonNetwork.IsMasterClient)
        {
            ReadSpawnFile();//적 스폰파일 읽기
        }

        isGameStart = true;

        fadeAni.SetTrigger("Out");//밝아지기

        CM.cardPanel.SetActive(false);
    }
    [PunRPC]
    public void StageEnd()
    {
        UpdateLifeIcon(ps.life);
        myplayerScript.godMode = true;
        if (!stageEndOnce) return;
        stageEndOnce = false;

        isPlaying = false;
        //ClearEnemys();

        clearAni.SetTrigger("Active");//클리어Ui

        //myplayer.transform.position = playerPos.position;//플래이어 위치 초기화

        stage++;//스테이지 증가

        if (stage > MaxStage)
        {
            FinalStageClear();
            challengeManager.ChallengeClear(0);
            challengeManager.ChallengeClear(1);
        }
        
        else
        {
            fadeAni.SetTrigger("In");//어두워 지기

            StartCoroutine(CardDelay());//카드고르기
        }
    }

    IEnumerator PoolingLoading()
    {
        canControll = false;
        GSM.loadingPanel.SetActive(true);
        yield return new WaitForSeconds(5);
        canControll = true;
        GSM.loadingPanel.SetActive(false);
    }

    IEnumerator CardDelay()
    {
        yield return new WaitForSeconds(3);
        NM.chatPanel.SetActive(true);
        CM.SelectCard();
    }
   

    void ReadSpawnFile()
    {
        spawnList.Clear();//초기화
        spawnIndex = 0;
        spawnEnd = false;

        //리스폰 파일 읽기
        TextAsset textFile = Resources.Load("EnemySpawnInfo" + "/" + "Map" + mapCode + "/" + "Stage" + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();//읽는거

            if(line == null)//밑부분이 비어있다면
                break;


            //리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);//쉼표를 기준으로 자른다
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
            
        }
        //텍스트 닫기
        stringReader.Close();

        nextSpawnDelay = spawnList[0].delay;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) PlayerInfoPanelMove();

        if (stopTime > 0)
        {
            stopTime -= Time.deltaTime;
            stop = true;
            NM.reconnectPanel.SetActive(true);
            if(stopTime < 0)
            {
                stop = false;
                NM.reconnectPanel.SetActive(false);
            }
        }

        if(!stop && isPlaying && !spawnEnd)
        curSpawnDelay += Time.deltaTime;

        if (mapFocus)
        {
            wordMap.transform.localPosition = Vector3.Lerp(wordMap.transform.localPosition, mapPos[mapCode], Time.deltaTime * 10);
            if (Mathf.Abs(wordMap.transform.localPosition.x - mapPos[mapCode].x) < 1 && Mathf.Abs(wordMap.transform.localPosition.y - mapPos[mapCode].y) < 1)
            {
                wordMap.transform.localPosition = mapPos[mapCode];
                mapFocus = false;
                mapTouchBarrier.SetActive(false);
            }
        }

        if (curSpawnDelay > nextSpawnDelay && PhotonNetwork.IsMasterClient)
        {

            SpawnEnemy();
            
            curSpawnDelay = 0;
        }
        //Player playerScript = player.GetComponent<Player>();
        
       

        if (setExpBarLerp)
        {
            if(lobbyExpPanel.activeSelf)
            {
                expImage.fillAmount = Mathf.Lerp(expImage.fillAmount, exp / maxExp, Time.deltaTime * 5);

                if (expImage.fillAmount >= (exp / maxExp) - 0.01f)//다찼다면
                {
                    expImage.fillAmount = exp / maxExp;
                    if (isLvUp)
                    {
                        playerLv++;
                        maxExp *= 1.1f;
                        maxExp = Mathf.Round(maxExp);
                        expImage.fillAmount = 0;
                        exp = overExp;
                        isLvUp = false;
                        ExpPanelUpdate();
                        GiveExp(mapExpAmount[mapCode]);
                    }
                    else
                        setExpBarLerp = false;
                }
            }
            if (roomExpPanal.activeSelf)
            {
                expImage2.fillAmount = Mathf.Lerp(expImage2.fillAmount, exp / maxExp, Time.deltaTime * 5);

                if (expImage2.fillAmount >= (exp / maxExp) - 0.01f)//다찼다면
                {
                    expImage2.fillAmount = exp / maxExp;
                    if (isLvUp)
                    {
                        playerLv++;
                        maxExp *= 1.1f;
                        maxExp = Mathf.Round(maxExp);
                        expImage2.fillAmount = 0;
                        exp = overExp;
                        isLvUp = false;
                        ExpPanelUpdate();
                        GiveExp(mapExpAmount[mapCode]);
                    }
                    else
                        setExpBarLerp = false;
                }
            }
            if (gamePlayPanel.activeSelf)
            {
                expImage3.fillAmount = Mathf.Lerp(expImage3.fillAmount, exp / maxExp, Time.deltaTime * 5);

                if (expImage3.fillAmount >= (exp / maxExp) - 0.01f)//다찼다면
                {
                    expImage3.fillAmount = exp / maxExp;
                    if (isLvUp)
                    {
                        playerLv++;
                        maxExp *= 1.1f;
                        maxExp = Mathf.Round(maxExp);
                        expImage3.fillAmount = 0;
                        exp = overExp;
                        isLvUp = false;
                        ExpPanelUpdate();
                        GiveExp(mapExpAmount[mapCode] / PhotonNetwork.CountOfPlayers);
                    }
                    else
                        setExpBarLerp = false;
                }
            }
        }
    }

    void SpawnEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            string enemyIndex = null;
            switch (spawnList[spawnIndex].type)
            {
                case "1":
                    enemyIndex = "Enemy1";
                    break;
                case "2":
                    enemyIndex = "Enemy2";
                    break;
                case "3":
                    enemyIndex = "Enemy3";
                    break;
                case "4":
                    enemyIndex = "Enemy4";
                    break;
                case "5":
                    enemyIndex = "Enemy5";
                    break;
                case "6":
                    enemyIndex = "Enemy6";
                    break;
                case "7":
                    enemyIndex = "Enemy7";
                    break;
                case "8":
                    enemyIndex = "Enemy8";
                    break;
                case "9":
                    enemyIndex = "Enemy9";
                    break;
                case "10":
                    enemyIndex = "Enemy10";
                    break;
                case "11":
                    enemyIndex = "Enemy11";
                    break;
                case "12":
                    enemyIndex = "Enemy12";
                    break;
                case "13":
                    enemyIndex = "Enemy13";
                    break;
                case "14":
                    enemyIndex = "Enemy14";
                    break;
                case "15":
                    enemyIndex = "Enemy15";
                    break;
                case "Boss1":
                    enemyIndex = "Boss1";
                    break;
                case "Boss2":
                    enemyIndex = "Boss2";
                    break;
                case "Boss3":
                    enemyIndex = "Boss3";
                    break;
            }

            int spawnPoint = spawnList[spawnIndex].point;
            curSpawnEnemy = OP.PoolInstantiate(enemyIndex, enemySpawnPoint[spawnPoint].transform.position, Quaternion.identity, -2, -1, 0, 0, false);
            

            if (spawnPoint == 5 || spawnPoint == 8)
            {
                curSpawnEnemy.transform.Rotate(Vector3.back * 90);
            }
            else if (spawnPoint == 6 || spawnPoint == 7)
            {
                curSpawnEnemy.transform.Rotate(Vector3.forward * 90);
            }
            else
            {

            }
        }
        //리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;//스폰다됨
            if(PhotonNetwork.IsMasterClient)
            curSpawnEnemy.GetComponent<EnemyBasicScript>().isLast = true;
            return;
        }

        //다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    public void UpdateBoomIcon(int boom)
    {
        for (int i = 0; i < 3; i++)//끄기
        {
            boomImage[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < boom; i++)
        {
            boomImage[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateLifeIcon(int life)
    {
        if (life == 1 || life == 0) border.SetActive(true);
        else border.SetActive(false);

        for (int i = 0; i < lifeImage.Length; i++)//끄기
        {
            lifeImage[i].sprite = lifeSprite[0]; ;
        }
        for (int i = 0; i < ps.maxLife; i++)
        {
            lifeImage[i].sprite = lifeSprite[1];
        }
        for (int i = 0; i < ps.life; i++)
        {
            lifeImage[i].sprite = lifeSprite[2];
        }
        NM.pv.RPC(nameof(NM.PlayerInfoUpdate), RpcTarget.All, NM.playerInfoGroupInt, NM.playerIconCode, jm.jobCode, ps.life);
    }

    //모바일
    public void Shot(bool a)
    {
        if(isAndroid && !myplayer.GetComponent<Player>().weaponFire)
        myplayer.GetComponent<Player>().isFire = a;
    }
    //모바일
    public void WeaponShot(bool a)
    {
        if (isAndroid && !myplayer.GetComponent<Player>().isFire)
            myplayer.GetComponent<Player>().weaponFire = a;
    }

    public void WeaponButtonUpdate()
    {
        bulletMaxUi.SetActive(false);
        myplayerScript.curBulletAmount = 0;
        if (myplayer.GetComponent<Player>().gotSpecialWeaponAbility)
        {
            if (isAndroid)
            {
                specialShotBotton_M.SetActive(true);

                weaponBulletText_M.enabled = true;
                weaponBulletText_M.text = myplayerScript.curBulletAmount.ToString() + "/" + myplayerScript.maxSpecialBullet.ToString(); ;
            }
            else if (!isAndroid)
            {
                weaponBulletText_D.gameObject.SetActive(true);
                weaponBulletText_D.text = myplayerScript.curBulletAmount.ToString() + "/" + myplayerScript.maxSpecialBullet.ToString(); ;
            }
        }
        else
        {
            if (isAndroid)
            {
                specialShotBotton_M.SetActive(false);

                weaponBulletText_M.enabled = false;
            }
            else if (!isAndroid)
            {
                weaponBulletText_D.gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator ReSpawnM()
    {
        yield return new WaitForSeconds(respawnCoolTIme);
        myplayer.GetComponent<Player>().canHit = true;
    }

    [PunRPC]
    void ReviveTeam(int Life)
    {
        if (myplayer.GetComponent<Player>().isDie)
        {
            myplayer.GetComponent<PhotonView>().RPC("PlayerIsAlive", RpcTarget.All);
            ps.life = Life;
            UpdateLifeIcon(ps.life);
            retryPanel.SetActive(false);

        }
        pv.RPC("AlivePlayerSet", RpcTarget.All);

    }

    [PunRPC]
    void Heal(int healAmount, int damageSkinCode)
    {
        if (jm.jobCode == 4) return;

        if (ps.life >= ps.maxLife) return;

        if (!myplayerScript.isDie)
        {
            ps.life += healAmount;
            UpdateLifeIcon(ps.life);
        }

        OP.DamagePoolInstantiate("DamageText", myplayerScript.transform.position, Quaternion.identity, 1, 2, damageSkinCode, true);
    }

    public void MakeExplosionEffect(Vector3 pos, string targetType)
    {
        GameObject explosion = OP.PoolInstantiate("Explosion", Vector3.up * 100, Quaternion.identity, -2, -1, 0, 0, false);
        Explosion explosionScript = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        //explosionScript.StartExplosion(targetType);
    }

    public void LobbyPlayerRework()
    {
        lobbyPlayer.sprite = lobbyCodyMainDummy[codyMainCode];
        lobbyPlayer.transform.GetChild(0).GetComponent<Image>().sprite = lobbyCodyDummy[codyBodyCode];

        lobbyPlayer.color = new Color(colorManager.playerMainColors[0] / 255f, colorManager.playerMainColors[1] / 255f, colorManager.playerMainColors[2] / 255f, 1);


        lobbyParticleDummy[codyParticleCode].Play();
    }

    bool playerInfoPanelActive;
    public void PlayerInfoPanelMove()
    {
        playerInfoPanelActive = !playerInfoPanelActive;

        playerInfoAni.SetBool("Active", playerInfoPanelActive);
    }

    public void LeftRoomAskOpenOrClose(bool a)
    {
        if (!isGameStart)
        {
            GoToLobby();


        }
        else
        {
            leftRoomPanel.SetActive(a);

        }
        SoundManager.Play("Btn_3");
    }

    

    public void HelpPanel(bool a)
    {
        helpPanel.SetActive(a);

        SoundManager.Play("Btn_3");
    }

    public void IconPanelOpenOrClose(bool a)
    {
        codyIconPanel.SetActive(a);

        codySelectUpdate[3].Select();

        SoundManager.Play("Btn_2");
    }

    

    public void CodyOpenOrClose(bool a)
    {
        codyPanel.SetActive(a);

        codySelectUpdate[1].Select();

        SoundManager.Play("Btn_2");
    }

    public void CodyMainOpenOrClose(bool a)
    {
        codyMainPanel.SetActive(a);

        codySelectUpdate[0].Select();

        SoundManager.Play("Btn_2");
    }

    public void JobPanelOpenOrClose(bool a)
    {
        jobPanel.SetActive(a);

        //codySelectUpdate[4].Select();
        if(!a)
        SoundManager.Play("Btn_2");
    }

    public void ReinForceOpenOrClose(bool a)
    {
        reinForcePanel.SetActive(a);

        RM.LobbyReinRework();
        RM.ReinForceRework();

        SoundManager.Play("Btn_2");
    }

    public void LeftGameOpenOrClose(bool a)
    {
        leftGamePanel.SetActive(a);

        SoundManager.Play("Btn_2");
    }
    public void LeftGame() => Application.Quit();
   
    public void FinalStageClear()
    {
        isGameEnd = true;
        finalStageClearPanel.SetActive(true);
        Player myplayerScript = myplayer.GetComponent<Player>();

        clearScoreText.text = gameScore.ToString();
        clearLifeText.text = (ps.life * 1000).ToString();
        clearTotalScore.text = (gameScore + (ps.life * 1000)).ToString();

        gamePlayExpPanel.SetActive(true);
        expGIveOnce = true;
        StartCoroutine(ExpGiveDelay(mapExpAmount[mapCode] * 3));
        StartCoroutine(GiveGold(mapExpAmount[mapCode] * 3));
    }

    public IEnumerator ExpGiveDelay(int ExpAmount)
    {
        yield return new WaitForSeconds(2);
        Debug.Log("0");
        GiveExp(ExpAmount);
    }
    float finalExpAmount;
    void GiveExp(float ExpAmount)
    {
        Debug.Log("1");

        if(expGIveOnce)
        {

            if (ps.expAmountPer == 0)
            {
                getExpAmountText.text = "+" + ExpAmount.ToString() + "Exp";
                finalExpAmount = ExpAmount;
            }
                
            else
            {
                Debug.Log("3");
                finalExpAmount = Mathf.Ceil(ExpAmount + (ExpAmount * (ps.expAmountPer / 100)));
                float bonusExpAmount = finalExpAmount - ExpAmount;
                getExpAmountText.text = "|어빌리티|" + "+" + finalExpAmount.ToString() + "(+" + bonusExpAmount + ")" + "Exp";
            }
            Debug.Log("4");
            expGIveOnce = false;
            exp += finalExpAmount;
        }
        if (exp >= maxExp)
        {

            overExp = exp - maxExp;
            exp = maxExp;

            isLvUp = true;
            setExpBarLerp = true;
            return;
        }

        SetExpPanel();
    }
    float finalGoldAmount;
    public IEnumerator GiveGold(int GoldAmount)
    {
        yield return new WaitForSeconds(2);
        if (ps.goldAmountPer == 0)
        {
            getGoldAmountText.text = "+" + GoldAmount.ToString() + "Gold";
            finalGoldAmount = GoldAmount;
        }
            
        else
        {
            finalGoldAmount = Mathf.Ceil(GoldAmount + (GoldAmount * (ps.goldAmountPer / 100)));

            float bonusGoldAmount = finalGoldAmount - GoldAmount;
            getGoldAmountText.text = "|어빌리티|" + "+" + finalGoldAmount.ToString() + "(+" + bonusGoldAmount + ")" + "Gold";
        }
        money += (int)finalGoldAmount;
        goldAmountText3.text = money.ToString();
        if(myplayerScript.isDie)
        StartCoroutine(DieReadyDelay());
    }

    IEnumerator DieReadyDelay()
    {
        yield return new WaitForSeconds(1f);
        CM.isReady = true;
        CM.pv.RPC("ReadyAmountReset", RpcTarget.All);
    }

    public void GoToLobby()
    {
        stage = 1;
        isGameStart = false;
        isPlaying = false;
        curSpawnDelay = 0;
        spawnIndex = 0;
        spawnEnd = false;

        once = true;
        // OP.a = 0;

        OP.EffectDestroyAll();

        if(isAndroid) mobileControlPanel.SetActive(false);
        else if(!isAndroid) deskTopControlPanel.SetActive(false);

        CM.cardPanel.SetActive(false);
        retryPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        
        finalStageClearPanel.SetActive(false);

        leftRoomPanel.SetActive(false);

        NM.lobbyPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();

        SoundManager.Play("Btn_3");
        SoundManager.Stop("LobbyMusic_2");
        SoundManager.Play("LobbyMusic_1");
    }

    public void PlayerDie()
    {
        retryPanel.SetActive(true);
    }
    public void GameOver()
    {
        retryPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        spawnEnd = true;
        isPlaying = false;
    }
    [PunRPC]
    void AllPlayerSet()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<Player>().isDie && players[i] != null)
            {
                allPlayers[i] = players[i].GetComponent<Player>();
            }
        }
    }
    [PunRPC]
    public void AlivePlayerSet()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < 4; i++)
        {
            alivePlayers[i] = null;
        }
        int a;
        a = 0;

        for (int i = 0; i < players.Length; i++)
        {

            if (!players[i].GetComponent<Player>().isDie)
            {
                alivePlayers[a] = players[i].GetComponent<Player>();
                a++;
            }
        }
        if (alivePlayers[0] == null)
        {
            GameOver();
            challengeManager.ChallengeClear(0);
        }
        else if(alivePlayers[0] && myplayer.GetComponent<Player>().isDie)
        {
            PlayerDie();
        }
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void CodyPanelClick(int a)
    {
        if(a == 0)
        {
            codyParticlePanel.SetActive(false);
            DTM.damageSkinChangePanel.SetActive(false);
            codyBodyPanel.SetActive(true);

            codySelectUpdate[1].Select();
        }

        else if (a == 1)
        {
            codyBodyPanel.SetActive(false);
            DTM.damageSkinChangePanel.SetActive(false);
            codyParticlePanel.SetActive(true);

            codySelectUpdate[2].Select();
        }

        else if (a == 2)
        {
            codyBodyPanel.SetActive(false);
            codyParticlePanel.SetActive(false);
            DTM.damageSkinChangePanel.SetActive(true);

            codySelectUpdate[5].Select();
        }

        SoundManager.Play("Btn_2");
    }





    public void OpenMap(bool a)
    {
        wordMap.transform.localPosition = new Vector3(0, 0, 0);
        mapPanel.SetActive(a);
        mapInfoPanel.SetBool("On", false);

        SoundManager.Play("Btn_2");
    }

    public void ClickMapPoint(int code)
    {
        mapCode = code;
        mapTouchBarrier.SetActive(true);
        mapFocus = true;

        mapNameinfoText.text = mapNames[code];
        mapSelectPanelImage.sprite = mapThumnails[code];
        mapCoinAmountinfoText.text = "라운드당 코인:" + mapCoinAmount[code].ToString();
        mapExpAmountinfoText.text = "라운드당 경험치:" + mapExpAmount[code].ToString();
        for (int i = 0; i < difficultStars.Length; i++)
        {
            difficultStars[i].SetActive(false);
            if (mapDifficulty[code] >= i)
                difficultStars[i].SetActive(true);
        }
        mapInfoPanel.SetBool("On",true);

        SoundManager.Play("Btn_3");
    }
    public void MapInfoClose()
    {
        mapInfoPanel.SetBool("On", false);

        SoundManager.Play("Btn_3");
    }
    public void SelectMapComplete()
    {
        mapInfoPanel.SetBool("On", false);
        mapThumnail.sprite = mapThumnails[mapCode];
        mapNameText.text = mapNames[mapCode];
        mapPanel.SetActive(false);

        SoundManager.Play("Btn_1");
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            stream.SendNext(alivePlayers);
        }
        else
        {
            alivePlayers = (Player[])stream.ReceiveNext();
        }
    }
}
