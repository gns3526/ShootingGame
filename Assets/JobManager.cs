using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class JobManager : MonoBehaviour
{
    [SerializeField] ObjectPooler OP;
    [SerializeField] GameManager GM;

    public Sprite[] jobIconDummy;

    public Player myplayerScript;
    public int jobCode;
    public Button skillBtn;

    public Image skillGuage_M;
    public Image skillGuage_D;
    public GameObject guageMaxUi;

    [Header("Class A")]
    [SerializeField] int dmgA;
    [SerializeField] int maxLifeA;
    [SerializeField] int lifeA;
    [SerializeField] int moveSpeedA;
    [SerializeField] float fireSpeedA;

    [SerializeField] int skillADamageAmount;
    [SerializeField] int skillAatkSpeed;
    [SerializeField] float skillCoolA;
    [SerializeField] float durationA;

    [Header("Class B")]
    [SerializeField] int dmgB;
    [SerializeField] int maxLifeB;
    [SerializeField] int lifeB;
    [SerializeField] int moveSpeedB;
    [SerializeField] float fireSpeedB;

    public int starterPetAmountB;
    public bool skillBOn;

    [SerializeField] float skillCoolB;
    [SerializeField] GameObject mobileSkillBPanel;
    public GameObject desktopSkillBPanel;
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

    public int barrierAmountC;
    public int barrierDuractionC;
    public float skillCoolC;

    [Header("Class D")]
    [SerializeField] int dmgD;
    [SerializeField] int maxLifeD;
    [SerializeField] int lifeD;
    [SerializeField] int moveSpeedD;
    [SerializeField] float fireSpeedD;

    [SerializeField] float skillCoolD;

    [Header("Class E")]
    [SerializeField] int dmgE;
    [SerializeField] int maxLifeE;
    [SerializeField] int lifeE;
    [SerializeField] int moveSpeedE;
    [SerializeField] float fireSpeedE;

    private bool shotBoolE;
    [SerializeField] float maxSkillGuageE;
    public float skillCoefficientE;

    [Header("Other")]
    public bool canUseSkill;
    [SerializeField] private GameObject mobileSkillLockOb;
    [SerializeField] private GameObject deskTopSkillLockOb;

    public float skillCool;
    public float curSkillCool;
    [SerializeField] float duration;

    public void OnEnableSkill()
    {
        curSkillCool = 0;
        duration = -1;

        guageMaxUi.SetActive(false);
        switch (jobCode)
        {
            case 0:
                SkillGuageEnable(true);
                skillCool = skillCoolA;
                break;
            case 1:
                SkillGuageEnable(true);
                skillCool = skillCoolB;
                break;
            case 2:
                SkillGuageEnable(true);
                skillCool = skillCoolC;
                break;
            case 3:
                SkillGuageEnable(true);
                skillCool = skillCoolD;
                break;
            case 4:
                SkillGuageEnable(true);
                skillCool = maxSkillGuageE;
                break;
        }
    }

    private void SkillGuageEnable(bool b)
    {
        if (GM.isAndroid)
            skillGuage_M.enabled = b;
        else if (!GM.isAndroid)
            skillGuage_D.enabled = b;
    }

    private void Update()
    {
        if (curSkillCool < skillCool && duration < 0 && GM.isPlaying)
        {
            if(jobCode != 4)
            curSkillCool += Time.deltaTime * (myplayerScript.skillCooldownPer / 100);

            if(GM.isAndroid)
                skillGuage_M.fillAmount = curSkillCool / skillCool;
            else
                skillGuage_D.fillAmount = curSkillCool / skillCool;


        }
        else if (curSkillCool > skillCool)
        {
            if (GM.isAndroid)
                skillGuage_M.fillAmount = 1;
            else
                skillGuage_D.fillAmount = 1;
            CanUseSkillUpdate(true);

            guageMaxUi.SetActive(true);
        }

        else if(duration > 0)
        {
            duration -= Time.deltaTime;


            if(jobCode == 0)
            {
                if (GM.isAndroid)
                    skillGuage_M.fillAmount = duration / durationA;
                else
                    skillGuage_D.fillAmount = duration / durationA;
            }
            if(duration < 0)
            {
                switch (jobCode)
                {
                    case 0:
                        myplayerScript.increaseDamagePer -= skillADamageAmount;
                        myplayerScript.attackSpeedPer -= skillAatkSpeed;
                        break;

                    case 2:
                        myplayerScript.skillC.pv.RPC(nameof(myplayerScript.skillC.BarrierOn), RpcTarget.All, false);
                        break;
                }
            }
        }
    }

    public void JobApply()
    {
        if (!myplayerScript.pv.IsMine) return;

        CanUseSkillUpdate(false);
        skillGuage_M.fillAmount = 1;

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
            case 4:
                myplayerScript.damage = dmgE;
                myplayerScript.maxLife = maxLifeE;
                myplayerScript.life = lifeE;
                myplayerScript.moveSpeed = moveSpeedE;
                myplayerScript.maxShotCoolTime = fireSpeedE;
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
            case 4:
                ShotTypeE();
                break;
        }
    }

    void ShotTypeA()
    {
        switch (myplayerScript.power)
        {
            case 1:
                GameObject bullet = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 8, true);
                bullet.GetComponent<BulletScript>().dmgPer = 50;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 1);
                break;
            case 2:
                GameObject bulletR = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.1f, Quaternion.identity, 0, -1, 6, true);
                GameObject bulletL = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.1f, Quaternion.identity, 0, -1, 6, true);
                bulletR.GetComponent<BulletScript>().dmgPer = 50;
                bulletL.GetComponent<BulletScript>().dmgPer = 50;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 1);
                break;
            case 3:
                GameObject bulletRR = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.35f, Quaternion.identity, 0, -1, 6, true);
                GameObject bulletM = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 1, -1, 6, true);
                GameObject bulletLL = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.35f, Quaternion.identity, 0, -1, 6, true);
                bulletRR.GetComponent<BulletScript>().dmgPer = 50;
                bulletM.GetComponent<BulletScript>().dmgPer = 75;
                bulletLL.GetComponent<BulletScript>().dmgPer = 50;

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
        bullet.GetComponent<BulletScript>().dmgPer = 75;

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
                    bullet1.GetComponent<BulletScript>().dmgPer = 0;
                    bC += aC;
                }
                GameObject bullet2 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                bullet2.GetComponent<BulletScript>().dmgPer = 0;
                break;
            case 2:
                aC = bulletSpreadC2 * 2 / bulletAmountC2;
                bC = -bulletSpreadC2;
                for (int i = 0; i < bulletAmountC2; i++)
                {
                    GameObject bullet3 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                    bullet3.GetComponent<BulletScript>().dmgPer = 10;
                    bC += aC;
                }
                GameObject bullet4 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                bullet4.GetComponent<BulletScript>().dmgPer = 10;
                break;
            case 3:
                aC = bulletSpreadC3 * 2 / bulletAmountC3;
                bC = -bulletSpreadC3;
                for (int i = 0; i < bulletAmountC3; i++)
                {
                    GameObject bullet5 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                    bullet5.GetComponent<BulletScript>().dmgPer = 20;
                    bC += aC;
                }
                GameObject bullet6 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                bullet6.GetComponent<BulletScript>().dmgPer = 20;
                break;

        }
        myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 4);
    }
   
    void ShotTypeD()
    {
        GameObject laserA = OP.PoolInstantiate("LaserS", myplayerScript.transform.position, Quaternion.Euler(0,0,180), -1, -1, 0, true);
        laserA.GetComponent<BulletScript>().parentOb = myplayerScript.gameObject;
        laserA.GetComponent<BulletScript>().dmgPer = 100;
    }

    void ShotTypeE()
    {
        if (shotBoolE)
        {
            shotBoolE = false;

            switch (myplayerScript.power)
            {
                case 1:
                    GameObject bullet1 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, -5), 0, -1, 7, true);
                    bullet1.GetComponent<BulletScript>().dmgPer = 10;

                    GameObject bullet2 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet2.GetComponent<BulletScript>().dmgPer = 10;

                    GameObject bullet3 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, 5), 0, -1, 7, true);
                    bullet3.GetComponent<BulletScript>().dmgPer = 10;
                    break;
                case 2:
                    GameObject bullet4 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, -3), 0, -1, 7, true);
                    bullet4.GetComponent<BulletScript>().dmgPer = 20;

                    GameObject bullet5 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet5.GetComponent<BulletScript>().dmgPer = 20;

                    GameObject bullet6 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, 3), 0, -1, 7, true);
                    bullet6.GetComponent<BulletScript>().dmgPer = 20;
                    break;
                case 3:
                    GameObject bullet7 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.4f, Quaternion.Euler(0, 0, -2), 0, -1, 7, true);
                    bullet7.GetComponent<BulletScript>().dmgPer = 30;

                    GameObject bullet8 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.1f, Quaternion.Euler(0, 0, -0.7f), 0, -1, 7, true);
                    bullet8.GetComponent<BulletScript>().dmgPer = 30;

                    GameObject bullet9 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.1f, Quaternion.Euler(0, 0, 0.7f), 0, -1, 7, true);
                    bullet9.GetComponent<BulletScript>().dmgPer = 30;

                    GameObject bullet10 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.4f, Quaternion.Euler(0, 0, 2), 0, -1, 7, true);
                    bullet10.GetComponent<BulletScript>().dmgPer = 30;
                    break;
            }

        }
        else
        {
            shotBoolE = true;

            switch (myplayerScript.power)
            {
                case 1:
                    GameObject bullet1 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.15f, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet1.GetComponent<BulletScript>().dmgPer = 10;

                    GameObject bullet2 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.15f, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet2.GetComponent<BulletScript>().dmgPer = 10;
                    break;
                case 2:
                    GameObject bullet3 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet3.GetComponent<BulletScript>().dmgPer = 10;

                    GameObject bullet4 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position                       , Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet4.GetComponent<BulletScript>().dmgPer = 10;

                    GameObject bullet5 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet5.GetComponent<BulletScript>().dmgPer = 10;
                    break;
                case 3:
                    GameObject bullet6 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.3f, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet6.GetComponent<BulletScript>().dmgPer = 20;

                    GameObject bullet7 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position                       , Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet7.GetComponent<BulletScript>().dmgPer = 20;

                    GameObject bullet8 = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.3f, Quaternion.Euler(0, 0, 0), 0, -1, 7, true);
                    bullet8.GetComponent<BulletScript>().dmgPer = 20;
                    break;
            }

        }

        myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 1);
    }


    public void CanUseSkillUpdate(bool a)
    {
        canUseSkill = a;

        if (a)
        {
            if (GM.isAndroid)
                mobileSkillLockOb.SetActive(false);
            else
                deskTopSkillLockOb.SetActive(false);
        }
        else
        {
            if (GM.isAndroid)
                mobileSkillLockOb.SetActive(true);
            else
                deskTopSkillLockOb.SetActive(true);
        }
    }

    public void SkillOnClick(bool active)
    {
        if (!canUseSkill) return;

        guageMaxUi.SetActive(false);

        switch (jobCode)
        {
            case 0:
                myplayerScript.increaseDamagePer += skillADamageAmount;
                myplayerScript.attackSpeedPer += skillAatkSpeed;
                
                myplayerScript.codyPv.RPC("SkillParticleActive", RpcTarget.All, true, 0, 0.5f);

                CanUseSkillUpdate(false);
                duration = durationA;
                curSkillCool = 0;
                break;
            case 1:
                if (active)
                {
                    if(GM.isAndroid)
                        mobileSkillBPanel.SetActive(true);
                    else if(!GM.isAndroid)
                        desktopSkillBPanel.SetActive(true);

                    SoundManager.Play("Btn_1");
                }
                else
                {
                    skillBOn = false;

                    if (GM.isAndroid)
                        mobileSkillBPanel.SetActive(false);
                    else if (!GM.isAndroid)
                        desktopSkillBPanel.SetActive(false);

                    skillBPoint.SetActive(false);

                    CanUseSkillUpdate(false);
                    curSkillCool = 0;

                    SoundManager.Play("Btn_2");
                }

                break;
            case 2:
                //myplayerScript.skillC.GetComponent<BarrierScript>().barrierCount = barrierAmount;
                //myplayerScript.skillC.pv.RPC(nameof(myplayerScript.skillC.BarrierOn), RpcTarget.All,true);
                BarrierScript barrier = OP.PoolInstantiate("SkillC", myplayerScript.gameObject.transform.position ,Quaternion.identity, -2, -1, -1, true).GetComponent<BarrierScript>();
                barrier.barrierCount = barrierAmountC;
                barrier.duraction = barrierDuractionC;

                barrier.BarrierActive();

                CanUseSkillUpdate(false);

                curSkillCool = 0;

                myplayerScript.pv.RPC(nameof(myplayerScript.SoundRPC), RpcTarget.All, 7);
                break;
            case 3:
                GameObject laserA = OP.PoolInstantiate("LaserM", myplayerScript.transform.position, Quaternion.Euler(0,0,180), -1, -1, 0, true);
                laserA.GetComponent<BulletScript>().parentOb = myplayerScript.gameObject;
                laserA.GetComponent<BulletScript>().dmgPer = 3000;

                CanUseSkillUpdate(false);
                curSkillCool = 0;
                break;
            case 4:
                if (myplayerScript.life == myplayerScript.maxLife) return;

                CanUseSkillUpdate(false);
                curSkillCool = 0;

                myplayerScript.life++;
                GM.OP.DamagePoolInstantiate("DamageText", myplayerScript.transform.position + Vector3.up * 0.5f, Quaternion.identity, 1, 2, GM.DTM.damageSkinCode, true);

                GM.UpdateLifeIcon(myplayerScript.life);
                break;
        }
    }
    public void SkillBOnClick()
    {
        guageMaxUi.SetActive(false);

        skillBOn = true;
        Vector2 mousePos = Input.mousePosition;
        Vector2 transPos = Camera.main.ScreenToWorldPoint(mousePos);
        skillBPoint.transform.position = new Vector3(transPos.x, transPos.y, 0);
        skillBPoint.SetActive(true);

        if (GM.isAndroid)
            mobileSkillBPanel.SetActive(false);
        else if (!GM.isAndroid)
            desktopSkillBPanel.SetActive(false);

        CanUseSkillUpdate(false);
        curSkillCool = 0;

        SoundManager.Play("Btn_2");
    }
}
