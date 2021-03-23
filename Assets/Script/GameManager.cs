using System.Collections;
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
    [SerializeField] NetworkManager NM;
    [SerializeField] ObjectPooler OP;
    [SerializeField] Cards CM;

    [Header("GamePlayInfo")]
    [SerializeField] int stage;
    [SerializeField] int MaxStage;
    public bool isGameStart;
    public bool isPlaying;
    [SerializeField] Animator startAni;
    [SerializeField] Animator clearAni;
    [SerializeField] Animator fadeAni;

    [Header("MonsterSpawn")]
    [SerializeField] Transform[] enemySpawnPoint;
    [SerializeField] float nextSpawnDelay;
    [SerializeField] float curSpawnDelay;
    [SerializeField] List<Spawn> spawnList;
    [SerializeField] int spawnIndex;
    [SerializeField] bool spawnEnd;
    GameObject curSpawnEnemy;

    [Header("Score")]
    [SerializeField] Text scoreText;
    [SerializeField] Image[] lifeImage;
    [SerializeField] Image[] boomImage;

    [SerializeField] Text clearScoreText;
    [SerializeField] Text clearLifeText;
    [SerializeField] Text clearTotalScore;

    [Header("Panels")]
    public GameObject gamePlayPanel;
    [SerializeField] GameObject gamePlayExpPanel;
    [SerializeField] Text getExpAmountText;
    [SerializeField] Text getGoldAmountText;
    

    [SerializeField] GameObject retryPanel;
    public GameObject controlPanel;
    [SerializeField] GameObject finalStageClearPanel;

    [SerializeField] GameObject codyPanel;
    [SerializeField] GameObject codyBodyPanel;
    [SerializeField] GameObject codyParticlePanel;
    [SerializeField] GameObject premiumColorPanel;

    [SerializeField] GameObject abilityPanel;

    public GameObject loginPanel;
    public GameObject roomExpPanal;
    public GameObject lobbyExpPanel;
    [SerializeField] GameObject gameOverPanel;

    [Header("Cards")]
    [SerializeField] List<GameObject> cards;
    [SerializeField] List<GameObject> cardsSave;

    [SerializeField] List<GameObject> rare;
    [SerializeField] List<GameObject> epic;
    [SerializeField] List<GameObject> unique;
    [SerializeField] List<GameObject> legendary;

    List<GameObject> rareSave;
    List<GameObject> epicSave;
    List<GameObject> uniqueSave;
    List<GameObject> legendarySave;

    [SerializeField] GameObject cardPanel;

    [SerializeField] int rarePer;
    [SerializeField] int epicPer;
    [SerializeField] int uniquePer;
    [SerializeField] int legendaryPer;

    [Header("ControlPanel")]
    public GameObject normalShotBotton;
    public GameObject specialShotBotton;

    public Text weaponBulletText;
    public Image weaponShotButtonImage;

    [Header("ColorPanel")]
    [SerializeField] Slider colorRSlider;
    [SerializeField] Slider colorGSlider;
    [SerializeField] Slider colorBSlider;
    [SerializeField] Text[] colorRGBTexts;
    [SerializeField] Image playerColorTest;

    [Header("Player")]
    [SerializeField] Transform playerPos;
    public GameObject myplayer;
    [SerializeField] float respawnCoolTIme;

    public Player[] alivePlayers;
    public Player[] allPlayers;

    public float[] playerColors;
    public int codyBodyCode;

    public int codyParticleCode;

    public int playerLv;
    public float exp;
    public float maxExp;
    [SerializeField] Text playerLvText;
    [SerializeField] Text nickNameText;
    [SerializeField] Text expText;
    [SerializeField] Image playerIcon;
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
    [SerializeField] Text goldAmountText3;

    [Header("PlayerStateInfo")]
    public int money;
    public int[] abilityCode;
    public int[] abilityValue;

    [Header("Map")]
    public int mapCode;
    public Button selectMapButton;

    [SerializeField] GameObject mapPanel;
    [SerializeField] Animator mapInfoPanel;
    [SerializeField] Image mapThumnail;
    [SerializeField] Text mapNameText;
    [SerializeField] GameObject[] difficultStars;
    [SerializeField] Text mapCoinAmountinfoText;
    [SerializeField] Text mapExpAmountinfoText;

    [SerializeField]bool mapFocus;
    [SerializeField] RectTransform wordMap;
    [SerializeField] Vector3[] mapPos;

    [SerializeField] Sprite[] mapThumnails;
    [SerializeField] string[] mapNames;
    [SerializeField] int[] mapDifficulty;
    [SerializeField] int[] mapCoinAmount;
    [SerializeField] int[] mapExpAmount;

    [Header("Other")]

    public PhotonView pv;

    public bool stop;
    public float stopTime;

    public bool isGameEnd;
    [SerializeField] bool setExpBarLerp;
    bool isLvUp;
    bool expGIveOnce;
    float overExp;

    [SerializeField] bool once;
    bool stageEndOnce;

    private void Awake()
    {
        spawnList = new List<Spawn>();

        cardsSave = new List<GameObject>(cards);

        rareSave = new List<GameObject>(rare);
        epicSave = new List<GameObject>(epic);
        uniqueSave = new List<GameObject>(unique);
        legendarySave = new List<GameObject>(legendary);
        //ReadSpawnFile();//적 스폰파일 읽기
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
            OP.PrePoolInstantiate();

            once = false;
            isGameEnd = false;
            
        }
        stageEndOnce = true;

        isPlaying = true;

        pv.RPC("AlivePlayerSet", RpcTarget.All);
        pv.RPC("AllPlayerSet", RpcTarget.All);

        NM.roomPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
        gamePlayExpPanel.SetActive(false);

        startAni.SetTrigger("Active");//스테이지Ui
        
        startAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nStart";
        clearAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nClear";

        //if (PhotonNetwork.IsMasterClient)
        //{
            ReadSpawnFile();//적 스폰파일 읽기
        //}

        isGameStart = true;

        fadeAni.SetTrigger("Out");//밝아지기

        cardPanel.SetActive(false);

        myplayer.GetComponent<Player>().godMode = false;


    }
    [PunRPC]
    public void StageEnd()
    {
        if (!stageEndOnce) return;
        stageEndOnce = false;

        isPlaying = false;
        //ClearEnemys();

        clearAni.SetTrigger("Active");//클리어Ui



        myplayer.GetComponent<Player>().godMode = true;
        myplayer.transform.position = playerPos.position;//플래이어 위치 초기화

        stage++;//스테이지 증가

        if (stage > MaxStage)
        {
            FinalStageClear();
        }
        
        else
        {
            fadeAni.SetTrigger("In");//어두워 지기

            Debug.Log("1111111111112");
            Invoke("SelectCard", 3);//카드고르기
        }

    }
    void ClearEnemys()
    {

    }
    
    public void SelectCard()
    {
        CM.isReady = false;
        CM.curMin = 1;
        CM.curSec = 0;
        CM.isCellectingTime = true;

        Player myplayerScript = myplayer.GetComponent<Player>();

        getExpAmountText.text = "";
        getGoldAmountText.text = "";
        goldAmountText3.text = money.ToString();

        gamePlayExpPanel.SetActive(true);
        expGIveOnce = true;
        StartCoroutine(ExpGiveDelay(200));
        StartCoroutine(GiveGold(10));

        if (myplayerScript.followers.Length == myplayerScript.followerAmount)
        {
            epic.RemoveAt(1);
            epic.RemoveAt(2);
        }


        for (int i = 0; i < 3; i++)
        {
            int a = Random.Range(0, 101);
            Debug.Log(a);
            if(0 <= a && a < rarePer)
            {
                int randomR = Random.Range(0, rare.Count);

                rare[randomR].SetActive(true);
                rare.RemoveAt(randomR);
                Debug.Log("레어");
            }
            else if (rarePer <= a && a < rarePer + epicPer)
            {
                int randomR = Random.Range(0, epic.Count);

                epic[randomR].SetActive(true);
                epic.RemoveAt(randomR);
                Debug.Log("에픽");
            }
            else if (rarePer + epicPer <= a && a < rarePer + epicPer + uniquePer)
            {
                int randomR = Random.Range(0, unique.Count);

                unique[randomR].SetActive(true);
                unique.RemoveAt(randomR);
                Debug.Log("유니크");
            }
            else if (rarePer + epicPer + uniquePer <= a && a <= rarePer + epicPer + uniquePer + legendaryPer)
            {
                int randomR = Random.Range(0, legendary.Count);

                legendary[randomR].SetActive(true);
                legendary.RemoveAt(randomR);
                Debug.Log("레전");
            }
        }
        cardPanel.SetActive(true);
    }
    public void SelectComplete()
    {
        gamePlayExpPanel.SetActive(false);
        pv.RPC("StageStart", RpcTarget.All);
    }
    public void ClearCards()
    {
        cards = new List<GameObject>(cardsSave);

        rare = new List<GameObject>(rareSave);
        epic = new List<GameObject>(epicSave);
        unique = new List<GameObject>(uniqueSave);
        legendary = new List<GameObject>(legendarySave);
        Debug.Log("카드 없앰1");
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetActive(false);
        }

        Debug.Log("카드 없앰2");
    }

    void ReadSpawnFile()
    {
        spawnList.Clear();//초기화
        spawnIndex = 0;
        spawnEnd = false;

        //리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage" + stage) as TextAsset;
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
            if(mapCode == 0)
            {
                wordMap.transform.localPosition = Vector3.Lerp(wordMap.transform.localPosition, mapPos[mapCode], Time.deltaTime * 5);
                if(Mathf.Abs(wordMap.transform.localPosition.x - mapPos[mapCode].x) < 0.8f && Mathf.Abs(wordMap.transform.localPosition.y - mapPos[mapCode].y) < 0.8f)
                {
                    wordMap.transform.localPosition = mapPos[mapCode];
                    mapFocus = false;
                }
            }
        }

        if (curSpawnDelay > nextSpawnDelay)
        {

            SpawnEnemy();
            
            curSpawnDelay = 0;
        }
        //Player playerScript = player.GetComponent<Player>();
        if(isGameStart)
        scoreText.text = string.Format("{0:n0}",myplayer.GetComponent<Player>().score);

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
                        GiveExp(0);
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
                        GiveExp(0);
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
                        GiveExp(0);
                    }
                    else
                        setExpBarLerp = false;
                }
            }
        }
        if(premiumColorPanel.activeSelf)
        PlayerColorTest();
    }

    void SpawnEnemy()
    {


        //GameObject enemy = OM.MakeObj(enemysName[enemyIndex]);//소환

        if (PhotonNetwork.IsMasterClient)
        {

            string enemyIndex = "None";
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
                case "Boss0":
                    enemyIndex = "Boss1";
                    break;
            }

            int spawnPoint = spawnList[spawnIndex].point;
            curSpawnEnemy = OP.PoolInstantiate(enemyIndex, enemySpawnPoint[spawnPoint].transform.position, Quaternion.identity);
            //enemy.transform.position = enemySpawnPoint[spawnPoint].transform.position;//위치

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
        for (int i = 0; i < lifeImage.Length; i++)//끄기
        {
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < life; i++)
        {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }
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
        if (myplayer.GetComponent<Player>().gotSpecialWeaponAbility)
        {
            specialShotBotton.SetActive(true);
            weaponBulletText.text = "0";
        }
        else
        {
            specialShotBotton.SetActive(false);
            weaponBulletText.text = "0";
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
            myplayer.GetComponent<Player>().life = Life;
            UpdateLifeIcon(myplayer.GetComponent<Player>().life);
            retryPanel.SetActive(false);

        }
        pv.RPC("AlivePlayerSet", RpcTarget.All);

    }

    public void MakeExplosionEffect(Vector3 pos, string targetType)
    {
        //GameObject explosion = OM.MakeObj("Explosion");
        GameObject explosion = OP.PoolInstantiate("Explosion", Vector3.up * 100, Quaternion.identity);
        Explosion explosionScript = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionScript.StartExplosion(targetType);
    }

    public void RandomChangePlayerColor()
    {
        R = Random.Range(0, 256);
        G = Random.Range(0, 256);
        B = Random.Range(0, 256);
        //Color dd = new Color(R, G, B, 1);
        //myplayer.GetComponent<SpriteRenderer>().color = dd;
        playerColors[0] = Mathf.Round(R);
        playerColors[1] = Mathf.Round(G);
        playerColors[2] = Mathf.Round(B);

        myplayer.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All,playerColors[0], playerColors[1], playerColors[2]);
    }

    void PlayerColorTest()
    {
        R = colorRSlider.value * 255;
        G = colorGSlider.value * 255;
        B = colorBSlider.value * 255;
        colorRGBTexts[0].text = Mathf.Round(R).ToString();
        colorRGBTexts[1].text = Mathf.Round(G).ToString();
        colorRGBTexts[2].text = Mathf.Round(B).ToString();
        playerColorTest.color = new Color(R / 255f, G / 255f, B / 255f, 1);
    }

    float R;
    float G;
    float B;
    public void PremiumColorChange()
    {
        R = colorRSlider.value * 255;
        G = colorGSlider.value * 255;
        B = colorBSlider.value * 255;

        playerColors[0] = Mathf.Round(R);
        playerColors[1] = Mathf.Round(G);
        playerColors[2] = Mathf.Round(B);

        myplayer.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All, playerColors[0], playerColors[1], playerColors[2]);

        premiumColorPanel.SetActive(false);
    }

    public void FinalStageClear()
    {
        isGameEnd = true;
        finalStageClearPanel.SetActive(true);
        Player myplayerScript = myplayer.GetComponent<Player>();

        clearScoreText.text = myplayerScript.score.ToString();
        clearLifeText.text = (myplayerScript.life * 1000).ToString();
        clearTotalScore.text = (myplayerScript.score + (myplayerScript.life * 1000)).ToString();

        gamePlayExpPanel.SetActive(true);
        expGIveOnce = true;
        StartCoroutine(ExpGiveDelay(400));
    }

    IEnumerator ExpGiveDelay(int ExpAmount)
    {
        yield return new WaitForSeconds(2);

        GiveExp(ExpAmount);
    }

    void GiveExp(int ExpAmount)
    {
        if(expGIveOnce)
        {
            getExpAmountText.text = "+" + ExpAmount.ToString();
            expGIveOnce = false;
            exp += ExpAmount;
        }

        if(exp >= maxExp)
        {

            overExp = exp - maxExp;
            exp = maxExp;

            isLvUp = true;
            setExpBarLerp = true;
            return;
        }

        SetExpPanel();
    }

    IEnumerator GiveGold(int GoldAmount)
    {
        yield return new WaitForSeconds(2);
        money += GoldAmount;
        getGoldAmountText.text = "+" + GoldAmount.ToString();
        goldAmountText3.text = money.ToString();
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

        retryPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        controlPanel.SetActive(false);
        finalStageClearPanel.SetActive(false);
        NM.lobbyPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
        
    }

    public void PlayerDie()
    {
        retryPanel.SetActive(true);
    }
    public void GameOver()
    {
        retryPanel.SetActive(false);
        gameOverPanel.SetActive(true);
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

    public void CodyBodyPanelClick(int a)
    {
        if(a == 0)
        {
            codyParticlePanel.SetActive(false);
            codyBodyPanel.SetActive(true);
        }

        else if (a == 1)
        {
            codyBodyPanel.SetActive(false);
            codyParticlePanel.SetActive(true);
        }
    }
    public void CodyOnClick(int index)
    {
        codyBodyCode = index;

        myplayer.transform.GetChild(0).GetComponent<PhotonView>().RPC("CodyRework", RpcTarget.All, index, codyParticleCode);
    }
    public void ParticleOnClick(int index)
    {
        codyParticleCode = index;

        myplayer.transform.GetChild(0).GetComponent<PhotonView>().RPC("CodyRework", RpcTarget.All, codyBodyCode, index);
    }


    public void CodyOpenOrClose(bool a)
    {
        codyPanel.SetActive(a);
        if (!a) return;
        codyParticlePanel.SetActive(false);
        codyBodyPanel.SetActive(true);
    }
    public void PremiumColorOpenOrClose(bool a)
    {
        premiumColorPanel.SetActive(a);

        if (!a) return;
        colorRSlider.value = playerColors[0] / 255f;
        colorGSlider.value = playerColors[1] / 255f;
        colorBSlider.value = playerColors[2] / 255f;
    }

    public void OpenMap(bool a)
    {
        wordMap.transform.localPosition = new Vector3(0, 0, 0);
        mapPanel.SetActive(a);
        mapInfoPanel.SetBool("On", false);
    }

    public void ClickMapPoint(int code)
    {
        mapCode = code;
        mapFocus = true;
        mapNameText.text = mapNames[code];
        mapCoinAmountinfoText.text = "라운드당 코인:" + mapCoinAmount[code].ToString();
        mapExpAmountinfoText.text = "라운드당 경험치:" + mapExpAmount[code].ToString();
        for (int i = 0; i < difficultStars.Length; i++)
        {
            difficultStars[i].SetActive(false);
            if (mapDifficulty[code] >= i)
                difficultStars[i].SetActive(true);
        }
        mapInfoPanel.SetBool("On",true);
    }
    public void MapInfoClose()
    {
        mapInfoPanel.SetBool("On", false);
    }
    public void SelectMapComplete()
    {
        mapInfoPanel.SetBool("On", false);
        mapPanel.SetActive(false);
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
