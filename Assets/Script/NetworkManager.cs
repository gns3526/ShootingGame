using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] PhotonView pv;

    [SerializeField] GameManager GM;
    [SerializeField] ObjectManager OM;
    [SerializeField] bool generateOnce;

    [SerializeField] InputField nickNameInput;
    [SerializeField] GameObject connectPanel;
    [SerializeField] GameObject respawnPanel;

    [SerializeField] Text playerAmountText;
    public int playerAmount;

    [Header("LobbyPanel")]
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] InputField roomInput;
    [SerializeField] Text welcomeText;
    [SerializeField] Text lobbyInforText;
    [SerializeField] Button[] roomCelBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Button nextBtn;

    [Header("RoomPanel")]
    [SerializeField] GameObject roomPanel;
    [SerializeField] Text listText;
    [SerializeField] Text roomInfoText;
    [SerializeField] GameObject[] playerInfoGroup;
    [SerializeField] int playerInfoGroupInt;
    [SerializeField] Button readyButton;
    [SerializeField] Button startButton;
    [SerializeField] GameObject[] readyImage;
    [SerializeField] bool[] playerReadyList;

    [SerializeField] bool isReady;

    bool canStart;
    [SerializeField] Text[] chatTextT;
    [SerializeField] InputField chatInput;

    [Header("ETC")]
    [SerializeField] Text curInfoText;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    public Player player;

    private void Awake()
    {
        Screen.SetResolution(540, 960, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    private void Update()
    {
        lobbyInforText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비/" + PhotonNetwork.CountOfPlayers + "접속";

        //if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
        //{
        //    connectPanel.SetActive(true);
        //    PhotonNetwork.Disconnect();
        //}
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();//1

    public override void OnConnectedToMaster()//2
    {
        PhotonNetwork.JoinLobby();

        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);//Make Room
    }

    public override void OnJoinedLobby()//3
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void MyListClick(int num)//방 클릭했을때
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        //최대 페이지 조정
        maxPage = (myList.Count % roomCelBtn.Length == 0) ? myList.Count / roomCelBtn.Length : myList.Count / roomCelBtn.Length + 1;

        //Back or NextButten
        backBtn.interactable = (currentPage <= 1) ? false : true;
        nextBtn.interactable = (currentPage >= maxPage) ? false : true;

        //페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * roomCelBtn.Length;
        for (int i = 0; i < roomCelBtn.Length; i++)
        {
            roomCelBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            roomCelBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";//방제목텍스트 변경
            roomCelBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

    public bool[] checkRedayss;


    void test()
    {
        // pv.RPC("readyCheck", RpcTarget.All);
        // pv.RPC("readyReset", RpcTarget.All);
    }



  

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, new RoomOptions { MaxPlayers = 4 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void OutingRoom() => PhotonNetwork.LeaveRoom();
    public override void OnJoinedRoom()
    {

        //pv.RPC("readyReset", RpcTarget.All); // hoon
        //readyReset();
        roomPanel.SetActive(true);
        chatInput.text = "";
        RoomRenewal();
        for (int i = 0; i < chatTextT.Length; i++)
        {
            chatTextT[i].text = "";
        }
    }
    public override void OnLeftRoom()
    {
        //isReadyLIst[playerInfoGroupInt] = false;

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        roomInput.text = ""; CreateRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        roomInput.text = ""; CreateRoom();
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)//notice everyone about joined new player
    {
        if (PhotonNetwork.LocalPlayer == newPlayer)
        {
            isReady = false;
        }
        RoomRenewal();


        //pv.RPC("readyReset", RpcTarget.All); // hoon
        //readyReset(); // hoon
        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //if( PhotonNetwork.LocalPlayer == otherPlayer)
        //{
        //    isReady = false;
        //}
        pv.RPC("ReadyRPO", RpcTarget.All, playerInfoGroupInt, 4);
        RoomRenewal();

        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()
    {

        //pv.RPC("readyReset", RpcTarget.All);//hoon
        //readyReset();

        listText.text = "";//player list text reset
        //Debug.Log(PhotonNetwork.IsMasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            readyButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
            Debug.Log("1111111111111111");
        }
        else
        {
            startButton.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(true);
        }

        for (int i = 0; i < playerInfoGroup.Length; i++)
        {
            playerInfoGroup[i].transform.GetChild(1).GetComponent<Text>().text = "";
            playerReadyList[i] = false;
            //playerInfoGroup[i].SetActive(false);
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ReadyON)
            {
                playerReadyList[i] = true;

            }
            else
            {
                playerReadyList[i] = false;

            }
        }

        int playerCount;

        for (playerCount = 0; playerCount < PhotonNetwork.PlayerList.Length; playerCount++)
        {
            listText.text += PhotonNetwork.PlayerList[playerCount].NickName + ((playerCount + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
            roomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 /" + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";

            playerInfoGroup[playerCount].SetActive(true);
            playerInfoGroup[playerCount].transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[playerCount].NickName;
            readyImage[playerCount].GetComponent<Image>().color = Color.red;


            if (PhotonNetwork.PlayerList[playerCount].NickName == PhotonNetwork.NickName)
            {
                playerInfoGroupInt = playerCount;
            }

            
        }
        pv.RPC("ReadyRPO", RpcTarget.All, playerInfoGroupInt, 3);
    }
    
    void readyReset()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            checkRedayss[i] = PhotonNetwork.PlayerList[i].ReadyON;
        }
    }
    public void Ready()
    {
        

        if (isReady)
        {
            isReady = false;
            

            // pv.RPC("readyCheck", RpcTarget.All); // hoon
            PhotonNetwork.LocalPlayer.ReadyON = false;
            
            //readyCheck() = false; // hoon
            //pv.RPC("readyReset", RpcTarget.All); //hoon
            pv.RPC("ReadyRPO", RpcTarget.All, playerInfoGroupInt, 0);
        }
        else
        {
            isReady = true;
            

            //pv.RPC("readyCheck", RpcTarget.All); // hoon
            PhotonNetwork.LocalPlayer.ReadyON = true;
            // readyCheck() = true; // hoon
            //pv.RPC("readyReset", RpcTarget.All); //hoon
            pv.RPC("ReadyRPO", RpcTarget.All, playerInfoGroupInt, 1);
        }
        RoomRenewal();
    }

    public void StartButton()
    {
        //if (isReadyLIst[0] && isReadyLIst[1] && isReadyLIst[2] && isReadyLIst[3]) canStart = true;
        //else canStart = false;

        //if (PhotonNetwork.IsMasterClient && canStart)
        //{
        //    readyButton.interactable = true;
        //}
        //else readyButton.interactable = false;
    }

    [PunRPC]
    void ReadyRPO(int index ,int colorI)
    {
        for (index = 0; index < PhotonNetwork.PlayerList.Length; index++)
        {
            Debug.Log(PhotonNetwork.PlayerList[index].ReadyON);

        }


        if(colorI == 1)
        {
            Debug.Log(PhotonNetwork.PlayerList.Length);
            //playerReadyList[index] = true;
            //PhotonNetwork.LocalPlayer.ReadyON = true;
            readyImage[index].GetComponent<Image>().color = Color.green;
        }
        else if(colorI == 0)
        {
            Debug.Log(PhotonNetwork.PlayerList.Length);
            //playerReadyList[index] = false;
            //PhotonNetwork.LocalPlayer.ReadyON = false;
            readyImage[index].GetComponent<Image>().color = Color.red;
        }
        else if(colorI == 3)
        {
            if (isReady)
            {
                Debug.Log(PhotonNetwork.PlayerList.Length);
                //playerReadyList[index] = true;
                //PhotonNetwork.LocalPlayer.ReadyON = true;
                readyImage[index].GetComponent<Image>().color = Color.green;
            }
            else
            {
                //playerReadyList[index] = false;
                //PhotonNetwork.LocalPlayer.ReadyON = false;
                readyImage[index].GetComponent<Image>().color = Color.red;
            }
        }
        else if (colorI == 4)
        {
            Debug.Log(PhotonNetwork.PlayerList.Length);
            readyImage[index].GetComponent<Image>().color = Color.red;
            //playerReadyList[index] = false;
            //PhotonNetwork.LocalPlayer.ReadyON = false;
        }
        //Debug.Log(isReady);
    }
    public void Send()
    {
        string msg = PhotonNetwork.NickName + ":" + chatInput.text;
        pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + ":" + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC]
    void ChatRPC(string msg)//recive everyone's chatting
    {
        bool isInput = false;
        for (int i = 0; i < chatTextT.Length; i++)
        {
            if(chatTextT[i].text == "")
            {
                isInput = true;
                chatTextT[i].text = msg;
                break;
            }
        }
        if (!isInput)//꽉차면 위로 한칸 올림
        {
            for (int i = 1; i < chatTextT.Length; i++)
            {
                chatTextT[i - 1].text = chatTextT[i].text;
            }
            chatTextT[chatTextT.Length - 1].text = msg;
        }
    }





    public void Spawn()
    {
        GameObject playerOB = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        if (generateOnce)
        {
            generateOnce = false;
            
            OM.Generate();
        }
        pv.RPC("ReadyRoomReset", RpcTarget.AllBuffered);

        pv.RPC("IncreasePlayerAmountRPC", RpcTarget.AllBuffered);
        respawnPanel.SetActive(false);
    }
    [PunRPC]
    void ReadyRoomReset()
    {
        //////
        PhotonView playerPv = player.GetComponent<PhotonView>();
        
        
    }



    public void DisConnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(false);
    }

    [PunRPC]
    void IncreasePlayerAmountRPC()
    {
        playerAmount++;
        playerAmountText.text = "플래이어수:" + playerAmount.ToString();

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine
        {
            stream.SendNext(checkRedayss);
            stream.SendNext(playerAmount);
            stream.SendNext(playerReadyList);
        }
        else
        {
            checkRedayss = (bool[])stream.ReceiveNext();
            playerAmount = (int)stream.ReceiveNext();
            playerReadyList = (bool[])stream.ReceiveNext();
        }
    }
}
