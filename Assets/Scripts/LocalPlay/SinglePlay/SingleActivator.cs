using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleActivator : Activator
{
    public CircleCollider2D CD;
    public AudioSource UpTimeSound, DownTimeSound, KeySound;
    protected override void Awake()
    {
        SpawnTime = 60.0f;
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
        ReturnSet();
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
    public void ReturnSet() {
        float i = Random.Range(0, 1f);
        //²Î
        if(0 < i && i < 0.3f)
        {
            SinglePlayManager.DownTime();
            DownTimeSound.Play();
        }
        else if(0.3f <= i && i < 0.8f)
        {
            SinglePlayManager.UpTime();
            UpTimeSound.Play();
        } else if(0.8f <= i && i < 1.0f)
        {
            if (SinglePlayManager.Key < SinglePlayManager.MaxKey)
            {
                SinglePlayManager.Key++;
                if (SinglePlayManager.Key == SinglePlayManager.MaxKey && !SinglePlayManager.AllKeyGathered)
                {
                    SinglePlayManager.InitialTime = 60.0f;
                    UpTimeSound.Play();

                } else {
                    KeySound.Play();
                }
            } 
        }
    }
}
