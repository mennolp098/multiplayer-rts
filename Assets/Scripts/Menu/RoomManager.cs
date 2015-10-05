using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomManager : NetworkLobbyPlayer {
	public GameObject usernameUI;

	private ConnectionHandler _connectionHandler;
	private Dictionary<int, string> _usernameConnected = new Dictionary<int, string>();
	private Dictionary<string, GameObject> _usernameUi = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Awake () {
		_connectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
		_connectionHandler.OnPlayerConnect += AddPlayer;
	}

	public void ToggleReady()
	{
		if(!readyToBegin)
		{
			readyToBegin = true;
		} else {
			readyToBegin = false;
		}
	}

	//If needed for later
	[Command]
	void CmdSendUsernameToServer(int id,string username)
	{
		RpcSendUsernameToClients(id, username);
	}

	[ClientRpc]
	void RpcSendUsernameToClients(int id, string username)
	{
		_usernameConnected.Add(id, username);
		DisplayPlayerUsername(username);
	}

	void DisplayPlayerUsername(string username)
	{
		Vector3 newPos = new Vector3(0,0,0);
		newPos.y = 0 + 30 * _usernameUi.Count;

		GameObject newUsernameUI = Instantiate(usernameUI, Vector3.zero, Quaternion.identity) as GameObject;
		newUsernameUI.transform.SetParent(this.transform);
		newUsernameUI.transform.localScale = new Vector3(1,1,1);
		newUsernameUI.transform.GetChild(0).GetComponent<Text>().text = username;
		newUsernameUI.GetComponent<RectTransform>().anchoredPosition = newPos;


		_usernameUi.Add(username, newUsernameUI);
	}

	void AddPlayer (NetworkConnection conn) 
	{
		if(isServer && !_usernameConnected.ContainsKey(conn.connectionId))
		{
			string username = PlayerPrefs.GetString("Username");
			if(username == "")
				username = "No name";

			_usernameConnected.Add(conn.connectionId, username);
			DisplayPlayerUsername(username);

			RpcSendUsernameToClients(conn.connectionId , username);
		}
	}
}
