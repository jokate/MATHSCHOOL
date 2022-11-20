using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstSinglePlayer : MonoBehaviour
{
    public List<Sprite> sprList;
    public GameObject Panel;
    public Image img;
    public TextMeshProUGUI txt;
    int idx = 0;

    private void Start()
    {
        img.sprite = sprList[idx];
        int Number = PlayerPrefs.GetInt("SingleStart");
        if (Number == 0)
        {
            txt.gameObject.SetActive(false);
            Time.timeScale = 0f;
            Panel.SetActive(true);
        }

    }
    public void Confirm()
    {
        ++idx;
        if(idx < sprList.Count)
        {
            img.sprite = sprList[idx];
        }
        else
        {
            Time.timeScale = 1;
            Panel.SetActive(false);
            PlayerPrefs.SetInt("SingleStart", 1);
            txt.gameObject.SetActive(true);
        }
    }
}
