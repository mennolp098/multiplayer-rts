using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomManager : NetworkLobbyPlayer {
	public GameObject usernameUI;
	//public bool readyToBegin = false;

	private ConnectionHandler _connectionHandler;
	private Dictionary<int, string> _usernamesConnected = new Dictionary<int, string>();
	private Dictionary<string, GameObject> _usernameUIs = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {
		/*
		_connectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
		_connectionHandler.OnPlayerConnect += AddPlayer;
		if(isServer)
		{
			//you will be the first to instantiate your controller so it will always be yours
			NetworkConnection conn = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<NetworkIdentity>().connectionToClient;
			AddPlayer(conn);
		} */
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
	
	[Command]
	void CmdSendUsernameToServer(int id,string username)
	{
		if(_usernamesConnected.ContainsValue(username))
		{
			username += "+";
		}
		_usernamesConnected.Add(id, username);

		//reset the list to add new player
		RpcResetUsernameForClients();

		//send all usernames to clients
		foreach(KeyValuePair<int, string> usernameConnected in _usernamesConnected)
		{
			Debug.Log("Sending username: " + usernameConnected.Value);
			RpcSendUsernameToClients(usernameConnected.Key, usernameConnected.Value);
		}
	}

	[ClientRpc]
	void RpcResetUsernameForClients()
	{
		if(!isServer)
		{
			_usernamesConnected.Clear();
		}
		foreach(KeyValuePair<string, GameObject> usernameUI in _usernameUIs)
		{
			Destroy(usernameUI.Value);
		}
		_usernameUIs.Clear();
	}

	[ClientRpc]
	void RpcSendUsernameToClients(int id, string username)
	{
		if(!isServer)
		{
			Debug.Log("Retrieving username: " + username);
			_usernamesConnected.Add(id, username);
		}
		DisplayPlayerUsername(username);
	}

	void DisplayPlayerUsername(string username)
	{
		Debug.Log("Generating username UI with username: " + username);
		Vector3 newPos = new Vector3(0,0,0);
		newPos.y = 0 + 30 * _usernameUIs.Count;

		GameObject newUsernameUI = Instantiate(usernameUI, Vector3.zero, Quaternion.identity) as GameObject;
		newUsernameUI.transform.SetParent(this.transform);
		newUsernameUI.transform.localScale = new Vector3(1,1,1);
		newUsernameUI.transform.GetChild(0).GetComponent<Text>().text = username;
		newUsernameUI.GetComponent<RectTransform>().anchoredPosition = newPos;


		_usernameUIs.Add(username, newUsernameUI);
	}

	void AddPlayer (NetworkConnection conn) 
	{
		string username = PlayerPrefs.GetString("Username");
		if(username == "")
			username = "No name";
		CmdSendUsernameToServer(conn.connectionId, username);
	}
}
