using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinMatchButton : MonoBehaviour {
	private MatchDesc _myMatchDesc;
	private RectTransform _myTransform;
	private Text _myText;
	void Awake()
	{
		_myTransform = GetComponent<RectTransform>();
		_myText = transform.GetChild(0).GetComponent<Text>();
	}

	public void SetMatch (MatchDesc info) {
		_myMatchDesc = info;
		_myText.text = _myMatchDesc.name;
		GetComponent<Button>().onClick.AddListener(() => JoinMatch());
	}
	private void JoinMatch()
	{
		ConnectionHandler _connectionHandler = GameObject.FindGameObjectWithTag(Tags.CONNECTIONHANDLER).GetComponent<ConnectionHandler>();
		_connectionHandler.JoinGame(_myMatchDesc);
	}
	public void SetPosition(Vector2 pos)
	{
		_myTransform.anchoredPosition = pos;
	}
}
