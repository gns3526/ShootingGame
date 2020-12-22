using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField] int stage;
    [SerializeField] Animator startAni;
    [SerializeField] Animator clearAni;
    [SerializeField] Animator fadeAni;
    [SerializeField] Transform playerPos;

    [SerializeField] string[] enemysName;
    [SerializeField] Transform[] enemySpawnPoint;

    [SerializeField] float nextSpawnDelay;
    [SerializeField] float curSpawnDelay;

    [SerializeField] GameObject player;

    [SerializeField] Text scoreText;
    [SerializeField] Image[] lifeImage;
    [SerializeField] Image[] boomImage;
    [SerializeField] GameObject retryPanel;

    [SerializeField] ObjectManager OM;


    [SerializeField] List<Spawn> spawnList;
    [SerializeField] int spawnIndex;
    [SerializeField] bool spawnEnd;
    private void Awake()
    {
        spawnList = new List<Spawn>();
        enemysName = new string[] { "EnemyS", "EnemyM", "EnemyL", "Boss0" };
        StageStart();
        ReadSpawnFile();//적 스폰파일 읽기
    }

    void StageStart()
    {
        startAni.SetTrigger("Active");//스테이지Ui
        
        startAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nStart";
        clearAni.GetComponent<Text>().text = "Stage" + stage.ToString() + "\nClear";

        ReadSpawnFile();//적 스폰파일 읽기

        fadeAni.SetTrigger("Out");//어두워지기

    }
    public void StageEnd()
    {
        clearAni.SetTrigger("Active");//클리어Ui

        fadeAni.SetTrigger("In");//밝아지기

        player.transform.position = playerPos.position;//플래이어 위치 초기화

        stage++;//스테이지 증가

        if (stage > 2)//구현한 스테이지 수를 넘었을때
            GameOver();
        else
            Invoke("StageStart", 5);//스테이지 시작



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

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            
            curSpawnDelay = 0;
        }
        Player playerScript = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}",playerScript.score);
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type) 
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "Boss0":
                enemyIndex = 3;
                break;
        }
        int spawnPoint = spawnList[spawnIndex].point;
        GameObject enemy = OM.MakeObj(enemysName[enemyIndex]);//소환
        enemy.transform.position = enemySpawnPoint[spawnPoint].transform.position;//위치

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
        enemyScript.player = player;
        enemyScript.GM = this;
        enemyScript.OM = OM;

        if(spawnPoint == 5 || spawnPoint == 8)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyScript.speed * (-1), -1);
        }
        else if (spawnPoint == 6 || spawnPoint == 7)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyScript.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyScript.speed * (-1));
        }

        //리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count)
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
        for (int i = 0; i < 3; i++)//끄기
        {
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < life; i++)
        {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void ReSpawnM()
    {
        Invoke("ReSpawn", 2);
    }
    void ReSpawn()
    {
        player.transform.position = new Vector3(0, -3.5f, 0);
        player.SetActive(true);

        player.GetComponent<Player>().canHit = true;
    }

    public void MakeExplosionEffect(Vector3 pos, string type)
    {
        GameObject explosion = OM.MakeObj("Explosion");
        Explosion explosionScript = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionScript.StartExplosion(type);
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
