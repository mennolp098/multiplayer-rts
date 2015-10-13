using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour {
	private ConnectionHandler _connectionHandler;
	private int _team = -1;
	private NetworkIdentity _myNetworkIdentity;
	private Vector3 _spawnPoint = new Vector3(0,0,0);

	public bool isReadyToBegin = false;
	public GameObject builder;
	public GameObject room;

	void Awake()
	{
		_myNetworkIdentity = GetComponent<NetworkIdentity>();
		ClientScene.RegisterPrefab(builder);
		ClientScene.RegisterPrefab(room);
	}

	void Start()
	{
		/*
		if(_myNetworkIdentity.isServer && _myNetworkIdentity.hasAuthority)
		{
			GameObject newRoom = Instantiate(room, Vector3.zero, Quaternion.identity) as GameObject;
			NetworkServer.SpawnWithClientAuthority(newRoom, _myNetworkIdentity.connectionToClient);
		}
		if(_myNetworkIdentity.hasAuthority)
		{
			string username = PlayerPrefs.GetString("Username");
			GameObject.FindGameObjectWithTag(Tags.ROOMMANAGER).GetComponent<RoomManager>().CmdGetUsername(_myNetworkIdentity.connectionToClient.connectionId, username);
		} */
	}

	[Command]
	public void CmdInit(Vector3 spawnPos, int team)
	{
		RpcSetSpawnPoint(spawnPos);
		RpcSetTeam(team);
		RpcSpawnBuilder();
	}

	[ClientRpc]
	public void RpcSpawnBuilder()
	{
		if(_myNetworkIdentity.hasAuthority)
		{
			GameObject newBuilder = Instantiate(builder, _spawnPoint, Quaternion.identity) as GameObject;
			NetworkServer.SpawnWithClientAuthority(newBuilder, _myNetworkIdentity.connectionToClient);
		}
	}

	[ClientRpc]
	public void RpcSetSpawnPoint(Vector3 pos)
	{
		if(_myNetworkIdentity.hasAuthority)
		{
			_spawnPoint = pos;
		}
	}
	
	[ClientRpc]
	public void RpcSetTeam(int team)
	{
		_team = team;
	}
}
