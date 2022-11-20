using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class StickyZone : MonoBehaviour
{
    private const float lifeTime = 10.0f;
    private CircleCollider2D CD;
    private void OnEnable()
    {
        if(PhotonNetwork.IsConnectedAndReady)
        {
            CD = GetComponent<CircleCollider2D>();
            StartCoroutine(SpawnTime());   
        }
    }


    public IEnumerator SpawnTime()
    {
        float time = lifeTime;
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        CD.enabled = false;
        Destroy(gameObject);
    }

}
