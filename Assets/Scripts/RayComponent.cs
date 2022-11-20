using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class RayComponent : MonoBehaviourPunCallbacks, IPunObservable
{
    public VariableJoystick joy;
    public Ray2D ray;
    public Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray2D();
        ray.origin = gameObject.transform.position;
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
        Move();
    }
    public void RaySet()
    {
        if (ray.direction.x > 0.5)  {
          ray.direction = new Vector2(1, 0);
        }
        else if (ray.direction.x < -0.5)
        {
            ray.direction = new Vector2(-1, 0);
        }
        else
        {
            if (ray.direction.y > 0)
            {
                ray.direction = new Vector2(0, 1);
            }
            else
            {
                ray.direction = new Vector2(0, -1);
            }
        }
    }
    public void moveRay()
    {
        ray.direction = movement;
    }

    
    public Ray2D GetRay()
    {
        return ray;
    }
    private void Move()
    {
        movement.Normalize();
        if (Mathf.Approximately(movement.x, 0) && Mathf.Approximately(movement.y, 0))
        {
            RaySet();
        }
        else
        {
            moveRay();
        }


    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext((Vector3)ray.direction);
        } else
        {
            ray.direction = (Vector3)stream.ReceiveNext();
        }
    }
}
