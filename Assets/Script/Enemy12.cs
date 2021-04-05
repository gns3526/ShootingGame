using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy12 : MonoBehaviour
{
    [SerializeField] EnemyBasicScript EB;

    [SerializeField] float stopCool;
    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] bool moveToSide;
    [SerializeField] float moveSpeed;
    [SerializeField] float sideSpeed;

    private void Start()
    {
        //moveSpeedOri = moveSpeed;
    }
    private void OnEnable()
    {
        //moveSpeed = moveSpeedOri;
        canMove = true;
        moveToSide = false;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(ShotAtPlayer());
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed));
        else if (moveToSide)
            if(transform.position.x > 0)
                transform.Translate(new Vector2(sideSpeed, 0));
        else
                transform.Translate(new Vector2(-sideSpeed, 0));

    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(stopCool);
        canMove = false;
        yield return new WaitForSeconds(maxAttackCool);

        for (int i = 0; i < 72; i++)//72
        {
            EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.AngleAxis(i * 5, Vector3.forward), EB.bulletCode[0], EB.bulletSpeedCode[0], false);
        }

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        //moveSpeed = 0.05f;
        yield return new WaitForSeconds(goCool);
        canMove = false;
        moveToSide = true;

    }
}
