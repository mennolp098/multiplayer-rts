using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
	public InputField _usernameInputField;

	public void SetUsername()
	{
		string newUsername = _usernameInputField.text;
		PlayerPrefs.SetString("Username", newUsername);
	}
}
