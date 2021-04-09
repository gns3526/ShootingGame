using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    [SerializeField] int starterPetAmountB;
    public bool skillBOn;
    [SerializeField] GameObject skillBPanel;
    public GameObject skillBPoint;

    [Header("Class C")]
    [SerializeField] int dmgC;
    [SerializeField] int maxLifeC;
    [SerializeField] int lifeC;
    [SerializeField] int moveSpeedC;
    [SerializeField] float fireSpeedC;


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

                for (int i = 0; i < starterPetAmountB; i++)
                    myplayerScript.AddFollower(1);
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
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 0, -1, 8, true);
                        break;
                    case 2:
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.1f, Quaternion.identity, 0, -1, 6, true);
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.1f, Quaternion.identity, 0, -1, 6, true);
                        break;
                    case 3:
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.right * 0.35f, Quaternion.identity, 0, -1, 6, true);
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position, Quaternion.identity, 1, -1, 6, true);
                        OP.PoolInstantiate("BulletBasic", myplayerScript.transform.position + Vector3.left * 0.35f, Quaternion.identity, 0, -1, 6, true);
                        break;
                }
                break;
            case 1:
                StartCoroutine(ShotTypeB());
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
                    skillBOn = true;
                    skillBPanel.SetActive(true);
                }
                else
                {
                    skillBOn = false;
                    skillBPanel.SetActive(false);
                    skillBPoint.SetActive(false);
                }
                break;
        }
    }
    public void aa()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 transPos = Camera.main.ScreenToWorldPoint(mousePos);
        skillBPoint.transform.position = new Vector3(transPos.x, transPos.y, 0);
        skillBPoint.SetActive(true);

        skillBPanel.SetActive(false);

    }
}
