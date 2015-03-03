using UnityEngine;
using System.Collections;
using System;
using ExitGames.Client.Photon;
using ATCommon;

public class PhotonServer : IPhotonPeerListener
{
	public enum ConnectionStatus
	{
		Connected,
		Disconnected,
		Unknown,
	}

    public event Action<EventData> OnServerEvent;
    public event Action<OperationResponse> OnServerOperationResponse;
    public event Action<StatusCode> OnServerConnectionChanged;

	public ConnectionStatus status;
	
    PhotonPeer _peer;

    public PhotonPeer Peer
    {
        get
        {
            return _peer;
        }
    }

	public PhotonServer()
	{
		status = ConnectionStatus.Disconnected;
	}

	#region IPhotonPeerListener implementation
	public void DebugReturn(DebugLevel level, string message)
	{

	}

	public void OnEvent(EventData eventData)
	{
        if(OnServerEvent != null)
        {
            OnServerEvent(eventData);
        }
	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{
        if(OnServerOperationResponse != null)
        {
            OnServerOperationResponse(operationResponse);
        }
	}

	public void OnStatusChanged(StatusCode statusCode)
	{
		switch (statusCode)
		{
			case StatusCode.Connect:
				status = ConnectionStatus.Connected;
				break;
			case StatusCode.Disconnect:
			case StatusCode.DisconnectByServer:
			case StatusCode.DisconnectByServerLogic:
			case StatusCode.DisconnectByServerUserLimit:
			case StatusCode.TimeoutDisconnect:
				status = ConnectionStatus.Disconnected;
				break;
			default:
				status = ConnectionStatus.Unknown;
				break;
		}
        if(OnServerConnectionChanged != null)
        {
            OnServerConnectionChanged(statusCode);
        }
	}
	#endregion

	public void Init(PhotonPeer peer, string serverAddress, string applicationName)
	{
		this._peer = peer;
		this._peer.Connect(serverAddress, applicationName);
	}

	public void Disconnect()
	{
		_peer.Disconnect();
	}

	public void Update()
	{
		_peer.Service();
	}
}
