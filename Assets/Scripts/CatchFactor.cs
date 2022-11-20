using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CatchFactor : MonoBehaviour
{
    [Header("Catch Point")]
    public GameObject CatchedPoint;
    public List<Dokkebi> LDokkebi;
    private CircleCollider2D circleCollider;
    public Button CatchedButton;


    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        LDokkebi = new List<Dokkebi>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetKillRange(float radius)
    {
        circleCollider.radius = radius;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        var player = collision.GetComponent<Dokkebi>();
        if(player != null && !LDokkebi.Contains(player) && player.state == Dokkebi.PlayerState.Idle)
        {
            LDokkebi.Add(player);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        var player = collision.GetComponent<Dokkebi>();
        if(player != null && LDokkebi.Contains(player))
        {
            LDokkebi.Remove(player);
        }
    }

    public Dokkebi FindNearestPlayer()
    {
        float dist = float.MaxValue;
        Dokkebi closeTarget = null;
        foreach(var target in LDokkebi)
        {
            float newDist = Vector3.Distance(transform.position, target.transform.position);
            if(newDist < dist)
            {
                dist = newDist;
                closeTarget = target;
            }
        }
        LDokkebi.Remove(closeTarget);
        return closeTarget;

    }


    public IEnumerator CatchCoolDown()
    {
        CatchedButton.enabled = false;
        float WaitTime = 10.0f;
        while (WaitTime > 0.0f)
        {
            WaitTime -= Time.deltaTime;
            yield return null;
        }
        CatchedButton.enabled = true;

    }
    public void Catch()
    {
        Dokkebi closest = FindNearestPlayer();
        if(closest != null)
        {
            closest.CatchedSet();
        }
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.CATCHED_PEOPLE, ++RuleHandler.CatchedPeople } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        gameObject.GetComponentInParent<Teacher>().EXPUP(2);
    }
}
