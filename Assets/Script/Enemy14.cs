using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy14 : MonoBehaviour
{
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float stopCool;
    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] bool moveToSide;
    [SerializeField] float moveSpeed;
    [SerializeField] float sideSpeed;
    //float moveSpeedOri;

    int playerAmount;
    int targetRandomNum;

    private void OnEnable()
    {
        //moveSpeed = moveSpeedOri;

        canMove = true;
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

        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed * Time.deltaTime));
        else if (moveToSide)
            if (transform.position.x > 0)
                transform.Translate(new Vector2(sideSpeed * Time.deltaTime, 0));
            else
                transform.Translate(new Vector2(-sideSpeed * Time.deltaTime, 0));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(stopCool);
        canMove = false;
        yield return new WaitForSeconds(maxAttackCool);

        GameObject bullet = EB.OP.PoolInstantiate("EnemyBullet5", transform.position, Quaternion.identity, EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);
        bullet.GetComponent<BulletScript>().bulletSpeed = 0.04f;
        bullet.GetComponent<BulletScript>().target = target;

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 5);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        //moveSpeed = 0.05f;
        yield return new WaitForSeconds(goCool);
        canMove = false;
        moveToSide = true;
    }
}
