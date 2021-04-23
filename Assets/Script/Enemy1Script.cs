using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy1Script : MonoBehaviour
{
    
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float maxAttackCool;

    [SerializeField] float moveSpeed;

    GameObject bullet;
    BulletScript bs;
    

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
        bullet = EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward), 0, -1, EB.bulletSpeedCode[0], false);//0.1f

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 1);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        StartCoroutine(ShotAtPlayer());
    }
}
