using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Ability
{
    public int[] SpeedValue, DamagePerValue, MaxHpValue, GodTime, AttackSpeed, DamageValue, FinalDamagePerValue, NormalMonsterDamagePer, bossMonsterDamagePer, CriticalPer
        , CriticalDmgPer, MissPer, PetDamagePer, PetAttackSpeed, BulletPenetratePer;
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
        CanResetAbilityCheck();
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
                case 5:
                    abilityText[i].text = "공격력" + GM.abilityValue[i] + "증가";
                    break;
                case 6:
                    abilityText[i].text = "최종데미지" + GM.abilityValue[i] + "% 증가";
                    break;
                case 7:
                    abilityText[i].text = "일반몬스터 공격력" + GM.abilityValue[i] + "% 증가";
                    break;
                case 8:
                    abilityText[i].text = "보스몬스터 공격력" + GM.abilityValue[i] + "% 증가";
                    break;
                case 9:
                    abilityText[i].text = "크리티컬 확률" + GM.abilityValue[i] + "% 증가";
                    break;
                case 10:
                    abilityText[i].text = "크리티컬 데미지" + GM.abilityValue[i] + "% 증가";
                    break;
                case 11:
                    abilityText[i].text = "회피확률" + GM.abilityValue[i] + "% 증가";
                    break;
                case 12:
                    abilityText[i].text = "펫 데미지" + GM.abilityValue[i] + "% 증가";
                    break;
                case 13:
                    abilityText[i].text = "펫 공격속도" + GM.abilityValue[i] + "% 증가";
                    break;
                case 14:
                    abilityText[i].text = "총알관통 확률" + GM.abilityValue[i] + "% 증가";
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
            int randomNum = Random.Range(0, 15);
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
            else if (randomNum == 6)//FinalDamage;
            {

                int randomValue = Random.Range(abilityPer.FinalDamagePerValue[0], abilityPer.FinalDamagePerValue[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "최종데미지" + randomValue + "% 증가";
                int a = abilityPer.FinalDamagePerValue[1] - abilityPer.FinalDamagePerValue[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.FinalDamagePerValue[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.FinalDamagePerValue[0] + b * 2)
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
            else if (randomNum == 7)//NormalMonsterDamage;
            {

                int randomValue = Random.Range(abilityPer.NormalMonsterDamagePer[0], abilityPer.NormalMonsterDamagePer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "일반몬스터 공격력" + randomValue + "% 증가";
                int a = abilityPer.NormalMonsterDamagePer[1] - abilityPer.NormalMonsterDamagePer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.NormalMonsterDamagePer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.NormalMonsterDamagePer[0] + b * 2)
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
            else if (randomNum == 8)//BossMonsterDamage;
            {

                int randomValue = Random.Range(abilityPer.bossMonsterDamagePer[0], abilityPer.bossMonsterDamagePer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "보스몬스터 공격력" + randomValue + "% 증가";
                int a = abilityPer.bossMonsterDamagePer[1] - abilityPer.bossMonsterDamagePer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.bossMonsterDamagePer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.bossMonsterDamagePer[0] + b * 2)
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
            else if (randomNum == 9)//CriticalPerIncrease;
            {

                int randomValue = Random.Range(abilityPer.CriticalPer[0], abilityPer.CriticalPer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "크리티컬 확률" + randomValue + "% 증가";
                int a = abilityPer.CriticalPer[1] - abilityPer.CriticalPer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.CriticalPer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.CriticalPer[0] + b * 2)
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
            else if (randomNum == 10)//CriticalDamagePer;
            {

                int randomValue = Random.Range(abilityPer.CriticalDmgPer[0], abilityPer.CriticalDmgPer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "크리티컬 데미지" + randomValue + "% 증가";
                int a = abilityPer.CriticalDmgPer[1] - abilityPer.CriticalDmgPer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.CriticalDmgPer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.CriticalDmgPer[0] + b * 2)
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
            else if (randomNum == 11)//MissPer;
            {

                int randomValue = Random.Range(abilityPer.MissPer[0], abilityPer.MissPer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "회피확률" + randomValue + "% 증가";
                int a = abilityPer.MissPer[1] - abilityPer.MissPer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.MissPer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.MissPer[0] + b * 2)
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
            else if (randomNum == 12)//PetDamage;
            {

                int randomValue = Random.Range(abilityPer.PetDamagePer[0], abilityPer.PetDamagePer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "펫 데미지" + randomValue + "% 증가";
                int a = abilityPer.PetDamagePer[1] - abilityPer.PetDamagePer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.PetDamagePer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.PetDamagePer[0] + b * 2)
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
            else if (randomNum == 13)//PetAttackSpeed;
            {

                int randomValue = Random.Range(abilityPer.PetAttackSpeed[0], abilityPer.PetAttackSpeed[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "펫 공격속도" + randomValue + "% 증가";
                int a = abilityPer.PetAttackSpeed[1] - abilityPer.PetAttackSpeed[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.PetAttackSpeed[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.PetAttackSpeed[0] + b * 2)
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
            else if (randomNum == 14)//Penetrate;
            {

                int randomValue = Random.Range(abilityPer.BulletPenetratePer[0], abilityPer.BulletPenetratePer[1] + 1);
                GM.abilityValue[i] = randomValue;
                abilityText[i].text = "총알관통 확률" + randomValue + "% 증가";
                int a = abilityPer.BulletPenetratePer[1] - abilityPer.BulletPenetratePer[0];
                float b = Mathf.Round(a / 3);
                if (randomValue < abilityPer.BulletPenetratePer[0] + b)
                {
                    abilityBack[i].sprite = abilityBackGrade[0];
                    abilityGrade[i] = 3;
                }
                else if (randomValue < abilityPer.BulletPenetratePer[0] + b * 2)
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

        CanResetAbilityCheck();
    }

    void CanResetAbilityCheck()
    {
        GM.goldAmountText.text = GM.money.ToString();
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
                case 1://AttackDamagePer;
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
                case 5://AttackDamage;
                    float f = myPlayerScript.damage;
                    myPlayerScript.damage += GM.abilityValue[i];
                    Debug.Log("공격력이" + f + "에서" + myPlayerScript.damage + "로 증가");
                    break;
                case 6://FinalDamagePer;
                    float g = myPlayerScript.finalDamagePer;
                    myPlayerScript.finalDamagePer += GM.abilityValue[i];
                    Debug.Log("최종데미지가" + g + "에서" + myPlayerScript.finalDamagePer + "로 증가");
                    break;
                case 7://NormalMonsterDamagePer;
                    float h = myPlayerScript.normalMonsterDamagePer;
                    myPlayerScript.normalMonsterDamagePer += GM.abilityValue[i];
                    Debug.Log("일반몬스터 공격력이" + h + "에서" + myPlayerScript.normalMonsterDamagePer + "로 증가");
                    break;
                case 8://BossMonsterDamagePer;
                    float j = myPlayerScript.bossDamagePer;
                    myPlayerScript.bossDamagePer += GM.abilityValue[i];
                    Debug.Log("보스몬스터 공격력이" + j + "에서" + myPlayerScript.bossDamagePer + "로 증가");
                    break;
                case 9://CriticalPer;
                    float k = myPlayerScript.criticalPer;
                    myPlayerScript.criticalPer += GM.abilityValue[i];
                    Debug.Log("크리티컬 확률이" + k + "에서" + myPlayerScript.criticalPer + "로 증가");
                    break;
                case 10://CriticalDamage;
                    float l = myPlayerScript.criticalDamagePer;
                    myPlayerScript.criticalDamagePer += GM.abilityValue[i];
                    Debug.Log("크리티컬 데미지가" + l + "에서" + myPlayerScript.criticalDamagePer + "로 증가");
                    break;
                case 11://MissPer;
                    float aa = myPlayerScript.missPercentage;
                    myPlayerScript.missPercentage += GM.abilityValue[i];
                    Debug.Log("회피확률이" + aa + "에서" + myPlayerScript.missPercentage + "로 증가");
                    break;
                case 12://PetDamage;
                    float bb = myPlayerScript.followerDamagePer;
                    myPlayerScript.followerDamagePer += GM.abilityValue[i];
                    Debug.Log("펫 데미지가" + bb + "에서" + myPlayerScript.missPercentage + "로 증가");
                    break;
                case 13://PetAttackSpeedr;
                    float cc = myPlayerScript.followerShotCoolReduce;
                    myPlayerScript.followerShotCoolReduce += GM.abilityValue[i];
                    Debug.Log("펫 공격속도가" + cc + "에서" + myPlayerScript.missPercentage + "로 증가");
                    break;
                case 14://PenetratePer;
                    float dd = myPlayerScript.penetratePer;
                    myPlayerScript.penetratePer += GM.abilityValue[i];
                    Debug.Log("총알 관통확률이" + dd + "에서" + myPlayerScript.missPercentage + "로 증가");
                    break;
            }
        }
    }
}
