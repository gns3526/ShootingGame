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
    [SerializeField] float maxHealth;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image healthImage;
    [SerializeField] GameObject healthBarGameObject;

    [SerializeField] float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;

    [SerializeField] float stopTImeMonster;
    [SerializeField] float goTimeMonster;
    [SerializeField] float dashWaitTIme;
    [SerializeField] float dashToPlayerPower;
    [SerializeField] bool isDashing;
    [SerializeField] bool canMoveMonster;

    [SerializeField] float monster5ShotCool;
    [SerializeField] bool monster6bool;

    [SerializeField] SpriteRenderer spriteRendererEnemy;
    [SerializeField] Rigidbody2D rigid;

    public GameObject target;
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
    int a;

    float normalBulletDmg;
    float criticalPlusDamage;
    float finalDamage;
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

        health = maxHealth;
        healthImage.fillAmount = 1;

        canMoveMonster = true;
        curShotCoolTime = 0;
        isDashing = false;
        if (enemyType == "Monster6")
            monster6bool = true;

        transform.rotation = Quaternion.identity;
        healthBarGameObject.transform.rotation = Quaternion.identity;

        creat();

        if (PhotonNetwork.IsMasterClient)
        StartCoroutine(Stop());
    }
    private void OnDisable()
    {
        curPosPv = new Vector3(16, 16, 0);
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
            Vector2 dir = target.transform.position - transform.position;
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
                pv.RPC("Hit", RpcTarget.All, 10000f);
                //Hit(10000);
            }

            if (transform.position.y < target.transform.position.y && monster6bool)
            {
                monster6bool = false;
                int random = Random.Range(0, 2);
                if (random == 0)
                    StartCoroutine(ShotTypeX());
                else
                    StartCoroutine(ShotTypePlus());
            }


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

        if (curShotCoolTime > maxShotCoolTime)
        {
            if (target.GetComponent<Player>().isDie)
            {
                creat();
                return;
            }
            if (enemyType == "Monster1")
            {
                GameObject bullet = OP.PoolInstantiate("EnemyBullet1", transform.position, Quaternion.identity);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Debug.Log("qwdqwd");


                float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                curShotCoolTime = 0;

            }
            else if (enemyType == "Monster3")
            {
                GameObject bulletR = OP.PoolInstantiate("EnemyBullet2", transform.position + Vector3.right * 0.3f, Quaternion.identity);
                //GameObject bulletL = OM.MakeObj("BulletEnemy2");
                GameObject bulletL = OP.PoolInstantiate("EnemyBullet2", transform.position + Vector3.left * 0.3f, Quaternion.identity);

                Vector3 dirR = target.transform.position - (transform.position + Vector3.right * 0.3f);
                Vector3 dirL = target.transform.position - (transform.position + Vector3.left * 0.3f);

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                rigidR.AddForce(dirR.normalized * 5, ForceMode2D.Impulse);
                rigidL.AddForce(dirL.normalized * 5, ForceMode2D.Impulse);

                curShotCoolTime = 0;

            }
            else if (enemyType == "Monster4")
            {
                canMoveMonster = false;
                StartCoroutine(LookAtPlayer());
                isDashing = true;
                curShotCoolTime = -100;

            }
            else if(enemyType == "Monster5")
            {
                int random = Random.Range(0, 2);
                canMoveMonster = false;
                if (random == 0)
                    StartCoroutine(ShotTypeX());
                else
                    StartCoroutine(ShotTypePlus());
                curShotCoolTime = -100;
            }
            else if(enemyType == "Monster7")
            {
                GameObject bulletR = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                bulletR.GetComponent<BulletScript>().parentOb = gameObject;
                curShotCoolTime = -100;
                StartCoroutine(GoMonster());
            }
            else if (enemyType == "Monster8")
            {
                GameObject bulletR = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                GameObject bulletL = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                GameObject bulletU = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                GameObject bulletD = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                bulletR.transform.rotation = Quaternion.Euler(0, 0, 90);
                bulletL.transform.rotation = Quaternion.Euler(0, 0, -90);
                bulletU.transform.rotation = Quaternion.Euler(0, 0, 180);
                bulletD.transform.rotation = Quaternion.Euler(0, 0, 0);
                curShotCoolTime = -100;
                StartCoroutine(GoMonster());
            }
            else if (enemyType == "Monster9")
            {
                GameObject bulletRU = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                GameObject bulletRD = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                GameObject bulletLU = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                GameObject bulletLD = OP.PoolInstantiate("LaserS", transform.position, Quaternion.identity);
                bulletRU.transform.rotation = Quaternion.Euler(0, 0, 135);
                bulletRD.transform.rotation = Quaternion.Euler(0, 0, 45);
                bulletLU.transform.rotation = Quaternion.Euler(0, 0, -135);
                bulletLD.transform.rotation = Quaternion.Euler(0, 0, -45);
                curShotCoolTime = -100;
                StartCoroutine(GoMonster());
            }
        }
    }
    IEnumerator GoMonster()
    {
        yield return new WaitForSeconds(goTimeMonster);
        canMoveMonster = true;
    }
    IEnumerator LookAtPlayer()
    {

        if (target.activeSelf)
        {
            yield return new WaitForSeconds(dashWaitTIme);
            float angle = Mathf.Atan2(target.transform.position.y - gameObject.transform.position.y, target.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
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
    IEnumerator ShotTypeX()
    {
        yield return new WaitForSeconds(monster5ShotCool);
        GameObject bulletRUp = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
        GameObject bulletRDown = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
        GameObject bulletLUp = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
        GameObject bulletLDown = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);

        Rigidbody2D rigidRUp = bulletRUp.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRDown = bulletRDown.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLUpown = bulletLUp.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLDown = bulletLDown.GetComponent<Rigidbody2D>();

        rigidRUp.AddForce(new Vector2(4,4), ForceMode2D.Impulse);
        rigidRDown.AddForce(new Vector2(4,-4), ForceMode2D.Impulse);
        rigidLUpown.AddForce(new Vector2(-4,4), ForceMode2D.Impulse);
        rigidLDown.AddForce(new Vector2(-4,-4), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        canMoveMonster = true;
    }
    IEnumerator ShotTypePlus()
    {
        yield return new WaitForSeconds(monster5ShotCool);
        GameObject bulletRight = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
        GameObject bulletLeft = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
        GameObject bulletUp = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);
        GameObject bulletDown = OP.PoolInstantiate("EnemyBullet2", transform.position, Quaternion.identity);

        Rigidbody2D rigidRight = bulletRight.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLeft = bulletLeft.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidUp = bulletUp.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidDown = bulletDown.GetComponent<Rigidbody2D>();

        rigidRight.AddForce(new Vector2(4, 0), ForceMode2D.Impulse);
        rigidLeft.AddForce(new Vector2(-4, 0), ForceMode2D.Impulse);
        rigidUp.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
        rigidDown.AddForce(new Vector2(0, -4), ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        canMoveMonster = true;
    }

    void Reload()
    {
        curShotCoolTime += Time.deltaTime;
    }

    [PunRPC]
    public void Hit(float Dmg)
    {
        if (health <= 0)
            return;

        Debug.Log("dddddddfdfdfdfd");

        if (enemyType == "Boss1")
        {
            ani.SetTrigger("Hit");
        }
        else
        {
            spriteRendererEnemy.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }

        health -= Mathf.Round(Dmg);
        healthImage.fillAmount = health / maxHealth;
        if (health <= 0)
        {
            Debug.Log("dddddddfdfdfdfd111");
            
            myPlayerScript.score += enemyScore;
            if (GM.pv.IsMine)
            {
                if (myPlayerScript.isAttackSpeedStack)
                {
                    myPlayerScript.attackSpeedStackint++;

                    if (myPlayerScript.attackSpeedStackint == 1)
                    {
                        myPlayerScript.attackSpeedStackint = 0;
                        myPlayerScript.attackSpeedStack++;
                    }
                }
                if (myPlayerScript.isDamageStack)
                {
                    myPlayerScript.damageStackint++;

                    if (myPlayerScript.damageStackint == 1)
                    {
                        myPlayerScript.damageStackint = 0;
                        myPlayerScript.damageStack++;
                    }
                }
            }
                
            Debug.Log("dddddddfdfdfdfd222");


            if (enemyType == "Boss1")
            {
                //gmPv.RPC("StageEnd", RpcTarget.All);
                Debug.Log("dddddddfdfdfdfd333");
                GM.StageEnd();
            }


            if (PhotonNetwork.IsMasterClient)
                OP.PoolInstantiate("Explosion", transform.position, Quaternion.identity);


            patternIndex = -1;
            curPatternCount = 0;
            isSpawn = false;

            transform.rotation = Quaternion.identity;
            OP.PoolDestroy(gameObject);
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

            int randomNum;
            randomNum = Random.Range(0, 101);

            normalBulletDmg = bullet.dmg * (myPlayerScript.increaseDamage / 100)
                     * (myPlayerScript.damageStack / 100);


            if (myPlayerScript.criticalPer > randomNum)
                criticalPlusDamage = normalBulletDmg * (myPlayerScript.criticalDamagePer / 100);

            else
                criticalPlusDamage = normalBulletDmg;

            if (isBoss)
                finalDamage = criticalPlusDamage * (myPlayerScript.bossDamagePer / 100);
            else
                finalDamage = criticalPlusDamage;


            pv.RPC("Hit", RpcTarget.All, finalDamage);
            // normalBulletDmg = (bullet.dmg * (myPlayerScript.increaseDamage / 100))
            //* (myPlayerScript.bossDamagePer / 100)
            //* (((myPlayerScript.damageStack * 10) / 100) + 1);

                    //Hit((bullet.dmg * (player.GetComponent<Player>().increaseDamage / 100))
                    //    * (player.GetComponent<Player>().bossDamagePer / 100));

                
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
            //stream.SendNext(healthImage.fillAmount);
            stream.SendNext(targetRandomNum);
            stream.SendNext(transform.position);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            //healthImage.fillAmount = (float)stream.ReceiveNext();
            targetRandomNum = (int)stream.ReceiveNext();
            curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
