using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JopManager : MonoBehaviour
{
    [SerializeField] ObjectPooler OP;


    public Player myplayerScript;
    public int jobCode;

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


    public void JobApply()
    {
        if (!myplayerScript.pv.IsMine) return;

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
        }
    }

    public void Shot()
    {
        switch (jobCode)
        {
            case 0:
                switch (myplayerScript.power)
                {
                    case 1:
                        GameObject bullet = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 8, true);
                        bullet.GetComponent<BulletScript>().dmgPer = 100;
                        break;
                    case 2:
                        GameObject bulletR = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.1f, Quaternion.identity, 0, -1, 6, true);
                        GameObject bulletL = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.1f, Quaternion.identity, 0, -1, 6, true);
                        break;
                    case 3:
                        GameObject bulletRR = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.35f, Quaternion.identity, 0, -1, 6, true);
                        GameObject bulletM = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 1, -1, 6, true);
                        GameObject bulletLL = OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.35f, Quaternion.identity, 0, -1, 6, true);
                        break;
                }
                break;
            case 1:
                StartCoroutine(ShotTypeB());
                break;
            case 2:
                switch (myplayerScript.power)
                {
                    case 1:
                        aC = bulletSpreadC1 * 2 / bulletAmountC1;
                        bC = -bulletSpreadC1;
                        for (int i = 0; i < bulletAmountC1; i++)
                        {
                            OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                            bC += aC;
                        }
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                        break;
                    case 2:
                        aC = bulletSpreadC2 * 2 / bulletAmountC2;
                        bC = -bulletSpreadC2;
                        for (int i = 0; i < bulletAmountC2; i++)
                        {
                            OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                            bC += aC;
                        }
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                        break;
                    case 3:
                        aC = bulletSpreadC3 * 2 / bulletAmountC3;
                        bC = -bulletSpreadC3;
                        for (int i = 0; i < bulletAmountC3; i++)
                        {
                            OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                            bC += aC;
                        }
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.Euler(0, 0, bC), 0, -1, 7, true);
                        break;
                }
                break;
        }
    }

    IEnumerator ShotTypeB()
    {
        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);
        yield return new WaitForSeconds(0.02f);
        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);
        yield return new WaitForSeconds(0.02f);
        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);

        if(myplayerScript.power > 1)
        {
            yield return new WaitForSeconds(0.02f);
            OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);
            yield return new WaitForSeconds(0.02f);
            OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);

            if(myplayerScript.power > 2)
            {
                yield return new WaitForSeconds(0.02f);
                OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);
                yield return new WaitForSeconds(0.02f);
                OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 7, true);
            }
        }
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
                }
                else
                {
                    skillBOn = false;
                    skillBPanel.SetActive(false);
                    skillBPoint.SetActive(false);
                }
                break;
            case 2:
                myplayerScript.skillC.GetComponent<BarrierScript>().barrierCount = 5;
                myplayerScript.skillC.gameObject.SetActive(true);
                break;
        }
    }
    public void aa()
    {
        skillBOn = true;
        Vector2 mousePos = Input.mousePosition;
        Vector2 transPos = Camera.main.ScreenToWorldPoint(mousePos);
        skillBPoint.transform.position = new Vector3(transPos.x, transPos.y, 0);
        skillBPoint.SetActive(true);

        skillBPanel.SetActive(false);

    }
}
