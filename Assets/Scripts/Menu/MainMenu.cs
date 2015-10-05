using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	public InputField hostNameInputField;
	public GameObject joinMatchButton;
	public GameObject joinMenu;

	private ConnectionHandler _myConnectionHandler;
	private List<GameObject> _allServers = new List<GameObject>();
	private List<MatchDesc> _allMatches = new List<MatchDesc>(); 
	void Awake()
	{
		_myConnectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
	}

	public void HostButtonPressed()
	{
		_myConnectionHandler.HostGame(hostNameInputField.text);
	}

	public void RefreshButtonPressed()
	{
		_allMatches = _myConnectionHandler.GetCurrentMatches();

		if(_allMatches.Count != 0)
		{
			for(int i = 0; i < _allMatches.Count; i++)
			{
				GameObject newServerBut = Instantiate(joinMatchButton, new Vector3(0,0,0),Quaternion.identity) as GameObject;
				newServerBut.transform.SetParent(joinMenu.transform);
				newServerBut.GetComponent<JoinMatchButton>().SetMatch(_allMatches[i]);
				newServerBut.GetComponent<JoinMatchButton>().SetPosition(new Vector3(250,-190 + i * -40,0));
				_allServers.Add(newServerBut);
			}
		}
	}
}
