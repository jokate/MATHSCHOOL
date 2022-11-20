using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ItemContainer : MonoBehaviour
{
    public Item[] Itemcontainer = new Item[2];
    public Image Item1;
    public Image Item2;
    public List<GameObject> itemGetter;
    public AudioSource audioSource;
    private void Update() {
        if (PhotonNetwork.InRoom == true)
        {
            if (gameObject.GetComponent<Dokkebi>().state == Dokkebi.PlayerState.Catched)
            {
                Itemcontainer = null;
                Item1.sprite = null;
                Item2.sprite = null;
            }
        }
    }

    // Start is called before the first frame update
    public void ItemFill(Item item) 
    {
        if(item == null)
        {
            return;
        }
        if(Itemcontainer[0] == null)
        {
            Itemcontainer[0] = item;
            itemGetter[item.itemCode - 1].SetActive(true);
            Item1.sprite = item.ItemIcon;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else if(Itemcontainer[1] == null)
        {
            Itemcontainer[1] = item;
            itemGetter[item.itemCode - 1].SetActive(true);
            Item2.sprite = item.ItemIcon;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }   

    
}
