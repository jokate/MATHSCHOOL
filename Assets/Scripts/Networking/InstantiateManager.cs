using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun;
public class InstantiateManager : MonoBehaviourPunCallbacks
{
    [Header("SpawnPoints")]
    public GameObject [] SpawnPoints;
    [Header("Spawn Prefab")]
    public GameObject CharacterPref;

    [Header("TeacherSpawnPoints")]
    public GameObject TSpawnPoint;

    [Header("Teacher Prefabk")]
    public GameObject TeacherPref;
    [Header("Catched Point")]
    public GameObject CatchedPoint;
    public static bool isteacher = false;
    public static int TeacherTotalNumber;
    public List<int> TeacherNumber;
    public static InstantiateManager instance = null;
    public static GameObject player;
    
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        object Number;
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.TEACHER_NUMBER, out Number))
        {
            TeacherTotalNumber = (int)Number;
            Debug.Log(TeacherTotalNumber);
        }
    }


    public void Start()
    {
           
        for(int i = 0; i < TeacherTotalNumber; i++)
        {
            object Number;
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RunningGameManager.TEACHER_FACTOR + i.ToString(), out Number))
            {
                Debug.Log("Setting TeacherNumber = " + (int)Number);
                TeacherNumber.Add((int)Number);
            }
        }



        if(!isTeacher())
        {
            isteacher = true;
            if (CharacterPref != null)
            {
                int RandomPoint = Random.Range(0, SpawnPoints.Length);
                player = PhotonNetwork.Instantiate(CharacterPref.name, SpawnPoints[RandomPoint].transform.position, Quaternion.identity);
                player.GetComponent<Dokkebi>().SetCatchedPoint(CatchedPoint.transform.position);
            }
            else
            {
                Debug.Log("error");
            }
        } else
        {
            isteacher = false;
            if (TeacherPref != null)
            {
                player = PhotonNetwork.Instantiate(TeacherPref.name, TSpawnPoint.transform.position, Quaternion.identity);

            }
            else
            {
                Debug.Log("error");
            }
        }


    }

       
       

    public bool isTeacher()
    {
        bool returnvalue = false;
        List<int> Keys = PhotonNetwork.CurrentRoom.Players.Keys.ToList().OrderBy(x => x).ToList();

        foreach(var a in Keys)
        {
            Debug.Log("Key is : " + a);
        }
        foreach (var Number in TeacherNumber)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == Keys[Number - 1] )
            {
                Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
                returnvalue = true;
                
            }
            
        }
        return returnvalue;
    }
    private void Update()
    {
    }




}

