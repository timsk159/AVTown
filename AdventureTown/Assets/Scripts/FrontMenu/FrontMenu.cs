using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrontMenu : MonoBehaviour 
{
	private string userName;
	private string password;

    void Start()
    {
        ServerConnector.Instance.OnLoginSuccessful += Server_OnLoginSuccessful;
        ServerConnector.Instance.OnLoginFailed += Server_OnLoginFailed;
    }

    void OnDestroy()
    {
        ServerConnector.Instance.OnLoginSuccessful -= Server_OnLoginSuccessful;
        ServerConnector.Instance.OnLoginFailed -= Server_OnLoginFailed;
    }

    void Server_OnLoginSuccessful()
    {
        //Setup account, load into game.
        Application.LoadLevel("Game");
    }

    void Server_OnLoginFailed()
    {
        BroadcastMessage("ErrorAnim");
        //Show Error msg
    }

	public void PlayButtonPressed()
	{
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            ServerConnector.Instance.TryLogin(userName, password);
        }
	}

    //Hack for tab-selection in editor.
#if UNITY_EDITOR
    public void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
        {
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "UserNameInput" && Input.GetKeyDown(KeyCode.Tab))
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(GameObject.Find("PasswordInput"));
            }
        }
    }
#endif
	public void OnUserNameInputChanged(string input)
	{
		userName = input;
	}

	public void OnPasswordInputChanged(string input)
	{
		password = input;
	}
}
