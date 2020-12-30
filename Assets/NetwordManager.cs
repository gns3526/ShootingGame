﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetwordManager : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField] PhotonView pv;

    [SerializeField] GameManager GM;

    [SerializeField] InputField nickNameInput;
    [SerializeField] GameObject connectPanel;
    [SerializeField] GameObject respawnPanel;

    [SerializeField] Text playerAmountText;
    public int playerAmount;

    private void Awake()
    {
        Screen.SetResolution(540, 960, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();//1

    public override void OnConnectedToMaster()//2
    {
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;//nickNameSetting
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);//Make Room
    }

    public override void OnJoinedRoom()//3
    {
        connectPanel.SetActive(false);
        Spawn();
    }
    public void Spawn()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        pv.RPC("IncreasePlayerAmountRPC", RpcTarget.AllBuffered);
        respawnPanel.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
    }



    public override void OnDisconnected(DisconnectCause cause)
    {
        connectPanel.SetActive(true);
        respawnPanel.SetActive(false);
    }

    [PunRPC]
    void IncreasePlayerAmountRPC()
    {
        playerAmount++;
        playerAmountText.text = "플래이어수:" + playerAmount.ToString();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerAmount);
        }
        else
        {
            playerAmount = (int)stream.ReceiveNext();
        }

    }
}
