using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSave : Activator {
    // Start is called before the first frame update

    // Start is called before the first frame update
    void Start()
    {

    }
    public CircleCollider2D CD;

    protected override void Awake()
    {

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
        
    }
    public override void Success()
    {
        gameObject.SetActive(false);
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


}
