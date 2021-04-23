using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy3 : MonoBehaviour
{
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float maxAttackCool;

    [SerializeField] float moveSpeed;

    int playerAmount;
    int targetRandomNum;
    private void OnEnable()
    {
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(ShotAtPlayer());
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
        transform.Translate(new Vector2(0, -moveSpeed));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(maxAttackCool);
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        GameObject bulletR = EB.OP.PoolInstantiate("EnemyBullet2", transform.position + Vector3.right * 0.2f, Quaternion.AngleAxis(angle + 90, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);
        GameObject bulletL = EB.OP.PoolInstantiate("EnemyBullet2", transform.position + Vector3.left * 0.2f, Quaternion.AngleAxis(angle + 90, Vector3.forward), EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 2);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        StartCoroutine(ShotAtPlayer());
    }
}
