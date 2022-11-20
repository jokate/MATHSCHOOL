using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    // �������� ���� �� �ȳ����� ��.
    EventGiver eventGiver;

    private void Start()
    {
        eventGiver = new EventGiver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi"))
        {
            eventGiver.RunEvent();
            Destroy(gameObject);
        }
    }
}
