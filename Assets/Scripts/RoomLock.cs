using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomLock : MonoBehaviour
{
    public TextMeshProUGUI WarningText;
    public BoxCollider2D Bx;
    public List<GameObject> Doors;
    public GameObject Player, Losing;
    float time = 5.0f;
    
    public bool isLock;
    private void Start()
    {
        time = 5.0f;
        isLock = false;
    
    }
    public IEnumerator RoomLocking()
    {

        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if (Player != null) {
            Losing.SetActive(true);
            StopAllCoroutines();
        }
        Bx.isTrigger = false;
        foreach(var door in Doors)
        {
            door.GetComponent<LocalDoor>().isOpen = false;
            door.GetComponent<CircleCollider2D>().enabled = false;
        }
        isLock = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dokkebi"))
        {
            Player = collision.gameObject;
            if (isLock)
            {
                WarningText.enabled = true;

            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dokkebi"))
        {
            if (isLock)
            {
                WarningText.text = (int)time + "ÃÊ µÚ¿¡ ¹æÀÌ ´ÝÈü´Ï´Ù!";
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dokkebi"))
        {
            Player = null;
            if (isLock)
            {
                WarningText.enabled = false;

            }
        }
    }
}
