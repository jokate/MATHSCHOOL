using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToildoor : LocalDoor
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
    }

    private void Update()
    {

    }

    public override void OpenOrClose()
    {
        if (state == DoorState.Opended)
        {
            state = DoorState.Locked;
            ColObject = CollisionDetected;
            LocalPlayer lc;
            if(ColObject.TryGetComponent<LocalPlayer>(out lc))
            {
                ColObject.GetComponent<LocalPlayer>().EnterToi(HidePoint.transform.position);
            } else
            {
                ColObject.GetComponent<SinglePlayer>().EnterToi(HidePoint.transform.position);
            }
        }
        else
        {
            state = DoorState.Opended;
            LocalPlayer lc;
            if (ColObject.TryGetComponent<LocalPlayer>(out lc))
            {
                ColObject.GetComponent<LocalPlayer>().ExitToi(ExitPoint.transform.position);
            }
            else
            {
                ColObject.GetComponent<SinglePlayer>().ExitToi(ExitPoint.transform.position);
            }
            ColObject.GetComponent<Plyables>().SettingButton.image.color = Color.white;
            ColObject.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
            ColObject = null;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != DoorState.Locked || ColObject == collision.gameObject)
        {
            if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
            {
                CollisionDetected = collision.gameObject;
                collision.GetComponent<Plyables>().SettingButton.image.color = Color.red;
                collision.GetComponent<Plyables>().SettingButton.onClick.AddListener(OpenOrClose);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state != DoorState.Locked || ColObject == collision.gameObject)
        {
            if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
            {
                CollisionDetected = null;
                collision.GetComponent<Plyables>().SettingButton.image.color = Color.white;
                collision.GetComponent<Plyables>().SettingButton.onClick.RemoveAllListeners();
            }
        }
    }


}
