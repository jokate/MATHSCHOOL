using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(Rigidbody2D))]
public class RocketPunch : MonoBehaviour
{
    Rigidbody2D rb2d;
    public GameObject dokkebi, Owner;

    bool isFront;
    float Speed = 24.0f;
    public Vector2 movement;

    private void OnEnable()
    {
        isFront = true;
        rb2d = GetComponent<Rigidbody2D>();
    }


    float GetAngle(Vector2 start, Vector2 end)
    {
       
        float dot = Vector3.Dot(start, end);
        float mag = Vector3.Magnitude(start) * Vector3.Magnitude(end);
        float angle = (dot / mag) * Mathf.Rad2Deg;
        return angle;
    }

    public void Initialze(GameObject owner)
    {
        Owner = owner;
        Debug.Log("RocketPunch Initialized");
        RayComponent ray;
        if (owner.TryGetComponent<RayComponent>(out ray))
        {
            movement = owner.GetComponent<RayComponent>().ray.direction;
            Debug.Log(-GetAngle(Vector2.right, movement));
            this.transform.Rotate(new Vector3(0, 0, 90 - GetAngle(Vector2.right, movement)));
        }
        else
        {
            movement = (GameObject.FindGameObjectWithTag("Dokkebi").transform.position - owner.transform.position).normalized;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dokkebi"))
        {
            if (!isFront && collision.gameObject == Owner)
            {
                PhotonView pb;
                if (TryGetComponent<PhotonView>(out pb))
                {
                    dokkebi.GetComponent<PhotonView>().RPC("ReturnNormal", RpcTarget.AllBuffered);
                } else
                {
                    dokkebi.GetComponent<SinglePlayer>().ReturnNormal();
                }
                Destroy(this.gameObject);
            }
            if (collision.gameObject != Owner && dokkebi == null)
            {
                dokkebi = collision.gameObject;
                isFront = false;
            }

        }   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    public void Detected()
    {
        PhotonView pb;
        movement = (dokkebi.transform.position - Owner.transform.position).normalized;
        if (dokkebi.TryGetComponent<PhotonView>(out pb))
        {
            dokkebi.GetComponent<PhotonView>().RPC("SetTransform", RpcTarget.AllBuffered, (Vector2)gameObject.transform.position);
        }
        else
        {
            dokkebi.GetComponent<SinglePlayer>().SetTransform((Vector2)gameObject.transform.position);
        }
    }
    private void Update()
    {

        if (isFront)
        {
            rb2d.velocity = movement * Speed;
        } else
        {
            Detected();
            this.transform.Rotate(new Vector3(0, 0, GetAngle(Vector2.right, -movement.normalized)));
            rb2d.velocity = -movement * Speed;
        }
    }

}
