using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDoor : MonoBehaviour
{
    public GameObject OpenedDoor;
    public GameObject ClosedDoor;
    public bool isOpen;
    public GameObject OpenDoorParti, CloseDoorParti;
    public AudioSource OpenSound, CloseSound;
    public virtual void Awake()
    {

    }
    public void Start()
    {
        isOpen = false;
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

    public virtual void OpenOrClose()
    {
        if(isOpen)
        {
            Debug.Log("DoorClose");
            CloseSound.Play();
        }
        else
        {
            Debug.Log("DoorOpen");
            OpenSound.Play();
        }
        isOpen = !isOpen;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            collision.gameObject.GetComponent<Plyables>().SettingButton.image.color = Color.red;
            if (isOpen)
            {
                OpenDoorParti.SetActive(true);
                
            }
            else
            {
                CloseDoorParti.SetActive(true);
            }
            collision.gameObject.GetComponent<Plyables>().SettingButton.onClick.AddListener(OpenOrClose);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            collision.gameObject.GetComponent<Plyables>().SettingButton.image.color = Color.red;
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            collision.gameObject.GetComponent<Plyables>().SettingButton.image.color = Color.white;
            if (isOpen)
            {
                CloseDoorParti.SetActive(false);
                OpenDoorParti.SetActive(false);
            }
            else
            {
                OpenDoorParti.SetActive(false);
                CloseDoorParti.SetActive(false);
            }
            collision.gameObject.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
        }
    }

    public IEnumerator doorLock()
    {
        float time = 15.0f;
        isOpen = false;
        bool basic = true;
        if(GetComponent<CircleCollider2D>().isActiveAndEnabled == false)
        {
            basic = false;
        }
        GetComponent<CircleCollider2D>().enabled = false;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        GetComponent<CircleCollider2D>().enabled = basic;
    }


}
