using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PlayerReadySet : MonoBehaviourPunCallbacks {
    #region Variable Set
    [Header("UI References")]
    public TextMeshProUGUI NameText;
    public Button PlayerReadyButton;
    public Button GameStartButton;
    public Button GameSettingButton;
    private bool isReady = false;
    #endregion

    #region Photon CallBack

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        GameStartButton.gameObject.SetActive(CheckPlayersReady());
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        GameStartButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        GameStartButton.gameObject.SetActive(CheckPlayersReady());
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("WaitPlayer"))
        {
            if (player.GetComponent<PhotonView>().Owner.ActorNumber == newMasterClient.ActorNumber)
            {
                Debug.Log("Master Client is : " + newMasterClient.NickName);
                player.GetComponent<PhotonView>().RPC("MasterSet", RpcTarget.AllBuffered);
            }
        }
        GameStartButton.gameObject.SetActive(CheckPlayersReady());
    }
    #endregion

    #region Public Methods
    public void SetPlayerReady(bool playerReady)
    {

        if (playerReady == true)
        {
            photonView.RPC("Setting", RpcTarget.AllBuffered, playerReady);

        }
        else
        {
            photonView.RPC("Setting", RpcTarget.AllBuffered, playerReady);
        }
    }

    public void Initialize()
    {
        SetPlayerReady(isReady);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("I'm Master Client");
            photonView.RPC("MasterSet", RpcTarget.AllBuffered);
        }

        PlayerReadyButton.onClick.AddListener(() =>
        {
            isReady = !isReady;
            Debug.Log(isReady);
            SetPlayerReady(isReady);
            ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.PLAYER_READY, isReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
        });
        GameStartButton.onClick.AddListener(() =>
        {
            StartCoroutine(StartGame());
        });

    }

    public IEnumerator StartGame()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            SettingTeacher();
            PhotonNetwork.CurrentRoom.IsVisible = false;
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.IS_START, true } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            yield return null;
        }
    }

    private void SettingTeacher()
    {
        List<int> SettingInt = new List<int>();
        object teacherNumber;
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.TEACHER_NUMBER, out teacherNumber)) {
            int i = 0;
            
            while(i < (int)teacherNumber) {
                Debug.Log((int)teacherNumber);
                int Settingnumber = Random.Range(1, PhotonNetwork.CurrentRoom.PlayerCount + 1);
                if(!SettingInt.Contains(Settingnumber))
                {
                    SettingInt.Add(Settingnumber);
                    i++;
                } else
                {
                    Debug.Log("There is Same value");
                }
            }
        }

        int index = 0;
        foreach (var TNumber in SettingInt) {
            PhotonNetwork.CurrentRoom.CustomProperties[RunningGameManager.TEACHER_FACTOR + index.ToString()] = TNumber;
            var props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.TEACHER_FACTOR + index.ToString(), TNumber } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            index++;

        }
        Debug.Log("Completed");
    }

    public void LeaveButtonSet()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region RPC Call

    public bool CheckPlayersReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                continue;
            }
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(RunningGameManager.PLAYER_READY, out isPlayerReady))
            {
                if ((bool)isPlayerReady == false)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    [PunRPC]
    public void Setting(bool isReady)
    {
        if(isReady)
        {
            NameText.color = Color.green;
        }
        else
        {
            NameText.color = Color.white;
        }
    }
    [PunRPC]
    public void MasterSet()
    {
        NameText.color = Color.yellow;
        PlayerReadyButton.gameObject.SetActive(false);
        GameStartButton.gameObject.SetActive(true);
    }

    #endregion




}
