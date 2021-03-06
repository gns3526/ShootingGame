﻿using System.Collections;
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

        bulletPlayer0 = new GameObject[100];
        bulletPlayer1 = new GameObject[100];
        bulletEnemy0 = new GameObject[50];
        bulletEnemy1 = new GameObject[50];
        bulletEnemy2 = new GameObject[50];
        bulletEnemy3 = new GameObject[50];


        bulletFollower0 = new GameObject[50];
        bulletFollower1 = new GameObject[50];

        explosion = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPow = new GameObject[20];
        itemBoom = new GameObject[20];
        //Generate();
    }
    public void Generate()
    {
        Debug.Log("생성");
        //Enemy Object
        for (int i = 0; i < enemy1.Length; i++)
        {
            enemy1[i] = Instantiate(enemy1_Prefap);
            //enemy1[i] = PhotonNetwork.Instantiate("Enemy1",Vector3.zero,Quaternion.identity);
            enemy1[i].GetComponent<PhotonView>().ViewID = 1010 + i;
            //enemy1[i].GetComponent<EnemyScript>().creat();
            enemy1[i].SetActive(false);
        }
        for (int i = 0; i < enemy2.Length; i++)
        {
            enemy2[i] = Instantiate(enemy2_Prefap);
            //enemy2[i] = PhotonNetwork.Instantiate("Enemy2", Vector3.zero, Quaternion.identity);
            enemy2[i].GetComponent<PhotonView>().ViewID = 1030 + i;
            //enemy2[i].GetComponent<EnemyScript>().creat();
            enemy2[i].SetActive(false);
        }
        for (int i = 0; i < enemy3.Length; i++)
        {
            enemy3[i] = Instantiate(enemy3_Prefap);
            //enemy3[i] = PhotonNetwork.Instantiate("Enemy3", Vector3.zero, Quaternion.identity);
            enemy3[i].GetComponent<PhotonView>().ViewID = 1050 + i;
            //enemy3[i].GetComponent<EnemyScript>().creat();
            enemy3[i].SetActive(false);
        }
        for (int i = 0; i < enemy4.Length; i++)
        {
            enemy4[i] = Instantiate(enemy4_Prefap);
            //enemy4[i] = PhotonNetwork.Instantiate("Enemy4", Vector3.zero, Quaternion.identity);
            enemy4[i].GetComponent<PhotonView>().ViewID = 1070 + i;
            //enemy4[i].GetComponent<EnemyScript>().creat();
            enemy4[i].SetActive(false);
        }
        for (int i = 0; i < boss0.Length; i++)
        {
            boss0[i] = Instantiate(boss0_Prefap);
            //boss0[i] = PhotonNetwork.Instantiate("BossEnemy", Vector3.zero, Quaternion.identity);
            boss0[i].GetComponent<PhotonView>().ViewID = 1090 + i;
            //boss0[i].GetComponent<EnemyScript>().creat();
            boss0[i].SetActive(false);
        }



        //Player Bullet
        for (int i = 0; i < bulletPlayer0.Length; i++)
        {
            bulletPlayer0[i] = Instantiate(bulletPlayer0_Prefap);
            //bulletPlayer0[i] = PhotonNetwork.Instantiate("Bullet0", Vector3.zero, Quaternion.identity);
            bulletPlayer0[i].GetComponent<PhotonView>().ViewID = 1100 + i;
            bulletPlayer0[i].SetActive(false);
        }
        for (int i = 0; i < bulletPlayer1.Length; i++)
        {
            bulletPlayer1[i] = Instantiate(bulletPlayer1_Prefap);
           // bulletPlayer1[i] = PhotonNetwork.Instantiate("Bullet1", Vector3.zero, Quaternion.identity);
            bulletPlayer1[i].GetComponent<PhotonView>().ViewID = 1200 + i;
            bulletPlayer1[i].SetActive(false);
        }

        //Monster Bullet
        for (int i = 0; i < bulletEnemy0.Length; i++)
        {
            bulletEnemy0[i] = Instantiate(bulletEnemy0_Prefap);
            //bulletEnemy0[i] = PhotonNetwork.Instantiate("EnemyBullet1", Vector3.zero, Quaternion.identity);
            bulletEnemy0[i].GetComponent<PhotonView>().ViewID = 1300 + i;
            bulletEnemy0[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy1.Length; i++)
        {
            bulletEnemy1[i] = Instantiate(bulletEnemy1_Prefap);
            //bulletEnemy1[i] = PhotonNetwork.Instantiate("EnemyBullet2", Vector3.zero, Quaternion.identity);
            bulletEnemy1[i].GetComponent<PhotonView>().ViewID = 1350 + i;
            bulletEnemy1[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy2.Length; i++)
        {
            bulletEnemy2[i] = Instantiate(bulletEnemy2_Prefap);
            //bulletEnemy2[i] = PhotonNetwork.Instantiate("EnemyBullet3", Vector3.zero, Quaternion.identity);
            bulletEnemy2[i].GetComponent<PhotonView>().ViewID = 1400 + i;
            bulletEnemy2[i].SetActive(false);
        }
        for (int i = 0; i < bulletEnemy3.Length; i++)
        {
            bulletEnemy3[i] = Instantiate(bulletEnemy3_Prefap);
            //bulletEnemy3[i] = PhotonNetwork.Instantiate("EnemyBullet4", Vector3.zero, Quaternion.identity);
            bulletEnemy3[i].GetComponent<PhotonView>().ViewID = 1450 + i;
            bulletEnemy3[i].SetActive(false);
        }

        //Pet Bullet
        for (int i = 0; i < bulletFollower0.Length; i++)
        {
            bulletFollower0[i] = Instantiate(bulletFollower0_Prefap);
            //bulletFollower0[i] = PhotonNetwork.Instantiate("FollowerBullet0", Vector3.zero, Quaternion.identity);
            bulletFollower0[i].GetComponent<PhotonView>().ViewID = 1500 + i;
            bulletFollower0[i].SetActive(false);
        }
        for (int i = 0; i < bulletFollower1.Length; i++)
        {
            bulletFollower1[i] = Instantiate(bulletFollower1_Prefap);
            //bulletFollower1[i] = PhotonNetwork.Instantiate("FollowerBullet1", Vector3.zero, Quaternion.identity);
            bulletFollower1[i].GetComponent<PhotonView>().ViewID = 1550 + i;
            bulletFollower1[i].SetActive(false);
        }

        //폭발 이펙트
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i] = Instantiate(explosion_Prefap);
            //explosion[i] = PhotonNetwork.Instantiate("Explosion", Vector3.zero, Quaternion.identity);
            explosion[i].GetComponent<PhotonView>().ViewID = 1600 + i;
            explosion[i].SetActive(false);
        }

        //Item
        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoin_Prefap);
            //itemCoin[i] = PhotonNetwork.Instantiate("ItemCoin", Vector3.zero, Quaternion.identity);
            explosion[i].GetComponent<PhotonView>().ViewID = 1620 + i;
            itemCoin[i].SetActive(false);
        }
        for (int i = 0; i < itemPow.Length; i++)
        {
            itemPow[i] = Instantiate(itemPow_Prefap);
            //itemPow[i] = PhotonNetwork.Instantiate("ItemPower", Vector3.zero, Quaternion.identity);
            explosion[i].GetComponent<PhotonView>().ViewID = 1640 + i;
            itemPow[i].SetActive(false);
        }
        for (int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemBoom_Prefap);
            //itemBoom[i] = PhotonNetwork.Instantiate("ItemBoom", Vector3.zero, Quaternion.identity);
            explosion[i].GetComponent<PhotonView>().ViewID = 1660 + i;
            itemBoom[i].SetActive(false);
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
                Code = -2;
                break;
            case "ItemBoom":
                targetPool = itemBoom;
                Code = -3;
                break;
            case "BulletPlayer0":
                targetPool = bulletPlayer0;
                Code = -4;
                break;
            case "BulletPlayer1":
                targetPool = bulletPlayer1;
                Code = -5;
                break;
            case "BulletEnemy1":
                targetPool = bulletEnemy0;
                Code = -6;
                break;
            case "BulletEnemy2":
                targetPool = bulletEnemy1;
                Code = -7;
                break;
            case "BulletEnemy3":
                targetPool = bulletEnemy2;
                Code = -8;
                break;
            case "BulletEnemy4":
                targetPool = bulletEnemy3;
                Code = -9;
                break;
            case "BulletFollower0":
                targetPool = bulletFollower0;
                Code = -10;
                break;
            case "BulletFollower1":
                targetPool = bulletFollower1;
                Code = -11;
                break;
            case "Explosion":
                targetPool = explosion;
                Code = -12;
                break;
      
        }
        for (int i = 0; i < targetPool.Length; i++)
        {
            Debug.Log(Code);
            if (!targetPool[i].activeSelf)//맨위에꺼가 꺼져있다면
            {
                if (Code == 0)
                {
                    //targetPool[i].GetComponent<EnemyScript>().isSpawn = true;
                }
                if (Code == 4)
                {
                    //targetPool[i].GetComponent<EnemyScript>().creat();
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
