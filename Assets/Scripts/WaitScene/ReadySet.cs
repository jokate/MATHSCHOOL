using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySet : PlayerSetup
{
    
    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<PlayerReadySet>().enabled = true;
            transform.GetComponent<MoveAble>().enabled = true;
            PlayerUI.SetActive(true);
            PlayerCamera.SetActive(true);
            CameraFactor.SetActive(true);

        }
        else
        {
            transform.GetComponent<MoveAble>().enabled = false;
            transform.GetComponent<PlayerReadySet>().enabled = false;
            PlayerUI.SetActive(false);
            PlayerCamera.SetActive(false);
            CameraFactor.SetActive(false);
        }
        Name();
    }

}
