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
    [SerializeField] Image healthImage2;
    public GameObject healthBarGameObject;
    public int[] bulletCode;
    public int[] bulletSpeedCode;


    [SerializeField] SpriteRenderer spriteRendererEnemy;
    [SerializeField] SpriteRenderer spriteRendererEnemy2;
    public PhotonView pv;

    Player myPlayerScript;
    public GameManager GM;
    public ObjectPooler OP;
    DamageTextManager dtm;
    PlayerState ps;

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
        //float maxHealthW = maxHealth * ((GM.stage * 0.3f) + 1);
        if (isBoss) health = maxHealth;
        else health = maxHealth * ((GM.stage * 0.3f) + 1);

        canFire = true;
        healthImage.fillAmount = 1;

        if (healthImage2 != null)
            healthImage2.fillAmount = 1;

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
        OP = GM.OP;
        dtm = GM.DTM;
        ps = GM.ps;
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        myPlayerScript = GM.myplayer.GetComponent<Player>();
    }


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!GM.isPlaying)
            {
                OP.PoolDestroy(gameObject);
            }
            if (reVive)
            {
                healthImage.fillAmount += 0.01f;
                Debug.Log("재생");
                if (healthImage.fillAmount == 1)
                {
                    GM.allBulletDelete = false;
                    canFire = true;
                    health = maxHealth;
                    reVive = false;
                    pv.RPC(nameof(GodModeRPC), RpcTarget.All, false);

                    patternIndex += 1;
                }
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

            if (!other.GetComponent<PhotonView>().IsMine) return;

            if (godMode)
                return;

            bulletScript = other.GetComponent<BulletScript>();
            Player myPlayerScript = GM.myplayer.GetComponent<Player>();

            bool ispetAttack = bulletScript.ispetAttack;
            float petDamagePer = 0;

            bool isCritical;

            int randomNum;
            randomNum = Random.Range(0, 101);

            if (ispetAttack)
                petDamagePer = ps.petDamagePer;

            // 내스텟 + (내 스텟 * 계수/100)

            normalBulletDmg = ps.damage + bulletScript.dmg + (ps.damage * (ps.increaseDamagePer / 100)) + (ps.damage * (bulletScript.dmgPer / 100))
                     + (ps.damage * (myPlayerScript.damageStack / 100)) + (ps.damage * (petDamagePer / 100));

            if (ps.criticalPer > randomNum)
            {
                isCritical = true;
                Debug.Log(ps.criticalDamagePer);
                Debug.Log("크리터짐");
                criticalPlusDamage = normalBulletDmg + (normalBulletDmg * (ps.criticalDamagePer / 100));
            }
            else
            {
                isCritical = false;
                Debug.Log("크리안터짐");
                criticalPlusDamage = normalBulletDmg;
            }


            if (isBoss)
                finalDamage = criticalPlusDamage + (criticalPlusDamage * (ps.bossDamagePer / 100));
            else
                finalDamage = criticalPlusDamage + (criticalPlusDamage * (ps.normalMonsterDamagePer / 100));

            finalDamage = (finalDamage + (finalDamage * (ps.finalDamagePer / 100)));

            if (ispetAttack)
                finalDamage = finalDamage / 2;

            Debug.Log(finalDamage);

            float finalDamageInt = Mathf.Round(finalDamage);

            pv.RPC(nameof(Hit), RpcTarget.All, finalDamage);


            float randomPosX = Random.Range(-0.3f, 0.3f);
            float randomPosY = Random.Range(-0.3f, 0.3f);
            Vector2 damagePos = new Vector2(transform.position.x + randomPosX, transform.position.y + randomPosY);

            if (isCritical && bulletScript.dmg == 0)
                OP.DamagePoolInstantiate("DamageText", damagePos, Quaternion.identity, (int)finalDamage, 1, dtm.damageSkinCode, false);
            else if(!isCritical && bulletScript.dmg > 0)
                OP.DamagePoolInstantiate("DamageText", damagePos, Quaternion.identity, (int)finalDamage, 3, dtm.damageSkinCode, false);
            else if (isCritical && bulletScript.dmg > 0)
                OP.DamagePoolInstantiate("DamageText", damagePos, Quaternion.identity, (int)finalDamage, 4, dtm.damageSkinCode, false);
            else
                OP.DamagePoolInstantiate("DamageText", damagePos, Quaternion.identity, (int)finalDamage, 0, dtm.damageSkinCode, false);

            Debug.Log(ps.damage + "+("+ ps.damage + "* (" + ps.increaseDamagePer + " / " + 100 + ")) + (" + ps.damage + "* (" + bulletScript.dmgPer + " / " + 100 + "))"
                     +"+ (" + ps.damage + "* (" + myPlayerScript.damageStack + " / " + 100 + ")) + (" + ps.damage + "* (" + petDamagePer + " / " + 100 + "))" );
            Debug.Log(ps.damage + ","+ ps.increaseDamagePer+ "," + bulletScript.dmgPer + "," + myPlayerScript.damageStack + "," + ps.petDamagePer + "," + bulletScript.dmg);
            Debug.Log("데미지 = "+ normalBulletDmg);

        }
    }

    void ReturnSprite()
    {
        spriteRendererEnemy.sprite = sprites[0];

        if (spriteRendererEnemy2 != null)
            spriteRendererEnemy2.sprite = sprites[0];
    }


    [PunRPC]
    public void Hit(float Dmg)
    {
        if (health <= 0)
            return;

        if(healthImage2 != null)
            healthImage2.fillAmount = health / (maxHealth * ((GM.stage * 0.3f) + 1));

        spriteRendererEnemy.sprite = sprites[1];
        if (spriteRendererEnemy2 != null)
            spriteRendererEnemy2.sprite = sprites[1];

        Invoke("ReturnSprite", 0.1f);

        health -= Dmg;

        if (isBoss)
            healthImage.fillAmount = health / maxHealth;
        else
            healthImage.fillAmount = health / (maxHealth * ((GM.stage * 0.3f) + 1));

        if (GM.myplayerScript.pv.IsMine)
        {
            JobManager jm = GM.jm;
            if (jm.jobCode == 4 && !bulletScript.ispetAttack)
                if (jm.curSkillCool < jm.skillCool)
                    jm.curSkillCool += jm.skillCoefficientE;
        }

        Debug.Log(Dmg);
        if (health <= 0)
        {
            GM.gameScore += enemyScore;
            GM.scoreText.text = "Score:" + string.Format("{0:n0}", GM.gameScore);
            if (isBoss && patternIndex < maxPattern)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    GM.allBulletDelete = true;
                    pv.RPC(nameof(GodModeRPC), RpcTarget.All, true);
                    canFire = false;
                    reVive = true;
                    
                }
                return;
            }
            
            if (GM.myplayerScript.pv.IsMine)
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
                OP.PoolInstantiate("Explosion", transform.position, Quaternion.identity, -2, -1, -1, 0, false);
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
