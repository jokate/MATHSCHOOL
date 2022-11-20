using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MoveAble : MonoBehaviourPunCallbacks
{
    public VariableJoystick joy;
    public float OriginalSpeed = 5.0f;
    public float speed;
    public Vector2 movement = new Vector2();
    protected Rigidbody2D rb2D;
    public SpriteRenderer SR;
    public bool stop;
    
    public AudioSource walkSound;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        stop = false;
        speed = OriginalSpeed;
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
#else
        movement.x = joy.Horizontal;
        movement.y = joy.Vertical;
#endif
        if(stop)
        {
            movement = Vector2.zero;
            joy.SetInput(Vector2.zero);
        }


        if (photonView.IsMine)
        {
            if (movement.x != 0)
            {
                photonView.RPC(nameof(Flip), RpcTarget.AllBuffered, movement.x);
            }
            movement.Normalize();
            Move();

            rb2D.velocity = movement * speed;
        }
    }
    private void Move()
    {

        if (Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y, 0))
        {
            anim.SetBool("isMove", false);
            walkSound.Stop();

        } else
        {
            anim.SetBool("isMove", true);
            if (!walkSound.isPlaying)
                walkSound.Play();
            anim.SetFloat("xdir", movement.x);
            anim.SetFloat("ydir", movement.y);
        }


    }
    


    [PunRPC]
    void Flip(float axis)
    {
        if(axis < 0.0f)
        {
            SR.flipX = true;
        }
        else
        {
            SR.flipX = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StickyZone")
        {
            if (gameObject.CompareTag("Teacher"))
            {
                speed = 3.0f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "StickyZone")
        {
            if (gameObject.CompareTag("Teacher"))
            {
                speed = 3.0f;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "StickyZone")
        {
            if (gameObject.CompareTag("Teacher"))
            {
                speed = OriginalSpeed;
            }

        }
    }
}
