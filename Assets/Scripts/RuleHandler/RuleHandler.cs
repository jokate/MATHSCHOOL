using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class RuleHandler : MonoBehaviourPunCallbacks
{
    private int time, resp;
    public TextMeshProUGUI GameCount, CatchedPeopleText, GameOverText;
    public bool NeedToRespawn;
    public static bool isFirst;
    bool countDown = true;
    bool NotReady = true;
    public static int CatchedPeople = 0;
    public GameObject RespBeacon, LoadingPref, win, lose, gameStart, Resp;
    public List<GameObject> RespList;
    public AudioSource start, winsound, losesound;

    
    private void Awake()
    {
        isFirst = true;
        NeedToRespawn = false;
        Debug.Log("Started");
        object timeObj;

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.PLAYTIME, out timeObj))
        {
            time = (int)timeObj * 60;
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.RESP_GENTIME, out timeObj)) {
            resp = (int)timeObj * 60;
        }
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.CATCHED_PEOPLE, 0 } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        Debug.Log("Catced : " + CatchedPeople);
        
    }

    private void Start()
    {
        countDown = true;
        CatchedPeopleText.text = CatchedPeople + "/" + (PhotonNetwork.CurrentRoom.PlayerCount - InstantiateManager.TeacherTotalNumber);
    }

    private void Update()
    {
        if (NotReady)
        {
            LoadingPref.SetActive(true);
            int i = 0;
            foreach (var game in GameObject.FindGameObjectsWithTag("Dokkebi"))
                i++;
            foreach (var game in GameObject.FindGameObjectsWithTag("Teacher"))
                i++;
            if (i == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                StartCoroutine(StartGame());
                NotReady = false;
            }
        }
        else
        {
            if (!countDown)
            {
                if (CatchedPeople >= (PhotonNetwork.CurrentRoom.PlayerCount / 2))
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        int number = Random.Range(0, RespList.Count);
                        if (PhotonNetwork.IsMasterClient)
                        {
                            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.POSITION_SET, number }, { RunningGameManager.RESP_BEACON, false } };
                            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                        }
                        if (InstantiateManager.isteacher)
                            Resp.SetActive(true);

                    }
                    if (NeedToRespawn)
                    {
                        NeedToRespawn = false;
                        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.RESP_BEACON, false } };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                        StartCoroutine(Respawn());

                    }
                }
            }
        }
       
    }


    IEnumerator StartGame()
    {
        StartSet(false);
        LoadingPref.SetActive(false);
        GameCount.enabled = false;
        GameOverText.enabled = true;
        float time = 5.0f;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            GameOverText.text = (int)(time + 1) + "초 뒤에 게임이 시작됩니다!";
            yield return null;
        }
        start.Play();
        GameOverText.enabled = false;
        GameCount.enabled = true;
        gameStart.SetActive(true);
        countDown = false;
        StartSet(true);
        StartCoroutine(CountDown());
   
    }

    public void StartSet(bool bSet)
    {
        foreach (var game in GameObject.FindGameObjectsWithTag("Dokkebi"))
            game.GetComponent<MoveAble>().enabled = bSet;
        foreach (var game in GameObject.FindGameObjectsWithTag("Teacher"))
            game.GetComponent<MoveAble>().enabled = bSet;
    }

    IEnumerator CountDown()
    {
        float ftime = time;
        while (ftime > 0.0f)
        {
            ftime -= Time.deltaTime;
            GameCount.text =((int)ftime / 60).ToString() + " : " + ((int)ftime % 60).ToString();
            yield return null;
        }
        GameCount.enabled = false;
        Debug.Log("Game Over");

        EndGame(true);

    }

    IEnumerator Respawn() {
        Debug.Log("Start");
        float ftime = resp;
        while(ftime > 0.0f)
        {
            ftime -= Time.deltaTime;
            yield return null;
        }
        int number = Random.Range(0, RespList.Count);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.POSITION_SET, number }};
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CatchedPeopleText.text = CatchedPeople + "/" + (PhotonNetwork.CurrentRoom.PlayerCount - InstantiateManager.TeacherTotalNumber);
        
        if(CatchedPeople == PhotonNetwork.CurrentRoom.PlayerCount - InstantiateManager.TeacherTotalNumber)
        {
            EndGame(false);
        }
        base.OnPlayerLeftRoom(otherPlayer);
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        object obj;
        if (propertiesThatChanged.ContainsKey(RunningGameManager.RESP_BEACON))
        { 
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RunningGameManager.RESP_BEACON, out obj))
                NeedToRespawn = (bool)obj;
        }
        if(propertiesThatChanged.ContainsKey(RunningGameManager.CATCHED_PEOPLE))
        {
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RunningGameManager.CATCHED_PEOPLE, out obj))
            {
                CatchedPeople = (int)obj;
                CatchedPeopleText.text = "잡힌 사람 : " + CatchedPeople + "/" + (PhotonNetwork.CurrentRoom.PlayerCount - InstantiateManager.TeacherTotalNumber);
                if (CatchedPeople == (PhotonNetwork.CurrentRoom.PlayerCount - InstantiateManager.TeacherTotalNumber))
                {
                    if (!InstantiateManager.isteacher)
                    {
                        Debug.Log("승리");
                    }
                    else
                    {
                        Debug.Log("패배");
                    }
                    EndGame(false);
                }
            }
         }
         if(propertiesThatChanged.ContainsKey(RunningGameManager.POSITION_SET))
         {
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RunningGameManager.POSITION_SET, out obj))
            {
                   Instantiate(RespBeacon, RespList[(int)obj].transform.position, Quaternion.identity);
            }
         }
    }
    public void EndGame(bool Teacherwin)
    {
        ExitGames.Client.Photon.Hashtable roomProp = new ExitGames.Client.Photon.Hashtable { { RunningGameManager.IS_START, false }, { RunningGameManager.TEACHER_FACTOR, 0 } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProp);

        if (Teacherwin)
        {
            if(InstantiateManager.isteacher)
            {
                win.SetActive(true);
                winsound.Play();
            }
            else
            {
                lose.SetActive(true);
                losesound.Play();
            }
        }
        else
        {
            if(InstantiateManager.isteacher)
            {

                lose.SetActive(true);
                losesound.Play();
            }   
            else
            {
                win.SetActive(true);
                winsound.Play();
            }
        }
    }



    IEnumerator LeftScene()
    {
        ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.PLAYER_READY, false } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.CurrentRoom.IsVisible = true;
        yield return null;
        RuleHandler.CatchedPeople = 0;
        LoadingPref.SetActive(true);
        Time.timeScale = 1;
        PhotonNetwork.LoadLevel("Main");
    }
    public void LeftSceneButtonPress()
    {
        StartCoroutine(LeftScene());
    }

      
}
