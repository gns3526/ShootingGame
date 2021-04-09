using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinForceManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] GameManager gm;

    [Header("ReinForce1")]
    [SerializeField] Text plainLvText;
    [SerializeField] Text successPer;
    [SerializeField] Text successText;
    [SerializeField] GameObject plusReinForce;
    [SerializeField] int coinCost;
    [SerializeField] Text costText;


    [Header("ReinForce2")]
    [SerializeField] Text reinPointAmount;
    [SerializeField] GameObject[] groups;

    [SerializeField] int[] dmgPer;
    [SerializeField] int[] atkSpd;
    [SerializeField] int[] criPer;
    [SerializeField] int[] criDmg;
    [SerializeField] int[] petDmg;
    [SerializeField] int[] petAtkSpd;

    [SerializeField] int[] upgradeInfo;
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
                        groups[0].transform.GetChild(2).GetComponent<Text>().text = "데미지: " + dmgPer[upgradeInfo[0] - 1] + "%";
                        break;
                    case 1:
                        groups[1].transform.GetChild(1).GetComponent<Text>().text = "공격속도 증가(Max)";
                        groups[1].transform.GetChild(2).GetComponent<Text>().text = "공격속도: " + atkSpd[upgradeInfo[1] - 1] + "%";
                        break;
                    case 2:
                        groups[2].transform.GetChild(1).GetComponent<Text>().text = "크리확률 증가(Max)";
                        groups[2].transform.GetChild(2).GetComponent<Text>().text = "크리확률: " + criPer[upgradeInfo[2] - 1] + "%";
                        break;
                    case 3:
                        groups[3].transform.GetChild(1).GetComponent<Text>().text = "크리데미지 증가(Max)";
                        groups[3].transform.GetChild(2).GetComponent<Text>().text = "크리데미지: " + criDmg[upgradeInfo[3] - 1] + "%";
                        break;
                    case 4:
                        groups[4].transform.GetChild(1).GetComponent<Text>().text = "펫 공격력 증가(Max)";
                        groups[4].transform.GetChild(2).GetComponent<Text>().text = "펫 공격력: " + petDmg[upgradeInfo[4] - 1] + "%";
                        break;
                    case 5:
                        groups[5].transform.GetChild(1).GetComponent<Text>().text = "펫 공격속도 증가(Max)";
                        groups[5].transform.GetChild(2).GetComponent<Text>().text = "펫 공격속도: " + petAtkSpd[upgradeInfo[5] - 1] + "%";
                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case 0:
                        groups[0].transform.GetChild(1).GetComponent<Text>().text = "공격력 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[0].transform.GetChild(2).GetComponent<Text>().text = "데미지: " + dmgPer[upgradeInfo[0] - 1] + "%" + " > " + dmgPer[upgradeInfo[0]] + "%";
                        break;
                    case 1:
                        groups[1].transform.GetChild(1).GetComponent<Text>().text = "공격속도 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[1].transform.GetChild(2).GetComponent<Text>().text = "공격속도: " + atkSpd[upgradeInfo[1] - 1] + "%" + " > " + atkSpd[upgradeInfo[1]] + "%";
                        break;
                    case 2:
                        groups[2].transform.GetChild(1).GetComponent<Text>().text = "크리확률 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[2].transform.GetChild(2).GetComponent<Text>().text = "크리확률: " + criPer[upgradeInfo[2] - 1] + "%" + " > " + criPer[upgradeInfo[2]] + "%";
                        break;
                    case 3:
                        groups[3].transform.GetChild(1).GetComponent<Text>().text = "크리데미지 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[3].transform.GetChild(2).GetComponent<Text>().text = "크리데미지: " + criDmg[upgradeInfo[3] - 1] + "%" + " > " + criDmg[upgradeInfo[3]] + "%";
                        break;
                    case 4:
                        groups[4].transform.GetChild(1).GetComponent<Text>().text = "펫 공격력 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[4].transform.GetChild(2).GetComponent<Text>().text = "펫 공격력: " + petDmg[upgradeInfo[4] - 1] + "%" + " > " + petDmg[upgradeInfo[4]] + "%";
                        break;
                    case 5:
                        groups[5].transform.GetChild(1).GetComponent<Text>().text = "펫 공격속도 증가" + "(" + "Lv." + upgradeInfo[i] + ")";
                        groups[5].transform.GetChild(2).GetComponent<Text>().text = "펫 공격속도: " + petAtkSpd[upgradeInfo[5] - 1] + "%" + " > " + petAtkSpd[upgradeInfo[5]] + "%";
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
        Debug.Log("ewewf");
        plainLvText.text = "Lv." + gm.plainLv.ToString();
        successPer.text = "성공확률:" + (100 - (gm.plainLv)).ToString();
        costText.text = coinCost.ToString();
        Disappear();
    }


    public void ReinForce1()
    {
        int randomNum = Random.RandomRange(0, 101);
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
    }
}
