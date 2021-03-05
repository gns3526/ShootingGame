using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyBasicScript : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] string enemyInfo;
    [SerializeField] int enemyScore;
    [SerializeField] bool isBoss;

    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image healthImage;
    public GameObject healthBarGameObject;

    [SerializeField] SpriteRenderer spriteRendererEnemy;
    public PhotonView pv;

    public Player myPlayerScript;
    public GameManager GM;
    public ObjectManager OM;
    public ObjectPooler OP;
    Animator ani;

    [SerializeField] int targetRandomNum;
    int a;

    float normalBulletDmg;
    float criticalPlusDamage;
    float finalDamage;

    Vector3 curPosPv;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        OP = FindObjectOfType<ObjectPooler>();
        GM = FindObjectOfType<GameManager>();

        myPlayerScript = GM.myplayer.GetComponent<Player>();
    }

    private void OnEnable()
    {
        health = maxHealth;
        healthImage.fillAmount = 1;
        transform.rotation = Quaternion.identity;
        healthBarGameObject.transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        curPosPv = new Vector3(16, 16, 0);
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
        }

        if (!pv.IsMine)
        {
            if ((transform.position - curPosPv).sqrMagnitude >= 3) transform.position = curPosPv;
            else
                transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 10);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BulletBorder" && !isBoss)
        {
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
        }
    }

    void ReturnSprite()
    {
        spriteRendererEnemy.sprite = sprites[0];
    }


    [PunRPC]
    public void Hit(float Dmg)
    {
        if (health <= 0)
            return;

        spriteRendererEnemy.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);


        health -= Mathf.Round(Dmg);
        healthImage.fillAmount = health / maxHealth;
        Debug.Log(Dmg);
        if (health <= 0)
        {

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

            if (isBoss)
            {
                GM.StageEnd();
            }

            if (PhotonNetwork.IsMasterClient)
                OP.PoolInstantiate("Explosion", transform.position, Quaternion.identity);

            transform.rotation = Quaternion.identity;
            OP.PoolDestroy(gameObject);
        }
    }

    [PunRPC]
    public void DistroyOb()
    {
        OP.PoolDestroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            stream.SendNext(health);
            stream.SendNext(targetRandomNum);
            stream.SendNext(transform.position);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            targetRandomNum = (int)stream.ReceiveNext();
            curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
