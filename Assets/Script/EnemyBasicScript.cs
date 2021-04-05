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
    public bool isPassingNodamage;
    public bool isLast;
    public bool godMode;
    public bool isBoss;
    public bool canFire;
    public int patternIndex;
    public int maxPattern;
    public bool reVive;

    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Image healthImage;
    public GameObject healthBarGameObject;
    public int[] bulletCode;
    public int[] bulletSpeedCode;

    [SerializeField] SpriteRenderer spriteRendererEnemy;
    public PhotonView pv;

    Player myPlayerScript;
    GameManager GM;
    public ObjectPooler OP;
    Animator ani;

    BulletScript bulletScript;

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
        canFire = true;
        healthImage.fillAmount = 1;
        transform.rotation = Quaternion.identity;
        healthBarGameObject.transform.rotation = Quaternion.identity;
    }

    private void OnDisable()
    {
        curPosPv = new Vector3(16, 16, 0);
        isLast = false;
    }


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GM.isPlaying)
            {
                //pv.RPC("Hit", RpcTarget.All, 10000f);
                //Hit(10000);
                OP.PoolDestroy(gameObject);
            }
        }

        if (reVive)
        {
            healthImage.fillAmount += 0.01f;
            if(healthImage.fillAmount == 1)
            {
                GM.allBulletDelete = false;
                canFire = true;
                health = maxHealth;
                reVive = false;
                godMode = false;
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
        if (other.tag == "BulletBorder" && !isBoss)//보스는 해당안됌
        {
            //일반 몬스터 해당
            if(!isLast)
            OP.PoolDestroy(gameObject);
            else
            {
                GM.pv.RPC("StageEnd", RpcTarget.All);
                OP.PoolDestroy(gameObject);
            }
        }
        else if (other.tag == "Bullet")
        {
            if (!other.GetComponent<BulletScript>().isPlayerAttack) return;

            if (godMode)
                return;

            bulletScript = other.GetComponent<BulletScript>();
            Player myPlayerScript = GM.myplayer.GetComponent<Player>();

            bool isFollowerAttack = bulletScript.isFollowerAttack;
            float followerPenalty = 1;
            int followerDamagePer = 100;

            int randomNum;
            randomNum = Random.Range(0, 101);

            if (isFollowerAttack)
            {
                followerPenalty = 0.5f;
                followerDamagePer = myPlayerScript.followerDamagePer;
            }



            normalBulletDmg = (bulletScript.dmg + myPlayerScript.damage) * (myPlayerScript.increaseDamagePer / 100)
                     * (myPlayerScript.damageStack / 100) * (followerDamagePer / 100);


            if (myPlayerScript.criticalPer > randomNum)
                criticalPlusDamage = normalBulletDmg * (myPlayerScript.criticalDamagePer / 100);

            else
                criticalPlusDamage = normalBulletDmg;

            if (isBoss)
                finalDamage = criticalPlusDamage * (myPlayerScript.bossDamagePer / 100);
            else
                finalDamage = criticalPlusDamage;

            finalDamage = finalDamage * (myPlayerScript.finalDamagePer / 100) * followerPenalty;

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
            if (isBoss && patternIndex < maxPattern)
            {
                if (PhotonNetwork.IsMasterClient)
                    GM.allBulletDelete = true;
                godMode = true;
                canFire = false;
                reVive = true;
                patternIndex += 1;
                return;
            }
            
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

            if (isLast)
            {
                GM.pv.RPC("StageEnd",RpcTarget.All);
            }

            if (PhotonNetwork.IsMasterClient)
              //  OP.PoolInstantiate("Explosion", transform.position, Quaternion.identity);

            transform.rotation = Quaternion.identity;

            if(PhotonNetwork.IsMasterClient)
            OP.PoolDestroy(gameObject);
        }
    }

    [PunRPC]
    void GodModeRPC()
    {
        godMode = true;
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
