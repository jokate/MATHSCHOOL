using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ItemManager : MonoBehaviourPunCallbacks
{
    #region Variable Set
    public delegate void ItemDelegate();
    public ItemContainer itemContainer;
    public List<AudioSource> ItemAudio;
    Dictionary<int, ItemDelegate> ItemGenerator;


    [Header("Item Pool")]
    public List<GameObject> ItemSet;


    #endregion
    private void Awake()
    {
        if(photonView.IsMine) {
            ItemGenerator = new Dictionary<int, ItemDelegate>();
            GeneratePool();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    #region Item Function
    public void GeneratePool()
    {
        ItemGenerator.Add(1,(() => {
            ItemAudio[0].Play();
            photonView.RPC(nameof(I1), RpcTarget.AllBuffered); }));
        ItemGenerator.Add(2, (()=> {
            ItemAudio[1].Play();
            photonView.RPC(nameof(I2), RpcTarget.AllBuffered);
        }));
        ItemGenerator.Add(3, (() => {
            ItemAudio[2].Play();
            photonView.RPC(nameof(I3), RpcTarget.AllBuffered);
        }));
        ItemGenerator.Add(4, (() => {
            ItemAudio[3].Play();
            photonView.RPC(nameof(I4), RpcTarget.AllBuffered);
        }));
        ItemGenerator.Add(5, (() => {
            ItemAudio[4].Play();
            photonView.RPC(nameof(I5), RpcTarget.AllBuffered);
        }));
        ItemGenerator.Add(6, (() => {
            ItemAudio[5].Play();
            photonView.RPC(nameof(I6), RpcTarget.AllBuffered);
        }));
        ItemGenerator.Add(7, I7);
        ItemGenerator.Add(8, (() => {
            ItemAudio[7].Play();
            photonView.RPC(nameof(I8), RpcTarget.AllBuffered);
        }));
    }
    [PunRPC]
    public void I1()
    {
        GameObject.Instantiate(ItemSet[0], gameObject.transform.position, Quaternion.identity);
    }
    [PunRPC]
    public void I2()
    {
        GameObject go = Instantiate(ItemSet[1], gameObject.transform.position + ((Vector3)GetComponent<RayComponent>().GetRay().direction) * 2, Quaternion.identity);
        go.GetComponent<RocketPunch>().Initialze(gameObject);

    }
    [PunRPC]
    public void I3()
    {
        StartCoroutine(gameObject.GetComponent<Dokkebi>().SmallCookie());
    }
    [PunRPC]
    public void I4()
    {
        Instantiate(ItemSet[2], gameObject.transform.position, Quaternion.identity);
    }
    [PunRPC]
    public void I5() {
        GameObject nearestdoor = null;
        float neardist = 0, currentdist = 0;
        
        foreach (var door in GameObject.FindGameObjectsWithTag("Door")) { 
            if(nearestdoor == null)
            {
                nearestdoor = door;
                neardist = (gameObject.transform.position - nearestdoor.transform.position).magnitude;
            } else
            {
                currentdist = (gameObject.transform.position - door.transform.position).magnitude;
                if(currentdist < neardist)
                {
                    neardist = currentdist;
                    nearestdoor = door;
                }
            }

        }
        StartCoroutine(nearestdoor.GetComponent<DoorScript>().doorLock());
    }

    [PunRPC]
    public void I6()
    {
        GameObject go = Instantiate(ItemSet[3], gameObject.transform.position, Quaternion.identity);
        go.GetComponent<ToiletTissue>().Initialize(GetComponent<RayComponent>().ray.direction);
    }

    public void I7() {
        ItemAudio[6].Play();
        photonView.RPC("Dash", RpcTarget.AllBuffered, 30.0f);
    }

    [PunRPC]
    public void I8()
    {
        Instantiate(ItemSet[4], gameObject.transform.position, Quaternion.identity);
    }
    public void ActivateItem(int ItemId)
    {
        ItemDelegate active = ItemGenerator[ItemId];
        active();
    }
    #endregion




    public void ItemUse1() {
        if(itemContainer.Itemcontainer[0] != null) {
            ActivateItem(itemContainer.Itemcontainer[0].itemCode);
            itemContainer.Item1.sprite = null;
            itemContainer.Itemcontainer[0] = null;
        }
    }

    public void ItemUse2() {
        if(itemContainer.Itemcontainer[1] != null) {
            ActivateItem(itemContainer.Itemcontainer[1].itemCode);
            itemContainer.Item2.sprite = null;
            itemContainer.Itemcontainer[1] = null;

        }
    }
}
