using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GameController : NetworkBehaviour {
	private List<Vector3> _spawnPositions = new List<Vector3>();
	private ConnectionHandler _connectionHandler;
	private NetworkIdentity _myNetworkIdentity;
	private List<NetworkPlayerController> _currentPlayerControllers = new List<NetworkPlayerController>();
	private List<int> _currentTeams = new List<int>();
	// Use this for initialization
	void Awake() {
		_myNetworkIdentity = GetComponent<NetworkIdentity>();
	}

	void Start () {
		if(_myNetworkIdentity.isServer)
		{
			_connectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
			_connectionHandler.OnPlayerAdded +=  RefreshPlayerControllers;

			GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag(Tags.SPAWNPOINT);
			foreach (var item in spawnpoints) 
			{
				_spawnPositions.Add(item.transform.position);
			}
			RefreshPlayerControllers();
		}
	}
	
	void RefreshPlayerControllers (NetworkConnection conn = null, short playerId = -1) {
		GameObject[] allPlayers = GameObject.FindGameObjectsWithTag(Tags.PLAYER);

		_currentPlayerControllers.Clear();
		foreach (var item in allPlayers) 
		{
			_currentPlayerControllers.Add(item.GetComponent<NetworkPlayerController>());
		}

		if(_currentPlayerControllers.Count > 1)
			StartGame();
	}

	void StartGame()
	{
		for (int i = 0; i < _currentPlayerControllers.Count; i++) 
		{
			int randomSpawnPoint = (int)Random.Range(0, _spawnPositions.Count);
			//Random teams for now
			int team = 0;
			float team1count = 0;
			float team2count = 0;
			for (int j = 0; j < _currentTeams.Count; j++) 
			{
				if(_currentTeams[i] == 0)
				{
					team1count++;
				}
				else
				{
					team2count++;
				}
			}
			if(team1count > team2count)
				team = 1;

			_currentPlayerControllers[i].CmdInit(_spawnPositions[randomSpawnPoint], team);
			_spawnPositions.RemoveAt(randomSpawnPoint);
		}
	}
}
