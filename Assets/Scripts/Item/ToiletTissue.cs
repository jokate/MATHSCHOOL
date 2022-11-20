using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody2D))]
public class ToiletTissue : MonoBehaviour
{
    Rigidbody2D rb2d;
    private Vector2 movement;
    float Speed = 15.0f;
    // Start is called before the first frame update
    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(LifeTime());
    }

    public void Initialize(Vector3 movement)
    {
        this.movement = movement;

        this.transform.Rotate(new Vector3(0, 0, GetAngle(new Vector2(0, 1), movement)));

    }

    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }


    // Update is called once per frame
    void Update()
    {
        rb2d.velocity = movement * Speed;
    }
    IEnumerator LifeTime()
    {
        float LTime = 3.0f;
        while (LTime > 0.0f)
        {
            LTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Teacher"))
        {
            collision.GetComponent<Teacher>().isStunned = true;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Dokkebi"))
        {
            Destroy(this.gameObject);
        }
    }
}
