using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;

public class SettingManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI MicText, SoundText;
    public AudioSource audios;
    public GameObject go;
    ExitGames.Client.Photon.Hashtable hash;
    private void Awake()
    {
        BasicMicSet();
        BasicSoundSet();
    }

    private void BasicMicSet()
    {
        if (PlayerPrefs.GetInt("Mic") == 0)
        {
            MicText.text = "On";
            hash = new ExitGames.Client.Photon.Hashtable() { { "mic", 1 } };
            
        }
        else
        {
            MicText.text = "Off";
            hash = new ExitGames.Client.Photon.Hashtable() { { "mic", 0 } };
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    private void BasicSoundSet()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            SoundText.text = "On";
            if (audios != null)
            {
                audios.volume = 0.25f;
            }
        }
        else
        {
            SoundText.text = "Off";
            if (audios != null)
            {
                audios.volume = 0.0f;
            }
        }
    }

    public void GameOff()
    {
        Application.Quit();
    }
    
    public void ResumeOnMultiPlay()
    {
        go.SetActive(false);
    }
    public void ResumeOnSinglePlay()
    {
        Time.timeScale = 1;
        go.SetActive(false);
    }
    public void SinglePlayLeft()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("LobbyScene");
    }
    public void MultiPlayLeft()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
        base.OnLeftRoom();
    }
    public void SoundSet()
    {
        if(PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
            SoundText.text = "Off";
            if(audios != null)
                audios.volume = 0.0f;
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
            SoundText.text = "On";
            if(audios != null)
                audios.volume = 0.25f;
        }
    }
    public void MicSet()
    {
        if (PlayerPrefs.GetInt("Mic") == 0)
        {
            PlayerPrefs.SetInt("Mic", 1);
            hash = new ExitGames.Client.Photon.Hashtable() { {"mic", 0 } };
            MicText.text = "Off";
        }
        else
        {
            PlayerPrefs.SetInt("Mic", 0);
            hash = new ExitGames.Client.Photon.Hashtable() { { "mic", 1} };
            MicText.text = "On";
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void SettingPrefActiveOnMUlti()
    {
        go.SetActive(true);
    }
    
    public void SettingPrefActiveOnSingle()
    {
        Time.timeScale = 0f;
        go.SetActive(true);
    }

}
