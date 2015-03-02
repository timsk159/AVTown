using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon;

public class Login : MonoBehaviour 
{
	PhotonServer server;

	void Start()
	{
		Application.runInBackground = true;
		server = new PhotonServer();
	}

	void Update()
	{
		if (server.status == PhotonServer.ConnectionStatus.Connected)
		{
			try
			{
				server.Update();
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
		}
	}

	public void OnApplicationQuit()
	{
		try
		{
			server.Disconnect();
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
	}

	public void OnLoginClicked()
	{
		PhotonPeer peer = new PhotonPeer(server, ConnectionProtocol.Udp);
		server.Init(peer, "localhost:5055", "ATServer"); 
	}

#if Debug_Networking
	void OnGUI()
	{
		GUILayout.Label(server.status);
	}
#endif
}
