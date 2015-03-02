using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrontMenu : MonoBehaviour 
{
	private string userName;
	private string password;

	public void PlayButtonPressed()
	{
		if (CheckLoginDetails(userName, password))
		{
			//Login, setup account, load into game.
			Application.LoadLevel("Game");
		}
	}

	bool CheckLoginDetails(string userName, string password)
	{
		return true;
	}

	public void OnUserNameInputChanged(string input)
	{
		userName = input;
	}

	public void OnPasswordInputChanged(string input)
	{
		password = input;
	}
}
