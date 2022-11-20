using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class NetworkManager :  MonoBehaviourPunCallbacks {
    [Header("Connection Status")]
    public Text connectionStatusText;

    [Header("Login UI Panel")]
    public InputField playerNameInput;
    public GameObject Login_UI_Panel;


    [Header("Game Object UI Panel")]
    public GameObject GameOptions_UI_Panel;

    [Header("Create Room UI Panel")]
    public GameObject Create_UI_Panel;
    public InputField roomNameInputField;
    public TextMeshProUGUI CountText;

    [Header("Inside Room UI Panel")]
    public GameObject InsideRoom_UI_Panel;
    public Text roomInfoText;
    public GameObject playerListPrefab;
    public GameObject playerLIstContent;
    public GameObject startGameButton;
    
    [Header("Room List UI Panel")]
    public GameObject RoomList_UI_Panel;
    public GameObject roomListEntryPrefab;
    public GameObject roomlListPanelGameObject;

    [Header("Join Random Room UI Panel")]
    public GameObject JoinRandomRoom_UI_Panel;

    [Header("Leave Room Button")]
    public GameObject LeaveButton;
    [Header("Number Text")]
    public TextMeshProUGUI StartingText;

    [Header("Loading Scene")]
    public GameObject LoadingPanel;

    public const float  startNumber = 5.0f;


    private int PlayerCount = 6;
    private Dictionary<string, RoomInfo> cacheRoomList;
    private Dictionary<string, GameObject> roomLIstGameObjects;
    //플레이어의 Identifier로 플레이어를 업데이트하는 방식
    private Dictionary<int, GameObject> playerListGameobjects;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            ActivatePanel(Login_UI_Panel.name);
        } else
        {
            ActivatePanel(GameOptions_UI_Panel.name);
        }
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerName")))
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("PlayerName");
            PhotonNetwork.ConnectUsingSettings();
        }

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 0);
        }
        if(PlayerPrefs.GetInt("Mic") == 0)
        {
            PlayerPrefs.SetInt("Mic", 0);
        }

        CountText.text = PlayerCount.ToString();
        cacheRoomList = new Dictionary<string, RoomInfo>();
        roomLIstGameObjects = new Dictionary<string, GameObject>();
        //한번에 씬을 씽크로 맞출 것이냐.
    }

    // Update is called once per frame
    void Update()
    {
        connectionStatusText.text = "Connection status : " + PhotonNetwork.NetworkClientState;
    }
    #endregion

    #region UI CallBacks

    public void OnSinglePlayClicked()
    {
        ActivatePanel(LoadingPanel.name);
        PhotonNetwork.LoadLevel("SinglePlay");
    }
    public void OnLoginButtonClicked()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PlayerPrefs.SetString("PlayerName", playerName);
            PhotonNetwork.ConnectUsingSettings();
            ActivatePanel(LoadingPanel.name);
        }
        else
        {
            Debug.Log("Player Name Invalid");
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = roomNameInputField.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "Room" + Random.Range(1000, 10000);

        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)PlayerCount;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { RunningGameManager.TEACHER_FACTOR, 0 }, { RoomProperty.PLAYTIME, 10 }, { RoomProperty.ITEM_GENTIME, 10 }, { RoomProperty.RESP_GENTIME, 3 }, { RoomProperty.TEACHER_NUMBER, 1 }  };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    } 
    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptions_UI_Panel.name);
    }
    public void OnShowRoomListButtonClicked()
    {
        if(!PhotonNetwork.InLobby) 
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(RoomList_UI_Panel.name);
    }
    //로비에 존재하면 룸들이 존재하는 것을 열람이 가능함 근데 나가면 그럴 필요가 없음.
    public void OnBackButtonClicked()
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(GameOptions_UI_Panel.name);
    }
    
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        ActivatePanel(LoadingPanel.name);
        PhotonNetwork.JoinRandomRoom();
    }
    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
           
            int Number = Random.Range(1, PhotonNetwork.CurrentRoom.PlayerCount + 1);
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.TEACHER_FACTOR, Number }};
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            
            Debug.Log("MasterClient");
            PhotonNetwork.LoadLevel("Dungeon1");

            
        }
    }
    #endregion


    #region Photon Callbacks
    //인터넷 연결 성공시
    public override void OnConnected()
    {
        Debug.Log("Connected To Internet");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is connected to Photon");
        if (PlayerPrefs.GetInt("TutorialFinish") == 0)
        {
            LoadingPanel.SetActive(true);
            SceneManager.LoadSceneAsync("TutorialScene");
        }
        else
        {
            ActivatePanel(GameOptions_UI_Panel.name);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is Created.");
        LoadingPanel.SetActive(true);

    }
    public override void OnJoinedRoom()
    {
        LoadingPanel.SetActive(true);
        ExitGames.Client.Photon.Hashtable Property = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.PLAYER_READY, false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(Property);
        PhotonNetwork.LoadLevel("Main");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        foreach(RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            //Room이 RemovedFromList 된 것 = 다 찼거나 아니면 삭제가 되었거나 둘중 하나임.
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cacheRoomList.ContainsKey(room.Name))
                {
                    cacheRoomList.Remove(room.Name);
                }
            }
            else
            {
                //update CacheRoom List
                if (cacheRoomList.ContainsKey(room.Name))
                {
                    cacheRoomList[room.Name] = room;
                }
                //add new Room tothe Cached Room
                else {
                    cacheRoomList.Add(room.Name, room);
                }

            }
         }

        foreach(RoomInfo room in cacheRoomList.Values)
        {
            GameObject roomlistEntryGameObject = Instantiate(roomListEntryPrefab);
            roomlistEntryGameObject.transform.SetParent(roomlListPanelGameObject.transform);
            roomlistEntryGameObject.transform.localScale = Vector3.one;
            roomlistEntryGameObject.transform.Find("RoomNameText").GetComponent<Text>().text = room.Name;
            roomlistEntryGameObject.transform.Find("RoomPlayersText").GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers;
            roomlistEntryGameObject.GetComponent<Button>().onClick.AddListener(() => OnJoinRoomButtonClicked(room.Name));
            roomLIstGameObjects.Add(room.Name, roomlistEntryGameObject);
        }

    }

    public override void OnLeftLobby()
    {
        ClearRoomListView();
        cacheRoomList.Clear();
    }

    //새로운 플레이어가 들어왔을 때 수행되는 Remote Callback

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        string roomName = "Room" + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { RunningGameManager.IS_START, false }, { RunningGameManager.TEACHER_FACTOR, 0 }, { RoomProperty.PLAYTIME, 10 }, { RoomProperty.ITEM_GENTIME, 10 }, { RoomProperty.RESP_GENTIME, 3 }, { RoomProperty.TEACHER_NUMBER, 1 } };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    #endregion

    #region Private Methods
    private void OnJoinRoomButtonClicked(string _roomName)
    {
        if(PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);
    }
    void ClearRoomListView()
    {
        foreach (var roomListGameObject in roomLIstGameObjects.Values)
        {
            Destroy(roomListGameObject);
        }
        roomLIstGameObjects.Clear();
    }
    #endregion

    #region Public Methods
    public void ActivatePanel(string panelToBeActivated)
    {
        Login_UI_Panel.SetActive(panelToBeActivated.Equals(Login_UI_Panel.name));
        GameOptions_UI_Panel.SetActive(panelToBeActivated.Equals(GameOptions_UI_Panel.name));
        Create_UI_Panel.SetActive(panelToBeActivated.Equals(Create_UI_Panel.name));
        InsideRoom_UI_Panel.SetActive(panelToBeActivated.Equals(InsideRoom_UI_Panel.name));
        RoomList_UI_Panel.SetActive(panelToBeActivated.Equals(RoomList_UI_Panel.name));
        JoinRandomRoom_UI_Panel.SetActive(panelToBeActivated.Equals(JoinRandomRoom_UI_Panel.name));
        LoadingPanel.SetActive(panelToBeActivated.Equals(LoadingPanel.name));
    }
    public void UpPress()
    {
        if(++PlayerCount > 11)
        {
            PlayerCount = 6;
        }
        CountText.text = PlayerCount.ToString();
    }
    public void DownPress()
    {
        if(--PlayerCount < 6)
        {
            PlayerCount = 11;
        }
        CountText.text = PlayerCount.ToString();
    }
    #endregion


}
