using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] PhotonView pv;
    [SerializeField] PhotonView pvGM;

    [SerializeField] GameManager GM;
    [SerializeField] ObjectPooler OP;
    [SerializeField] Cards CM;
    [SerializeField] GoogleSheetManager GSM;
    [SerializeField] AbilityManager AM;

    [SerializeField] Animator StartButtonAni;

    public InputField nickNameInput;
    [SerializeField] GameObject connectPanel;
    [SerializeField] GameObject respawnPanel;

    [SerializeField] Text playerAmountText;
    public int playerAmount;
    [SerializeField] int readyPlayerAmount;

    [Header("LobbyPanel")]
    public GameObject lobbyPanel;
    [SerializeField] InputField roomInput;
    [SerializeField] Text welcomeText;
    [SerializeField] Text lobbyInforText;
    [SerializeField] Button[] roomCelBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Button nextBtn;

    [Header("RoomPanel")]
    public GameObject roomPanel;
    [SerializeField] Text listText;
    [SerializeField] Text roomInfoText;
    [SerializeField] GameObject[] playerInfoGroup;
    public int playerInfoGroupInt;
    [SerializeField] GameObject[] playerLIst;
    [SerializeField] Button startButton;

    [SerializeField] BoxCollider2D readyArea;
    GameObject[] temp;

    bool canStart;
    [SerializeField] Text[] chatTextT;
    [SerializeField] InputField chatInput;
    [SerializeField] Animator chatAni;
    public bool isChating;

    public Sprite[] icons;
    public Image img;

    [Header("ETC")]
    [SerializeField] Text curInfoText;
    [SerializeField] InputField lobbymapinput;
    [SerializeField] InputField roommapinput;
    [SerializeField] string roomOption;
    [SerializeField] bool isFilter;

    [Header("PlayerInfo")]
    public int playerIconCode;


    [Header("Others")]
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    public GameObject myPlayer;

    float stopTime;
    public GameObject reconnectPanel;

    public bool isNickNameSame;

    private void Awake()
    {
        img.sprite = icons[playerIconCode];
        Screen.SetResolution(540, 960, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    private void Update()
    {
        lobbyInforText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비/" + PhotonNetwork.CountOfPlayers + "접속";

        if (Input.GetKeyDown(KeyCode.Return))
        {

            Send();

            chatInput.ActivateInputField();

            chatInput.Select();
            Debug.Log("체팅");

        }
        else if (chatInput.isFocused && !isChating)
        {
            chatAni.SetTrigger("On");

            isChating = true;
        }
        else if(!chatInput.isFocused && isChating)
        {
            chatAni.SetTrigger("Off");
            chatInput.text = "";
            isChating = false;
        }
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();//1


    public override void OnConnectedToMaster()//2
    {
        PhotonNetwork.JoinLobby();
        GSM.loadingPanel.SetActive(false);
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);//Make Room
    }

    public override void OnJoinedLobby()//3
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        connectPanel.SetActive(false);
        GM.controlPanel.SetActive(false);

        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        welcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";

        GM.SetExpPanel();

        GM.goldAmountText.text = GM.money.ToString();

        myList.Clear();
        MyListRenewal();
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
        Debug.Log(myList);
    }

    public void JoinPeilterRoom()
    {
        Hashtable proPer = new Hashtable() { { roomOption, lobbymapinput.text } };
        PhotonNetwork.JoinRandomRoom(proPer, 4);
        
    }

    public void ChangeMap()
    {
        Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;

        cp[roomOption] = roommapinput.text;
        print(cp[roomOption]);
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
        Hashtable cc = myList[0].CustomProperties;
        print(cc[roomOption]);
        MyListRenewal();
    }


    public void CreateRoom()
    {
        string[] LobbyOptions = new string[1];
        LobbyOptions[0] = roomOption;
        Hashtable customProperties = new Hashtable() {
        { roomOption, lobbymapinput.text }
    };
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomPropertiesForLobby = LobbyOptions;
        roomOptions.CustomRoomProperties = customProperties;
        PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, roomOptions, TypedLobby.Default);
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void OutingRoom()
    {
        PhotonNetwork.LeaveRoom();
    }



    public override void OnJoinedRoom()
    {
        Spawn();

        if (PhotonNetwork.IsMasterClient)
        {
            GM.selectMapButton.interactable = true;
        }
        else
            GM.selectMapButton.interactable = false;

        if(GM.isAndroid)
            GM.controlPanel.SetActive(true);

        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        GM.gamePlayPanel.SetActive(false);

        GM.SetExpPanel();

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
        RoomRenewal();

        Player player = myPlayer.GetComponent<Player>();

        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        
        player.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All, GM.playerColors[0], GM.playerColors[1], GM.playerColors[2]);
        myPlayer.transform.GetChild(0).GetComponent<PhotonView>().RPC("CodyRework", RpcTarget.All, GM.codyBodyCode, GM.codyParticleCode);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Invoke("RoomRenewal", 0.1f);
        RoomRenewal();

        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
        if (GM.isGameStart)
        {
            if (otherPlayer.IsMasterClient)
                //pv.RPC("KickAll", RpcTarget.All);
                GM.GoToLobby();
        }

    }


    [PunRPC]
    void num(int index , int ssss)
    {
        if (0 == index)
            playerInfoGroup[index].GetComponent<Player_Icon>().image.sprite = icons[ssss];
        else if (1 == index)
            playerInfoGroup[index].GetComponent<Player_Icon>().image.sprite = icons[ssss];
        else if (2 == index)
            playerInfoGroup[index].GetComponent<Player_Icon>().image.sprite = icons[ssss];
        else if (3 == index)
            playerInfoGroup[index].GetComponent<Player_Icon>().image.sprite = icons[ssss];
    }
    


    public void RoomRenewal()
    {


        if (pv.IsMine)
            myPlayer.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All, GM.playerColors[0], GM.playerColors[1], GM.playerColors[2]);

        listText.text = "";//player list text reset


        playerAmount = PhotonNetwork.PlayerList.Length;
      

        if (PhotonNetwork.IsMasterClient)
        {
            readyArea.enabled = true;
            startButton.gameObject.SetActive(true); //내가반장

            pv.RPC("IncreaseReadyAmount", RpcTarget.All, true, true);
            temp = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.position.x < 0)
                {
                    pv.RPC("IncreaseReadyAmount", RpcTarget.All, true, false);
                }
            }
        }
        else
        {
            readyArea.enabled = false;
            startButton.gameObject.SetActive(false); // 딴사람 일경우 레디버튼 없음 
        }


        for (int i = 0; i < 4; i++)
        {
            if(i < PhotonNetwork.PlayerList.Length)
            {
                roomInfoText.text = "방제목 : " + PhotonNetwork.CurrentRoom.Name;

                //playerInfoGroup[i].  = 게임메니저 초상화가져옴
                playerInfoGroup[i].SetActive(true);
                
                playerInfoGroup[i].transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
                if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.NickName)
                {
                    playerInfoGroupInt = i;
                    pv.RPC("num", RpcTarget.All, playerInfoGroupInt , playerIconCode);
                }
            }
            else
            {
                playerInfoGroup[i].SetActive(false);
                playerInfoGroup[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }

    public void StartButton()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        pvGM.RPC("StageStart", RpcTarget.All);


    }




    public void Send()
    {
        if (chatInput.text == "") return;
        string msg = PhotonNetwork.NickName + ":" + chatInput.text;
        pv.RPC("ChatRPC", RpcTarget.All, msg);
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

        myPlayer = PhotonNetwork.Instantiate("Player", new Vector3(1.6f, 0, 0), Quaternion.identity);
        GM.myplayer = myPlayer;
        CM.player = myPlayer.GetComponent<Player>();
        AM.myPlayerScript = myPlayer.GetComponent<Player>();

        GM.WeaponButtonUpdate();
        GM.UpdateLifeIcon(myPlayer.GetComponent<Player>().life);
        GM.pv.RPC("AlivePlayerSet", RpcTarget.All);

        //OP.PrePoolInstantiate();


        respawnPanel.SetActive(false);

        
        //GM.AlivePlayerSet();
    }

    public void DisConnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        GM.controlPanel.SetActive(false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        connectPanel.SetActive(true);
    }


    [PunRPC]
    void KickAll()
    {
        GM.GoToLobby();
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
        }

        if (PhotonNetwork.IsMasterClient && (playerAmount == readyPlayerAmount))
        {
            startButton.interactable = true;
            // 애니메이션 트리거 추가
            //StartButtonAni.SetTrigger("GameOn");
            StartButtonAni.SetBool("GameOn", true);
        }
        else
        {
            startButton.interactable = false;
            // 애니메이션 트리거 추가
            //StartButtonAni.SetTrigger("GameOff");
            StartButtonAni.SetBool("GameOn", false);
        }
    }


    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine
        {
            stream.SendNext(readyPlayerAmount);
        }
        else
        {
            readyPlayerAmount = (int)stream.ReceiveNext();
        }
    }
}
