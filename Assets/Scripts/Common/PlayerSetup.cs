using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Rendering.Universal;
using TMPro;
public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject PlayerUI;
    public GameObject PlayerCamera;
    public GameObject CameraFactor;
    public TextMeshProUGUI Nametext;
    public Light2D lightComponent;
    void Awake()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<MathPidAPIUse>().enabled = true;
            PlayerUI.SetActive(true);
            PlayerCamera.SetActive(true);
            CameraFactor.SetActive(true);
            if(gameObject.tag == "Dokkebi") {
                transform.GetComponent<ItemContainer>().enabled = true;
            } else if(gameObject.tag == "Teacher") {
                transform.GetComponent<AbilityManager>().enabled = true;
            }
            lightComponent.enabled = true;

        }
        else
        {
            transform.GetComponent<MathPidAPIUse>().enabled = false;
            PlayerUI.SetActive(false);
            PlayerCamera.SetActive(false);
            CameraFactor.SetActive(false);
            if(gameObject.tag == "Dokkebi") {
                transform.GetComponent<ItemContainer>().enabled = false;
            } else if(gameObject.tag == "Teacher") {
                transform.GetComponent<AbilityManager>().enabled = false;
            }
            lightComponent.enabled = false;
        }
        Name();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Name()
    {
        if (Nametext != null)
        {
            Nametext.text = photonView.Owner.NickName;
        }
        if(gameObject.CompareTag("Teacher"))
        {
            Nametext.color = Color.red;
        }
    }
}
