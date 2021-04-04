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
    [SerializeField] GameManager GM;

    [SerializeField] float maxAttackCool;

    [SerializeField] float moveSpeed;

    int a;
    int targetRandomNum;
    private void OnEnable()
    {
        GM = FindObjectOfType<GameManager>();
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;

        if (!PhotonNetwork.IsMasterClient) return;
        creat();
        StartCoroutine(ShotAtPlayer());
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
        transform.Translate(new Vector2(0, -moveSpeed));
    }

    IEnumerator ShotAtPlayer()
    {
        yield return new WaitForSeconds(maxAttackCool);
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        GameObject bullet = EB.OP.PoolInstantiate("EnemyBullet2", transform.position + Vector3.right * 0.2f, Quaternion.AngleAxis(angle + 90, Vector3.forward));
        GameObject bullet2 = EB.OP.PoolInstantiate("EnemyBullet2", transform.position + Vector3.left * 0.2f, Quaternion.AngleAxis(angle + 90, Vector3.forward));
        bullet.GetComponent<BulletScript>().bulletSpeed = 0.1f;
        bullet2.GetComponent<BulletScript>().bulletSpeed = 0.1f;
        EB.healthBarGameObject.transform.rotation = Quaternion.identity;
        StartCoroutine(ShotAtPlayer());
    }
}
