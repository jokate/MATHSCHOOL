using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWindowSetup : MonoBehaviour
{
    public AbilityManager AM;
    private void OnEnable() {
        AM.Initialize();
    }
} 
