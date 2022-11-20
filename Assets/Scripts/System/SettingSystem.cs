using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingSystem : MonoBehaviour
{
    public int PlayerNumber;
    public GameObject Dokkebi;
    public GameObject Teacher;
    int TeacherNumber;
    public void Awake()
    {
        PlayerNumber = 10;
        TeacherNumber = Random.Range(0, 10);
    }

    public void Start()
    {
        for (int i = 0; i < PlayerNumber; i++)
        {
            if (i != TeacherNumber)
            {
                GameObject obj = Instantiate(Dokkebi);
                obj.name = "Dokkebi_" + i.ToString();
    
                Vector2 pos = new Vector2(i, 0);
                obj.transform.position = pos;
                obj.tag = "Dokkebi";
            } 
            else
            {
                GameObject obj = Instantiate(Teacher);
                obj.name = "Teacher";
                Vector2 pos = new Vector2(i, 0);
                obj.transform.position = pos;
                obj.tag = "Teacher";
            }
        }
    }
}
