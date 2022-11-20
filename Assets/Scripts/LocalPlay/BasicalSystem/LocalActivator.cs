using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalActivator : Activator
{
    public List<Item> ItemSet;
    public GameObject Edge;

    public CircleCollider2D CD;

    protected override void Awake()
    {
        SpawnTime = 10.0f;
    }

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

    protected override IEnumerator SuccessedTimeTicks()
    {
        state = BOXState.Wait;
        yield return null;
    }

    protected override IEnumerator FailedTimeTicks()
    {
        state = BOXState.Wait;
        yield return null;
    }

    public Item returnItem()
    {
        if(ItemSet.Count == 0)
        {
            return null;
        }
        this.enabled = false;
        return ItemSet[Random.Range(0, ItemSet.Count)];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi") && state == BOXState.Normal)
        {
            Edge.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dokkebi"))
        {
            Edge.SetActive(false);
        }
    }

}
