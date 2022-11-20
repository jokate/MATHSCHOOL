using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Survivor : MonoBehaviour
{
    public GameObject Winning;
    public TextMeshProUGUI SpawnText;
    public AudioSource winSound;
    public GameObject arrkkebi;
   
    bool isBorder;

    private void OnEnable()
    {
        Arrkkebi.Target = this.gameObject;
        arrkkebi.SetActive(true);
    }

    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi"))
        {
            StartCoroutine(StartText());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi"))
        {
            StopAllCoroutines();
        }
    }


    IEnumerator StartText()
    {
        float time = 5.0f;
        SpawnText.enabled = true;
        while(time > 0.0f)
        {
            SpawnText.text = "탈출까지 " + (int)(time + 1) + "초"; 
            time -= Time.deltaTime;
            yield return null;
        }
        SpawnText.enabled = false;
        winSound.Play();
        Winning.SetActive(true);
  

        SinglePlayManager.SetFalse();
    }

}
