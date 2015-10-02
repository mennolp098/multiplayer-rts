using UnityEngine;
using System.Collections;

public class RoomManager : MonoBehaviour {
	ConnectionHandler _connectionHandler;
	// Use this for initialization
	void Awake () {
		_connectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
