using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using Photon.SocketServer.Rpc;
using ATCommon;

namespace ATServer
{
	class UnityPeer : PeerBase
	{
		private readonly ILogger log = LogManager.GetCurrentClassLogger();

		public UnityPeer(IRpcProtocol protocol, IPhotonPeer peer)
			: base(protocol, peer) 
		{
			log.Debug("Connection recieved from: " + peer.GetRemoteIP());
		}
		
		protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
		{
			log.Debug("Client Disconnected");
		}

		protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
		{
			switch ((OperationCode)operationRequest.OperationCode)
			{
				case OperationCode.Login:
					var username = (string)operationRequest.Parameters[(byte)ParameterCode.Username];
					var password = (string)operationRequest.Parameters[(byte)ParameterCode.Password];
					if (CheckLoginDetails(username, password))
					{

					}
					break;
				default:
					log.Error("Unknowned OperationRequest recieved");
					break;
			}
		}

		bool CheckLoginDetails(string username, string password)
		{
			return true;
		}
	}
}
