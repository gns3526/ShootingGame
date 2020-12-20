using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] string enemyName;
    [SerializeField] int enemyScore;
    public float speed;
    [SerializeField] int health;
    [SerializeField] int maxHealth;
    [SerializeField] Sprite[] sprites;

    [SerializeField] float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;

    [SerializeField] GameObject bullet0;
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject coinItem;
    [SerializeField] GameObject powItem;
    [SerializeField] GameObject boomItem;

    [SerializeField] SpriteRenderer renderer;

    public GameObject player;
    public GameManager GM;
    public ObjectManager OM;
    Animator ani;

    [SerializeField] int patternIndex;
    [SerializeField] int curPatternCount;
    [SerializeField] int[] MaxPatternCount;

    [SerializeField] bool once;
    private void Awake()
    {
        if(enemyName == "Boss0")
        {
            ani = GetComponent<Animator>();
        }
    }
    private void OnEnable()
    {
        health = maxHealth;
        if(enemyName == "Boss0")
        {
            Invoke("Stop", 3);
        }
    }

    void Stop()
    {
        Debug.Log("멈춤");
        if (!gameObject.activeSelf)//활성화 되어있지 않다면
            return;//되돌림
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        Debug.Log("Todrkrk");
        if (once)
        {
            once = false;
            Invoke("Think", 2);
        }
    }

    void Think()
    {
        Debug.Log("생각");
        patternIndex = patternIndex >= 3 ? 0 : patternIndex + 1;//패턴갯수 오버하면 0으로만듬
        curPatternCount = 0;
        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()//앞으로 4발
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
        Debug.Log("더하기");

        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            Invoke("FireFoward", 1f);
        }
        else
        {
            Invoke("Think", 2);
        }
    }
    void FireShot()//플래이어방향으로 샷건
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
        Debug.Log("더하기");

        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            Invoke("FireShot", 0.3f);
        }
        else
        {
            Invoke("Think", 2);
        }
    }
    void FireArc()//부체모양
    {
        GameObject bullet = OM.MakeObj("BulletBoss0");
        bullet.transform.position = transform.position;//초기화
        bullet.transform.rotation = Quaternion.identity;//초기화

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / MaxPatternCount[patternIndex]), -1);//Cos도 가능
        rigid.AddForce(dir.normalized * 5, ForceMode2D.Impulse);

        curPatternCount++;
        Debug.Log("더하기");

        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            Invoke("FireArc", 0.1f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }
    void FireAround()//원형태로 뿌림
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
        Debug.Log("더하기");

        if (curPatternCount < MaxPatternCount[patternIndex])
        {
            Invoke("FireAround", 1f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }
    private void Update()
    {
        if (enemyName == "Boss0")
            return;
        Fire();
        Reload();
    }
    void Fire()
    {
        if (curShotCoolTime < maxShotCoolTime) return;

        if(enemyName == "S")
        {
            GameObject bullet = OM.MakeObj("BulletEnemy0");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dir = player.transform.position - transform.position;
            rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
        }
        else if(enemyName == "L")
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
        if(enemyName == "Boss0")
        {
            ani.SetTrigger("Hit");
        }
        else
        {
            renderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }



        if(health <= 0)
        {
            Player playerScript = player.GetComponent<Player>();
            playerScript.score += enemyScore;


            int random = enemyName == "Boss0" ? 0 : Random.Range(0, 10);
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
            once = true;
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            GM.MakeExplosionEffect(transform.position, enemyName);

            //보스죽음
            if(enemyName == "Boss0")
            {
                GM.StageEnd();
            }
        }
    }
    void ReturnSprite()
    {
        renderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BulletBorder" && enemyName != "Boss0") gameObject.SetActive(false);
        else if (other.tag == "PlayerBullet")
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            Hit(bullet.dmg);

            other.gameObject.SetActive(false);
        }
    }
}
