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
    [SerializeField] int readyPlayerAmount;

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
    [SerializeField] Button startButton;

    [SerializeField] bool isReady;
    [SerializeField] BoxCollider2D readyArea;
    GameObject[] temp;

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
        connectPanel.SetActive(false);
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

    public void OutingRoom()
    {
        pv.RPC("IncreasePlayerAmountRPC", RpcTarget.AllBuffered, false);
        PhotonNetwork.LeaveRoom();
    }
    

    public override void OnJoinedRoom()
    {
        Spawn();
        pv.RPC("IncreasePlayerAmountRPC", RpcTarget.AllBuffered, true);
        //pv.RPC("readyReset", RpcTarget.All); // hoon
        //readyReset();
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        chatInput.text = "";
        RoomRenewal();
        for (int i = 0; i < chatTextT.Length; i++)
        {
            chatTextT[i].text = "";
        }
    }
    public override void OnLeftRoom()
    {


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
        Invoke("RoomRenewal", 0.1f);
        //RoomRenewal();

        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    public void RoomRenewal()
    {
        listText.text = "";//player list text reset


        playerAmount = PhotonNetwork.PlayerList.Length;


        if (PhotonNetwork.IsMasterClient)
        {
            readyArea.enabled = true;
            startButton.gameObject.SetActive(true);

            pv.RPC("IncreaseReadyAmount", RpcTarget.All, true, true);
            temp = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.position.x < 0)
                {
                    pv.RPC("IncreaseReadyAmount", RpcTarget.All, true, false);
                }
                else
                {

                }
            }
        }
        else
        {
            readyArea.enabled = false;
            startButton.gameObject.SetActive(false);
        }

        int playerCount;

        for (playerCount = 0; playerCount < PhotonNetwork.PlayerList.Length; playerCount++)
        {
            listText.text += PhotonNetwork.PlayerList[playerCount].NickName + ((playerCount + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
            roomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 /" + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";

            playerInfoGroup[playerCount].SetActive(true);
            playerInfoGroup[playerCount].transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[playerCount].NickName;


            if (PhotonNetwork.PlayerList[playerCount].NickName == PhotonNetwork.NickName)
            {
                playerInfoGroupInt = playerCount;
            }

            
        }
        
    }

    public void StartButton()
    {

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
        for (int i = 0; chatTextT.Length < chatTextT.Length; i--)
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
        GameObject playerOB = PhotonNetwork.Instantiate("Player", new Vector3(1.6f, 0, 0), Quaternion.identity);
        if (generateOnce)
        {
            generateOnce = false;
            
            OM.Generate();
        }
        pv.RPC("ReadyRoomReset", RpcTarget.AllBuffered);

        
        respawnPanel.SetActive(false);
    }

    public void DisConnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        connectPanel.SetActive(true);
    }



    [PunRPC]
    void IncreaseReadyAmount(bool isIncrease, bool isReset)
    {
        if (isReset)
        {
            readyPlayerAmount = 0;
        }
        else
        {
            if (isIncrease)
            {
                Debug.Log("엥ㄹㄹ리잉");
                readyPlayerAmount++;
            }
            else if (!isIncrease)
            {
                Debug.Log("엥ㄹㄹ리잉22");
                readyPlayerAmount--;
            }
        }

        if (PhotonNetwork.IsMasterClient && (playerAmount == readyPlayerAmount))
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine
        {
            stream.SendNext(checkRedayss);
            stream.SendNext(readyPlayerAmount);
        }
        else
        {
            checkRedayss = (bool[])stream.ReceiveNext();
            readyPlayerAmount = (int)stream.ReceiveNext();
        }
    }
}
