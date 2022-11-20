using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class DokkebiActivator : Activator
{

    public List<Item> ItemSet;
    public GameObject Edge;
    public CircleCollider2D CD;

    public void OnEnable()
    {
        state = BOXState.Normal;
        CD = GetComponent<CircleCollider2D>();
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
        float Count = WaitTime;
        while (Count > 0.0f)
        {
            Count -= Time.deltaTime;
            yield return null;
        }

        state = BOXState.Normal;
        BackToNormal();
        yield return null;
    }

    public Item returnItem()
    {
        int num = Random.Range(0, ItemSet.Count);
        return ItemSet[num];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi") && state == BOXState.Normal)
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                Edge.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Dokkebi"))
        {
            if (collision.GetComponent<PhotonView>().IsMine)
            {
                Edge.SetActive(false);
            }
        }
    }
}
