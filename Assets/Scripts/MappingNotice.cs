using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MappingNotice : MonoBehaviour
{
    public TextMeshProUGUI MapText;
    public string nameString;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi") || collision.CompareTag("Teacher"))
            MapText.text = nameString;
    }
}
