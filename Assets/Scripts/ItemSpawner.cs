using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    const int MaxKey = 3;
    public GameObject player;
    public List<GameObject> prefabs, CircleSpawn;
    public List<Item> itemlist;
    float time = 10.0f;
    bool isFirst = true;
    public GameObject pref;

    private void Start()
    {
        time = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(SinglePlayManager.Key == MaxKey)
        {
            SinglePlayManager.AllKeyGathered = true;
            if(isFirst)
            {
                ActivateCircle();
                isFirst = false;
            }
            if(time > 0.0f)
            {
                time -= Time.deltaTime;
            }
            else
            {
                time = 10.0f;
                int idx = Random.Range(0, itemlist.Count);
                ItemSpawn(idx);
            }
        }
    }

    void ActivateCircle()
    {
        int random = Random.Range(0, CircleSpawn.Count);
        pref.transform.position = CircleSpawn[random].transform.position;
        pref.SetActive(true);
    }
    public void ItemSpawn(int idx)
    {
        Vector3 newVec = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
        GameObject go;
        switch (itemlist[idx].itemCode)
        {
            case 0:
                newVec = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                go = Instantiate(prefabs[0], player.transform.position + newVec, Quaternion.identity);
                go.GetComponent<Stalk>().Initialize(-newVec);
                break;
            case 1:
                newVec = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
                Instantiate(prefabs[1], player.transform.position + newVec, Quaternion.identity);
                break;
            case 5:
                GameObject nearestdoor = null;
                float neardist = 0, currentdist = 0;

                foreach (var door in GameObject.FindGameObjectsWithTag("Door"))
                {
                    if (nearestdoor == null)
                    {
                        nearestdoor = door;
                        neardist = (gameObject.transform.position - nearestdoor.transform.position).magnitude;
                        Debug.Log(door.name + " " + neardist);
                    }
                    else
                    {
                        currentdist = (gameObject.transform.position - door.transform.position).magnitude;
                        Debug.Log(door.name + " " + currentdist);
                        if (currentdist < neardist)
                        {
                            neardist = currentdist;
                            nearestdoor = door;
                        }
                    }

                }
                Debug.Log(nearestdoor.name);
                StartCoroutine(nearestdoor.GetComponent<LocalDoor>().doorLock());
                break;
        }
    }
}
