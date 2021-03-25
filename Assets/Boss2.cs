using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Boss2 : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] EnemyBasicScript EB;
    [SerializeField] GameManager GM;

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
        GM = FindObjectOfType<GameManager>();

        if (!PhotonNetwork.IsMasterClient) return;

        StartCoroutine(Stop());
    }

    private void Update()
    {
        if (canMove)
            transform.Translate(new Vector2(0, -moveSpeed));
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
    public IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopCool);
        if (gameObject.activeSelf)
        {
            Debug.Log("멈춤");

            canMove = false;

            StartCoroutine(Think(2));
        }
    }

    IEnumerator Rest(float restTime)
    {
        yield return new WaitForSeconds(restTime);
        switch (patternIndex)
        {
            case 0:
                StartCoroutine(Pattern1());
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
        }
    }

    IEnumerator Think(float waitTime)
    {
        //if (!gameObject.activeSelf)//활성화 되어있지 않다면
        //    return;//되돌림
        yield return new WaitForSeconds(waitTime);
        Debug.Log("생각");
        
        curPatternCount = 0;
        creat();
        switch (patternIndex)
        {
            case 0:
                StartCoroutine(Pattern1());
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
        }
    }
    IEnumerator Pattern1()//플래이어에게 연속발사
    {
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        int randomAngle = Random.Range(-10, 11);
        int randomFire = Random.Range(1, 5);//0.3 0.45

        if(randomFire == 1)
        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;
        else if(randomFire == 2)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;
        else if (randomFire == 3)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;
        else if (randomFire == 4)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[0]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            StartCoroutine(Pattern1());
        }
        else
        {
            curPatternCount = 0;
            StartCoroutine(Rest(4f));
        }
    }
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
            rigid.AddForce(dir.normalized * 2/*속도*/, ForceMode2D.Impulse);

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
    }
}
