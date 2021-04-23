using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy6 : MonoBehaviour
{
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;

    bool once;

    int targetRandomNum;
    int randomNum;
    int playerAmount;


    private void OnEnable()
    {
        canMove = true;
        once = true;

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;


    }
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        SearchPlayer();
    }
    public void SearchPlayer()
    {

        if (EB.GM.alivePlayers[3])
        {
            playerAmount = 4;
        }
        else if (EB.GM.alivePlayers[2])
        {
            playerAmount = 3;
        }
        else if (EB.GM.alivePlayers[1])
        {
            playerAmount = 2;
        }
        else if (EB.GM.alivePlayers[0])
        {
            playerAmount = 1;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            targetRandomNum = Random.Range(0, playerAmount);
            target = EB.GM.alivePlayers[targetRandomNum].gameObject;
        }

    }
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed));

        if(transform.position.y < target.transform.position.y && once)
        {
            once = false;
            StartCoroutine(ShotAtPlayer());
        }
    }

    IEnumerator ShotAtPlayer()
    {
        canMove = false;
        yield return new WaitForSeconds(maxAttackCool);
        randomNum = Random.Range(0, 2);
        if (randomNum == 0)
        {
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(45, Vector3.forward), -1, -1, 5, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(135, Vector3.forward), -1, -1, 5, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-45, Vector3.forward), -1, -1, 5, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-135, Vector3.forward), -1, -1, 5, false);
        }
        else
        {
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(0, Vector3.forward), -1, -1, 5, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(90, Vector3.forward), -1, -1, 5, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(180, Vector3.forward), -1, -1, 5, false);
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-90, Vector3.forward), -1, -1, 5, false);
        }

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 2);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(goCool);
        canMove = true;
    }
}
