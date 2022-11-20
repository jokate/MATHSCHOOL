using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Vent : MonoBehaviour
{
    #region Variable Set
    public GameObject VentTeleportPosition;
    public GameObject ventEdge;
    GameObject Detected;
    #endregion

    #region Collision Detected

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teacher") || collision.CompareTag("Dokkebi")) {
            PhotonView pb;
            if (collision.TryGetComponent<PhotonView>(out pb))
            {
                if (pb.IsMine)
                {
                    Detected = collision.gameObject;
                    collision.GetComponent<Plyables>().SettingButton.image.color = Color.red;
                    ventEdge.SetActive(true);
                    collision.GetComponent<Plyables>().SettingButton.onClick.AddListener(ButtonSet);
                }
            } else
            {
                Detected = collision.gameObject;
                collision.GetComponent<Plyables>().SettingButton.image.color = Color.red;
                ventEdge.SetActive(true);
                collision.GetComponent<Plyables>().SettingButton.onClick.AddListener(ButtonSet);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teacher") || collision.CompareTag("Dokkebi"))
        {
            PhotonView pb;
            if (collision.TryGetComponent<PhotonView>(out pb))
            {
                if (pb.IsMine)
                {
                    Detected = null;
                    collision.GetComponent<Plyables>().SettingButton.image.color = Color.white;
                    ventEdge.SetActive(false);
                    collision.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
                }
            } else
            {
                Detected = null;
                collision.GetComponent<Plyables>().SettingButton.image.color = Color.white; 
                ventEdge.SetActive(false);
                collision.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
            }
        }
    }
    #endregion

    public void ButtonSet()
    {
        PhotonView pb;

        if (Detected.TryGetComponent<PhotonView>(out pb)) {
            Detected.GetComponent<PhotonView>().RPC("ChangePos", RpcTarget.AllBuffered, VentTeleportPosition.transform.position);
        } else
        {
            LocalPlayer lc;
            if (Detected.TryGetComponent<LocalPlayer>(out lc))
            {
                Detected.GetComponent<LocalPlayer>().ChangePos(VentTeleportPosition.transform.position);
            } else
            {
                Detected.GetComponent<SinglePlayer>().ChangePos(VentTeleportPosition.transform.position);
            }
        }
    }


}
