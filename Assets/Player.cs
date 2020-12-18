using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] bool isTouchTop;
    [SerializeField] bool isTouchLeft;
    [SerializeField] bool isTouchRight;
    [SerializeField] bool isTouchBottom;

    public int life;
    public int score;
    [SerializeField] float moveSpeed;
    [SerializeField] int power;
    [SerializeField] int maxPower;
    [SerializeField] int boom;
    [SerializeField] int maxBoom;
    [SerializeField] float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;

    [SerializeField] GameObject bullet0;
    [SerializeField] GameObject bullet1;
    [SerializeField] GameObject boomEffect;

    [SerializeField] Animator ani;

    [SerializeField] GameManager GM;
    [SerializeField] ObjectManager OM;
    public bool canHit;
    [SerializeField] bool isBoomActive;

    [SerializeField] GameObject[] followers;
    private void Update()
    {
        Move();
        Fire();
        Reload();
        UseBoom();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1)) h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1)) v = 0;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * moveSpeed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            ani.SetInteger("AxisX", (int)h);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1")) return;

        if (curShotCoolTime < maxShotCoolTime) return;

        switch (power)
        {
            case 1:
                GameObject bullet = OM.MakeObj("BulletPlayer0");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = OM.MakeObj("BulletPlayer0");
                GameObject bulletL = OM.MakeObj("BulletPlayer0");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default:
                GameObject bulletRR = OM.MakeObj("BulletPlayer0");
                bulletRR.transform.position = transform.position + Vector3.right * 0.35f;

                GameObject bulletCC = OM.MakeObj("BulletPlayer1");
                bulletCC.transform.position = transform.position;

                GameObject bulletLL = OM.MakeObj("BulletPlayer0");
                bulletLL.transform.position = transform.position + Vector3.left * 0.35f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }


        curShotCoolTime = 0;
    }

    void Reload()
    {
        curShotCoolTime += Time.deltaTime;
    }
    void UseBoom()//폭탄사용
    {
        if (!Input.GetButton("Fire2"))
            return;
        if (isBoomActive)
            return;
        if (boom == 0)
            return;

        boom--;
        isBoomActive = true;
        GM.UpdateBoomIcon(boom);

        boomEffect.SetActive(true);
        Invoke("BoomFalse", 4);

        GameObject[] enemyL = OM.GetPool("EnemyL");
        GameObject[] enemyM = OM.GetPool("EnemyM");
        GameObject[] enemyS = OM.GetPool("EnemyS");
        for (int i = 0; i < enemyL.Length; i++)//모든적데미지주기
        {
            if (enemyL[i].activeSelf)
            {
                EnemyScript enemyScript = enemyL[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemyM.Length; i++)//모든적데미지주기
        {
            if (enemyM[i].activeSelf)
            {
                EnemyScript enemyScript = enemyM[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }
        for (int i = 0; i < enemyS.Length; i++)//모든적데미지주기
        {
            if (enemyS[i].activeSelf)
            {
                EnemyScript enemyScript = enemyS[i].GetComponent<EnemyScript>();
                enemyScript.Hit(1000);
            }
        }

        GameObject[] BulletEnemy0 = OM.GetPool("BulletEnemy0");
        GameObject[] bulletEnemy1 = OM.GetPool("BulletEnemy1");

        for (int i = 0; i < BulletEnemy0.Length; i++)
        {
            if (BulletEnemy0[i].activeSelf)
            {
                BulletEnemy0[i].SetActive(false);
            }
        }
        for (int i = 0; i < bulletEnemy1.Length; i++)
        {
            if (BulletEnemy0[i].activeSelf)
            {
                bulletEnemy1[i].SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Border")
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
            }
        }
        else if (other.tag == "Enemy" || other.tag == "EnemyBullet")
        {
            if (canHit)
            {
                life--;
            }
            canHit = false;
            GM.UpdateLifeIcon(life);

            if(life == 0)
            {
                GM.GameOver();
            }
            else
            {
                GM.ReSpawnM();
            }
            gameObject.SetActive(false);
            
        }
        else if(other.tag == "Item")
        {
            Item itemScript = other.GetComponent<Item>();
            switch (itemScript.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Pow":
                    if (power == maxPower)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Boom":
                    if (boom == maxBoom)
                    {
                        score += 500;
                    }
                    else
                    {
                        boom++;
                        GM.UpdateBoomIcon(boom);
                    }
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }

    void AddFollower()
    {
        if(power == 4)
        {
            followers[0].SetActive(true);
        }
        else if (power == 5)
        {
            followers[1].SetActive(true);
        }
        else if (power == 6)
        {
            followers[2].SetActive(true);
        }
    }

    void BoomFalse()
    {
        boomEffect.SetActive(false);
        isBoomActive = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Border")
        {
            switch (other.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
            }
        }
    }
}
