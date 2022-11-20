using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MappingNoticeMult : MonoBehaviour
{
    public TextMeshProUGUI MapText;
    public string nameString;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
        {
            if (collision.gameObject.GetComponent<PhotonView>().IsMine)
            {
                MapText.text = nameString;
            }
        }
    }
}
