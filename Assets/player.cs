using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class player{

    public int health { get; set; }
    public int attack { get; set; }
    public float movementSpeed { get; set; }
	public int score { get; set; }
	public int deathcount { get; set; }
	public float posX { get; set; }
	public float posY { get; set; }
	public string playerID { get; set; }
}
