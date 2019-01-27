using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public string name;
    public int idPlayer;
    public Sprite image;
    public float health;
    public float maxHealth;
    public float attack;
    public float movementSpeed;
    public float score;
    public int deathCount;
}
