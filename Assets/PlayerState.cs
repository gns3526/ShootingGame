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

    [SerializeField] GameObject playerStatPanel;

    [SerializeField] ScrollRect scrollView;
    [SerializeField] GameObject[] stateTitleTextGroup;
    [SerializeField] Text[] stateTexts;

    [SerializeField] string[] state;

    [SerializeField] Color[] colors;

    [Header("PlayerStatInfo")]
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


    int index;
    private void Start()
    {
        for (int i = 0; i < stateTitleTextGroup.Length; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().text = state[i];
        }


        index = 0;
        for (int i = 0; i < 4; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().color = colors[0];
            stateTexts[i].color = colors[0];
        }
        for (int i = 4; i < 10; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().color = colors[1];
            stateTexts[i].color = colors[1];
        }
        for (int i = 10; i < 12; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().color = colors[2];
            stateTexts[i].color = colors[2];
        }
        for (int i = 12; i < 15; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().color = colors[3];
            stateTexts[i].color = colors[3];
        }
        for (int i = 15; i < 16; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().color = colors[4];
            stateTexts[i].color = colors[4];
        }
        for (int i = 16; i < 18; i++)
        {
            stateTitleTextGroup[i].transform.GetChild(0).GetComponent<Text>().color = colors[5];
            stateTexts[i].color = colors[5];
        }
    }

    public void PlayerStatOpenOrClose(bool a)
    {
        playerStatPanel.SetActive(a);

        if (a)
            scrollView.content.localPosition = new Vector3(0, 1, 0);


        stateTexts[0].text = maxLife.ToString();

        stateTexts[1].text = moveSpeed.ToString() + "%";

        stateTexts[2].text = missPercentage.ToString() + "%";

        stateTexts[3].text = godTime.ToString() + "/Sec";

        stateTexts[4].text = damage.ToString();

        if(gm.myplayerScript != null)
            stateTexts[5].text = (increaseDamagePer + gm.myplayerScript.damageStack + 100).ToString() + "%";
        else
            stateTexts[5].text = (increaseDamagePer + 100).ToString() + "%";

        if (gm.myplayerScript != null)
            stateTexts[6].text = (attackSpeedPer + gm.myplayerScript.attackSpeedStack).ToString() + "%";
        else
            stateTexts[6].text = attackSpeedPer.ToString() + "%";

        stateTexts[7].text = (petDamagePer + 100).ToString() + "%";

        stateTexts[8].text = (petAttackSpeedPer + 100).ToString() + "%";

        stateTexts[9].text = penetratePer.ToString() + "%";

        stateTexts[10].text = criticalPer.ToString() + "%";

        stateTexts[11].text = (criticalDamagePer + 100).ToString() + "%";

        stateTexts[12].text = (normalMonsterDamagePer + 100).ToString() + "%";

        stateTexts[13].text = (bossDamagePer + 100).ToString() + "%";

        stateTexts[14].text = (finalDamagePer + 100).ToString() + "%";

        stateTexts[15].text = skillCooldownPer.ToString() + "%";

        stateTexts[16].text = (goldAmountPer + 100).ToString() + "%";

        stateTexts[17].text = (expAmountPer + 100).ToString() + "%";

        SoundManager.Play("Btn_3");
    }

    public void Reset_StatPanelOpenOrClose(bool a)
    {

        playerStatPanel.SetActive(a);

        StatReset();

        if (a)
            scrollView.content.localPosition = new Vector3(0, 1, 0);

        stateTexts[0].text = maxLife.ToString();

        stateTexts[1].text = moveSpeed.ToString() + "%";

        stateTexts[2].text = missPercentage.ToString() + "%";

        stateTexts[3].text = godTime.ToString() + "/Sec";

        stateTexts[4].text = damage.ToString();

        stateTexts[5].text = (increaseDamagePer + 100).ToString() + "%";

        stateTexts[6].text = attackSpeedPer.ToString() + "%";

        stateTexts[7].text = (petDamagePer + 100).ToString() + "%";

        stateTexts[8].text = petAttackSpeedPer.ToString() + "%";

        stateTexts[9].text = penetratePer.ToString() + "%";

        stateTexts[10].text = criticalPer.ToString() + "%";

        stateTexts[11].text = (criticalDamagePer + 100).ToString() + "%";

        stateTexts[12].text = (normalMonsterDamagePer + 100).ToString() + "%";

        stateTexts[13].text = (bossDamagePer + 100).ToString() + "%";

        stateTexts[14].text = (finalDamagePer + 100).ToString() + "%";

        stateTexts[15].text = skillCooldownPer.ToString() + "%";

        stateTexts[16].text = (goldAmountPer + 100).ToString() + "%";

        stateTexts[17].text = (expAmountPer + 100).ToString() + "%";

        SoundManager.Play("Btn_3");
    }

    public void StatReset()
    {
        ///기본 스텟

        maxLife = 0;
        moveSpeed = 100;
        damage = 0;
        increaseDamagePer = 0;
        attackSpeedPer = 100;
        bossDamagePer = 0;
        normalMonsterDamagePer = 0;
        criticalPer = 35;
        criticalDamagePer = 50;
        petDamagePer = 0;
        petAttackSpeedPer = 100;
        penetratePer = 10;
        finalDamagePer = 0;

        maxShotCoolTime = 0;

        skillCooldownPer = 100;

        curShotCoolTime = 0;
        godTime = 2;
        missPercentage = 0;

        goldAmountPer = 0;
        expAmountPer = 0;

        ///

        jm.JobApply();
        Debug.Log(attackSpeedPer);
        am.AbilityApply();
        Debug.Log(attackSpeedPer);
        rfm.ReinForceApply();
        Debug.Log(attackSpeedPer);
    }
}
