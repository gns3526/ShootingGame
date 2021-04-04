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
        EB.godMode = true;
        curPatternCount = 0;
        EB.patternIndex = 0;
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

            EB.godMode = false;
            canMove = false;

            StartCoroutine(Rest(2));
            //StartCoroutine(Attack1());
        }
    }


    IEnumerator Rest(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("생각");

        curPatternCount = 0;
        creat();
        switch (EB.patternIndex)
        {
            case 0:
                //StartCoroutine(Pattern1());
                break;
            case 1:
                //StartCoroutine(Pattern2());
                break;
            case 2:
               // StartCoroutine(Pattern3());
                break;
            case 3:
              //  StartCoroutine(Pattern4());
                break;
        }
    }
    /*
    IEnumerator Pattern1()//플래이어에게 연속발사
    {
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        int randomAngle = Random.Range(-10, 11);
        int randomFire = Random.Range(1, 5);//0.3 0.45



        if (randomFire == 1)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;
        else if (randomFire == 2)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;
        else if (randomFire == 3)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;
        else if (randomFire == 4)
            EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle + 90)).GetComponent<BulletScript>().bulletSpeed = 0.14f;

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[0]);
        if (curPatternCount < MaxPatternCount[EB.patternIndex] && EB.canFire)
        {
            StartCoroutine(Pattern1());
        }
        else
        {
            curPatternCount = 0;
            StartCoroutine(Rest(4f));
        }
    }

    

    IEnumerator Pattern2()//플래이어에게 연속발사
    {
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
        int randomAngle1 = Random.Range(-5, 6);
        int randomAngle2 = Random.Range(-5, 6);
        int randomAngle3 = Random.Range(-5, 6);
        int randomAngle4 = Random.Range(-5, 6);
        int randomAngle5 = Random.Range(-5, 6);
        int randomAngle6 = Random.Range(-5, 6);
        int randomAngle7 = Random.Range(-5, 6);
        int randomAngle8 = Random.Range(-5, 6);

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle1 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle2 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle3 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle4 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle5 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.right * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle6 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, angle + randomAngle7 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        EB.OP.PoolInstantiate("EnemyBullet4", transform.position + Vector3.left * 0.45f, Quaternion.Euler(0, 0, angle + randomAngle8 + 90)).GetComponent<BulletScript>().bulletSpeed = 0.2f;

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[1]);
        if (curPatternCount < MaxPatternCount[EB.patternIndex] && EB.canFire)
        {
            StartCoroutine(Pattern2());
        }
        else
        {
            curPatternCount = 0;
            StartCoroutine(Rest(0));
        }
    }

    IEnumerator Pattern3()
    {
        
        int randomAngle = Random.Range(0, 361);

        for (int i = 0; i < 40; i++)
        {
            EB.OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.AngleAxis(i * 9 + randomAngle, Vector3.forward)).GetComponent<BulletScript>().bulletSpeed = 0.08f;
        }

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[2]);
        if (curPatternCount < MaxPatternCount[EB.patternIndex] && EB.canFire)
        {
            StartCoroutine(Pattern3());
        }
        else
        {
            curPatternCount = 0;
            StartCoroutine(Rest(2));
        }
    }
    IEnumerator Pattern4()
    {
        
        int randomAngle = Random.Range(0, 361);

        EB.OP.PoolInstantiate("LaserMiddle", target.transform.position, Quaternion.Euler(0, 0, randomAngle));



        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[3]);
        if (curPatternCount < MaxPatternCount[EB.patternIndex] && EB.canFire)
        {
            StartCoroutine(Pattern4());
        }
        else
        {
            curPatternCount = 0;
            StartCoroutine(Rest(0));
        }
    }

    IEnumerator Attack1()
    {
        yield return new WaitForSeconds(5);

        if(EB.canFire)
        for (int i = 0; i < 40; i++)
        {
            EB.OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.AngleAxis(i * 9, Vector3.forward)).GetComponent<BulletScript>().bulletSpeed = 0.08f;
        }
        if(EB.patternIndex <= 2)
        StartCoroutine(Attack1());
    }*/
}



    

