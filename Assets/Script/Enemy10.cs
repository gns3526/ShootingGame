using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy10 : MonoBehaviour
{
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float stopCool;
    [SerializeField] float shotCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    int playerAmount;
    int targetRandomNum;
    private void OnEnable()
    {
        canMove = true;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (!PhotonNetwork.IsMasterClient) return;

        StartCoroutine(DashToPlayer());
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

    }

    IEnumerator DashToPlayer()
    {
        
        yield return new WaitForSeconds(stopCool);
        canMove = false;

        yield return new WaitForSeconds(shotCool);
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.Euler(0, 0, angle + 90), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);
        EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.Euler(0, 0, angle + 110), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);
        EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.Euler(0, 0, angle + 70), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 3);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        //moveSpeed = 0.05f;
        yield return new WaitForSeconds(goCool);
        canMove = true;
    }
}
