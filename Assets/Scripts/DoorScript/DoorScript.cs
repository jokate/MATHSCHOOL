using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class DoorScript : MonoBehaviourPunCallbacks
{
    public GameObject OpenedDoor;
    public GameObject ClosedDoor;
    public GameObject OpenDoorParti, CloseDoorParti;
    public AudioSource doorOpen, doorClose;
    bool isOpen;

    public virtual void Awake()
    {
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { gameObject.name, false} };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
    public void Start()
    {
        object isOpened;
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(gameObject.name, out isOpened))
        {
            isOpen = (bool)isOpened;
        }
    }

    public void Update()
    {
        if (isOpen)
        {
            OpenedDoor.SetActive(true);
            ClosedDoor.SetActive(false);
        }
        else
        {
            ClosedDoor.SetActive(true);
            OpenedDoor.SetActive(false);
        }
    }


    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        object isOpened;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(gameObject.name, out isOpened))
        {
         
            isOpen = (bool)isOpened;
        }
    }
    public virtual void OpenOrClose()
    {
        if (isOpen)
        {
            doorClose.Play();
        }
        else
        {
            doorOpen.Play();
        }
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { {gameObject.name, !isOpen } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<Plyables>().SettingButton.image.color = Color.red; 
                if(isOpen)
                {
                    OpenDoorParti.SetActive(true);
                } else
                {
                    CloseDoorParti.SetActive(true);
                }
                collision.gameObject.GetComponent<Plyables>().SettingButton.onClick.AddListener(OpenOrClose);
                
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                if (isOpen)
                {
                    OpenDoorParti.SetActive(true);
                }
                else
                {
                    CloseDoorParti.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<Plyables>().SettingButton.image.color = Color.white;
                if (isOpen)
                {
                    OpenDoorParti.SetActive(false);
                }
                else
                {
                    CloseDoorParti.SetActive(false);
                }
                collision.gameObject.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
            }
        }
    }

    public IEnumerator doorLock()
    {
        float time = 15.0f;
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable { { gameObject.name, false } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        GetComponent<CircleCollider2D>().enabled = false;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        GetComponent<CircleCollider2D>().enabled = true;


    }
}

