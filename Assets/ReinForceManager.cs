using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinForceManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] GameManager gm;
    [SerializeField] SpecialSkinManager specialSkinManager;

    public Player myplayerScript;

    [Header("ReinForce1")]
    [SerializeField] Button mainPlainUpgradeButton;

    [SerializeField] Text plainLvText;
    [SerializeField] Text successPer;
    [SerializeField] Text successText;
    [SerializeField] GameObject plusReinForce;
    [SerializeField] int coinCost;
    [SerializeField] Text costText;


    [Header("ReinForce2")]
    [SerializeField] Text reinPointAmount;
    [SerializeField] GameObject[] groups;

    public int[] dmgPer;
    public int[] atkSpd;
    public int[] criPer;
    public int[] criDmg;
    public int[] petDmg;
    public int[] petAtkSpd;

    public int[] upgradeInfo;
    [SerializeField] int[] upgradeMax;
    [SerializeField] int[] upgradeCost;

    public void ReinForceRework()
    {
        reinPointAmount.text = gm.reinPoint.ToString();
        for (int i = 0; i < upgradeInfo.Length; i++)
        {
            if (upgradeInfo[i] == upgradeMax[i])
            {
                switch (i)
                {
                    case 0:
                        groups[0].transform.GetChild(1).GetComponent<Text>().text = "공격력 증가(Max)";
                        groups[0].transform.GetChild(2).GetComponent<Text>().text = "데미지: " + dmgPer[upgradeInfo[0]] + "%";
                        break;
                    case 1:
                        groups[1].transform.GetChild(1).GetComponent<Text>().text = "공격속도 증가(Max)";
                        groups[1].transform.GetChild(2).GetComponent<Text>().text = "공격속도: " + atkSpd[upgradeInfo[1]] + "%";
                        break;
                    case 2:
                        groups[2].transform.GetChild(1).GetComponent<Text>().text = "크리확률 증가(Max)";
                        groups[2].transform.GetChild(2).GetComponent<Text>().text = "크리확률: " + criPer[upgradeInfo[2]] + "%";
                        break;
                    case 3:
                        groups[3].transform.GetChild(1).GetComponent<Text>().text = "크리데미지 증가(Max)";
                        groups[3].transform.GetChild(2).GetComponent<Text>().text = "크리데미지: " + criDmg[upgradeInfo[3]] + "%";
                        break;
                    case 4:
                        groups[4].transform.GetChild(1).GetComponent<Text>().text = "펫 공격력 증가(Max)";
                        groups[4].transform.GetChild(2).GetComponent<Text>().text = "펫 공격력: " + petDmg[upgradeInfo[4]] + "%";
                        break;
                    case 5:
                        groups[5].transform.GetChild(1).GetComponent<Text>().text = "펫 공격속도 증가(Max)";
                        groups[5].transform.GetChild(2).GetComponent<Text>().text = "펫 공격속도: " + petAtkSpd[upgradeInfo[5]] + "%";
                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case 0:
                        groups[0].transform.GetChild(1).GetComponent<Text>().text = "공격력 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[0].transform.GetChild(2).GetComponent<Text>().text = "데미지: " + dmgPer[upgradeInfo[0]] + "%" + " > " + dmgPer[upgradeInfo[0] + 1] + "%";
                        break;
                    case 1:
                        groups[1].transform.GetChild(1).GetComponent<Text>().text = "공격속도 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[1].transform.GetChild(2).GetComponent<Text>().text = "공격속도: " + atkSpd[upgradeInfo[1]] + "%" + " > " + atkSpd[upgradeInfo[1] + 1] + "%";
                        break;
                    case 2:
                        groups[2].transform.GetChild(1).GetComponent<Text>().text = "크리확률 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[2].transform.GetChild(2).GetComponent<Text>().text = "크리확률: " + criPer[upgradeInfo[2]] + "%" + " > " + criPer[upgradeInfo[2] + 1] + "%";
                        break;
                    case 3:
                        groups[3].transform.GetChild(1).GetComponent<Text>().text = "크리데미지 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[3].transform.GetChild(2).GetComponent<Text>().text = "크리데미지: " + criDmg[upgradeInfo[3]] + "%" + " > " + criDmg[upgradeInfo[3] + 1] + "%";
                        break;
                    case 4:
                        groups[4].transform.GetChild(1).GetComponent<Text>().text = "펫 공격력 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[4].transform.GetChild(2).GetComponent<Text>().text = "펫 공격력: " + petDmg[upgradeInfo[4]] + "%" + " > " + petDmg[upgradeInfo[4] + 1] + "%";
                        break;
                    case 5:
                        groups[5].transform.GetChild(1).GetComponent<Text>().text = "펫 공격속도 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[5].transform.GetChild(2).GetComponent<Text>().text = "펫 공격속도: " + petAtkSpd[upgradeInfo[5]] + "%" + " > " + petAtkSpd[upgradeInfo[5] + 1] + "%";
                        break;
                }
            }
        }
        for (int i = 0; i < upgradeInfo.Length; i++)
        {
            groups[i].transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = upgradeCost[i].ToString();
            if(gm.reinPoint < upgradeCost[i])
                groups[i].transform.GetChild(3).GetComponent<Button>().interactable = false;
            else
                groups[i].transform.GetChild(3).GetComponent<Button>().interactable = true;
        }
    }


    public void LobbyReinRework()
    {
        plainLvText.text = "Lv." + gm.plainLv.ToString();
        successPer.text = "성공확률:" + (100 - (gm.plainLv)).ToString();
        costText.text = coinCost.ToString();
        Disappear();

        CheckCanUpgrade();
    }


    public void ReinForce1()
    {
        int randomNum = Random.RandomRange(1, 101);
        gm.money -= coinCost;
        costText.text = coinCost.ToString();
        gm.goldAmountText.text = gm.money.ToString();
        if(100 - (gm.plainLv) >= randomNum)
        {
            gm.plainLv++;
            gm.reinPoint++;
            successText.color = Color.green;
            successText.text = "강화성공!";

            plusReinForce.SetActive(true);
        }
        else
        {
            successText.color = Color.red;
            successText.text = "강화실패!";

            plusReinForce.SetActive(false);
        }
        successText.enabled = true;
        plainLvText.text = "Lv." + gm.plainLv.ToString();
        successPer.text = "성공확률:" + (100 - (gm.plainLv)).ToString();

        if (gm.plainLv == 100)
            specialSkinManager.ChallengeClear(2);

        CheckCanUpgrade();

        SoundManager.Play("Btn_2");
    }

    public void CheckCanUpgrade()
    {
        if (gm.money >= coinCost)
        {
            costText.color = Color.white;
            mainPlainUpgradeButton.interactable = true;
        }
        else
        {
            costText.color = Color.red;
            mainPlainUpgradeButton.interactable = false;
        }
        if (gm.plainLv == 100)
        {
            plainLvText.text = "Lv.Max";
            successPer.text = "";
            successPer.text = "";
            plusReinForce.SetActive(false);
            mainPlainUpgradeButton.interactable = false;
            mainPlainUpgradeButton.transform.GetChild(0).gameObject.SetActive(false);
            costText.text = "";
        }
    }

    public void Disappear()
    {
        successText.enabled = false;
        plusReinForce.SetActive(false);
    }

    public void ReinForce2(int index)
    {
        upgradeInfo[index]++;
        gm.reinPoint -= upgradeCost[index];
        ReinForceRework();

        SoundManager.Play("Btn_2");
    }

    public void ReinForceApply()
    {
        myplayerScript.increaseDamagePer += gm.plainLv;
        myplayerScript.moveSpeed += gm.plainLv * 0.5f;

        myplayerScript.increaseDamagePer += dmgPer[upgradeInfo[0]] - 100;
        myplayerScript.attackSpeedPer += atkSpd[upgradeInfo[1]] - 100;
        myplayerScript.criticalPer += criPer[upgradeInfo[2]] - 100;
        myplayerScript.criticalDamagePer += criDmg[upgradeInfo[3]] - 100;
        myplayerScript.petDamagePer += petDmg[upgradeInfo[4]] - 100;
        myplayerScript.petAttackSpeedPer += petAtkSpd[upgradeInfo[5]] - 100;
    }
}
