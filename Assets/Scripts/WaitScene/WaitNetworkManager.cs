using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class WaitNetworkManager : MonoBehaviourPunCallbacks
{

    Dictionary<int, Player> playerListGameObjects;
    public GameObject StartButton;
    public GameObject ReadyButton;
    // Start is called before the first frame update
    void Start()
    {
        playerListGameObjects = new Dictionary<int, Player>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public bool CheckPlayersReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                continue;
            }
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(RunningGameManager.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
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

    #region Photon CallBacks
    public override void OnJoinedRoom()
    {
        if(playerListGameObjects == null)
        {
            playerListGameObjects = new Dictionary<int, Player>();
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {

            object isPlayerReady;

            if (player.CustomProperties.TryGetValue(RunningGameManager.PLAYER_READY, out isPlayerReady))
            {
                
            }


        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {


            object isPlayerReady;

            if (player.CustomProperties.TryGetValue(RunningGameManager.PLAYER_READY, out isPlayerReady))
            {
                
            }


    

        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;

            if (player.CustomProperties.TryGetValue(RunningGameManager.PLAYER_READY, out isPlayerReady))
            {

            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;

            if (player.CustomProperties.TryGetValue(RunningGameManager.PLAYER_READY, out isPlayerReady))
            {

            }
        }
    }
    #endregion
}
