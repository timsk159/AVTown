using UnityEngine;
using System;
using System.Collections;

//Defines and recieves RPC's.
//Usually used on the server by ClientConnector.cs
public class ClientToServerConnector : uLink.MonoBehaviour 
{
    private static ClientToServerConnector _instance;
    public static ClientToServerConnector Instance
    {
        get { return _instance; }
    }
    public string serverIP = "127.0.0.1";
    public int port = 7100;

    public event Action OnLoginSuccess;
    public event Action OnLoginFailed;

    void Awake()
    {
        Application.runInBackground = true;
        _instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (Application.webSecurityEnabled)
        {
            Security.PrefetchSocketPolicy(uLink.NetworkUtility.ResolveAddress(serverIP).ToString(), 843);
            Security.PrefetchSocketPolicy(uLink.MasterServer.ipAddress, 843);
        }
        uLink.Network.Connect(serverIP, port);
    }

    void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection mode)
    {
        Debug.LogError("Disconnected from server for reason: " + mode);
        Application.LoadLevel(0);
    }

    public void SendLoginAttempt(string username, string password)
    {
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            print("Sending RPC");
            networkView.RPC("Login", uLink.RPCMode.Server, username, password);
        }
    }

    public void Debug_Earn()
    {
        CharacterData.Instance.networkView.RPC("DebugEarnRequest", uLink.RPCMode.Server);

    }

    public void Debug_Spend()
    {
        CharacterData.Instance.networkView.RPC("DebugSpendRequest", uLink.RPCMode.Server);
    }


    #region RPC's
    [RPC]
    void LoginSuccessful()
    {
        if (OnLoginSuccess != null)
        {
            OnLoginSuccess();
        }
    }

    [RPC]
    void LoginFailed()
    {
        if (OnLoginFailed != null)
        {
            OnLoginFailed();
        }
    }
    #endregion
}
