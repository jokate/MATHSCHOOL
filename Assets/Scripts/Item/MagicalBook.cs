using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MagicalBook : MonoBehaviourPunCallbacks
{
    GameObject TeacherDetection;
    public GameObject WarningPref;
    public Canvas canvas;
    public Camera cam;
    RectTransform rect, canvasRect;
    bool isTeacher = false, isBorder;
    public AudioSource AS;

    private void OnEnable()
    {
        foreach (var teacher in GameObject.FindGameObjectsWithTag("Teacher"))
        {
            if (teacher.GetComponent<PhotonView>().IsMine)
            {
                isTeacher = true;
            }
        }
        isBorder = false;

        if (!isTeacher)
            SettingCam();
        rect = WarningPref.GetComponent<RectTransform>();
        canvasRect = canvas.GetComponent<RectTransform>();
        StartCoroutine(LiveStart());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Teacher"))
        {
            TeacherDetection = collision.gameObject;
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0f);
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(Detected());
        }
    }
    IEnumerator Detected()
    {
        if (!isTeacher)
        {
            WarningPref.SetActive(true);
            AS.Play();
        }
        
        float time = 10.0f;
        while(time > 0.0f)
        {
            this.gameObject.transform.position = TeacherDetection.transform.position;
            time -= Time.deltaTime;
            FindDir();
            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator LiveStart()
    {
        float time = 30.0f;
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
    //학생 쪽만 판단하게 하고 싶음.. 



    public void FindDir()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, TeacherDetection.transform.position);
        rect.anchoredPosition = screenPoint - canvasRect.sizeDelta * 0.5f;
        if (isBorder)
        {
            BorderLine(rect.anchoredPosition);
        }
    }

    public void SettingCam()
    {
        foreach (var kkebi in GameObject.FindGameObjectsWithTag("Dokkebi"))
        {
            PhotonView pb;
            if (kkebi.TryGetComponent<PhotonView>(out pb))
            {
                if (pb.IsMine)
                {
                    cam = kkebi.GetComponentInChildren<Camera>();
                }
            } else
            {
                cam = kkebi.GetComponentInChildren<Camera>();
            }
        }

    }
    private void OnBecameInvisible()
    {
        isBorder = true;
 
    }
    private void OnBecameVisible()
    {
        isBorder = false;
        rect.localEulerAngles = new Vector3(0, 0, 0);
    }
    public void BorderLine(Vector3 screenPos)
    {

        float reciprocal;
        float rotation;
        Vector2 distance = new Vector3(screenPos.x, screenPos.y);
        // X axis
        if (Mathf.Abs((distance.x)) > Mathf.Abs(distance.y))
        {
            reciprocal = -Mathf.Abs((canvasRect.sizeDelta.x * 0.5f) / distance.x);
            rotation = (distance.x > 0) ? 90 : -90;
        }
        // Y axis
        else
        {
            reciprocal = -Mathf.Abs((canvasRect.sizeDelta.y * 0.5f)/ distance.y);
            rotation = (distance.y > 0) ? 180 : 0;
        }

        rect.anchoredPosition = new Vector3(distance.x * -reciprocal,distance.y * -reciprocal, 1);
        rect.localEulerAngles = new Vector3(0, 0, rotation);


    }

}
