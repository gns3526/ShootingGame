using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    [SerializeField] NetworkManager NM;
    [SerializeField] ObjectPooler OP;
    [SerializeField] Cards CM;

    [SerializeField] int stage;
    [SerializeField] int MaxStage;
    public bool isGameStart;
    [SerializeField] Animator startAni;
    [SerializeField] Animator clearAni;
    [SerializeField] Animator fadeAni;
    [SerializeField] Transform playerPos;

    [SerializeField] Transform[] enemySpawnPoint;

    [SerializeField] float nextSpawnDelay;
    [SerializeField] float curSpawnDelay;

    public GameObject myplayer;
    [SerializeField] float respawnCoolTIme;

    [SerializeField] Text scoreText;
    [SerializeField] Image[] lifeImage;
    [SerializeField] Image[] boomImage;
    [SerializeField] GameObject scorePanel;

    [SerializeField] GameObject retryPanel;

    [SerializeField] List<Spawn> spawnList;
    [SerializeField] int spawnIndex;
    [SerializeField] bool spawnEnd;

    [SerializeField] List<GameObject> cards;
    [SerializeField] List<GameObject> cardsSave;
    [SerializeField] GameObject cardPanel;

    [Header("Player State")]
    [SerializeField] bool isDie;

    //
    [SerializeField] bool generateOnce;

    public PhotonView pv;


    [SerializeField] bool once;

    private void Awake()
    {
        spawnList = new List<Spawn>();

        cardsSave = new List<GameObject>(cards);

        //ReadSpawnFile();//적 스폰파일 읽기
    }

    [PunRPC]
    void StageStart()
    {
        if (once)
        {
            OP.PrePoolInstantiate();
            once = false;
        }



        NM.roomPanel.SetActive(false);
        scorePanel.SetActive(true);

        startAni.SetTrigger("Active");//스테이지Ui
        
        startAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nStart";
        clearAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nClear";

        if (PhotonNetwork.IsMasterClient)
        {
            ReadSpawnFile();//적 스폰파일 읽기
        }

        isGameStart = true;

        fadeAni.SetTrigger("Out");//밝아지기

        cardPanel.SetActive(false);

        myplayer.GetComponent<Player>().godMode = false;
    }
    [PunRPC]
    public void StageEnd()
    {
        ClearEnemys();

        clearAni.SetTrigger("Active");//클리어Ui

        fadeAni.SetTrigger("In");//어두워 지기

        myplayer.GetComponent<Player>().godMode = true;
        myplayer.transform.position = playerPos.position;//플래이어 위치 초기화

        stage++;//스테이지 증가

        if (stage > MaxStage)//구현한 스테이지 수를 넘었을때
            GameOver();
        else
            Invoke("SelectCard", 3);//카드고르기
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
        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, cards.Count);
            cards[random].SetActive(true);
            cards.RemoveAt(random);
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
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd && isGameStart)
        {
            SpawnEnemy();
            
            curSpawnDelay = 0;
        }
        //Player playerScript = player.GetComponent<Player>();
        //scoreText.text = string.Format("{0:n0}",playerScript.score);

        if (Input.GetKeyDown(KeyCode.Y)) pv.RPC("StageStart",RpcTarget.AllBuffered);
    }

    void SpawnEnemy()
    {
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
            //GameObject enemy = OM.MakeObj(enemysName[enemyIndex]);//소환

            GameObject enemy = OP.PoolInstantiate(enemyIndex, enemySpawnPoint[spawnPoint].transform.position, Quaternion.identity);
            //enemy.transform.position = enemySpawnPoint[spawnPoint].transform.position;//위치
            Debug.Log(enemy.name);
            Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            enemyScript.GM = this;
            enemyScript.gmPv = pv;

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

    public IEnumerator ReSpawnM()
    {
        //Invoke("ReSpawn", respawnCoolTIme);
        yield return new WaitForSeconds(respawnCoolTIme);
        myplayer.GetComponent<Player>().canHit = true;
        pv.RPC("ReSpawn", RpcTarget.AllBuffered);
        //ReSpawn();
    }

    [PunRPC]
    void ReSpawn()
    {

        myplayer.SetActive(true);
       // Debug.Log("리스폰");
      
    }

    [PunRPC]
    void ReviveTeam()
    {
        if (myplayer.GetComponent<Player>().isDie)
        {
            isDie = false;
            myplayer.GetComponent<Player>().life = 1;
        }
    }

    public void MakeExplosionEffect(Vector3 pos, string targetType)
    {
        //GameObject explosion = OM.MakeObj("Explosion");
        GameObject explosion = OP.PoolInstantiate("Explosion", Vector3.up * 100, Quaternion.identity);
        Explosion explosionScript = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionScript.StartExplosion(targetType);
    }

    public void GameOver()
    {
        retryPanel.SetActive(true);
    }
    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }


}
