﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy9 : MonoBehaviour
{
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float stopCool;
    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    //float moveSpeedOri;


    private void Start()
    {
       // moveSpeedOri = moveSpeed;
    }
    private void OnEnable()
    {
        //moveSpeed = moveSpeedOri;
        canMove = true;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        StartCoroutine(ShotAtPlayer());
    }

    private void Update()
    {
        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(stopCool);
        canMove = false;
        yield return new WaitForSeconds(maxAttackCool);

        EB.OP.PoolInstantiate("LaserS", transform.position, Quaternion.AngleAxis(45, Vector3.forward));
        EB.OP.PoolInstantiate("LaserS", transform.position, Quaternion.AngleAxis(135, Vector3.forward));
        EB.OP.PoolInstantiate("LaserS", transform.position, Quaternion.AngleAxis(-45, Vector3.forward));
        EB.OP.PoolInstantiate("LaserS", transform.position, Quaternion.AngleAxis(-135, Vector3.forward));

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        //moveSpeed = 0.04f;
        yield return new WaitForSeconds(goCool);
        canMove = true;
    }
}
