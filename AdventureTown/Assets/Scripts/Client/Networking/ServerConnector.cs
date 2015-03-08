using UnityEngine;
using System.Collections;


public class ServerConnector : uLink.MonoBehaviour 
{
    private ServerConnector _instance;
    public ServerConnector Instance
    {
        get { return _instance; }
    }
    public string serverIP = "127.0.0.1";
    public int port = 7100;

    void Awake()
    {
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
}
