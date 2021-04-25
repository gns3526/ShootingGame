﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JobManager : MonoBehaviour
{
    [SerializeField] ObjectPooler OP;


    public Player myplayerScript;
    public int jobCode;
    public Button skillBtn;
    public Image skillGuage;

    [Header("Class A")]
    [SerializeField] int dmgA;
    [SerializeField] int maxLifeA;
    [SerializeField] int lifeA;
    [SerializeField] int moveSpeedA;
    [SerializeField] float fireSpeedA;


    [Header("Class B")]
    [SerializeField] int dmgB;
    [SerializeField] int maxLifeB;
    [SerializeField] int lifeB;
    [SerializeField] int moveSpeedB;
    [SerializeField] float fireSpeedB;

    public int starterPetAmountB;
    public bool skillBOn;
    [SerializeField] GameObject skillBPanel;
    public GameObject skillBPoint;

    [Header("Class C")]
    [SerializeField] int dmgC;
    [SerializeField] int maxLifeC;
    [SerializeField] int lifeC;
    [SerializeField] int moveSpeedC;
    [SerializeField] float fireSpeedC;

    public int missPerC;
    public float bulletAmountC1;
    [SerializeField] float bulletSpreadC1;
    public float bulletAmountC2;
    [SerializeField] float bulletSpreadC2;
    public float bulletAmountC3;
    [SerializeField] float bulletSpreadC3;
    float aC;
    float bC;

    public float skillCoolC;
    [SerializeField] float durationC;

    [Header("Class D")]
    [SerializeField] int dmgD;
    [SerializeField] int maxLifeD;
    [SerializeField] int lifeD;
    [SerializeField] int moveSpeedD;
    [SerializeField] float fireSpeedD;

    [SerializeField] float skillCoolD;

    [Header("Other")]
    float skillCool;
    public float curSkillCool;

    public void OnEnableSkill()
    {
        curSkillCool = 100;
        switch (jobCode)
        {
            case 0:
                skillGuage.enabled = false;
                break;
            case 1:
                skillGuage.enabled = false;
                break;
            case 2:
                skillGuage.enabled = true;
                skillCool = skillCoolC;
                break;
            case 3:
                skillGuage.enabled = true;
                skillCool = skillCoolD;
                break;
        }
    }

    private void Update()
    {
        if (curSkillCool < skillCool)
        {
            curSkillCool += Time.deltaTime * (myplayerScript.skillCooldownPer / 100);
            skillGuage.fillAmount = curSkillCool / skillCool;

            if (curSkillCool > skillCool)
            {
                skillBtn.interactable = true;
                myplayerScript.skillC.pv.RPC(nameof(myplayerScript.skillC.BarrierOn), RpcTarget.All, false);
            }
        }
    }

    public void JobApply()
    {
        if (!myplayerScript.pv.IsMine) return;

        skillBtn.interactable = true;
        skillGuage.fillAmount = 1;

        skillBPoint.SetActive(false);

        switch (jobCode)
        {
            case 0:
                myplayerScript.damage = dmgA;
                myplayerScript.maxLife = maxLifeA;
                myplayerScript.life = lifeA;
                myplayerScript.moveSpeed = moveSpeedA;
                myplayerScript.maxShotCoolTime = fireSpeedA;
                break;
            case 1:
                myplayerScript.damage = dmgB;
                myplayerScript.maxLife = maxLifeB;
                myplayerScript.life = lifeB;
                myplayerScript.moveSpeed = moveSpeedB;
                myplayerScript.maxShotCoolTime = fireSpeedB;
                break;
            case 2:
                myplayerScript.damage = dmgC;
                myplayerScript.maxLife = maxLifeC;
                myplayerScript.life = lifeC;
                myplayerScript.moveSpeed = moveSpeedC;
                myplayerScript.maxShotCoolTime = fireSpeedC;
                break;
            case 3:
                myplayerScript.damage = dmgD;
                myplayerScript.maxLife = maxLifeD;
                myplayerScript.life = lifeD;
                myplayerScript.moveSpeed = moveSpeedD;
                myplayerScript.maxShotCoolTime = fireSpeedD;
                break;
        }
    }

    public void Shot()
    {
        switch (jobCode)
        {
            case 0:
                ShotTypeA();
                break;
            case 1:
                StartCoroutine(ShotTypeB());
                break;
            case 2:
                ShotTypeC();
                break;
            case 3:
                ShotTypeD();
                break;
        }
    }

    void ShotTypeA()
    {
        switch (myplayerScript.power)
        {
            case 1:
                GameObject bullet = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 8, true);
                bullet.GetComponent<BulletScript>().dmgPer = 100;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 1);
                break;
            case 2:
                GameObject bulletR = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.1f, Quaternion.identity, 0, -1, 6, true);
                GameObject bulletL = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.1f, Quaternion.identity, 0, -1, 6, true);
                bulletR.GetComponent<BulletScript>().dmgPer = 100;
                bulletL.GetComponent<BulletScript>().dmgPer = 100;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 1);
                break;
            case 3:
                GameObject bulletRR = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.35f, Quaternion.identity, 0, -1, 6, true);
                GameObject bulletM = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 1, -1, 6, true);
                GameObject bulletLL = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.35f, Quaternion.identity, 0, -1, 6, true);
                bulletRR.GetComponent<BulletScript>().dmgPer = 100;
                bulletM.GetComponent<BulletScript>().dmgPer = 150;
                bulletLL.GetComponent<BulletScript>().dmgPer = 100;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 2);
                break;
        }
    }

    IEnumerator ShotTypeB()
    {
        ShotB();
        yield return new WaitForSeconds(0.02f);
        ShotB();
        yield return new WaitForSeconds(0.02f);
        ShotB();

        if (myplayerScript.power > 1)
        {
            yield return new WaitForSeconds(0.02f);
            ShotB();
            yield return new WaitForSeconds(0.02f);
            ShotB();

            if (myplayerScript.power > 2)
            {
                yield return new WaitForSeconds(0.02f);
                ShotB();
                yield return new WaitForSeconds(0.02f);
                ShotB();
            }
        }
    }
    void ShotB()
    {
        GameObject bullet = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);
        bullet.GetComponent<BulletScript>().dmgPer = 150;

        myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 1);
    }

    void ShotTypeC()
    {
        switch (myplayerScript.power)
        {
            case 1:
                aC = bulletSpreadC1 * 2 / bulletAmountC1;
                bC = -bulletSpreadC1;
                for (int i = 0; i < bulletAmountC1; i++)
                {
                    GameObject bullet1 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                    bullet1.GetComponent<BulletScript>().dmgPer = 100;
                    bC += aC;
                }
                GameObject bullet2 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                bullet2.GetComponent<BulletScript>().dmgPer = 100;
                break;
            case 2:
                aC = bulletSpreadC2 * 2 / bulletAmountC2;
                bC = -bulletSpreadC2;
                for (int i = 0; i < bulletAmountC2; i++)
                {
                    GameObject bullet3 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                    bullet3.GetComponent<BulletScript>().dmgPer = 120;
                    bC += aC;
                }
                GameObject bullet4 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                bullet4.GetComponent<BulletScript>().dmgPer = 120;
                break;
            case 3:
                aC = bulletSpreadC3 * 2 / bulletAmountC3;
                bC = -bulletSpreadC3;
                for (int i = 0; i < bulletAmountC3; i++)
                {
                    GameObject bullet5 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                    bullet5.GetComponent<BulletScript>().dmgPer = 150;
                    bC += aC;
                }
                GameObject bullet6 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                bullet6.GetComponent<BulletScript>().dmgPer = 150;
                break;

        }
        myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 4);
    }
   
    void ShotTypeD()
    {
        GameObject laserA = OP.PoolInstantiate("LaserS", myplayerScript.transform.position, Quaternion.Euler(0,0,180), -1, -1, 0, true);
        laserA.GetComponent<BulletScript>().parentOb = myplayerScript.gameObject;
        laserA.GetComponent<BulletScript>().dmgPer = 200;
    }

    public void SkillOnClick(bool active)
    {
        switch (jobCode)
        {
            case 0:

                break;
            case 1:
                if (active)
                {
                    skillBPanel.SetActive(true);

                    SoundManager.Play("Btn_1");
                }
                else
                {
                    skillBOn = false;
                    skillBPanel.SetActive(false);
                    skillBPoint.SetActive(false);

                    SoundManager.Play("Btn_2");
                }

                break;
            case 2:
                myplayerScript.skillC.GetComponent<BarrierScript>().barrierCount = 5;
                myplayerScript.skillC.pv.RPC(nameof(myplayerScript.skillC.BarrierOn), RpcTarget.All,true);

                skillBtn.interactable = false;
                curSkillCool = 0;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 7);
                break;
            case 3:
                GameObject laserA = OP.PoolInstantiate("LaserM", myplayerScript.transform.position, Quaternion.Euler(0,0,180), -1, -1, 0, true);
                laserA.GetComponent<BulletScript>().parentOb = myplayerScript.gameObject;
                laserA.GetComponent<BulletScript>().dmgPer = 6000;

                skillBtn.interactable = false;
                curSkillCool = 0;
                break;
        }
    }
    public void SkillBOnClick()
    {
        skillBOn = true;
        Vector2 mousePos = Input.mousePosition;
        Vector2 transPos = Camera.main.ScreenToWorldPoint(mousePos);
        skillBPoint.transform.position = new Vector3(transPos.x, transPos.y, 0);
        skillBPoint.SetActive(true);

        skillBPanel.SetActive(false);

        SoundManager.Play("Btn_2");
    }
}