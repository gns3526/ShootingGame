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
    [SerializeField] GameManager GM;

    [SerializeField] float stopCool;
    [SerializeField] float shotCool;
    [SerializeField] float goCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    int a;
    int targetRandomNum;
    private void OnEnable()
    {
        canMove = true;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        GM = FindObjectOfType<GameManager>();

        if (!PhotonNetwork.IsMasterClient) return;
        creat();
        StartCoroutine(DashToPlayer());

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

    }

    IEnumerator DashToPlayer()
    {
        
        yield return new WaitForSeconds(stopCool);
        canMove = false;

        yield return new WaitForSeconds(shotCool);
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.Euler(0, 0, angle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.1f;
        EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.Euler(0, 0, angle + 110)).GetComponent<BulletScript>().bulletSpeed = 0.1f;
        EB.OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.Euler(0, 0, angle + 70)).GetComponent<BulletScript>().bulletSpeed = 0.1f;

        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        //moveSpeed = 0.05f;
        yield return new WaitForSeconds(goCool);
        canMove = true;
    }
}
