using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ability")]
public class Ability : ScriptableObject
{
    public int AbilityCode;
    public enum Type {
        PassiveStatUP,
        PassiveAbil,
        ActiveAbil
    };

    public Type AbilityType;
    public Sprite ShowImage, ActiveSpr;
    public GameObject Pref;
}
