using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy13 : MonoBehaviour
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

    int playerAmount;
    int targetRandomNum;

    private void OnEnable()
    {
        //moveSpeed = moveSpeedOri;

        canMove = true;
        moveToSide = false;
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
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;

        EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward) ,EB.bulletCode[0], -1, EB.bulletSpeedCode[0], false);
        EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward) ,EB.bulletCode[1], -1, EB.bulletSpeedCode[1], false);
        EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward) ,EB.bulletCode[2], -1, EB.bulletSpeedCode[2], false);
        EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward) ,EB.bulletCode[3], -1, EB.bulletSpeedCode[3], false);
        EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(angle + 90, Vector3.forward) ,EB.bulletCode[4], -1, EB.bulletSpeedCode[4], false);

        EB.pv.RPC(nameof(EB.SoundRPC), RpcTarget.All, 3);

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(goCool);
        canMove = false;
        moveToSide = true;

    }
}
