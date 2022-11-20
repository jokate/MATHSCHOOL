using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalk : MonoBehaviour
{
    float Speed = 15.0f;
    private Vector2 movement;
    Rigidbody2D rb2d;

    public void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(LifeTime());
    }
    public void Initialize(Vector3 movement)
    {
        this.movement = movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Teacher")) {

            if (collision.CompareTag("Dokkebi"))
            {
                Dokkebi dk;
                if (collision.gameObject.TryGetComponent<Dokkebi>(out dk))
                {
                    collision.GetComponent<Dokkebi>().isHit = true;
                }
                else
                {
                    collision.GetComponent<SinglePlayer>().isHit = true;
                }
                Destroy(gameObject);
            }


        }
      
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        rb2d.velocity = movement * Speed;
        gameObject.transform.Rotate(new Vector3(0, 0, 1) * 480f * Time.deltaTime);
    }
    IEnumerator LifeTime()
    {
        float LTime = 3.0f;
        while(LTime > 0.0f)
        {
            LTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

}
