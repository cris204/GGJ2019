using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public string name;
    public int idPlayer;
    public Sprite image;
    public int health;
    public int maxHealth;
    public int attack;
    public float movementSpeed;
    public float score;
    public int deathCount;
}
