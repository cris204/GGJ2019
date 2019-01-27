using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class jsonClass{
    public string type { get; set; }
    public string attack { get; set; }
    public string movementSpeed { get; set; }
	public string score { get; set; }
	public string deathcount { get; set; }
	public float playerOne_posX {get; set;}
	public float playerOne_posY {get; set;}
	public float playerTwo_posX {get; set;}
	public float playerTwo_posY {get; set;}
	public float playerThree_posX {get; set;}
	public float playerThree_posY {get; set;}
	public float playerFour_posX {get; set;}
	public float playerFour_posY {get; set;}
}