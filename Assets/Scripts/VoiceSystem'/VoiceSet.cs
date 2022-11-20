using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class VoiceSet : MonoBehaviourPunCallbacks
{
    public AudioSource voiceAS;
    public int micset;

    public void Start()
    {
        SceneLoaded();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        object obj;
        if (SceneManager.GetActiveScene().name == "Main")
        {
            foreach (var player in GameObject.FindGameObjectsWithTag("WaitPlayer"))
            {
                if (targetPlayer == player.GetComponent<PhotonView>().Owner)
                {
                    if (changedProps.TryGetValue("mic", out obj))
                    {
                        int vol = (int)obj;
                        player.gameObject.GetComponent<AudioSource>().volume = (float)vol;
                    }
                }
            }
        }
    }

    public void SceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "Dungeon1")
        {
            Debug.Log("SceneLoaded");
            object obj;
            //OnPlay
            foreach (var player in GameObject.FindGameObjectsWithTag("Dokkebi"))
            {
                if (player.GetComponent<PhotonView>().Owner.CustomProperties.TryGetValue("mic", out obj))
                {
                    int vol = (int)obj;
                    player.gameObject.GetComponent<AudioSource>().volume = (float)vol;
                }
            }
            foreach (var player in GameObject.FindGameObjectsWithTag("Teacher"))
            {
                if (player.GetComponent<PhotonView>().Owner.CustomProperties.TryGetValue("mic", out obj))
                {
                    int vol = (int)obj;
                    player.gameObject.GetComponent<AudioSource>().volume = (float)vol;
                }
            }
        }
    }






}
