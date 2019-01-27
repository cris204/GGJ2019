using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class playerController : MonoBehaviour
{
    jsonMsg playerJson = new jsonMsg();
	public GameObject playerOnline;

	private Rigidbody2D rb_local;
	private Rigidbody2D rb_online;
	// Use this for initialization
	void Start () {
        playerJson.ready = "False";
		playerJson.posX = 0.0f;
		playerJson.posY = 0.0f;
        startSession();
		// playerJson.playerID = "2";
		rb_local = GetComponent<Rigidbody2D>();
		rb_online = playerOnline.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		POST();
		float horizontal = Input.GetAxis("HorizontalPlayer1");
		float vertical = Input.GetAxis("VerticalPlayer1");
		if(playerJson.playerID == "1"){
			playerJson.posX = horizontal;
			playerJson.posY = vertical;
		}else if(playerJson.playerID == "2"){
			playerJson.posX = horizontal;
			playerJson.posY = vertical;
		}
	}

	public WWW POST()
	{
		WWW www;
		Hashtable postHeader = new Hashtable();
		postHeader.Add("Content-Type", "application/json");

		string jsonToServer = JsonConvert.SerializeObject(playerJson);
		var formData = System.Text.Encoding.UTF8.GetBytes(jsonToServer);

		www = new WWW("http://192.168.1.101:5000/update", formData, postHeader);
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
			jsonResponse jsonObj = JsonConvert.DeserializeObject<jsonResponse>(response);
            if(jsonObj.start == "True"){
			    rb_local.velocity = new Vector2(jsonObj.playerOne_posX, jsonObj.playerOne_posY)*100*Time.deltaTime;
			    rb_online.velocity = new Vector2(jsonObj.playerTwo_posX, jsonObj.playerTwo_posY)*100*Time.deltaTime;
            }
		}
	}

	public WWW startSession()
	{
		WWW www;
		Hashtable postHeader = new Hashtable();
		postHeader.Add("Content-Type", "application/json");

		string jsonToServer = JsonConvert.SerializeObject(playerJson);
		var formData = System.Text.Encoding.UTF8.GetBytes(jsonToServer);

		www = new WWW("http://192.168.1.101:5000/start", formData, postHeader);
		StartCoroutine(waitForPlayerID(www));
		return www;
	}

	IEnumerator waitForPlayerID(WWW data)
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
			jsonResponse jsonObj = JsonConvert.DeserializeObject<jsonResponse>(response);
            playerJson.playerID = jsonObj.player_num;
            playerJson.ready = "True";
            Debug.Log("New player! -> playerID: " + playerJson.playerID);
		}
	}
}
