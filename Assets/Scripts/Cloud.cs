using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Cloud : MonoBehaviour
{
    GameObject teacher;
    public GameObject cloud;
    public AudioSource cloudAs;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(LifeTime());
    }
    IEnumerator LifeTime()
    {
        float time = 30.0f;
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator CloudFormation()
    {
        float time = 15.0f;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        teacher.GetComponentInChildren<CatchFactor>().CatchedButton.gameObject.SetActive(false);
        if(teacher.GetComponent<PhotonView>().IsMine)
            cloud.SetActive(true);
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        if (teacher.GetComponent<PhotonView>().IsMine)
            cloud.SetActive(false);
        teacher.GetComponentInChildren<CatchFactor>().CatchedButton.gameObject.SetActive(true);
        Destroy(gameObject);

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Teacher"))
        {
            cloudAs.Play();
            StopAllCoroutines();
            teacher = collision.gameObject;
            StartCoroutine(CloudFormation());
        }
    }
    
}
