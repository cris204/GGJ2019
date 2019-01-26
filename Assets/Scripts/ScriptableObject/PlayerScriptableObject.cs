using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public class PlayerScriptableObject : ScriptableObject
{
    public string name;
    public Sprite image;
    public int health;
    public int maxHealth;
    public int baseAttack;
    public int attack;
    public int baseMovementSpeed;
    public float movementSpeed;
    public int score;
    public int deathCount;
}
