using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class ConnectionHandler : MonoBehaviour {
	private NetworkManager _myNetworkManager;
	void Awake()
	{
		_myNetworkManager = gameObject.GetComponent<NetworkManager>();
	}

	void Start()
	{
		//start matchmaker when scene is initialised
		_myNetworkManager.StartMatchMaker();
	}

	public void HostGame(string roomName)
	{
		//set hostname + host game with default options
		_myNetworkManager.matchName = roomName;
		_myNetworkManager.matchMaker.CreateMatch(roomName, _myNetworkManager.matchSize, true, "", _myNetworkManager.OnMatchCreate);
	}

	public List<MatchDesc> GetCurrentMatches()
	{
		//refresh the match list
		_myNetworkManager.matchMaker.ListMatches(0,20, "", _myNetworkManager.OnMatchList);
		//return the match list
		return _myNetworkManager.matches;
	}
}
