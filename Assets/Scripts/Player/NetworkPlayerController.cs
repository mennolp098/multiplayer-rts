using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour {
	private ConnectionHandler _connectionHandler;
	private int _team = -1;
	private NetworkIdentity _myNetworkIdentity;
	private Vector3 _spawnPoint = new Vector3(0,0,0);

    public string username;
    public int connectionID;
	public bool isReadyToBegin = false;
	public GameObject builder;
	

	void Awake()
	{
		_myNetworkIdentity = GetComponent<NetworkIdentity>();
		ClientScene.RegisterPrefab(builder);
	}

	void Start()
	{
		
		if(_myNetworkIdentity.hasAuthority)
        {
            connectionID = _myNetworkIdentity.connectionToClient.connectionId;
            username = PlayerPrefs.GetString("Username");
        }
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
