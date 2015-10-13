using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LobbyManager : NetworkBehaviour {
    private ConnectionHandler _connectionHandler;
    private NetworkIdentity _myNetworkIdentity;
    private List<NetworkPlayerController> _currentPlayerControllers = new List<NetworkPlayerController>();
    private RoomManager _roomManager;

    public GameObject room;

    void Awake()
    {
        _myNetworkIdentity = GetComponent<NetworkIdentity>();
        ClientScene.RegisterPrefab(room);
    }
	void Start () {
        if (_myNetworkIdentity.isServer)
        {
            _connectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
            GameObject newRoom = Instantiate(room, Vector3.zero, Quaternion.identity) as gameObject;
            NetworkServer.Spawn(newRoom);
            _roomManager = GameObject.FindGameObjectWithTag(Tags.ROOMMANAGER).GetComponent<RoomManager>();
            _connectionHandler.OnPlayerAdded += AddPlayerController;
            AddPlayerController();
        }
	}
    void AddPlayerController(NetworkConnection conn = null, short playerId = -1)
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag(Tags.PLAYER);

        foreach (var item in allPlayers)
        {
            if (!_currentPlayerControllers.Contains(item.GetComponent<NetworkPlayerController>()))
            {
                NetworkPlayerController newNetworkPlayer = item.GetComponent<NetworkPlayerController>();
                _currentPlayerControllers.Add(newNetworkPlayer);
                _roomManager.CmdGetUsername(newNetworkPlayer.connectionID, newNetworkPlayer.username);
            }
        }
    }
}
