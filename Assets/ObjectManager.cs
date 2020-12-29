using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ObjectManager : MonoBehaviour
{
    [SerializeField] GameObject enemy1_Prefap;
    [SerializeField] GameObject enemy2_Prefap;
    [SerializeField] GameObject enemy3_Prefap;
    [SerializeField] GameObject enemy4_Prefap;
    [SerializeField] GameObject boss0_Prefap;
    [SerializeField] GameObject itemCoin_Prefap;
    [SerializeField] GameObject itemPow_Prefap;
    [SerializeField] GameObject itemBoom_Prefap;
    [SerializeField] GameObject bulletPlayer0_Prefap;
    [SerializeField] GameObject bulletPlayer1_Prefap;
    [SerializeField] GameObject bulletEnemy0_Prefap;
    [SerializeField] GameObject bulletEnemy1_Prefap;
    [SerializeField] GameObject bulletEnemy2_Prefap;
    [SerializeField] GameObject bulletEnemy3_Prefap;
    [SerializeField] GameObject bulletFollower0_Prefap;
    [SerializeField] GameObject bulletFollower1_Prefap;
    
    [SerializeField] GameObject explosion_Prefap;

    GameObject[] enemy1;
    GameObject[] enemy2;
    GameObject[] enemy3;
    GameObject[] enemy4;
    GameObject[] boss0;


    GameObject[] itemCoin;
    GameObject[] itemPow;
    GameObject[] itemBoom;

    GameObject[] bulletPlayer0;
    GameObject[] bulletPlayer1;
    GameObject[] bulletEnemy0;
    GameObject[] bulletEnemy1;
    GameObject[] bulletEnemy2;
    GameObject[] bulletEnemy3;

    GameObject[] bulletFollower0;
    GameObject[] bulletFollower1;

    GameObject[] explosion;

    GameObject[] targetPool;

    int Code;

    [SerializeField] PhotonView pv;

    private void Awake()
    {
        enemy1 = new GameObject[20];//필요한 갯수
        enemy2 = new GameObject[20];
        enemy3 = new GameObject[20];
        enemy4 = new GameObject[20];

        boss0 = new GameObject[10];

        itemCoin = new GameObject[20];
        itemPow = new GameObject[20];
        itemBoom = new GameObject[20];

        bulletPlayer0 = new GameObject[100];
        bulletPlayer1 = new GameObject[100];
        bulletEnemy0 = new GameObject[100];
        bulletEnemy1 = new GameObject[100];
        bulletEnemy3 = new GameObject[1000];
        bulletEnemy2 = new GameObject[200];

        bulletFollower0 = new GameObject[100];
        bulletFollower1 = new GameObject[50];

        explosion = new GameObject[20];

        Generate();
    }
    void Generate()
    {
        //Enemy Object
        for (int i = 0; i < enemy1.Length; i++)
        {
            enemy1[i] = Instantiate(enemy1_Prefap);
            enemy1[i].GetComponent<PhotonView>().ViewID = 10 + i;
           // enemy1[i].GetComponent<EnemyScript>().creat();
            enemy1[i].SetActive(false);
        }
        for (int i = 0; i < enemy2.Length; i++)
        {
            enemy2[i] = Instantiate(enemy2_Prefap);
            enemy2[i].GetComponent<PhotonView>().ViewID = 50 + i;
           // enemy2[i].GetComponent<EnemyScript>().creat();
            enemy2[i].SetActive(false);
        }
        for (int i = 0; i < enemy3.Length; i++)
        {
            enemy3[i] = Instantiate(enemy3_Prefap);
            enemy3[i].GetComponent<PhotonView>().ViewID = 100 + i;
           // enemy3[i].GetComponent<EnemyScript>().creat();
            enemy3[i].SetActive(false);
        }
        for (int i = 0; i < enemy4.Length; i++)
        {
            enemy4[i] = Instantiate(enemy4_Prefap);
            enemy4[i].GetComponent<PhotonView>().ViewID = 150 + i;
          //  enemy4[i].GetComponent<EnemyScript>().creat();
            enemy4[i].SetActive(false);
        }
        for (int i = 0; i < boss0.Length; i++)
        {
            boss0[i] = Instantiate(boss0_Prefap);
            boss0[i].GetComponent<PhotonView>().ViewID = 200 + i;
          //  boss0[i].GetComponent<EnemyScript>().creat();
            boss0[i].SetActive(false);
        }

        //Item
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

        //Player Bullet
        for (int i = 0; i < bulletPlayer0.Length; i++)
        {
            bulletPlayer0[i] = Instantiate(bulletPlayer0_Prefap);
            bulletPlayer0[i].GetComponent<PhotonView>().ViewID = 350 + i;
            bulletPlayer0[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayer1.Length; i++)
        {
            bulletPlayer1[i] = Instantiate(bulletPlayer1_Prefap);
            bulletPlayer1[i].GetComponent<PhotonView>().ViewID = 450 + i;
            bulletPlayer1[i].SetActive(false);
        }

        //Monster Bullet
        for (int i = 0; i < bulletEnemy0.Length; i++)
        {
            bulletEnemy0[i] = Instantiate(bulletEnemy0_Prefap);
            bulletEnemy0[i].GetComponent<PhotonView>().ViewID = 550 + i;
            bulletEnemy0[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy1.Length; i++)
        {
            bulletEnemy1[i] = Instantiate(bulletEnemy1_Prefap);
            bulletEnemy1[i].GetComponent<PhotonView>().ViewID = 650 + i;
            bulletEnemy1[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy2.Length; i++)
        {
            bulletEnemy2[i] = Instantiate(bulletEnemy2_Prefap);
            bulletEnemy2[i].GetComponent<PhotonView>().ViewID = 1650 + i;
            bulletEnemy2[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy3.Length; i++)
        {
            bulletEnemy3[i] = Instantiate(bulletEnemy3_Prefap);
            bulletEnemy3[i].GetComponent<PhotonView>().ViewID = 1850 + i;
            bulletEnemy3[i].SetActive(false);
        }

        //Pet Bullet
        for (int i = 0; i < bulletFollower0.Length; i++)
        {
            bulletFollower0[i] = Instantiate(bulletFollower0_Prefap);
            bulletFollower0[i].GetComponent<PhotonView>().ViewID = 3000 + i;
            bulletFollower0[i].SetActive(false);
        }
        for (int i = 0; i < bulletFollower1.Length; i++)
        {
            bulletFollower1[i] = Instantiate(bulletFollower1_Prefap);
            bulletFollower1[i].GetComponent<PhotonView>().ViewID = 3100 + i;
            bulletFollower1[i].SetActive(false);
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
            case "1":
                targetPool = enemy1;
                Code = 4;
                break;
            case "2":
                targetPool = enemy2;
                Code = 4;
                break;
            case "3":
                targetPool = enemy3;
                Code = 4;
                break;
            case "4":
                targetPool = enemy4;
                Code = 4;
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
            case "BulletEnemy1":
                targetPool = bulletEnemy0;
                Code = -1;
                break;
            case "BulletEnemy2":
                targetPool = bulletEnemy1;
                Code = -1;
                break;
            case "BulletEnemy3":
                targetPool = bulletEnemy2;
                Code = -1;
                break;
            case "BulletEnemy4":
                targetPool = bulletEnemy3;
                Code = -1;
                break;
            case "BulletFollower0":
                targetPool = bulletFollower0;
                Code = -1;
                break;
            case "BulletFollower1":
                targetPool = bulletFollower1;
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
                if (Code == 0)
                {
                    targetPool[i].GetComponent<EnemyScript>().isSpawn = true;
                }
                if (Code == 4)
                {
                    targetPool[i].GetComponent<EnemyScript>().creat();
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
            case "1":
                targetPool = enemy1;
                break;
            case "2":
                targetPool = enemy2;
                break;
            case "3":
                targetPool = enemy3;
                break;
            case "4":
                targetPool = enemy4;
                break;
            case "Boss1":
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
            case "BulletEnemy1":
                targetPool = bulletEnemy0;
                break;
            case "BulletEnemy2":
                targetPool = bulletEnemy1;
                break;
            case "BulletEnemy3":
                targetPool = bulletEnemy2;
                break;
            case "BulletEnemy4":
                targetPool = bulletEnemy3;
                break;
            case "BulletFollower0":
                targetPool = bulletFollower0;
                break;
            case "BulletFollower1":
                targetPool = bulletFollower1;
                Code = -1;
                break;
            case "Explosion":
                targetPool = explosion;
                break;
        }
        return targetPool;
    }
}
