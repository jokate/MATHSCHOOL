using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Text;
using Photon.Realtime;

public struct Roomoptions
{
    public int Time;
    public int TeacherNumber;
    public int ItemGenTime;
    public int RespawnBeaconGenTime;
}

public class RoomManagement : MonoBehaviourPunCallbacks
{
    public GameObject RoomManagementUI;
    private int time;
    private int teacherNumber;
    private int itemGenTime;
    private int respawnBeaconGenTime;

    #region Public Variable Set
    [Header("Update UI")]
    public TextMeshProUGUI TimeUI, TeacherNumberUI, ItemGenTImeUI, RespawnBeaconGenTimeUI, EntireText;
    [Header("Beacon UI")]
    public GameObject BeaconObject;


    #endregion
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        RoomManagementUI.SetActive(true);
        Initialize();
        UpdateEntireText();
    }

    #region Initialize and Text Processing

    private void Initialize()
    {
        InitializingValue();
        TimeUI.text = time.ToString();
        TeacherNumberUI.text = teacherNumber.ToString();
        ItemGenTImeUI.text = itemGenTime.ToString();
        RespawnBeaconGenTimeUI.text = respawnBeaconGenTime.ToString();
    }
    private void InitializingValue()
    {
        object valObj;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.PLAYTIME, out valObj);
        time = (int)valObj;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.ITEM_GENTIME, out valObj);
        itemGenTime = (int)valObj;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.TEACHER_NUMBER, out valObj);
        teacherNumber= (int)valObj;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.RESP_GENTIME, out valObj);
        respawnBeaconGenTime = (int)valObj;
    }
    private void UpdateEntireText()
    { 
        StringBuilder sb = new StringBuilder();
        sb.Append("게임 설정 \n");
        sb.Append($"게임 시간 (분) : {time} \n");
        sb.Append($"선생님 수 (명) : {teacherNumber} \n");
        sb.Append($"상자 재생성 시간 (초) : {itemGenTime} \n");
        sb.Append($"부활 비컨 재생성 시간 (분) : {respawnBeaconGenTime} \n");
        EntireText.text = sb.ToString();
    }

    #endregion
    //관찰 변수 초기 선언 함수. 

    //관찰 변수가 업데이트가 되었을 때.
    #region Photon Call
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        object objValue;
        if (propertiesThatChanged.ContainsKey(RoomProperty.PLAYTIME))
        {
            Debug.Log("Time Changed");
            propertiesThatChanged.TryGetValue(RoomProperty.PLAYTIME, out objValue);
            time = (int)objValue;
            TimeUI.text = time.ToString();
         }
        else if (propertiesThatChanged.ContainsKey(RoomProperty.TEACHER_NUMBER))
        {
            Debug.Log("Teacher Number Changed");
            propertiesThatChanged.TryGetValue(RoomProperty.TEACHER_NUMBER, out objValue);
            teacherNumber = (int)objValue;
            TeacherNumberUI.text = teacherNumber.ToString();

        } else if(propertiesThatChanged.ContainsKey(RoomProperty.RESP_GENTIME))
        {
            Debug.Log("Respawn Beacon Number Changed");
            propertiesThatChanged.TryGetValue(RoomProperty.RESP_GENTIME, out objValue);
            respawnBeaconGenTime = (int)objValue;
            RespawnBeaconGenTimeUI.text = respawnBeaconGenTime.ToString();
        }
        else if(propertiesThatChanged.ContainsKey(RoomProperty.ITEM_GENTIME))
        {
            Debug.Log("Item Generate Number Changed");
            propertiesThatChanged.TryGetValue(RoomProperty.ITEM_GENTIME, out objValue);
            itemGenTime = (int)objValue;
            ItemGenTImeUI.text = itemGenTime.ToString();
        }
        UpdateEntireText();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }
    #endregion
    //관찰 변수 업데이팅 함수
    #region Update Button Process
    public void Close()
    {
        BeaconObject.SetActive(false);
    }
    public void UpTimeValue()
    {
        time += 5;
        if(time > 30)
        {
            time = 10;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable(){ { RoomProperty.PLAYTIME, time}};
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }
    public void DownTimeValue()
    {
        time -= 5;
        if (time < 10)
        {
            time = 30;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.PLAYTIME, time } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }
    public void UpTeacherValue()
    {
        teacherNumber++;
        if (teacherNumber > 2)
        {
            teacherNumber = 1; 
        }
        TeacherNumberUI.text = teacherNumber.ToString();
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.TEACHER_NUMBER, teacherNumber } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }
    public void DownTeacherValue()
    {
        teacherNumber--;
        if (teacherNumber < 1)
        {
            teacherNumber = 2;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.TEACHER_NUMBER, teacherNumber } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }
    public void UpItemGenTimeValue()
    {
        itemGenTime += 5;
        if(itemGenTime > 30)
        {
            itemGenTime = 10;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.ITEM_GENTIME, itemGenTime } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }

    public void DownItemGenTimeValue()
    {
        itemGenTime -= 5;
        if (itemGenTime < 10)
        {
            itemGenTime = 30;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.ITEM_GENTIME, itemGenTime } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }

    public void UpRespTimeValue()
    {
        respawnBeaconGenTime++;
        if (respawnBeaconGenTime > 5)
        {
            respawnBeaconGenTime = 1;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.RESP_GENTIME, respawnBeaconGenTime} };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);
    }
    public void DownRespTimeValue()
    {
        respawnBeaconGenTime--;
        if (respawnBeaconGenTime < 1)
        {
            respawnBeaconGenTime = 5;
        }
        ExitGames.Client.Photon.Hashtable roomprops = new ExitGames.Client.Photon.Hashtable() { { RoomProperty.RESP_GENTIME, respawnBeaconGenTime } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomprops);

    }
    #endregion
}
