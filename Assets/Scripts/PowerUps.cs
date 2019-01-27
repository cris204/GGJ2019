using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public PowerUpsScriptableObject powerUp;

    private string type;
    private SpriteRenderer item;
    private float value;
    private float durationTime;



    private void Start()
    {
        transform.gameObject.tag = powerUp.type;
        type = powerUp.type;
        item = GetComponent<SpriteRenderer>();
        Item.sprite = powerUp.item;
        Value = powerUp.value;
        DurationTime = powerUp.durationTime;
    }

    #region Get&Set
    public string Type { get => type; set => type = value; }
    public SpriteRenderer Item { get => item; set => item = value; }
    public float Value { get => value; set => this.value = value; }
    public float DurationTime { get => durationTime; set => durationTime = value; }
    #endregion
}
