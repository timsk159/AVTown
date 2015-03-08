using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrontMenu : MonoBehaviour 
{
	private string userName;
	private string password;

    void Start()
    {
        ClientToServerConnector.Instance.OnLoginSuccess += OnLoginSuccess;
        ClientToServerConnector.Instance.OnLoginFailed += OnLoginFailed;
    }

    void OnDestroy()
    {
        ClientToServerConnector.Instance.OnLoginSuccess -= OnLoginSuccess;
        ClientToServerConnector.Instance.OnLoginFailed -= OnLoginFailed;
    }

	public void PlayButtonPressed()
	{
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            print("Logging in");
            ClientToServerConnector.Instance.SendLoginAttempt(userName, password);
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

    void OnLoginFailed()
    {

    }

    void OnLoginSuccess()
    {
        StartCoroutine(LevelLoader.Instance.LoadLevel("Game"));
    }
}
