using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class SaveBeacon : Activator
{
    public List<GameObject> SpawnPoint;
    List<GameObject> InJail;
    // Start is called before the first frame update
    void Start()
    {
       if(InJail == null)
       {
            InJail = new List<GameObject>();
       } 
    }
    public CircleCollider2D CD;

    protected override void Awake()
    {
        object num;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperty.RESP_GENTIME, out num))
        {
            int Number = (int)num;
            SpawnTime = Number;
        }
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
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { { RunningGameManager.RESP_BEACON, true } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        Destroy(gameObject);
    }
    public override void Success()
    {
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

    public void SaveALL()
    {

        SettingList();
        foreach(var jailed in InJail)
        {
            jailed.GetComponent<Dokkebi>().Spawn();
        }
        RuleHandler.CatchedPeople = 0;
        ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable() { {RunningGameManager.CATCHED_PEOPLE, 0 }, {RunningGameManager.RESP_BEACON, true } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        RuleHandler.isFirst = true;
        Destroy(gameObject);
    }

    private void SettingList()
    {
        foreach (var dkkebi in GameObject.FindGameObjectsWithTag("Dokkebi"))
        {
            Dokkebi dokkebi = dkkebi.GetComponent<Dokkebi>();
            if (dokkebi.state == Dokkebi.PlayerState.InJail)
            {
                InJail.Add(dkkebi);
            }
        }
    }
    

}
