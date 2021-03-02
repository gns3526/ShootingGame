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
    [SerializeField] GameManager GM;

    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;

    bool once;

    int targetRandomNum;
    int randomNum;
    int a;


    private void OnEnable()
    {
        canMove = true;
        once = true;
        GM = FindObjectOfType<GameManager>();
        creat();
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
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
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(45, Vector3.forward));
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(135, Vector3.forward));
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-45, Vector3.forward));
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-135, Vector3.forward));
        }
        else
        {
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(0, Vector3.forward));
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(90, Vector3.forward));
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(180, Vector3.forward));
            EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.AngleAxis(-90, Vector3.forward));
        }
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(goCool);
        canMove = true;
    }
}
