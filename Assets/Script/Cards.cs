using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Cards : MonoBehaviourPunCallbacks,IPunObservable
{
    public Player myPlayerScript;

    [SerializeField] ObjectPooler OP;
    [SerializeField] GameManager GM;
    public PhotonView pv;

    [SerializeField] int readyAmount;

    public int curMin;
    public float curSec;

    public bool isCellectingTime;


    [SerializeField] Text curTimeText;
    [SerializeField] Text curConditionText;
    [SerializeField] int cTInt;
    public bool isReady;
    [SerializeField] float cTTime;
    public void CardS(int num)
    {

        switch (num)
        {
            case 1:
                myPlayerScript.attackSpeedPer += 50;

                break;
            case 2:
                myPlayerScript.moveSpeed += 20;
                break;
            case 3:
                myPlayerScript.increaseDamagePer += 50;
                break;
            case 4:
                if (myPlayerScript.power != myPlayerScript.maxPower)
                {
                    myPlayerScript.power++;
                }
                break;
            case 5:
                if (myPlayerScript.maxLife != 10)
                {
                    myPlayerScript.maxLife++;
                    myPlayerScript.life++;
                    GM.UpdateLifeIcon(myPlayerScript.life);
                }
                break;
            case 6:
                myPlayerScript.life = myPlayerScript.maxLife;
                GM.UpdateLifeIcon(myPlayerScript.life);
                break;
            case 7:
                OP.PoolInstantiate("Pet", myPlayerScript.transform.position, Quaternion.identity, -3, 0, -1, true);
                break;
            case 8:
                OP.PoolInstantiate("Pet", myPlayerScript.transform.position, Quaternion.identity, -3, 1, -1, true);
                break;
            case 9:
                myPlayerScript.godTime += 2;
                break;
            case 10:
                myPlayerScript.missPercentage += 10;
                break;
            case 11:
                GM.pv.RPC("ReviveTeam", RpcTarget.All, 1);
                break;
            case 12:
                myPlayerScript.isDamageStack = true;
                break;
            case 13:
                myPlayerScript.isAttackSpeedStack = true;
                break;
            case 14:
                myPlayerScript.bossDamagePer += 60;
                break;
            case 15:
                myPlayerScript.criticalPer += 10;
                break;
            case 16:
                myPlayerScript.criticalDamagePer += 50;
                break;
            case 17:
                myPlayerScript.isSpecialBulletAbility1 = true;
                myPlayerScript.isSpecialBulletAbility2 = false;
                break;
            case 18:
                myPlayerScript.isSpecialBulletAbility1 = false;
                myPlayerScript.isSpecialBulletAbility2 = true;
                break;
            case 19:
                myPlayerScript.gotSpecialWeaponAbility = true;
                myPlayerScript.weaponCode = 1;
                myPlayerScript.toTalChargeTime = 2;
                myPlayerScript.curChargeTime = myPlayerScript.toTalChargeTime;
                myPlayerScript.curBulletAmount = 0;
                myPlayerScript.maxSpecialBullet = 20;
                myPlayerScript.weaponTotalShotCoolTime = 0.5f;
                myPlayerScript.curWeaponShotCoolTime = -1;

                GM.WeaponButtonUpdate();
                break;
        }
        StartCoroutine(SelectDelay());
        GM.ClearCards();
    }

    IEnumerator SelectDelay()
    {
        if (curSec > 4)
            yield return new WaitForSeconds(4);
        else
            yield return new WaitForSeconds(curSec);
        isReady = true;
        pv.RPC("ReadyAmountReset", RpcTarget.All);
    }

    [PunRPC]
    void ReadyAmountReset()
    {
        readyAmount++;

        if(PhotonNetwork.PlayerList.Length == readyAmount)
        {
            curSec = -11;
            GM.SelectComplete();
            readyAmount = 0;
        }
    }

    [PunRPC]
    void ReadyAmountSetZero()
    {
        readyAmount = 0;
    }


    private void Update()
    {
        if (isCellectingTime)
        {
            curSec -= Time.deltaTime;
            if (curSec < 0 && curMin > 0)
            {
                curMin--;
                curSec = 59;
            }
            if (curSec > 0)
            {
                curTimeText.text = string.Format("{0:00}:{1:00}", curMin, curSec);
            }
            else if(curSec < -10)
            {
                return;
            }
            else
            {
                isCellectingTime = false;
                GM.ClearCards();
                GM.SelectComplete();
                pv.RPC("ReadyAmountSetZero", RpcTarget.All);
            }

            if (isReady)
            {
                cTTime -= Time.deltaTime;
                if(cTTime < 0)
                {
                    cTTime = 0.3f;
                    if(cTInt == 0)
                    {
                        curConditionText.text = "Waiting for another myPlayerScript.";
                        cTInt = 1;
                    }
                    else if (cTInt == 1)
                    {
                        curConditionText.text = "Waiting for another myPlayerScript..";
                        cTInt = 2;
                    }
                    else if (cTInt == 2)
                    {
                        curConditionText.text = "Waiting for another myPlayerScript...";
                        cTInt = 0;
                    }
                }
            }
            else
            {
                curConditionText.text = "";
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(readyAmount);
        }
        else
        {
            readyAmount = (int)stream.ReceiveNext();
        }
    }
}
