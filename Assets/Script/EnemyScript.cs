using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public string enemyType;
    [SerializeField] bool isBoss;
    [SerializeField] int enemyScore;
    public float speed;
    [SerializeField] float health;
    [SerializeField] float healthpv;
    [SerializeField] float maxHealth;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image healthImage;
    [SerializeField] GameObject healthBarGameObject;

    [SerializeField] float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;

    [SerializeField] float stopTImeMonster;
    [SerializeField] float dashWaitTIme;
    [SerializeField] float dashToPlayerPower;
    [SerializeField] bool isDashing;
    [SerializeField] bool canMoveMonster;

    [SerializeField] SpriteRenderer spriteRendererEnemy;
    [SerializeField] Rigidbody2D rigid;

    public GameObject player;
    public GameObject[] players;
    public Player myPlayerScript;
    public GameManager GM;
    public ObjectManager OM;
    [SerializeField] ObjectPooler OP;
    Animator ani;
    [SerializeField] int patternIndex;
    [SerializeField] int curPatternCount;
    [SerializeField] int[] MaxPatternCount;
    [SerializeField] float[] fireCoolTime;

    //
    [SerializeField] PhotonView pv;
    public PhotonView gmPv;
    public bool isSpawn;

    Vector3 curPosPv;

    [SerializeField] int targetRandomNum;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        OP = FindObjectOfType<ObjectPooler>();
        GM = FindObjectOfType<GameManager>();

        myPlayerScript = GM.myplayer.GetComponent<Player>();
        gmPv = GM.pv;
    }


    private void OnEnable()
    {
        curPosPv = new Vector3(16, 16, 0);

        creat();
        Debug.Log("켜짐");
        health = maxHealth;
        healthImage.fillAmount = 1;

        canMoveMonster = true;
        curShotCoolTime = 0;
        isDashing = false;

        transform.rotation = Quaternion.identity;
        healthBarGameObject.transform.rotation = Quaternion.identity;


        if(PhotonNetwork.IsMasterClient)
        StartCoroutine(Stop());
    }
    private void OnDisable()
    {
        curPosPv = new Vector3(16, 16, 0);
    }
    public void creat()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (PhotonNetwork.IsMasterClient)
        {
            targetRandomNum = Random.Range(0, PhotonNetwork.PlayerList.Length);
        }
        player = players[targetRandomNum]; // AA
    }



    public IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopTImeMonster);
        if (gameObject.activeSelf)
        {
            Debug.Log("멈춤");

            canMoveMonster = false;

            if(enemyType == "Boss1")
            {
                StartCoroutine(Think(0));
            }
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
        }
    }

    IEnumerator FireFoward()//앞으로 4발
    {
        GameObject bulletR = OP.PoolInstantiate("EnemyBullet4",transform.position,Quaternion.identity);
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = OP.PoolInstantiate("EnemyBullet4", transform.position, Quaternion.identity);
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = OP.PoolInstantiate("EnemyBullet4", transform.position, Quaternion.identity);
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = OP.PoolInstantiate("EnemyBullet4", transform.position, Quaternion.identity);
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[0]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            StartCoroutine(FireFoward());
        }
        else
        {
            StartCoroutine(Think(2));
        }
    }
    IEnumerator FireShot()//플래이어방향으로 샷건
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.identity);
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            //Vector2 dir = player[Random.Range(0,player.Length)].transform.position - transform.position;
            Vector2 dir = player.transform.position - transform.position;
            Vector2 randomVector = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2));
            dir += randomVector;
            rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
        }
        curPatternCount++;
        yield return new WaitForSeconds(fireCoolTime[1]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            //Invoke("FireShot", 0.3f);
            StartCoroutine(FireShot());
        }
        else
        {
            //Invoke("Think", 2);
            StartCoroutine(Think(2));
        }
    }
    IEnumerator FireArc()//부체모양
    {
        GameObject bullet = OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.identity);
        bullet.transform.position = transform.position;//초기화
        bullet.transform.rotation = Quaternion.identity;//초기화

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
            StartCoroutine(Think(3));
        }
    }
    IEnumerator FireAround()//원형태로 뿌림
    {
        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;//roundNumA와roundNumB의 수를 교차
        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = OP.PoolInstantiate("EnemyBullet3", transform.position, Quaternion.identity);
            bullet.transform.position = transform.position;//초기화
            bullet.transform.rotation = Quaternion.identity;//초기화

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
            StartCoroutine(Think(3));
        }
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GM.isPlaying)
            {
                //pv.RPC("Hit", RpcTarget.All, 10000);
                Hit(10000);
            }
            healthImage.fillAmount = health / maxHealth;
            Move();
            if (enemyType == "Boss1")
                return;

            Fire();
            Reload();
        }

        if (!pv.IsMine)
        {
            if ((transform.position - curPosPv).sqrMagnitude >= 3) transform.position = curPosPv;
            else
                transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 10);
        }
    }
    void Move()
    {
        if (canMoveMonster)
        {
            if (isDashing)
            {
                transform.Translate(new Vector2(0, -dashToPlayerPower));
            }
            else
            transform.Translate(new Vector2(0, -speed));
        }
    }
    void Fire()
    {
        if (player == null)
        {
            creat();
            return;
        }


        if (curShotCoolTime > maxShotCoolTime)
        {
            
            if (enemyType == "Monster1")
            {

                if (PhotonNetwork.IsMasterClient)
                {
                    //GameObject bullet = OM.MakeObj("BulletEnemy1");
                    GameObject bullet = OP.PoolInstantiate("EnemyBullet1", transform.position, Quaternion.identity);
                    //bullet.transform.position = transform.position;
                    Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                    Debug.Log("qwdqwd");

                    // Vector3 dir = player[Random.Range(0,player.Length)].transform.position - transform.position;

                    //Vector3 dir = players[Random.Range(0, players.Length)].transform.position - transform.position;

                    //Vector3 dir = player.transform.position - transform.position;
                    //rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
                    //rigid.AddForce(new Vector2(0,-5), ForceMode2D.Impulse);
                    
                    float angle = Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                    bullet.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                    curShotCoolTime = 0;
                }

            }
            else if (enemyType == "Monster3")
            {
                if (player.activeSelf)
                {

                    //GameObject bulletR = OM.MakeObj("BulletEnemy2");
                    if (PhotonNetwork.IsMasterClient)
                    {
                        GameObject bulletR = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
                        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
                        //GameObject bulletL = OM.MakeObj("BulletEnemy2");
                        GameObject bulletL = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
                        bulletL.transform.position = transform.position + Vector3.left * 0.3f;

                        Vector3 dirR = player.transform.position - (transform.position + Vector3.right * 0.3f);
                        Vector3 dirL = player.transform.position - (transform.position + Vector3.left * 0.3f);

                        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                        rigidR.AddForce(dirR.normalized * 5, ForceMode2D.Impulse);
                        rigidL.AddForce(dirL.normalized * 5, ForceMode2D.Impulse);
                        curShotCoolTime = 0;
                    }
                    
                }
                else
                {
                    //GameObject bulletR = OM.MakeObj("BulletEnemy2");
                    //bulletR.transform.position = transform.position + Vector3.right * 0.3f;
                    //Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    //GameObject bulletL = OM.MakeObj("BulletEnemy2");
                    //bulletL.transform.position = transform.position + Vector3.left * 0.3f;
                    //Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                    //rigidR.AddForce(Vector2.down, ForceMode2D.Impulse);
                    //rigidL.AddForce(Vector2.down, ForceMode2D.Impulse);
                    curShotCoolTime = 0;
                }
            }
            else if (enemyType == "Monster4")
            {
                    canMoveMonster = false;
                    StartCoroutine(LookAtPlayer());
                    isDashing = true;
                    curShotCoolTime = -100;
                
            }
        }
    }
    IEnumerator LookAtPlayer()
    {

        if (player.activeSelf)
        {
            yield return new WaitForSeconds(dashWaitTIme);
            float angle = Mathf.Atan2(player.transform.position.y - gameObject.transform.position.y, player.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            healthBarGameObject.transform.rotation = Quaternion.identity;

            canMoveMonster = true;
        }
        else
        {
            yield return new WaitForSeconds(dashWaitTIme);
            canMoveMonster = true;
        }
    }

    void Reload()
    {
        curShotCoolTime += Time.deltaTime;
    }

    [PunRPC]
    public void Hit(int Dmg)
    {
        if (!myPlayerScript.pv.IsMine) return;

        if (health <= 0)
            return;

        int randomNum;
        randomNum = Random.Range(0, 101);

        if (myPlayerScript.criticalPer > randomNum)
            health -= Mathf.Round(Dmg * (myPlayerScript.criticalDamagePer / 100));

        else
            health -= Mathf.Round(Dmg);



        if (enemyType == "Boss1")
        {
            ani.SetTrigger("Hit");
        }
        else
        {
            spriteRendererEnemy.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }


        if (health <= 0)
        {
            myPlayerScript.score += enemyScore;

            if (myPlayerScript.isAttackSpeedStack)
            {
                myPlayerScript.attackSpeedStackint++;

                if (myPlayerScript.attackSpeedStackint == 10)
                {
                    myPlayerScript.attackSpeedStackint = 0;
                    myPlayerScript.attackSpeedStack++;
                }
            }
            if (myPlayerScript.isDamageStack)
            {
                myPlayerScript.damageStackint++;

                if (myPlayerScript.damageStackint == 10)
                {
                    myPlayerScript.damageStackint = 0;
                    myPlayerScript.damageStack++;
                }
            }
            /*
            int random = enemyType == "Boss1" ? 0 : Random.Range(0, 10);
            if(random < 4)
            {
                //없으
            }
            else if (random < 6)//코인
            {
                GameObject ItemCoin = OM.MakeObj("ItemCoin");
                ItemCoin.transform.position = transform.position;
            }
            else if (random < 8)//파워
            {
                //GameObject ItemPow = OM.MakeObj("ItemPow");
                //ItemPow.transform.position = transform.position;
            }
            else if (random < 10)//폭
            {
                GameObject ItemBoom = OM.MakeObj("ItemBoom");
                ItemBoom.transform.position = transform.position;
            }*/
            patternIndex = -1;
            curPatternCount = 0;
            isSpawn = false;

            if (PhotonNetwork.IsMasterClient)
                OP.PoolInstantiate("Explosion", transform.position, Quaternion.identity);

            OP.PoolDestroy(gameObject);

            transform.rotation = Quaternion.identity;
            //GM.MakeExplosionEffect(transform.position, enemyType);

            if (enemyType == "Boss1")
            {
                gmPv.RPC("StageEnd", RpcTarget.All);
            }
        }






    }
    void ReturnSprite()
    {
        spriteRendererEnemy.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BulletBorder" && !isBoss)
        {
            isSpawn = false;
            OP.PoolDestroy(gameObject);
        }
        else if (other.tag == "PlayerBullet")
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            Player myPlayerScript = GM.myplayer.GetComponent<Player>();

            if (isBoss)
                pv.RPC("Hit", RpcTarget.All, (bullet.dmg * (myPlayerScript.increaseDamage / 100))
                    * (myPlayerScript.bossDamagePer / 100) * (((myPlayerScript.damageStack * 10)/100) + 1));
                //Hit((bullet.dmg * (player.GetComponent<Player>().increaseDamage / 100))
                //    * (player.GetComponent<Player>().bossDamagePer / 100));

            else
                pv.RPC("Hit", RpcTarget.All, (bullet.dmg * (myPlayerScript.increaseDamage / 100))
                    * (((myPlayerScript.damageStack * 10) / 100) + 1));
                //Hit((bullet.dmg * (player.GetComponent<Player>().increaseDamage / 100)));

                //Hit(bullet.dmg + player.GetComponent<Player>().increaseDamage);

            isSpawn = false;
            //other.gameObject.SetActive(false);


        }
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            stream.SendNext(health);
            stream.SendNext(healthImage.fillAmount);
            stream.SendNext(targetRandomNum);
            stream.SendNext(transform.position);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            healthImage.fillAmount = (float)stream.ReceiveNext();
            targetRandomNum = (int)stream.ReceiveNext();
            curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
