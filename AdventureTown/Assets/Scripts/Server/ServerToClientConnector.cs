using UnityEngine;
using System.Collections;

//Defines and recieves RPC's. Also sends responses and events to client
//Usually used on the client by ServerConnector.cs
#if IsServer
public class ServerToClientConnector : uLink.MonoBehaviour
{
    private static ServerToClientConnector _instance;
    public static ServerToClientConnector Instance
    {
        get { return _instance; }
    }

    public NetworkInstantiateObject characterDataPrefabData;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    [RPC]
    void Login(string userName, string password, uLink.NetworkMessageInfo info)
    {
        print("Recieved login request");
        if (CheckLoginDetails(userName, password))
        {
            //Retrieve user data from DB.

            characterDataPrefabData.Instantiate(info.sender, new object[2] { 10, 10 });
            networkView.RPC("LoginSuccessful", info.sender);
        }
        else
        {
            networkView.RPC("LoginFailed", info.sender);
        }
    }

    bool CheckLoginDetails(string userName, string password)
    {
        return true;
    }
}
#endif