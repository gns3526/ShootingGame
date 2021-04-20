﻿using System.Collections;
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

    [Header("Manager")]
    [SerializeField] GameManager GM;
    [SerializeField] ObjectPooler OP;
    [SerializeField] Cards CM;
    [SerializeField] GoogleSheetManager GSM;
    [SerializeField] AbilityManager AM;
    [SerializeField] JobManager JM;
    [SerializeField] ReinForceManager RM;

    [SerializeField] Animator StartButtonAni;

    public InputField nickNameInput;
    public GameObject connectPanel;
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
    [SerializeField] string roomOption;
    [SerializeField] bool isFilter;
    Hashtable cc;

    [Header("PlayerIcon")]
    public int playerIconCode;

    [SerializeField] int iconCycleNum;
    public Image loginPlayerIconImage;

    [Header("Others")]
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    [SerializeField] Text curPage;

    public GameObject myPlayer;

    float stopTime;
    public GameObject reconnectPanel;

    public bool isNickNameSame;

    private void Awake()
    {
        //test
        

        img.sprite = icons[playerIconCode];
        Screen.SetResolution(540, 960, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    private void Update()
    {
        lobbyInforText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "Lobby/" + PhotonNetwork.CountOfPlayers + "Connect";

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


    public void PlayerIconChangeOnLogin(int num)//-1 ? 1
    {
        if (iconCycleNum + num == -1)
            iconCycleNum = icons.Length - 1;
        else if (iconCycleNum + num == icons.Length)
            iconCycleNum = 0;
        else
            iconCycleNum = iconCycleNum + num;
        loginPlayerIconImage.sprite = icons[iconCycleNum];
        playerIconCode = iconCycleNum;
    }

    public override void OnConnectedToMaster()//2
    {
        PhotonNetwork.JoinLobby();
        GSM.loadingPanel.SetActive(false);
        //GM.LobbyPlayerRework();
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);//Make Room
    }

    public override void OnJoinedLobby()//3
    {
        Debug.Log("로비에 접속");
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        connectPanel.SetActive(false);
        GM.controlPanel.SetActive(false);

        JM.skillBPoint.SetActive(false);
        JM.skillBOn = false;

        PhotonNetwork.LocalPlayer.NickName = nickNameInput.text;
        welcomeText.text = "Welcome, " + PhotonNetwork.LocalPlayer.NickName;

        GM.SetExpPanel();

        GM.goldAmountText.text = GM.money.ToString();
        RM.LobbyReinRework();

        myList.Clear();
        MyListRenewal();

        GM.LobbyPlayerRework();
    }

    public void MyListClick(int num)//방 클릭했을때
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else
        PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        curPage.text = currentPage.ToString();
        //최대 페이지 조정
        maxPage = (myList.Count % roomCelBtn.Length == 0) ? myList.Count / roomCelBtn.Length : myList.Count / roomCelBtn.Length + 1;

        //Back or NextButten
        backBtn.interactable = (currentPage <= 1) ? false : true;
        nextBtn.interactable = (currentPage >= maxPage) ? false : true;

        //페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * roomCelBtn.Length;
        for (int i = 0; i < roomCelBtn.Length; i++)
        {
            Debug.Log("00");
            roomCelBtn[i].gameObject.SetActive((multiple + i < myList.Count) ? true : false);
            roomCelBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";//방제목텍스트 변경
            roomCelBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
            if (roomCelBtn[i].transform.GetChild(1).GetComponent<Text>().text == "")
            {
                Debug.Log("11");
                roomCelBtn[i].transform.GetChild(2).GetComponent<Image>().sprite = null;
                roomCelBtn[i].transform.GetChild(3).GetComponent<Text>().text = null;
            }
            else
            {
                Debug.Log("22");
                cc = myList[i + multiple].CustomProperties;
                roomCelBtn[i].transform.GetChild(2).GetComponent<Image>().sprite = GM.mapThumnails[(int)cc[roomOption]];
                roomCelBtn[i].transform.GetChild(3).GetComponent<Text>().text = GM.mapNames[(int)cc[roomOption]];
            }
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

        //cp[roomOption] = roommapinput.text;
        //print(cp[roomOption]);
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

    public void RoomMakePanelOpenOrClose(bool a)
    {
        GM.makeRoomPanel.SetActive(a);
    }

    public void CreateRoom()
    {
        string[] LobbyOptions = new string[1];
        LobbyOptions[0] = roomOption;
        Hashtable customProperties = new Hashtable() {
        { roomOption, GM.mapCode}
    };
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.CustomRoomPropertiesForLobby = LobbyOptions;
        roomOptions.CustomRoomProperties = customProperties;
        PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, roomOptions, TypedLobby.Default);
        GM.makeRoomPanel.SetActive(false);
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void OutingRoom()
    {
        PhotonNetwork.LeaveRoom();
    }



    public override void OnJoinedRoom()
    {
        Spawn();

        //Hashtable map = PhotonNetwork.CurrentRoom.CustomProperties;
        //test;
        //GM.curMapCode = (int)map[roomOption];
        GM.curMapCode = 0;

        GM.roomMapThumnail.sprite = GM.mapThumnails[GM.curMapCode];
        GM.roomMapName.text = GM.mapNames[GM.curMapCode];

        if (GM.isAndroid)
            GM.controlPanel.SetActive(true);
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        GM.gamePlayPanel.SetActive(false);

        GM.SetExpPanel();

        chatInput.text = "";
        RoomRenewal();

        JM.OnEnableSkill();
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

        //if(PhotonNetwork.IsMasterClient)
        //player.transform.GetChild(0).GetComponent<PhotonView>().RPC("CodyRework", RpcTarget.All, GM.codyMainCode, GM.codyBodyCode, GM.codyParticleCode);

        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + " joined the game</color>");
        
        player.GetComponent<PhotonView>().RPC("ChangeColorRPC", RpcTarget.All, GM.playerColors[0], GM.playerColors[1], GM.playerColors[2]);
        //myPlayer.transform.GetChild(0).GetComponent<PhotonView>().RPC("CodyRework", RpcTarget.All, GM.codyBodyCode, GM.codyParticleCode);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RoomRenewal();

        pv.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + otherPlayer.NickName + " left the game</color>");
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

        if(myPlayer != null)
        myPlayer.GetComponent<Player>().codyPv.RPC("CodyRework", RpcTarget.All, GM.codyMainCode, GM.codyBodyCode, GM.codyParticleCode);

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
                roomInfoText.text = "Room title : " + PhotonNetwork.CurrentRoom.Name;

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
        Player myplayerScript = myPlayer.GetComponent<Player>();

        myplayerScript.GM = GM;
        myplayerScript.NM = this;
        myplayerScript.OP = OP;
        myplayerScript.AM = AM;
        myplayerScript.JM = JM;
        myplayerScript.RFM = RM;

        GM.myplayer = myPlayer;
        GM.myplayerScript = myplayerScript;
        CM.player = myplayerScript;
        AM.myPlayerScript = myplayerScript;
        JM.myplayerScript = myplayerScript;
        RM.myplayerScript = myplayerScript;

        GM.WeaponButtonUpdate();
        GM.UpdateLifeIcon(myPlayer.GetComponent<Player>().life);
        GM.pv.RPC("AlivePlayerSet", RpcTarget.All);




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
