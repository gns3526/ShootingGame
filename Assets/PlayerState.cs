using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    [SerializeField] JobManager jm;
    [SerializeField] AbilityManager am;
    [SerializeField] ReinForceManager rfm;
    [SerializeField] GameManager gm;

    [SerializeField] GameObject[] stateTextGroup;

    [SerializeField] string[] state;

    public int life;
    public int maxLife;
    public float moveSpeed;
    public float damage;
    public float increaseDamagePer;
    public float attackSpeedPer;
    public float bossDamagePer;
    public float normalMonsterDamagePer;
    public float criticalPer;
    public float criticalDamagePer;
    public float petDamagePer;
    public float petAttackSpeedPer;
    public float penetratePer;
    public float finalDamagePer;

    public float maxShotCoolTime;

    public int skillCooldownPer;

    public float curShotCoolTime;
    public float godTime;
    public int missPercentage;

    public float goldAmountPer;
    public float expAmountPer;
    private void Start()
    {
        for (int i = 0; i < stateTextGroup.Length; i++)
        {
            stateTextGroup[i].transform.GetChild(0).GetComponent<Text>().text = state[i];
        }
    }

    public void StateUpdate()
    {
        maxLife = 0;
        moveSpeed = 0;
        damage = 0;
        increaseDamagePer = 0;
        attackSpeedPer = 0;
        bossDamagePer = 0;
        normalMonsterDamagePer = 0;
        criticalPer = 35;
        criticalDamagePer = 0;
        petDamagePer = 0;
        petAttackSpeedPer = 0;
        penetratePer = 10;
        finalDamagePer = 0;

        maxShotCoolTime = 0;

        skillCooldownPer = 0;

        curShotCoolTime = 0;
        godTime = 2;
        missPercentage = 0;

        goldAmountPer = 0;
        expAmountPer = 0;

        jm.JobApply();
        am.AbilityApply();
        rfm.ReinForceApply();
    }


    private void OnEnable()
    {
        int damageJob;
        int maxLifeJob;
        int moveSpeedJob;
        int fireSpeedJob;

        switch (jm.jobCode)
        {
            case 0:
                damageJob = jm.dmgA;
                maxLifeJob = jm.maxLifeA;
                moveSpeedJob = jm.moveSpeedA;
                fireSpeedJob = (int)jm.fireSpeedA;
                break;
            case 1:
                damageJob = jm.dmgB;
                maxLifeJob = jm.maxLifeB;
                moveSpeedJob = jm.moveSpeedB;
                fireSpeedJob = (int)jm.fireSpeedB;
                break;
            case 2:
                damageJob = jm.dmgC;
                maxLifeJob = jm.maxLifeC;
                moveSpeedJob = jm.moveSpeedC;
                fireSpeedJob = (int)jm.fireSpeedC;
                break;
            case 3:
                damageJob = jm.dmgD;
                maxLifeJob = jm.maxLifeD;
                moveSpeedJob = jm.moveSpeedD;
                fireSpeedJob = (int)jm.fireSpeedD;
                break;
            case 4:
                damageJob = jm.dmgE;
                maxLifeJob = jm.maxLifeE;
                moveSpeedJob = jm.moveSpeedE;
                fireSpeedJob = (int)jm.fireSpeedE;
                break;
        }


        //stateTextGroup[0].transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (damageJob + am.);


    }
}
