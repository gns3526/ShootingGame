using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Boss3 : MonoBehaviour
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

    int pattern2Index;

    int a;
    int targetRandomNum;

    GameObject curShotBullet;
    GameObject bulletPv;
    [SerializeField] float[] bulletAniDelay;
    public Sprite[] bulletAni1;
    public Sprite[] bulletAni2;
    public Sprite[] bulletAni3;

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
        }
    }


    IEnumerator Rest(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("생각");

        curPatternCount = 0;
        creat();

        int random = Random.Range(0, 3);
        Debug.Log(random);
        switch (random)
        {
            case 0:
                StartCoroutine(Pattern1());
                break;
            case 1:
                StartCoroutine(Pattern2());
                break;
            case 2:
                StartCoroutine(Pattern3());
                break;
        }
    }

    IEnumerator Pattern1()//
    {
        curShotBullet = EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, 0), 3, EB.bulletSpeedCode[0], false);
        EB.pv.RPC("BulletAniRPC", RpcTarget.All, 0);

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[0]);
        if (curPatternCount < MaxPatternCount[0] && EB.canFire)
        {
            StartCoroutine(Pattern1());
        }
        else
        {
            StartCoroutine(Rest(2f));
        }
    }



    IEnumerator Pattern2()//플래이어에게 연속발사
    {
        pattern2Index++;
        if (pattern2Index == 3)
            pattern2Index = 0;

        switch (pattern2Index)
        {
            case 0:
                curShotBullet = EB.OP.PoolInstantiate("BulletBasic", transform.position + Vector3.right * 0.4f, Quaternion.Euler(0, 0, 0), 4, EB.bulletSpeedCode[1], false);
                break;
            case 1:
                curShotBullet = EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, 0), 4, EB.bulletSpeedCode[1], false);
                break;
            case 2:
                curShotBullet = EB.OP.PoolInstantiate("BulletBasic", transform.position + Vector3.left * 0.4f, Quaternion.Euler(0, 0, 0), 4, EB.bulletSpeedCode[1], false);
                break;
        }
        EB.pv.RPC("BulletAniRPC", RpcTarget.All, 1);

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[1]);
        if (curPatternCount < MaxPatternCount[1] && EB.canFire)
        {
            StartCoroutine(Pattern2());
        }
        else
        {
            StartCoroutine(Rest(2));
        }
    }

    IEnumerator Pattern3()
    {
        float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;


        curShotBullet = EB.OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, angle + 90), 5, EB.bulletSpeedCode[2], false);
        EB.pv.RPC("BulletAniRPC", RpcTarget.All, 2);

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[2]);
        if (curPatternCount < MaxPatternCount[2] && EB.canFire)
        {
            StartCoroutine(Pattern3());
        }
        else
        {
            StartCoroutine(Rest(2));
        }
    }


    [PunRPC]
    void BulletAniRPC(int pattern)
    {
        Debug.Log("총알 0");
        switch (pattern)
        {
            case 0:
                for (int i = 0; i < bulletAni1.Length; i++)
                    curShotBullet.GetComponent<BulletScript>().bulletAniSprites[i] = bulletAni1[i];
                Debug.Log("총알 1");
                break;
            case 1:
                for (int i = 0; i < bulletAni2.Length; i++)
                    curShotBullet.GetComponent<BulletScript>().bulletAniSprites[i] = bulletAni2[i];
                Debug.Log("총알 2");
                break;
            case 2:
                for (int i = 0; i < bulletAni3.Length; i++)
                    curShotBullet.GetComponent<BulletScript>().bulletAniSprites[i] = bulletAni3[i];
                Debug.Log("총알 3");
                break;
        }
        curShotBullet.GetComponent<BulletScript>().bulletAniDelay = bulletAniDelay[pattern];
        Debug.Log("총알 4");
    }
}
