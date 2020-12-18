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
    public ObjectManager OM;
    Animator ani;

    [SerializeField] int patternIndex;
    [SerializeField] int curPatternCount;
    [SerializeField] int[] MaxPatternCount;
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
            Invoke("Stop", 2);
            
        }
    }

    void Stop()
    {
        if (!gameObject.activeSelf)//활성화 되어있지 않다면
            return;//되돌림
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;//패턴갯수 오버하면 0으로만듬
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
        curPatternCount++;

        if(curPatternCount < MaxPatternCount[patternIndex])
            Invoke("FireFoward", 2);
        Invoke("Think", 2);
    }
    void FireShot()//플래이어방향으로 샷건
    {
        curPatternCount++;

        if (curPatternCount < MaxPatternCount[patternIndex])
            Invoke("FireShot", 2);
        Invoke("Think", 2);
    }
    void FireArc()//부체모양
    {
        curPatternCount++;

        if (curPatternCount < MaxPatternCount[patternIndex])
            Invoke("FireArc", 2);
        Invoke("Think", 2);
    }
    void FireAround()//원형태로 뿌림
    {
        curPatternCount++;

        if (curPatternCount < MaxPatternCount[patternIndex])
            Invoke("FireAround", 2);
        Invoke("Think", 2);
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

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
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
