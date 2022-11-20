using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPhase : MonoBehaviour
{
    [SerializeField]
    Button Bt;
    
    public void OnEnable()
    {
        Debug.Log("Abled");
    }

    public void OnDisable()
    {
        Debug.Log("Disabled");
        Bt.onClick.RemoveAllListeners();
    }

}