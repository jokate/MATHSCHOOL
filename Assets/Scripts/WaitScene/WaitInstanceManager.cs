using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class WaitInstanceManager : MonoBehaviourPunCallbacks
{
    [Header("GameObejctInfo")]
    public GameObject WaitingGameObject;
    [Header("Spawn Point")]
    public GameObject SpawnPoint, LoadingPref;
    public WaitInstanceManager instance;
    public Dictionary<int, GameObject> PlayerLists;
    // Start is called before the first frame update


    public bool isStart = false;
    void Start()
    {
        LoadingPref.SetActive(false);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject player = PhotonNetwork.Instantiate(WaitingGameObject.name, SpawnPoint.transform.position, Quaternion.identity);
            player.GetComponent<PlayerReadySet>().Initialize();

        }
    }

    // Start is called before the first frame update
 

    // Update is called once per frame
    void Update()
    {
    }


    public override void OnLeftRoom()
    {
        SceneManager.LoadSceneAsync("LobbyScene");
    }

    public override void OnJoinedRoom()
    {
        
    }




    #region Photon CallBacks
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if(propertiesThatChanged.ContainsKey(RunningGameManager.IS_START))
        {
            isStart = (bool)propertiesThatChanged[RunningGameManager.IS_START];
            if(isStart)
            {
                PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);


                LoadingPref.SetActive(true);
                PhotonNetwork.LoadLevel("Dungeon1");
      
            }
        }
    }

    #endregion
}
