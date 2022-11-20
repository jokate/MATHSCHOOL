using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeacherActivator : Activator {
    // Start is called before the first frame update

    private CircleCollider2D CD;
    public GameObject Edge;
    private void OnEnable()
    {
        state = BOXState.Normal;
        Debug.Log("Spawned");
        CD = GetComponent<CircleCollider2D>();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void Activated()
    {
        
        base.Activated();
        CD.enabled = false;
        CD.enabled = true;

    }

    public override void Failed()
    {
        base.Failed();
    }

    public override void Success()
    {
        base.Success();
    }
    protected override IEnumerator FailedTimeTicks()
    {
        return base.FailedTimeTicks();
    }
    protected override IEnumerator SuccessedTimeTicks()
    {
        return base.SuccessedTimeTicks();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Teacher"))
        {
            if(collision.GetComponent<PhotonView>().IsMine)
            {
                Edge.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Teacher"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                Edge.SetActive(false);
            }
        }
        
    }
}
