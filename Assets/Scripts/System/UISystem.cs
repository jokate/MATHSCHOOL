using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISystem : MonoBehaviour
{
    public GameObject uis;
    static public GameObject UISet;

    static public void UIStart()
    {
        UISet.SetActive(true);
    }

    static public void Init(GameObject GO)
    {
        UISet = GO;
    }

    private void Awake()
    {
        Init(uis);
    }


}
