using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] string enemyName;
    [SerializeField] int enemyScore;
    public float speed;
    [SerializeField] int health;
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
    private void Update()
    {
        Fire();
        Reload();
    }
    void Fire()
    {
        if (curShotCoolTime < maxShotCoolTime) return;

        if(enemyName == "S")
        {
            GameObject bullet = Instantiate(bullet0, transform.position, transform.rotation);
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dir = player.transform.position - transform.position;
            rigid.AddForce(dir.normalized * 4, ForceMode2D.Impulse);
        }
        else if(enemyName == "L")
        {
            GameObject bulletR = Instantiate(bullet1, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bullet1, transform.position + Vector3.left * 0.3f, transform.rotation);

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
        renderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);
        if(health <= 0)
        {
            Player playerScript = player.GetComponent<Player>();
            playerScript.score += enemyScore;


            int random = Random.Range(0, 10);
            if(random < 4)
            {

            }
            else if (random < 6)//코인
            {
                Instantiate(coinItem, transform.position, Quaternion.identity);
            }
            else if (random < 8)//파워
            {
                Instantiate(powItem, transform.position, Quaternion.identity);
            }
            else if (random < 10)//폭
            {
                Instantiate(boomItem, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
    void ReturnSprite()
    {
        renderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BulletBorder") Destroy(gameObject);
        else if (other.tag == "PlayerBullet")
        {
            BulletScript bullet = other.GetComponent<BulletScript>();
            Hit(bullet.dmg);

            Destroy(other.gameObject);
        }
    }
}
