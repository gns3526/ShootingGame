using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Enemy14 : MonoBehaviour
{
    public GameObject target;

    [SerializeField] EnemyBasicScript EB;
    [SerializeField] GameManager GM;

    [SerializeField] float stopCool;
    [SerializeField] float maxAttackCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] bool moveToSide;
    [SerializeField] float moveSpeed;
    [SerializeField] float sideSpeed;
    //float moveSpeedOri;

    int a;
    int targetRandomNum;

    private void Start()
    {
        //moveSpeedOri = moveSpeed;
    }
    private void OnEnable()
    {
        //moveSpeed = moveSpeedOri;
        GM = FindObjectOfType<GameManager>();
        canMove = true;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (!PhotonNetwork.IsMasterClient) return;
        StartCoroutine(ShotAtPlayer());
        creat();
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
        if (!PhotonNetwork.IsMasterClient) return;

        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed));
        else if (moveToSide)
            if (transform.position.x > 0)
                transform.Translate(new Vector2(sideSpeed, 0));
            else
                transform.Translate(new Vector2(-sideSpeed, 0));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(stopCool);
        canMove = false;
        yield return new WaitForSeconds(maxAttackCool);

        GameObject bullet = EB.OP.PoolInstantiate("EnemyBullet5", transform.position, Quaternion.identity);
        bullet.GetComponent<BulletScript>().bulletSpeed = 0.04f;
        bullet.GetComponent<BulletScript>().target = target;
        

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        //moveSpeed = 0.05f;
        yield return new WaitForSeconds(goCool);
        canMove = false;
        moveToSide = true;
    }
}
