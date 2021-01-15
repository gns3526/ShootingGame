using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Cards : MonoBehaviourPunCallbacks,IPunObservable
{
    public Player player;
    [SerializeField] GameManager GM;

    [SerializeField] int readyAmount;
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
        }


    }



    [PunRPC]
    void ReadyAmountReset()
    {
        readyAmount++;


        if(PhotonNetwork.CountOfPlayersInRooms == readyAmount)
        {
            GM.SelectComplete();
            readyAmount = 0;
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
