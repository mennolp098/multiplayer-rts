using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class ConnectionHandler : NetworkManager {
	public delegate void ConnectionPlayerIdDelegate(NetworkConnection conn, short playerControllerId);
	public delegate void ConnectionDelegate(NetworkConnection conn);
	public delegate void NormalDelegate();
	public event ConnectionPlayerIdDelegate OnPlayerAdded;
	public event ConnectionDelegate OnClientConnected;
	public event ConnectionDelegate OnServerReadyUp;
	public event ConnectionDelegate OnPlayerConnect;
	public event ConnectionDelegate OnPlayerDisconnect;
	public event NormalDelegate OnMatchesRetrieved;

	public bool isMatchmakingEnabled;

	void Start()
	{
		//start matchmaker when scene is initialised
		if(isMatchmakingEnabled)
		{
			StartMatchMaker();
			//AppID appid;
			//appid = (AppID)94451;
			//matchMaker.SetProgramAppID(appid);
		}
	}

	public void HostGame(string roomName)
	{
		//set hostname + host game with default options
		if(isMatchmakingEnabled) //match maker
		{
			matchName = roomName;
			matchMaker.CreateMatch(roomName, 4, true, "", OnMatchCreate);
		} else { //lan start host
			StartHost();
		}
	}
	public void JoinGame(MatchDesc matchDescription = null)
	{
		if(isMatchmakingEnabled && matchDescription != null) //matchmaker
		{
			matchMaker.JoinMatch(matchDescription.networkId, "", OnMatchJoined);
		} else { //lan start client
			StartClient();
		}
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
		if(OnMatchesRetrieved != null)
			OnMatchesRetrieved();
	}

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		base.OnServerAddPlayer (conn, playerControllerId);
		if(OnPlayerAdded != null)
			OnPlayerAdded(conn, playerControllerId);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		Debug.Log("Client connected at: " + conn.address);
		if(OnClientConnected != null)
			OnClientConnected(conn);
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		Debug.Log("Player connected in server at: " + conn.address);
		if(OnPlayerConnect != null)
			OnPlayerConnect(conn);
	}
	public override void OnServerReady (NetworkConnection conn)
	{
		base.OnServerReady (conn);
		Debug.Log("server is ready");
		if(OnServerReadyUp != null)
			OnServerReadyUp(conn);
	}
	public override void OnStartHost ()
	{
		base.OnStartHost();
		Debug.Log("Starting host at: " + networkAddress);
	}
	public override void OnStopHost ()
	{
		base.OnStopHost();
		Debug.Log("Host stopped at: " + networkAddress);
	}
	public override void OnStopClient ()
	{
		base.OnStopClient();
		Debug.Log("Client Stopped");
	}
	public override void OnClientDisconnect (NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
		Debug.Log("Client disconnected at: " + conn.address);
		if(OnPlayerDisconnect != null)
			OnPlayerDisconnect(conn);
	}
	public override void OnServerDisconnect (NetworkConnection conn)
	{
		base.OnServerDisconnect(conn);
		Debug.Log("Player disconnected in server at: " + conn.address);
		if(OnPlayerDisconnect != null)
			OnPlayerDisconnect(conn);
	}
}
