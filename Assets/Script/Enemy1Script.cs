﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy1Script : MonoBehaviour
{
    
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;
    [SerializeField] GameManager GM;

    [SerializeField] float maxAttackCool;

    [SerializeField] float moveSpeed;

    GameObject bullet;
    BulletScript bs;
    

    int a;
    int targetRandomNum;
    private void OnEnable()
    {
        GM = FindObjectOfType<GameManager>();
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        if (!PhotonNetwork.IsMasterClient) return;
        creat();
        StartCoroutine(ShotAtPlayer());
    }

    public void creat()
    {

        if (GM.alivePlayers[3])
        {
            a = 4;
        }
        else if (GM.alivePlayers[2])
        {
            a = 3;
        }
        else if (GM.alivePlayers[1])
        {
            a = 2;
        }
        else if (GM.alivePlayers[0])
        {
            a = 1;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            targetRandomNum = Random.Range(0, a);
            target = GM.alivePlayers[targetRandomNum].gameObject;
        }

    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        transform.Translate(new Vector2(0, -moveSpeed));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(maxAttackCool);

        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        bullet = EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward), 0, EB.bulletSpeedCode[0], false);//0.1f
        bs = bullet.GetComponent<BulletScript>();

        Debug.Log("발사");

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        StartCoroutine(ShotAtPlayer());
    }
}
