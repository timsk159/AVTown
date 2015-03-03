using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ATCommon;
using System.Text;

public class ServerConnector : MonoBehaviour 
{
    private static ServerConnector _instance;

    public static ServerConnector Instance
    {
        get
        {
            if(_instance == null)
            {
                throw new NullReferenceException("No singleton instance for ServerConnector");
            }
            return _instance;
        }
    }

    public event Action OnLoginSuccessful;
    public event Action OnLoginFailed;
    public event Action OnServerConnected;
    public event Action OnServerDisconnected;

	PhotonServer server;

    [SerializeField]
    bool debuggingEnabled;

    int outgoingOperationCount;

    void Awake()
    {
        _instance = this;
    }

	void Start()
	{
		Application.runInBackground = true;
		server = new PhotonServer();
        DontDestroyOnLoad(this);
        server.OnServerConnectionChanged += server_OnServerConnectionChanged;
        server.OnServerEvent += server_OnServerEvent;
        server.OnServerOperationResponse += server_OnServerOperationResponse;
	}

    void OnDestroy()
    {
        server.OnServerConnectionChanged -= server_OnServerConnectionChanged;
        server.OnServerEvent -= server_OnServerEvent;
        server.OnServerOperationResponse -= server_OnServerOperationResponse;
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

    void server_OnServerOperationResponse(OperationResponse opResponse)
    {
        switch (opResponse.OperationCode)
        {
            case (byte)OperationCode.Login:
                if (opResponse.ReturnCode == (byte)ReturnCode.Ok)
                {
                    if (OnLoginSuccessful != null)
                        OnLoginSuccessful();
                }
                else
                {
                    if (OnLoginFailed != null)
                    {
                        OnLoginFailed();
                    }
                }
                break;

            default:
                Debug.LogError("Unknown operation response");
                break;
        }
    }

    void server_OnServerEvent(EventData evenData)
    {

    }

    void server_OnServerConnectionChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                if (OnServerConnected != null)
                    OnServerConnected();
                break;
            case StatusCode.Disconnect:
            case StatusCode.DisconnectByServer:
            case StatusCode.DisconnectByServerLogic:
            case StatusCode.DisconnectByServerUserLimit:
            case StatusCode.TimeoutDisconnect:
                if(OnServerDisconnected != null)
                    OnServerDisconnected();
                break;
            default:
                Debug.LogError("Unknown connection state from server");
                break;
        }
    }

	void OnApplicationQuit()
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

    public void TryLogin(string userName, string password)
    {
        PhotonPeer peer = new PhotonPeer(server, ConnectionProtocol.Udp);
        server.Init(peer, "localhost:5055", "ATServer"); 
        
        var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.Username, userName }, 
                    { (byte)ParameterCode.Password, password }, 
                };
        SendOperation(OperationCode.Login, data, true, (byte)ChannelID.System);
    }

    public void SendOperation(OperationCode operationCode, Dictionary<byte, object> parameter, bool sendReliable, byte channelId)
    {
        Debug.Log("Sending op");
        if (debuggingEnabled)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("{0}: send operation {1}:", server.Peer.PeerID, operationCode);
            foreach (var entry in parameter)
            {
                builder.AppendFormat(" {0}=", (ParameterCode)entry.Key);
                if (entry.Value is float[])
                {
                    builder.Append("float[");
                    foreach (float number in (float[])entry.Value)
                    {
                        builder.AppendFormat("{0:0.00},", number);
                    }

                    builder.Append("]");
                }
                else
                {
                    builder.Append(entry.Value);
                }
            }

            Debug.Log(builder.ToString());
        }

        //Send the op
        server.Peer.OpCustom((byte)operationCode, parameter, sendReliable, channelId);

        // avoid operation congestion (QueueOutgoingUnreliableWarning)
        outgoingOperationCount++;
        if (outgoingOperationCount > 10)
        {
            server.Peer.SendOutgoingCommands();
            outgoingOperationCount = 0;
        }
    }
}
