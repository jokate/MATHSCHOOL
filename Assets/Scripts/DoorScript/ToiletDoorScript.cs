using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ToiletDoorScript : DoorScript
{
    GameObject CollisionDetected;
    GameObject ColObject;
    public GameObject HidePoint, ExitPoint;
    bool CanUse = true;
    public enum DoorState
    {
        Opended = 0,
        Locked,
    };

    public DoorState state;

    private new void Awake()
    {
        state = DoorState.Opended;
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { gameObject.name, (int)state } };
    }

    private void Update()
    {
        
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        object isOpened;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(gameObject.name, out isOpened))
        {
            state = (DoorState)isOpened;
        }

        if (propertiesThatChanged.ContainsKey(RunningGameManager.CATCHED_PEOPLE))
        {
            int number;
            object obj;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RunningGameManager.CATCHED_PEOPLE, out obj))
            {
                number = (int)obj;
                Debug.Log(number);
                if (number == (PhotonNetwork.CurrentRoom.PlayerCount - InstantiateManager.TeacherTotalNumber - 1))
                {

                    if (ColObject != null)
                    {
                        OpenOrClose();
                    }
                    foreach (var circle in GetComponents<CircleCollider2D>())
                    {
                        circle.enabled = false;
                    }
                }
                else if (number == 0)
                {
                    foreach (var circle in GetComponents<CircleCollider2D>())
                    {
                        circle.enabled = true;
                    }
                }
            }
        }

    }
    public override void OpenOrClose()
    {
        if(state == DoorState.Opended)
        {
            ExitGames.Client.Photon.Hashtable props =new ExitGames.Client.Photon.Hashtable() { { gameObject.name, (int)DoorState.Locked } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            ColObject = CollisionDetected;
            ColObject.GetComponent<PhotonView>().RPC("EnterToi", RpcTarget.AllBuffered, HidePoint.transform.position);
        }
        else
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { gameObject.name, (int)DoorState.Opended} };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            ColObject.GetComponent<PhotonView>().RPC("ExitToi", RpcTarget.AllBuffered, ExitPoint.transform.position);
            ColObject.GetComponent<Plyables>().SettingButton.image.color = Color.white;
            ColObject.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
            ColObject = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(state != DoorState.Locked || ColObject == collision.gameObject)
        {
            if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
            {
                if (collision.GetComponent<PhotonView>().IsMine)
                {
                    CollisionDetected = collision.gameObject;
                    collision.GetComponent<Plyables>().SettingButton.image.color = Color.red;
                    collision.GetComponent<Plyables>().SettingButton.onClick.AddListener(OpenOrClose);
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state != DoorState.Locked || ColObject == collision.gameObject)
        {
            if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
            {
                if (collision.GetComponent<PhotonView>().IsMine)
                {
                    CollisionDetected = null;
                    collision.GetComponent<Plyables>().SettingButton.image.color = Color.white;
                    collision.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
                }
            }
        }
    }


}
