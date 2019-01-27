using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;

public class request : MonoBehaviour {

	player playerOne = new player();
	public void Start(){
		POST();
	}
	public WWW POST()
	{
		WWW www;
		Hashtable postHeader = new Hashtable();
		postHeader.Add("Content-Type", "application/json");

		playerOne.health = 100;
		playerOne.attack = 300;
		playerOne.movementSpeed = 0.8f;
		playerOne.score = 1230;
		playerOne.deathcount = 2;
		playerOne.posX = 0;
		playerOne.posY = 0;

		string jsonToServer = JsonConvert.SerializeObject(playerOne);
		var formData = System.Text.Encoding.UTF8.GetBytes(jsonToServer);

		www = new WWW("http://localhost:5000/", formData, postHeader);
		StartCoroutine(WaitForRequest(www));
		return www;
	}

	IEnumerator WaitForRequest(WWW data)
	{
		yield return data;
		if (data.error != null)
		{
			Debug.Log(data.error);
		}
		else
		{
			Debug.Log("Data from server: " + data.text);
			string response = data.text;
			Debug.Log(response);
			jsonClass jsonObj = JsonConvert.DeserializeObject<jsonClass>(response);
			Debug.Log(jsonObj.type);
		}
	}
}


