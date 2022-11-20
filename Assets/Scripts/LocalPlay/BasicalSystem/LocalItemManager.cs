using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalItemManager : MonoBehaviour
{
    #region Variable Set
    public delegate void ItemDelegate();
    public ItemContainer itemContainer;
    public List<AudioSource> itemAS; 
    Dictionary<int, ItemDelegate> ItemGenerator;


    [Header("Item Pool")]
    public List<GameObject> ItemSet;


    #endregion
    private void Awake()
    {
        ItemGenerator = new Dictionary<int, ItemDelegate>();
        GeneratePool();

    }
    // Start is called before the first frame update
    void Start()
    {
    }

    #region Item Function
    public void GeneratePool()
    {
        ItemGenerator.Add(1, I1);
        ItemGenerator.Add(2, I2);
        ItemGenerator.Add(3, I3);
        ItemGenerator.Add(4, I4);
        ItemGenerator.Add(5, I5);
        ItemGenerator.Add(6, I6);
        ItemGenerator.Add(7, I7);
        ItemGenerator.Add(8, I8);
    }
    public void I1()
    {
        itemAS[0].Play();
        GameObject.Instantiate(ItemSet[0], gameObject.transform.position, Quaternion.identity);
    }
    public void I2() { 
        GameObject go = Instantiate(ItemSet[1], gameObject.transform.position + ((Vector3)GetComponent<RayComponent>().GetRay().direction) * 2, Quaternion.identity);
        go.GetComponent<RocketPunch>().Initialze(gameObject);

    }
    public void I3()
    {
        itemAS[2].Play();
        StartCoroutine(gameObject.GetComponent<LocalPlayer>().SmallCookie());
    }
    public void I4()
    {
        itemAS[3].Play();
        Instantiate(ItemSet[2], gameObject.transform.position, Quaternion.identity);
    }

    public void I5()
    {
        itemAS[4].Play();
        GameObject nearestdoor = null;
        float neardist = 0, currentdist = 0;

        foreach (var door in GameObject.FindGameObjectsWithTag("Door"))
        {
            if (nearestdoor == null)
            {
                nearestdoor = door;
                neardist = (gameObject.transform.position - nearestdoor.transform.position).magnitude;
            }
            else
            {
                currentdist = (gameObject.transform.position - door.transform.position).magnitude;
                if (currentdist < neardist)
                {
                    neardist = currentdist;
                    nearestdoor = door;
                }
            }

        }
        StartCoroutine(nearestdoor.GetComponent<LocalDoor>().doorLock());
    }

    public void I6()
    {
        itemAS[5].Play();
        GameObject go = Instantiate(ItemSet[3], gameObject.transform.position, Quaternion.identity);
        go.GetComponent<ToiletTissue>().Initialize(GetComponent<RayComponent>().ray.direction);
    }

    public void I7()
    {
        itemAS[6].Play();
        StartCoroutine(GetComponent<LocalPlayer>().Dash(30.0f));
    }

    public void I8()
    {
        itemAS[7].Play();
        Instantiate(ItemSet[4], gameObject.transform.position, Quaternion.identity);
    }
    public void ActivateItem(int ItemId)
    {
        ItemDelegate active = ItemGenerator[ItemId];
        active();
    }
    #endregion


    public void ItemUse1()
    {
        if (itemContainer.Itemcontainer[0] != null)
        {
            ActivateItem(itemContainer.Itemcontainer[0].itemCode);
            itemContainer.Item1.sprite = null;
            itemContainer.Itemcontainer[0] = null;
        }
    }

    public void ItemUse2()
    {
        if (itemContainer.Itemcontainer[1] != null)
        {
            ActivateItem(itemContainer.Itemcontainer[1].itemCode);
            itemContainer.Item2.sprite = null;
            itemContainer.Itemcontainer[1] = null;

        }
    }
}
