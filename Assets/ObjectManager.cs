using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] GameObject enemyL_Prefap;
    [SerializeField] GameObject enemyM_Prefap;
    [SerializeField] GameObject enemyS_Prefap;
    [SerializeField] GameObject boss0_Prefap;
    [SerializeField] GameObject itemCoin_Prefap;
    [SerializeField] GameObject itemPow_Prefap;
    [SerializeField] GameObject itemBoom_Prefap;
    [SerializeField] GameObject bulletPlayer0_Prefap;
    [SerializeField] GameObject bulletPlayer1_Prefap;
    [SerializeField] GameObject bulletEnemy0_Prefap;
    [SerializeField] GameObject bulletEnemy1_Prefap;
    [SerializeField] GameObject bulletFollower0_Prefap;
    [SerializeField] GameObject bulletBoss0_Prefap;
    [SerializeField] GameObject bulletBoss1_Prefap;
    [SerializeField] GameObject explosion_Prefap;

    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;
    GameObject[] boss0;

    GameObject[] itemCoin;
    GameObject[] itemPow;
    GameObject[] itemBoom;

    GameObject[] bulletPlayer0;
    GameObject[] bulletPlayer1;
    GameObject[] bulletEnemy0;
    GameObject[] bulletEnemy1;
    GameObject[] bulletFollower0;
    GameObject[] bulletBoss0;
    GameObject[] bulletBoss1;

    GameObject[] explosion;

    GameObject[] targetPool;

    int Code;

    private void Awake()
    {
        enemyL = new GameObject[20];//필요한 갯수
        enemyM = new GameObject[20];
        enemyS = new GameObject[20];
        boss0 = new GameObject[10];

        itemCoin = new GameObject[20];
        itemPow = new GameObject[20];
        itemBoom = new GameObject[20];

        bulletPlayer0 = new GameObject[100];
        bulletPlayer1 = new GameObject[100];
        bulletEnemy0 = new GameObject[100];
        bulletEnemy1 = new GameObject[100];
        bulletBoss0 = new GameObject[1000];
        bulletBoss1 = new GameObject[200];

        bulletFollower0 = new GameObject[20];

        explosion = new GameObject[20];

        Generate();
    }
    void Generate()
    {
        //적
        for (int i = 0; i < enemyL.Length; i++)
        {
            enemyL[i] = Instantiate(enemyL_Prefap);
            enemyL[i].SetActive(false);
        }
        for (int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemyM_Prefap);
            enemyM[i].SetActive(false);
        }
        for (int i = 0; i < enemyS.Length; i++)
        {
            enemyS[i] = Instantiate(enemyS_Prefap);
            enemyS[i].SetActive(false);
        }
        for (int i = 0; i < boss0.Length; i++)
        {
            boss0[i] = Instantiate(boss0_Prefap);
            Debug.Log("11111");
            boss0[i].SetActive(false);
            Debug.Log("22222");
        }

        //아이템
        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoin_Prefap);
            itemCoin[i].SetActive(false);
        }
        for (int i = 0; i < itemPow.Length; i++)
        {
            itemPow[i] = Instantiate(itemPow_Prefap);
            itemPow[i].SetActive(false);
        }
        for (int i = 0; i < itemBoom.Length; i++)
        { 
            itemBoom[i] = Instantiate(itemBoom_Prefap);
            itemBoom[i].SetActive(false);
        }

        //플래이어 총알
        for (int i = 0; i < bulletPlayer0.Length; i++)
        {
            bulletPlayer0[i] = Instantiate(bulletPlayer0_Prefap);
            bulletPlayer0[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayer1.Length; i++)
        {
            bulletPlayer1[i] = Instantiate(bulletPlayer1_Prefap);
            bulletPlayer1[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy0.Length; i++)
        {
            bulletEnemy0[i] = Instantiate(bulletEnemy0_Prefap);
            bulletEnemy0[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy1.Length; i++)
        {
            bulletEnemy1[i] = Instantiate(bulletEnemy1_Prefap);
            bulletEnemy1[i].SetActive(false);
        }

        //펫 총알
        for (int i = 0; i < bulletFollower0.Length; i++)
        {
            bulletFollower0[i] = Instantiate(bulletFollower0_Prefap);
            bulletFollower0[i].SetActive(false);
        }

        //보스총알
        for (int i = 0; i < bulletBoss0.Length; i++)
        {
            bulletBoss0[i] = Instantiate(bulletBoss0_Prefap);
            bulletBoss0[i].SetActive(false);
        }
        for (int i = 0; i < bulletBoss1.Length; i++)
        {
            bulletBoss1[i] = Instantiate(bulletBoss1_Prefap);
            bulletBoss1[i].SetActive(false);
        }
        
        //폭발 이펙트
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i] = Instantiate(explosion_Prefap);
            explosion[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {

        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                Code = -1;
                break;
            case "EnemyM":
                targetPool = enemyM;
                Code = -1;
                break;
            case "EnemyS":
                targetPool = enemyS;
                Code = -1;
                break;
            case "Boss0":
                targetPool = boss0;
                Code = 0;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                Code = -1;
                break;
            case "ItemPow":
                targetPool = itemPow;
                Code = -1;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                Code = -1;
                break;
            case "BulletPlayer0":
                targetPool = bulletPlayer0;
                Code = -1;
                break;
            case "BulletPlayer1":
                targetPool = bulletPlayer1;
                Code = -1;
                break;
            case "BulletEnemy0":
                targetPool = bulletEnemy0;
                Code = -1;
                break;
            case "BulletEnemy1":
                targetPool = bulletEnemy1;
                Code = -1;
                break;
            case "BulletFollower0":
                targetPool = bulletFollower0;
                Code = -1;
                break;
            case "BulletBoss0":
                targetPool = bulletBoss0;
                Code = -1;
                break;
            case "BulletBoss1":
                targetPool = bulletBoss1;
                Code = -1;
                break;
            case "Explosion":
                targetPool = explosion;
                Code = -1;
                break;

        }
        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)//맨위에꺼가 꺼져있다면
            {
                if(Code == 0)
                {
                    targetPool[i].GetComponent<EnemyScript>().isSpawn = true;
                }


                targetPool[i].SetActive(true);
                return targetPool[i];//켜준다
            }

        }
        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "Boss0":
                targetPool = boss0;
                break;
            case "ItemCoin":
                targetPool = itemCoin;
                break;
            case "ItemPow":
                targetPool = itemPow;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                break;
            case "BulletPlayer0":
                targetPool = bulletPlayer0;
                break;
            case "BulletPlayer1":
                targetPool = bulletPlayer1;
                break;
            case "BulletEnemy0":
                targetPool = bulletEnemy0;
                break;
            case "BulletEnemy1":
                targetPool = bulletEnemy1;
                break;
            case "BulletFollower0":
                targetPool = bulletFollower0;
                break;
            case "BulletBoss0":
                targetPool = bulletBoss0;
                break;
            case "BulletBoss1":
                targetPool = bulletBoss1;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }
        return targetPool;
    }
}
