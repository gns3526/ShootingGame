using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Boss1 : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] EnemyBasicScript EB;

    [SerializeField] int patternIndex;
    [SerializeField] int curPatternCount;
    [SerializeField] int[] MaxPatternCount;
    [SerializeField] float[] fireCoolTime;

    [SerializeField] float stopCool;

    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;
    int a;
    int targetRandomNum;

    private void OnEnable()
    {
        canMove = true;
        EB.pv.RPC(nameof(EB.GodModeRPC), RpcTarget.All, true);

        if (PhotonNetwork.IsMasterClient)
        StartCoroutine(Stop());
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        SearchPlayer();
    }

    private void Update()
    {
        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed));
    }
    public void SearchPlayer()
    {

        if (EB.GM.alivePlayers[3])
        {
            a = 4;
        }
        else if (EB.GM.alivePlayers[2])
        {
            a = 3;
        }
        else if (EB.GM.alivePlayers[1])
        {
            a = 2;
        }
        else if (EB.GM.alivePlayers[0])
        {
            a = 1;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            targetRandomNum = Random.Range(0, a);
            target = EB.GM.alivePlayers[targetRandomNum].gameObject;
        }
    }
    public IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopCool);
        if (gameObject.activeSelf)
        {
            EB.pv.RPC(nameof(EB.GodModeRPC), RpcTarget.All, false);

            canMove = false;

            //StartCoroutine(Think(2));
        }
    }
    
    IEnumerator Think(float waitTime)
    {
        //if (!gameObject.activeSelf)//활성화 되어있지 않다면
        //    return;//되돌림
        yield return new WaitForSeconds(waitTime);
        Debug.Log("생각");
        patternIndex = patternIndex >= 3 ? 0 : patternIndex + 1;//패턴갯수 오버하면 0으로만듬
        curPatternCount = 0;
        /*
        switch (patternIndex)
        {
            case 0:
                StartCoroutine(FireFoward());
                break;
            case 1:
                StartCoroutine(FireShot());
                break;
            case 2:
                StartCoroutine(FireArc());
                break;
            case 3:
                StartCoroutine(FireAround());
                break;
        }*/
    }
    IEnumerator FireFoward()//앞으로 4발
    {
        GameObject bulletR = EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.3f, Quaternion.identity, 3, 1, 6, false);

        GameObject bulletRR = EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.45f, Quaternion.identity, 3, 1, 6, false);

        GameObject bulletL = EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.3f, Quaternion.identity, 3, 1, 6, false);

        GameObject bulletLL = EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.45f, Quaternion.identity, 3, 1, 6, false);



        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[0]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            StartCoroutine(FireFoward());
        }
        else
        {
            StartCoroutine(Think(1));
        }
    }
    /*
    IEnumerator FireShot()//플래이어방향으로 샷건
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = EB.OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.identity);
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            bullet.GetComponent<BulletScript>().isBossBullet = true;

            //Vector2 dir = player[Random.Range(0,player.Length)].transform.position - transform.position;
            Vector2 dir = target.transform.position - transform.position;
            Vector2 randomVector = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2));
            dir += randomVector;
            rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
        }
        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[1]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            StartCoroutine(FireShot());
        }
        else
        {
            StartCoroutine(Think(1));
        }
    }
    IEnumerator FireArc()//부체모양
    {
        GameObject bullet = EB.OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.identity);
        bullet.transform.position = transform.position;//초기화
        bullet.transform.rotation = Quaternion.identity;//초기화

        bullet.GetComponent<BulletScript>().isBossBullet = true;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / MaxPatternCount[patternIndex]), -1);//Cos도 가능
        rigid.AddForce(dir.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[2]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            StartCoroutine(FireArc());
        }
        else
        {
            StartCoroutine(Think(1));
        }
    }
    IEnumerator FireAround()//원형태로 뿌림
    {
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;//roundNumA와roundNumB의 수를 교차
        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = EB.OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.identity);
            bullet.transform.position = transform.position;//초기화
            bullet.transform.rotation = Quaternion.identity;//초기화

            bullet.GetComponent<BulletScript>().isBossBullet = true;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));//Cos도 가능
            rigid.AddForce(dir.normalized * 2, ForceMode2D.Impulse);

            Vector3 roVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(roVec);

        }

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[3]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            StartCoroutine(FireAround());
        }
        else
        {
            StartCoroutine(Think(1));
        }
    }*/
}
