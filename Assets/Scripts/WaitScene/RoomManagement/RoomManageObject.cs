using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RoomManageObject : MonoBehaviour
{
    public GameObject RoomManagementUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "WaitPlayer")
        {
            if (collision.GetComponent<PhotonView>().Owner.IsMasterClient)
            {
                collision.GetComponent<PlayerReadySet>().GameSettingButton.gameObject.SetActive(true);
                collision.GetComponent<PlayerReadySet>().GameSettingButton.onClick.AddListener(Activate);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WaitPlayer")
        {
            collision.GetComponent<PlayerReadySet>().GameSettingButton.gameObject.SetActive(false);
            collision.GetComponent<PlayerReadySet>().GameSettingButton.onClick.RemoveAllListeners();
        }
    }
    private void Activate()
    {

        RoomManagementUI.SetActive(true);

    }
}
