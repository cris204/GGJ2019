using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class playerController : MonoBehaviour {

	player playerLocal = new player();
	public GameObject playerOnline;

	private Rigidbody2D rb_local;
	private Rigidbody2D rb_online;
	// Use this for initialization
	void Start () {
		playerLocal.playerID = "1";
		playerLocal.posX = 0.0f;
		playerLocal.posY = 0.0f;
		rb_local = GetComponent<Rigidbody2D>();
		rb_online = playerOnline.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		POST();
        
		float horizontal = Input.GetAxis("HorizontalPlayer1");
		float vertical = Input.GetAxis("VerticalPlayer1");
		if(playerLocal.playerID == "1"){
			playerLocal.posX = horizontal;
			playerLocal.posY = vertical;
		}else if(playerLocal.playerID == "2"){
			playerLocal.posX = horizontal;
			playerLocal.posY = vertical;
		}
		playerLocal.health = 100;
		playerLocal.attack = 300;
		playerLocal.movementSpeed = 0.8f;
		playerLocal.score = 1230;
		playerLocal.deathcount = 2;
	}

	public WWW POST()
	{
        
        WWW www;
		Hashtable postHeader = new Hashtable();
		postHeader.Add("Content-Type", "application/json");

		string jsonToServer = JsonConvert.SerializeObject(playerLocal);
		var formData = System.Text.Encoding.UTF8.GetBytes(jsonToServer);

		www = new WWW("http://192.168.1.101:5000/update", formData, postHeader);
		StartCoroutine(WaitForRequest(www));
		return www;
	}

	IEnumerator WaitForRequest(WWW data)
	{
        
        yield return data;
        Debug.Log("si");
        if (data.error != null)
		{
			Debug.Log(data.error);
		}
		else
		{
			Debug.Log("Data from server: " + data.text);
			string response = data.text;
			jsonClass jsonObj = JsonConvert.DeserializeObject<jsonClass>(response);
			Debug.Log(transform.position);
			// playerOnline.transform.position = new Vector2(jsonObj.playerTwo_posX, jsonObj.playerTwo_posY);
			// transform.position = new Vector2(jsonObj.playerOne_posX, jsonObj.playerOne_posY);
			rb_local.velocity = new Vector2(jsonObj.playerOne_posX, jsonObj.playerOne_posY)*100*Time.deltaTime;
			rb_online.velocity = new Vector2(jsonObj.playerTwo_posX, jsonObj.playerTwo_posY)*100*Time.deltaTime;
		}
        
    }
}
