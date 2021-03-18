using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ability
{
    public int[] SpeedValue, DamagePerValue, MaxHpValue, GodTime, AttackSpeed, DamageValue;
}
public class AbilityManager : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] GameManager GM;

    [SerializeField] GameObject abilityPanel;

    [SerializeField] int needCoinForAbility;
    [SerializeField] Text myCoinAmountInAbility;

    [SerializeField] Image[] abilityBack;
    [SerializeField] Sprite[] abilityBackGrade;
    [SerializeField] Text[] abilityText;

    [SerializeField] Text needCoinText;
    [SerializeField] Button abilityResetBtn;

    [SerializeField] Ability abilityPer;
    public int[] abilityGrade;


    public Player myPlayerScript;

    
    public void AbilityOpenOrClose(bool a)
    {
        abilityPanel.SetActive(a);
        CanResetAbility();
        needCoinText.text = needCoinForAbility.ToString();
        if (!a) return;
        for (int i = 0; i < 3; i++)
        {
            switch (GM.abilityCode[i])
            {
                case 0:
                    abilityText[i].text = "움직임 속도" + GM.abilityValue[i] + "증가";
                    break;
                case 1:
                    abilityText[i].text = "공격력" + GM.abilityValue[i] + "% 증가";
                    break;
                case 2:
                    abilityText[i].text = "최대 체력" + GM.abilityValue[i] + "증가";
                    break;
                case 3:
                    abilityText[i].text = "피격시 무적시간" + GM.abilityValue[i] + "초 증가";
                    break;
                case 4:
                    abilityText[i].text = "공격속도" + GM.abilityValue[i] + "% 증가";
                    break;
            }
            switch (abilityGrade[i])
            {
                case 1:
                    abilityBack[i].sprite = abilityBackGrade[2];
                    break;
                case 2:
                    abilityBack[i].sprite = abilityBackGrade[1];
                    break;
                case 3:
                    abilityBack[i].sprite = abilityBackGrade[0];
                    break;
            }
        }
    }

    public void AbilityChange()
    {
        GM.money -= needCoinForAbility;
        for (int i = 0; i < 3; i++)
        {
            int randomNum = Random.Range(0, 5);//테스트
            if (randomNum == 0)//MoveSpeed;
            {

                int randomValue = Random.Range(abilityPer.SpeedValue[0], abilityPer.SpeedValue[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "움직임 속도" + randomValue + "증가";
                int a = abilityPer.SpeedValue[1] - abilityPer.SpeedValue[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.SpeedValue[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.SpeedValue[0] + b * 2)
                {
                    abilityBack[i].sprite = abilityBackGrade[1];
                    abilityGrade[i] = 2;
                }
                else
                {
                    abilityBack[i].sprite = abilityBackGrade[2];
                    abilityGrade[i] = 1;
                }
            }
            else if (randomNum == 1)//AttackDamagePer;
            {

                int randomValue = Random.Range(abilityPer.DamagePerValue[0], abilityPer.DamagePerValue[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "공격력" + randomValue + "% 증가";
                int a = abilityPer.DamagePerValue[1] - abilityPer.DamagePerValue[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.DamagePerValue[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.DamagePerValue[0] + b * 2)
                {
                    abilityBack[i].sprite = abilityBackGrade[1];
                    abilityGrade[i] = 2;
                }
                else
                {
                    abilityBack[i].sprite = abilityBackGrade[2];
                    abilityGrade[i] = 1;
                }
            }
            else if (randomNum == 2)//MaxHp;
            {

                int randomValue = Random.Range(abilityPer.MaxHpValue[0], abilityPer.MaxHpValue[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "최대 체력" + randomValue + "증가";

                if (randomValue == 1)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue == 2)
                {
                    abilityBack[i].sprite = abilityBackGrade[1];
                    abilityGrade[i] = 2;
                }
                else
                {
                    abilityBack[i].sprite = abilityBackGrade[2];
                    abilityGrade[i] = 1;
                }
            }
            else if (randomNum == 3)//GodTime;
            {

                int randomValue = Random.Range(abilityPer.GodTime[0], abilityPer.GodTime[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "피격시 무적시간" + randomValue + "초 증가";
                int a = abilityPer.GodTime[1] - abilityPer.GodTime[0];
                float b = Mathf.Round(a / 3);
                if (randomValue == 1)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue == 2)
                {
                    abilityBack[i].sprite = abilityBackGrade[1];
                    abilityGrade[i] = 2;
                }
                else
                {
                    abilityBack[i].sprite = abilityBackGrade[2];
                    abilityGrade[i] = 1;
                }
            }
            else if (randomNum == 4)//AttackSpeed;
            {

                int randomValue = Random.Range(abilityPer.AttackSpeed[0], abilityPer.AttackSpeed[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "공격속도" + randomValue + "% 증가";
                int a = abilityPer.AttackSpeed[1] - abilityPer.AttackSpeed[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.AttackSpeed[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.AttackSpeed[0] + b * 2)
                {
                    abilityBack[i].sprite = abilityBackGrade[1];
                    abilityGrade[i] = 2;
                }
                else
                {
                    abilityBack[i].sprite = abilityBackGrade[2];
                    abilityGrade[i] = 1;
                }
            }
            else if (randomNum == 5)//AttackDamage;
            {

                int randomValue = Random.Range(abilityPer.DamageValue[0], abilityPer.DamageValue[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "공격력" + randomValue + "증가";
                int a = abilityPer.DamageValue[1] - abilityPer.DamageValue[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.DamageValue[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.DamageValue[0] + b * 2)
                {
                    abilityBack[i].sprite = abilityBackGrade[1];
                    abilityGrade[i] = 2;
                }
                else
                {
                    abilityBack[i].sprite = abilityBackGrade[2];
                    abilityGrade[i] = 1;
                }
            }
            GM.abilityCode[i] = randomNum;
        }
        CanResetAbility();
    }

    void CanResetAbility()
    {
        myCoinAmountInAbility.text = "X" + GM.money.ToString();
        if (GM.money >= needCoinForAbility)
        {
            needCoinText.color = Color.white;
            abilityResetBtn.interactable = true;
        }
        else
        {
            needCoinText.color = Color.red;
            abilityResetBtn.interactable = false;
        }
    }

    public void AbilityApply()
    {
        for (int i = 0; i < 3; i++)
        {
            switch (GM.abilityCode[i])
            {
                case 0://MoveSpeed;
                    float a = myPlayerScript.moveSpeed;
                    myPlayerScript.moveSpeed += GM.abilityValue[i];
                    Debug.Log("움직임속도가" + a + "에서" + myPlayerScript.moveSpeed + "로 증가");
                    break;
                case 1://AttackDamage;
                    int b = myPlayerScript.increaseDamagePer;
                    myPlayerScript.increaseDamagePer += GM.abilityValue[i];
                    Debug.Log("공격력이" + b + "에서" + myPlayerScript.increaseDamagePer + "로 증가");
                    break;
                case 2://MaxHp;
                    int c = myPlayerScript.maxLife;
                    myPlayerScript.maxLife += GM.abilityValue[i];
                    myPlayerScript.life = myPlayerScript.maxLife;
                    Debug.Log("체력이" + c + "에서" + myPlayerScript.maxLife + "로 증가");
                    GM.UpdateLifeIcon(myPlayerScript.life);
                    break;
                case 3://GodTime;
                    float d = myPlayerScript.godTime;
                    myPlayerScript.godTime += GM.abilityValue[i];
                    Debug.Log("피격시 무적시간이" + d + "에서" + myPlayerScript.godTime + "로 증가");
                    break;
                case 4://AttackSpeed;
                    float e = myPlayerScript.shotCoolTimeReduce;
                    myPlayerScript.shotCoolTimeReduce += GM.abilityValue[i];
                    Debug.Log("공격속도가" + e + "에서" + myPlayerScript.shotCoolTimeReduce + "로 증가");
                    break;
            }
        }
    }
}
