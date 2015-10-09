
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
	void Awake()
	{
		_myConnectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
		_myConnectionHandler.OnMatchesRetrieved += GenerateJoinButtons;
	}

	public void HostButtonPressed()
	{
		if(hostNameInputField.text != "")
		{
			_myConnectionHandler.HostGame(hostNameInputField.text);
			gameObject.SetActive(false);
		} else {
			Debug.LogError("Server needs a name");
		}
	}

	public void RefreshButtonPressed()
	{
		_myConnectionHandler.GetCurrentMatches();
	}

	public void GenerateJoinButtons()
	{
		foreach(GameObject serverButton in _allServers)
		{
			Destroy(serverButton);
		}
		_allServers.Clear();

		List<MatchDesc> matches = _myConnectionHandler.matches;
		if((matches != null) && matches.Count != 0)
		{
			for(int i = 0; i < matches.Count; i++)
			{
				GameObject newServerBut = Instantiate(joinMatchButton, new Vector3(0,0,0),Quaternion.identity) as GameObject;
				newServerBut.transform.SetParent(joinMenu.transform);
				newServerBut.GetComponent<JoinMatchButton>().SetMatch(matches[i]);
				newServerBut.GetComponent<JoinMatchButton>().SetPosition(new Vector3(0,0 + i * -20,0));
				_allServers.Add(newServerBut);
			}
		}
	}
}
