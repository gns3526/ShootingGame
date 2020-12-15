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
    public bool canHit;
    [SerializeField] bool isBoomActive;
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
                GameObject bullet = Instantiate(bullet0, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = Instantiate(bullet0, transform.position + Vector3.right * 0.1f, transform.rotation);
                GameObject bulletL = Instantiate(bullet0, transform.position + Vector3.left * 0.1f, transform.rotation);

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = Instantiate(bullet0, transform.position + Vector3.right * 0.25f, transform.rotation);
                GameObject bulletCC = Instantiate(bullet1, transform.position, transform.rotation);
                GameObject bulletLL = Instantiate(bullet0, transform.position + Vector3.left * 0.25f, transform.rotation);

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
    void UseBoom()
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

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyScript enemyScript = enemies[i].GetComponent<EnemyScript>();
            enemyScript.Hit(1000);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i]);
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
            Destroy(other.gameObject);
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
                    else power++;
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
            Destroy(other.gameObject);
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
