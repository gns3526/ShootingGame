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

    [Header("Score")]
    [SerializeField] Text scoreText;
    [SerializeField] Image[] lifeImage;
    [SerializeField] Image[] boomImage;

    [SerializeField] Text clearScoreText;
    [SerializeField] Text clearLifeText;
    [SerializeField] Text clearTotalScore;

    [Header("Panels")]
    public GameObject scorePanel;
    [SerializeField] GameObject retryPanel;
    public GameObject controlPanel;
    [SerializeField] GameObject finalStageClearPanel;
    [SerializeField] GameObject codyPanel;
    public GameObject loginPanel;
    public GameObject expPanal;

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

    [SerializeField] int[] gacha;

    [SerializeField] int rarePer;
    [SerializeField] int epicPer;
    [SerializeField] int uniquePer;
    [SerializeField] int legendaryPer;

    [Header("ControlPanel")]
    public GameObject normalShotBotton;
    public GameObject specialShotBotton;

    public Text weaponBulletText;
    public Image weaponShotButtonImage;

    [Header("Player")]
    [SerializeField] Transform playerPos;
    public GameObject myplayer;
    [SerializeField] float respawnCoolTIme;

    public Player[] alivePlayers;

    public float[] playerColors;
    public int codyBodyCode;

    [SerializeField] int playerLv;
    [SerializeField] float exp;
    [SerializeField] float maxExp;
    [SerializeField] Text playerLvText;
    [SerializeField] Text nickNameText;
    [SerializeField] Text expText;
    [SerializeField] Image playerIcon;
    [SerializeField] Image expImage;
    

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
        float expp = Mathf.Round(exp);
        float maxExpp = Mathf.Round(maxExp);
        playerIcon.sprite = NM.icons[NM.playerIconCode];
        playerLvText.text = playerLv.ToString() + ".Lv";
        expText.text = exp.ToString() + "/" + maxExp.ToString();
        nickNameText.text = PhotonNetwork.LocalPlayer.NickName;
    }

    [PunRPC]
    void StageStart()
    {
        if (once)
        {
            OP.PrePoolInstantiate();
            expPanal.SetActive(false);
            once = false;
            isGameEnd = false;
        }
        isPlaying = true;
        

        NM.roomPanel.SetActive(false);
        scorePanel.SetActive(true);

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
    //[PunRPC]
    public void StageEnd()
    {
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
        /*
        GameObject[] enemy1 = OM.GetPool("1");
        GameObject[] enemy2 = OM.GetPool("2");
        GameObject[] enemy3 = OM.GetPool("3");
        GameObject[] enemy4 = OM.GetPool("4");

        for (int i = 0; i < enemy1.Length; i++)//모든적데미지주기
        {
            if (enemy1[i].activeSelf)
            {
                EnemyScript enemyScript = enemy1[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemy2.Length; i++)//모든적데미지주기
        {
            if (enemy2[i].activeSelf)
            {
                EnemyScript enemyScript = enemy2[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemy3.Length; i++)//모든적데미지주기
        {
            if (enemy3[i].activeSelf)
            {
                EnemyScript enemyScript = enemy3[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemy4.Length; i++)//모든적데미지주기
        {
            if (enemy4[i].activeSelf)
            {
                EnemyScript enemyScript = enemy4[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }

        GameObject[] BulletEnemy0 = OM.GetPool("BulletEnemy0");
        GameObject[] BulletEnemy1 = OM.GetPool("BulletEnemy1");
        GameObject[] BulletEnemy2 = OM.GetPool("BulletEnemy2");
        GameObject[] BulletEnemy3 = OM.GetPool("BulletEnemy3");

        for (int i = 0; i < BulletEnemy0.Length; i++)
        {
            if (BulletEnemy0[i].activeSelf)
            {
                BulletEnemy0[i].SetActive(false);
            }
        }
        for (int i = 0; i < BulletEnemy1.Length; i++)
        {
            if (BulletEnemy1[i].activeSelf)
            {
                BulletEnemy1[i].SetActive(false);
            }
        }
        for (int i = 0; i < BulletEnemy2.Length; i++)
        {
            if (BulletEnemy2[i].activeSelf)
            {
                BulletEnemy2[i].SetActive(false);
            }
        }
        for (int i = 0; i < BulletEnemy3.Length; i++)
        {
            if (BulletEnemy3[i].activeSelf)
            {
                BulletEnemy3[i].SetActive(false);
            }
        }*/
    }
    
    public void SelectCard()
    {
        CM.isReady = false;
        CM.curMin = 1;
        CM.curSec = 0;
        CM.isCellectingTime = true;

        Player myplayerScript = myplayer.GetComponent<Player>();

        //rare40
        //epic70
        //unique90
        //legen101
        if(myplayerScript.followers.Length == myplayerScript.followerAmount)
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



            //int random = Random.Range(0, cards.Count);
            //cards[random].SetActive(true);
            //cards.RemoveAt(random);
        }
        cardPanel.SetActive(true);
    }
    public void SelectComplete()
    {

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
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetExpPanel();

        if(!stop && isPlaying)
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd && isGameStart)
        {

            SpawnEnemy();
            
            curSpawnDelay = 0;
        }
        //Player playerScript = player.GetComponent<Player>();
        if(isGameStart)
        scoreText.text = string.Format("{0:n0}",myplayer.GetComponent<Player>().score);

        if (setExpBarLerp)
        {

            //if(expImage.fillAmount > exp / maxExp)
            //{
            //    expImage.fillAmount = 0;
            //}
            expImage.fillAmount = Mathf.Lerp(expImage.fillAmount, exp / maxExp, Time.deltaTime * 5);
            if (expImage.fillAmount >= (exp / maxExp) -0.01f)//다찼다면
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
                    GiveExp();
                }
                else
                setExpBarLerp = false;
            }
        }
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
                case "Boss0":
                    enemyIndex = "Boss1";
                    break;
            }

            int spawnPoint = spawnList[spawnIndex].point;
            GameObject enemy = OP.PoolInstantiate(enemyIndex, enemySpawnPoint[spawnPoint].transform.position, Quaternion.identity);
            //enemy.transform.position = enemySpawnPoint[spawnPoint].transform.position;//위치
            Debug.Log(enemy.name);
            Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            //enemyScript.GM = this;
            //enemyScript.myPlayerScript = myplayer.GetComponent<Player>();
            //enemyScript.gmPv = pv;

            if (spawnPoint == 5 || spawnPoint == 8)
            {
                enemy.transform.Rotate(Vector3.back * 90);
            }
            else if (spawnPoint == 6 || spawnPoint == 7)
            {
                enemy.transform.Rotate(Vector3.forward * 90);
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

    public void ChangeShotType()
    {
        Player myplayerScript = myplayer.GetComponent<Player>();
        if (myplayerScript.gotSpecialWeaponAbility)
        {
            myplayerScript.specialShot = !myplayerScript.specialShot;

            if (myplayerScript.specialShot)
            {
                normalShotBotton.SetActive(false);
                specialShotBotton.SetActive(true);
            }
            else
            {
                specialShotBotton.SetActive(false);
                normalShotBotton.SetActive(true);
            }
        }
    }

    public void Shot(bool a)
    {
        myplayer.GetComponent<Player>().isFire = a;
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
        float R = Random.Range(0, 1f);
        float G = Random.Range(0, 1f);
        float B = Random.Range(0, 1f);
        //Color dd = new Color(R, G, B, 1);
        //myplayer.GetComponent<SpriteRenderer>().color = dd;
        playerColors[0] = R;
        playerColors[1] = G;
        playerColors[2] = B;

        myplayer.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All,playerColors[0], playerColors[1], playerColors[2]);
    }

    public void FinalStageClear()
    {
        isGameEnd = true;
        finalStageClearPanel.SetActive(true);
        Player myplayerScript = myplayer.GetComponent<Player>();

        clearScoreText.text = myplayerScript.score.ToString();
        clearLifeText.text = (myplayerScript.life * 1000).ToString();
        clearTotalScore.text = (myplayerScript.score + (myplayerScript.life * 1000)).ToString();

        expPanal.SetActive(true);
        expGIveOnce = true;
        Invoke("GiveExp", 2);
    }

    void GiveExp()
    {
        if(expGIveOnce)
        {
            expGIveOnce = false;
            exp += 400;
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
    public void GoToLobby()
    {
        stage = 1;
        isGameStart = false;
        isPlaying = false;
        curSpawnDelay = 0;
        spawnIndex = 0;
        spawnEnd = false;

        once = true;


        scorePanel.SetActive(false);
        controlPanel.SetActive(false);
        finalStageClearPanel.SetActive(false);
        NM.lobbyPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
        
    }

    public void GameOver()
    {
        retryPanel.SetActive(true);
        pv.RPC("AlivePlayerSet", RpcTarget.All);
        //AlivePlayerSet();
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
    }

    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }


    public void CodyOnClick(int index)
    {
        codyBodyCode = index;

        myplayer.transform.GetChild(0).GetComponent<PhotonView>().RPC("CodyRework", RpcTarget.All, index);

    }

    

    public void CodyOpenOrClose(bool a)
    {
        codyPanel.SetActive(a);
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
