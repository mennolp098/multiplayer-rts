using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class ConnectionHandler : NetworkLobbyManager {
	public delegate void ConnectionDelegate(NetworkConnection conn);
	public delegate void NormalDelegate();
	public event ConnectionDelegate OnPlayerConnect;
	public event ConnectionDelegate OnPlayerDisconnect;
	public event NormalDelegate OnMatchesRetrieved;

	void Start()
	{
		//start matchmaker when scene is initialised
		StartMatchMaker();
	}

	public void HostGame(string roomName)
	{
		//set hostname + host game with default options
		matchName = roomName;
		matchMaker.CreateMatch(roomName, matchSize, true, "", OnMatchCreate);
	}

	public void JoinGame(MatchDesc matchDescription)
	{
		matchMaker.JoinMatch(matchDescription.networkId, "", OnMatchJoined);
	}

	public void GetCurrentMatches()
	{
		//refresh the match list
		matchMaker.ListMatches(0,20, "", OnMatchList);
	}

	//All network events
	public override void OnMatchList (ListMatchResponse matchList)
	{
		base.OnMatchList (matchList);
		OnMatchesRetrieved();
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		Debug.Log("Client connected: " + conn.address);
		if(OnPlayerConnect != null)
			OnPlayerConnect(conn);
	}
	public override void OnServerConnect(NetworkConnection conn)
	{
		Debug.Log("ServerConnected at: " + conn.address);
		if(OnPlayerConnect != null)
			OnPlayerConnect(conn);
	}
	public override void OnStartHost ()
	{
		Debug.Log("Starting Host at: " + networkAddress);
	}
	public override void OnStopHost ()
	{
		Debug.Log("Host stopped at: " + networkAddress);
	}
	public override void OnStopClient ()
	{
		Debug.Log("Client Stopped");
	}
	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.Log("Client disconnected: " + conn.address);
		if(OnPlayerDisconnect != null)
			OnPlayerDisconnect(conn);
	}
	public override void OnServerDisconnect (NetworkConnection conn)
	{
		Debug.Log("Server disconnected: " + conn.address);
		if(OnPlayerDisconnect != null)
			OnPlayerDisconnect(conn);
	}
}
