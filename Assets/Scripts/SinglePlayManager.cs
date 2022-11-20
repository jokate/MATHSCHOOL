using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using Photon.Pun;
public class SinglePlayManager : MonoBehaviour
{
    public static int Key;
    public static float InitialTime;
    const float downTime = 5.0f;
    public const int MaxKey = 3;
    public TextMeshProUGUI text, SpawnText, KeyText;
    static bool CountDownStart = false;
    public GameObject go, LoadingPanel, Lose, start;
    public List<string> RoomName;
    public List<RoomLock> LockingRoom;
    public static bool AllKeyGathered = false;
    public AudioSource startsound, loseSound;
 
   
    private void Start()
    {
        CountDownStart = false;
        AllKeyGathered = false;
        InitialTime = 30.0f;
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        KeyText.text = Key + "/" + MaxKey;
        if (!RoomName.Any())
        {
            loseSound.Play();
            Lose.SetActive(true);
        }

        if (CountDownStart)
        {
            if (InitialTime > 0.0f)
            {
                if (!AllKeyGathered)
                {
                    text.text = "0 : " + (int)InitialTime;
                    
                } else
                {
                    text.text = "0 : " + (int)InitialTime;
                }
                InitialTime -= Time.deltaTime;
            } 
            else
            {
                UpTime();
                if (!AllKeyGathered)
                {
                    int count = Random.Range(0, RoomName.Count);
                    StartCoroutine(LockMoment(count));
                } else
                {
                    Lose.SetActive(true);
                    loseSound.Play();
                }
            }
        } 
    }
    public static void UpTime()
    {
        if (!AllKeyGathered)
        {
            InitialTime = 30.0f;
        }
        else
        {
            InitialTime = 60.0f;
        }
    }
    public static void DownTime()
    {
        InitialTime -= downTime;
        if(InitialTime <  0.0f)
        {
            InitialTime = 0.0f;
        }
    }
    IEnumerator GameStart()
    {
        float time = 5.0f;
        go.GetComponent<LocalMove>().enabled = false;
        go.GetComponent<SinglePlayer>().CookieButton.enabled = false;
        go.GetComponent<SinglePlayer>().DashButton.enabled = false;
        SpawnText.enabled = true;
        text.enabled = false;
        KeyText.enabled = false;
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            SpawnText.text = (int)(time + 1) + "초 뒤에 게임이 시작됩니다!";
            yield return null;
        }
        SpawnText.enabled = false;
        text.enabled = true;
        KeyText.enabled = true;
        startsound.Play();
        go.GetComponent<SinglePlayer>().CookieButton.enabled = true;
        go.GetComponent<SinglePlayer>().DashButton.enabled = true;
        go.GetComponent<LocalMove>().enabled = true;
        start.SetActive(true);
        CountDownStart = true;
    }
    IEnumerator LockMoment(int count) {
        float time = 5.0f;
        SpawnText.enabled = true;
        StartCoroutine(LockingRoom[count].RoomLocking());
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            SpawnText.text = RoomName[count] + "이(가) 닫힐 예정입니다";
            yield return null;
        }
        RoomName.RemoveAt(count);
        LockingRoom.RemoveAt(count);
        SpawnText.enabled = false;
        
    }

    public void LeftScene()
    {
        LoadingPanel.SetActive(true);
        PhotonNetwork.LoadLevel("LobbyScene");
        Time.timeScale = 1;
    }
    public static void SetFalse()
    {
        CountDownStart = false;
    }
}
