﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy5 : MonoBehaviour
{
    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float stopCool;
    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;

    int randomNum;
    private void OnEnable()
    {
        canMove = true;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(ShotAtPlayer());
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (canMove)
        transform.Translate(new Vector2(0, -moveSpeed * Time.deltaTime));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(stopCool);
        canMove = false;
        yield return new WaitForSeconds(maxAttackCool);
        randomNum = Random.Range(0, 2);
        if(randomNum == 0)
        {
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(45, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(135, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-45, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-135, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
        }
        else
        {
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(0, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(90, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(180, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-90, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], 0, false);
        }

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 2);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(goCool);
        canMove = true;
    }
}
