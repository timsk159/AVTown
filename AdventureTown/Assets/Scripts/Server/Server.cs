using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if IsServer
[Serializable]
public class NetworkInstantiateObject
{
    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public GameObject proxyPrefab;

    public Vector3 startPosition = new Vector3(0, 0, 0);
    public Vector3 startRotation = new Vector3(0, 0, 0);

    public void Instantiate(uLink.NetworkPlayer player, object[] initialData)
    {
        Quaternion rotation = Quaternion.Euler(startRotation);

        var dat = player.localData as Dictionary<string, object>;
        var sessionID = (int)dat["sessionID"];
        uLink.Network.Instantiate(player, null, clientPrefab, serverPrefab, startPosition, rotation, sessionID, initialData);
    }
}

public class Server : uLink.MonoBehaviour
{
    public int port = 7100;
    public int maxConnections = 64;
    public int targetFrameRate = 60;
    GameSessionManager sessionManager;

    void Start()
    {
        Application.runInBackground = true;
        Application.targetFrameRate = targetFrameRate;
        uLink.Network.InitializeServer(maxConnections, port);
    }

    void uLink_OnServerInitialized()
    {
        Debug.Log("Server successfully started on port " + uLink.Network.listenPort);
        sessionManager = new GameSessionManager(1, 65536);
    }

    void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
    {
        uLink.Network.DestroyPlayerObjects(player);
        uLink.Network.RemoveRPCs(player);

        // this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
        uLink.Network.RemoveInstantiates(player);
        sessionManager.RemovePlayer(player);
    }

    void uLink_OnPlayerApproval(uLink.NetworkPlayerApproval playerApproval)
    {
        Debug.Log("Approving player connection");
        playerApproval.Approve();
    }

    void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
    {
        Debug.Log("Connection recieved from: " + player.ipAddress);
        int sessionID = 0;
        if (sessionManager.TryAddPlayerToAnySession(player, out sessionID))
        {
            var dat = new Dictionary<string, object>();
            dat.Add("sessionID", sessionID);
            player.localData = dat;
        }
        else
        {
            uLink.Network.CloseConnection(player, true);
        }
    }
}
#endif