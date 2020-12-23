using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public int enemyNum;
    [SerializeField] int enemyScore;
    public float speed;
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image healthImage;

    [SerializeField] bool isShootingMonster;
    [SerializeField] bool isChasingMonster;

    [SerializeField] float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;

    [SerializeField] float stopTImeMonster;

    [SerializeField] SpriteRenderer spriteRendererEnemy;

    public GameObject player;
    public GameManager GM;
    public ObjectManager OM;
    Animator ani;

    [SerializeField] int patternIndex;
    [SerializeField] int curPatternCount;
    [SerializeField] int[] MaxPatternCount;
    [SerializeField] float[] fireCoolTime;

    public bool isSpawn;
    private void Awake()
    {

         ani = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        Debug.Log("켜짐");
        health = maxHealth;
        healthImage.fillAmount = 1;

        StartCoroutine(Stop());

    }
    
    public IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopTImeMonster);
        if (gameObject.activeSelf)
        {
            Debug.Log("멈춤");
            Rigidbody2D rigid = GetComponent<Rigidbody2D>();
            rigid.velocity = Vector2.zero;

            StartCoroutine(Think(0));
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
        GameObject bulletR = OM.MakeObj("BulletBoss1");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = OM.MakeObj("BulletBoss1");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = OM.MakeObj("BulletBoss1");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = OM.MakeObj("BulletBoss1");
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
        Debug.Log("44444444444444");
        yield return new WaitForSeconds(fireCoolTime[0]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            //Invoke("FireFoward", 1f);
            StartCoroutine(FireFoward());
        }
        else
        {
            //Invoke("Think", 2);
            StartCoroutine(Think(2));
        }
    }
    IEnumerator FireShot()//플래이어방향으로 샷건
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = OM.MakeObj("BulletBoss0");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dir = player.transform.position - transform.position;
            Vector2 randomVector = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2));
            dir += randomVector;
            rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
        }
        curPatternCount++;
        Debug.Log("33333333333333");
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
        GameObject bullet = OM.MakeObj("BulletBoss0");
        bullet.transform.position = transform.position;//초기화
        bullet.transform.rotation = Quaternion.identity;//초기화

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / MaxPatternCount[patternIndex]), -1);//Cos도 가능
        rigid.AddForce(dir.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;
        Debug.Log("22222222222");
        yield return new WaitForSeconds(fireCoolTime[2]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            //Invoke("FireArc", 0.1f);
            StartCoroutine(FireArc());
        }
        else
        {
            //Invoke("Think", 3);
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
            GameObject bullet = OM.MakeObj("BulletBoss0");
            bullet.transform.position = transform.position;//초기화
            bullet.transform.rotation = Quaternion.identity;//초기화

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));//Cos도 가능
            rigid.AddForce(dir.normalized * 2/*속도*/, ForceMode2D.Impulse);

            Vector3 roVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(roVec);

        }

        curPatternCount++;
        Debug.Log("111111111111");
        yield return new WaitForSeconds(fireCoolTime[3]);
        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            //Invoke("FireAround", 1f);
            StartCoroutine(FireAround());
        }
        else
        {
            //Invoke("Think", 3);
            StartCoroutine(Think(3));
        }
    }
    private void Update()
    {
        if (enemyNum == 4)
            return;
        Fire();
        Reload();
    }
    void Fire()
    {
        if (curShotCoolTime < maxShotCoolTime) return;

        if(enemyNum == 1)
        {
            GameObject bullet = OM.MakeObj("BulletEnemy0");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dir = player.transform.position - transform.position;
            rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
        }
        else if(enemyNum == 2)
        {
            GameObject bulletR = OM.MakeObj("BulletEnemy1");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            GameObject bulletL = OM.MakeObj("BulletEnemy1");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Vector3 dirR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            rigidR.AddForce(dirR.normalized * 5, ForceMode2D.Impulse);
            rigidL.AddForce(dirL.normalized * 5, ForceMode2D.Impulse);
        }

        curShotCoolTime = 0;
    }

    void Reload()
    {
        curShotCoolTime += Time.deltaTime;
    }
    public void Hit(int Dmg)
    {
        if (health <= 0)
            return;

        health -= Dmg;
        healthImage.fillAmount = health / maxHealth;
        if (enemyNum == 4)
        {
            ani.SetTrigger("Hit");
        }
        else
        {
            spriteRendererEnemy.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }



        if(health <= 0)
        {
            Player playerScript = player.GetComponent<Player>();
            playerScript.score += enemyScore;


            int random = enemyNum == 4 ? 0 : Random.Range(0, 10);
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
                GameObject ItemPow = OM.MakeObj("ItemPow");
                ItemPow.transform.position = transform.position;
            }
            else if (random < 10)//폭
            {
                GameObject ItemBoom = OM.MakeObj("ItemBoom");
                ItemBoom.transform.position = transform.position;
            }
            patternIndex = -1;
            curPatternCount = 0;
            isSpawn = false;
            


            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            GM.MakeExplosionEffect(transform.position, enemyNum);

            //보스죽음
            if(enemyNum == 4)
            {
                GM.StageEnd();
            }
        }
    }
    void ReturnSprite()
    {
        spriteRendererEnemy.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BulletBorder" && enemyNum != 4)
        {
            isSpawn = false;
            gameObject.SetActive(false);
        }
        else if (other.tag == "PlayerBullet")
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            Hit(bullet.dmg);

            isSpawn = false;
            other.gameObject.SetActive(false);
        }
    }
}
