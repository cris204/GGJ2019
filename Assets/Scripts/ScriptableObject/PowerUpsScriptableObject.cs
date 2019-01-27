using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Item")]
public class PowerUpsScriptableObject : ScriptableObject
{
    public string type;
    public Sprite item;
    public float value;
    public float durationTime;
}
