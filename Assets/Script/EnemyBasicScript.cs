﻿using System.Collections;
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
    public GameManager GM;
    public ObjectPooler OP;
    Animator ani;

    BulletScript bulletScript;

    [SerializeField] int targetRandomNum;
    int a;

    float normalBulletDmg;
    float criticalPlusDamage;
    float finalDamage;

    Vector3 curPosPv;

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

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void Start()
    {
        if (!pv.IsMine) return;
        ani = GetComponent<Animator>();
        myPlayerScript = GM.myplayer.GetComponent<Player>();
    }


    private void Update()
    {
        healthImage.fillAmount = health / maxHealth;

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
                pv.RPC(nameof(GodModeRPC), RpcTarget.All, false);
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

            bool ispetAttack = bulletScript.ispetAttack;
            float petPenalty = 1;
            float petDamagePer = 100;

            bool isCritical;

            int randomNum;
            randomNum = Random.Range(0, 101);

            if (ispetAttack)
            {
                petPenalty = 0.5f;
                petDamagePer = myPlayerScript.petDamagePer;
            }

            // 내스텟 + (내 스텟 * 계수/100)

            normalBulletDmg = myPlayerScript.damage + (myPlayerScript.damage * (myPlayerScript.increaseDamagePer / 100)) + (myPlayerScript.damage * (bulletScript.dmgPer / 100))
                     + (myPlayerScript.damage * (myPlayerScript.damageStack / 100)) + (myPlayerScript.damage * (petDamagePer / 100));


            if (myPlayerScript.criticalPer > randomNum)
            {
                isCritical = true;
                criticalPlusDamage = normalBulletDmg + (normalBulletDmg * (myPlayerScript.criticalDamagePer / 100));
            }
            else
            {
                isCritical = false;
                criticalPlusDamage = normalBulletDmg;
            }


            if (isBoss)
                finalDamage = criticalPlusDamage + (criticalPlusDamage * (myPlayerScript.bossDamagePer / 100));
            else
                finalDamage = criticalPlusDamage;

            finalDamage = (finalDamage + (finalDamage * (myPlayerScript.finalDamagePer / 100))) * petPenalty;

            Debug.Log(finalDamage);

            float finalDamageInt = Mathf.Round(finalDamage);

            pv.RPC(nameof(Hit), RpcTarget.All, finalDamage);

            if (isCritical)
                OP.PoolInstantiate("DamageText", transform.position, Quaternion.identity, -4 , (int)finalDamage, 2, false);
            else
                OP.PoolInstantiate("DamageText", transform.position, Quaternion.identity, -4 , (int)finalDamage, 1, false);

            Debug.Log(myPlayerScript.damage + "+("+ myPlayerScript.damage + "* (" + myPlayerScript.increaseDamagePer + " / " + 100 + ")) + (" + myPlayerScript.damage + "* (" + bulletScript.dmgPer + " / " + 100 + "))"
                     +"+ (" + myPlayerScript.damage + "* (" + myPlayerScript.damageStack + " / " + 100 + ")) + (" + myPlayerScript.damage + "* (" + petDamagePer + " / " + 100 + "))" );
            Debug.Log(myPlayerScript.damage + ","+myPlayerScript.increaseDamagePer+ "," + bulletScript.dmgPer + "," + myPlayerScript.damageStack + "," + myPlayerScript.petDamagePer);
            Debug.Log("데미지 = "+ normalBulletDmg);
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

        health -= Dmg;

        Debug.Log(Dmg);
        if (health <= 0)
        {
            GM.gameScore += enemyScore;
            GM.scoreText.text = string.Format("{0:n0}", GM.gameScore);
            if (isBoss && patternIndex < maxPattern)
            {
                if (PhotonNetwork.IsMasterClient)
                    GM.allBulletDelete = true;
                pv.RPC(nameof(GodModeRPC), RpcTarget.All, true);
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
                        myPlayerScript.attackSpeedStack += 1;
                    }
                }
                if (myPlayerScript.isDamageStack)
                {
                    myPlayerScript.damageStackint++;

                    if (myPlayerScript.damageStackint == 1)
                    {
                        myPlayerScript.damageStackint = 0;
                        myPlayerScript.damageStack += 1;
                    }
                }
            }

            if (isLast)
            {
                GM.pv.RPC("StageEnd",RpcTarget.All);
            }

            transform.rotation = Quaternion.identity;

            if (PhotonNetwork.IsMasterClient)
            {
                OP.PoolInstantiate("Explosion", transform.position, Quaternion.identity, -2, -1, -1, false);
                GM.OP.PoolDestroy(gameObject);
            }

        }
    }

    [PunRPC]
    public void GodModeRPC(bool set)
    {
        godMode = set;
    }

    [PunRPC]
    public void SoundRPC(int soundNum)
    {
        switch (soundNum)
        {
            case 1:
                SoundManager.Play("Gun_1");
                break;
            case 2:
                SoundManager.Play("Gun_2");
                break;
            case 3:
                SoundManager.Play("Gun_3");
                break;
            case 4:
                SoundManager.Play("Explosion_1");
                break;
            case 5:
                SoundManager.Play("Explosion_2");
                break;
        }
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
