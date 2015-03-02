using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class PhotonServer : IPhotonPeerListener
{
	public enum ConnectionStatus
	{
		Connected,
		Disconnected,
		Unknown,
	}

	public ConnectionStatus status;
	PhotonPeer peer;

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

	}

	public void OnOperationResponse(OperationResponse operationResponse)
	{

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
	}
	#endregion

	public void Init(PhotonPeer peer, string serverAddress, string applicationName)
	{
		this.peer = peer;
		this.peer.Connect(serverAddress, applicationName);
	}

	public void Disconnect()
	{
		peer.Disconnect();
	}

	public void Update()
	{
		peer.Service();
	}
}
