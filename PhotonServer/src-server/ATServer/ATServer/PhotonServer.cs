using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;

namespace ATServer
{
    public class PhotonServer : ApplicationBase
	{

		#region Application Base Implementation

		protected override PeerBase CreatePeer(InitRequest initRequest)
		{
			return new UnityPeer(initRequest.Protocol, initRequest.PhotonPeer);
		}

		protected override void Setup()
		{
			var file = new FileInfo(Path.Combine(BinaryPath, "Log4Net.config"));
			if (file.Exists)
			{
				LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
				XmlConfigurator.ConfigureAndWatch(file);
			}
		}

		protected override void TearDown()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
