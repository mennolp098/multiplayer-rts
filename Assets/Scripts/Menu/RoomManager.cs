using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomManager : NetworkBehaviour {
	public GameObject usernameUI;
	private Dictionary<int, string> _usernamesConnected = new Dictionary<int, string>();
	private Dictionary<int, GameObject> _usernameUIs = new Dictionary<int, GameObject>();

	[Command]
	public void CmdGetUsername(int connId,string username)
	{
		Debug.Log(username);
		_usernamesConnected.Add(connId, username);
		DisplayPlayerUsername(connId);
	}
	
	void DisplayPlayerUsername(int connId)
	{
		Vector3 newPos = new Vector3(0,0,0);
		newPos.y = 0 + 30 * _usernameUIs.Count;
		
		GameObject newUsernameUI = Instantiate(usernameUI, Vector3.zero, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(newUsernameUI);
		newUsernameUI.transform.SetParent(this.transform);
		newUsernameUI.GetComponent<RectTransform>().anchoredPosition = newPos;
		newUsernameUI.GetComponentInChildren<Text>().text = _usernamesConnected[connId];

		_usernameUIs.Add(connId, newUsernameUI);
	}
}
