using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Player_Icon : MonoBehaviourPun
{ 
    [SerializeField] NetworkManager NM;

    public int playerIconCode;
    [SerializeField] Sprite[] imageGroup;

    public Image image;
    [SerializeField] Text text;
    /*
   
    
    public void bbb()
    {
        for (int i = 0; i < 4; i++)
        {
            if (text.text == PhotonNetwork.NickName)
            {
                playerIconCode = NM.playerIconCode;
            }
        }
    }

    public void aaa()
    {
        switch (playerIconCode)
        {
            case 1:
                image.sprite = imageGroup[playerIconCode];
                break;
            case 2:
                image.sprite = imageGroup[playerIconCode];
                break;
            case 3:
                image.sprite = imageGroup[playerIconCode];
                break;
            case 4:
                image.sprite = imageGroup[playerIconCode];
                break;
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            stream.SendNext(playerIconCode);
        }
        else
        {
            playerIconCode = (int)stream.ReceiveNext();
        }
    }
    */
}


