using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
[RequireComponent(typeof(BoxCollider2D))]
public class Activator : MonoBehaviourPunCallbacks
{
    public GameObject Timecounter;
    public Image Timecount;
    public float WaitTime = 10.0f;
    public float SpawnTime = 10.0f;

    public enum BOXState
    {
        Success,
        Failed,
        Normal,
        Wait,
        Activated,
        Destroyed,
        Spawning,

    };
    protected BOXState state = BOXState.Normal;


    protected virtual void Update()
    {
        if (state == BOXState.Failed)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            state = BOXState.Wait;
            Debug.Log("Failed");
            StartCoroutine(FailedTimeTicks());
        }
        else if (state == BOXState.Destroyed)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            state = BOXState.Wait;
            StartCoroutine(SuccessedTimeTicks());
        }
    }

    protected virtual void Awake()
    {
        object num;
        if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.ITEM_GENTIME, out num))
        {
            int Number = (int)num;
            SpawnTime = Number;
        }

        Debug.Log("스폰 시간 = " + SpawnTime);
    }


    public virtual void Failed()
    {
        state = BOXState.Failed;

    }


    public virtual void BackToNormal()
    {
        state = BOXState.Normal;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    public virtual void Activated()
    {
        state = BOXState.Activated;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    public virtual void Success()
    {
        state = BOXState.Destroyed;
    }

    protected virtual IEnumerator FailedTimeTicks()
    {
        Timecounter.SetActive(true);
        Debug.Log("Failed");
        float Count = WaitTime;
        while (Count > 0.0f)
        {

            Count -= Time.deltaTime;
            Timecount.fillAmount = 1 - (Count / WaitTime);
            yield return null;
        }
        Timecounter.SetActive(false);
        BackToNormal();

        yield return null;
    }


    protected virtual IEnumerator SuccessedTimeTicks()
    {
        Timecounter.SetActive(true);
        float Count = SpawnTime;

        while (Count > 0.0f)
        {
            Count -= Time.deltaTime;
            Timecount.fillAmount = 1 - (Count / SpawnTime);
            yield return null;
        }
        Timecounter.SetActive(false);
        BackToNormal();
        yield return null;
    }
    public BOXState GetState()
    {
        return state;
    }

    protected void Destroyed()
    {
        var spr = gameObject.GetComponent<SpriteRenderer>();
        var poly = gameObject.GetComponent<BoxCollider2D>();
        spr.enabled = false;
        poly.enabled = false;
    }
    protected void Spawaned()
    {
        var spr = gameObject.GetComponent<SpriteRenderer>();
        var poly = gameObject.GetComponent<BoxCollider2D>();
        spr.enabled = true;
        poly.enabled = true;
        BackToNormal();
    }

}
