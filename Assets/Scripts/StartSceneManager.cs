using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartSceneManager : MonoBehaviour
{
    public void MoveScene()
    {
        SceneManager.LoadSceneAsync("LobbyScene");
    }
}
