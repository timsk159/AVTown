using UnityEngine;
using System.Collections;

namespace Server
{
    public class Server : uLink.MonoBehaviour
    {
        public int port = 7100;
        public int maxConnections = 64;
        public int targetFrameRate = 60;

        void Start()
        {
            Application.targetFrameRate = targetFrameRate;

            uLink.Network.InitializeServer(maxConnections, port);
        }

        void uLink_OnServerInitialized()
        {
            Debug.Log("Server successfully started on port " + uLink.Network.listenPort);
        }

        void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
        {
            uLink.Network.DestroyPlayerObjects(player);
            uLink.Network.RemoveRPCs(player);

            // this is not really necessery unless you are removing NetworkViews without calling uLink.Network.Destroy
            uLink.Network.RemoveInstantiates(player);
        }

        void uLink_OnPlayerApproval(uLink.NetworkPlayerApproval playerApproval)
        {
            Debug.Log("Approving player connection");
            playerApproval.Approve();
        }

        void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
        {
            Debug.Log("Connection recieved from: " + player.ipAddress);
        }
    }
}