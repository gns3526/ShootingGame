using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Cards : MonoBehaviourPunCallbacks,IPunObservable
{
    public Player player;
    [SerializeField] GameManager GM;
    [SerializeField] PhotonView pv;

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
                if (player.shotCoolTimeReduce != 10)
                {
                    player.shotCoolTimeReduce -= 10;
                }

                break;
            case 2:
                player.moveSpeed += 1;
                break;
            case 3:
                player.increaseDamage += 3;
                break;
            case 4:
                if (player.power != player.maxPower)
                {
                    player.power++;
                }
                break;
            case 5:
                if (player.maxLife != 10)
                {
                    player.maxLife++;
                    player.life++;
                    GM.UpdateLifeIcon(player.life);
                }
                break;
            case 6:
                player.life = player.maxLife;
                GM.UpdateLifeIcon(player.life);
                break;
            case 7:
                player.AddFollower(1);
                break;
            case 8:
                player.AddFollower(2);
                break;
            case 9:
                player.godTime += 2;
                break;
            case 10:
                player.godTime += 2;
                break;
        }
        Debug.Log("카드 고르기1");
        GM.ClearCards();
        isReady = true;
        pv.RPC("ReadyAmountReset", RpcTarget.All);
        Debug.Log("카드 고르기2");
    }



    [PunRPC]
    void ReadyAmountReset()
    {
        readyAmount++;

        if(PhotonNetwork.PlayerList.Length == readyAmount)
        {
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
                        curConditionText.text = "다른플래이어 대기중.";
                        cTInt = 1;
                    }
                    else if (cTInt == 1)
                    {
                        curConditionText.text = "다른플래이어 대기중..";
                        cTInt = 2;
                    }
                    else if (cTInt == 2)
                    {
                        curConditionText.text = "다른플래이어 대기중...";
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
