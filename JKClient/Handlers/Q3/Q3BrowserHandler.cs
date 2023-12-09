﻿using System.Collections.Generic;

namespace JKClient {
	public class Q3BrowserHandler : Q3NetHandler, IBrowserHandler {
		private const string MasterQuake3Arena = "master.quake3arena.com";
		private const string MasterIOQuake3 = "master.ioquake3.org";
		private const string MasterMaverickServers = "master.maverickservers.com";
		private const ushort PortMasterQ3 = 27950;
		public virtual bool NeedStatus { get; private set; }
		public Q3BrowserHandler(ProtocolVersion protocol) : base(protocol) {}
		public virtual IEnumerable<ServerBrowser.ServerAddress> GetMasterServers() {
			return new ServerBrowser.ServerAddress[] {
				new ServerBrowser.ServerAddress(Q3BrowserHandler.MasterQuake3Arena, Q3BrowserHandler.PortMasterQ3),
				new ServerBrowser.ServerAddress(Q3BrowserHandler.MasterIOQuake3, Q3BrowserHandler.PortMasterQ3),
				new ServerBrowser.ServerAddress(Q3BrowserHandler.MasterMaverickServers, Q3BrowserHandler.PortMasterQ3)
			};
		}
		public virtual void HandleInfoPacket(in ServerInfo serverInfo, in InfoString info) {
			this.NeedStatus = true;
			serverInfo.Version = ClientVersion.Q3_v1_32;
			if (info.Count <= 0) {
				return;
			}
			serverInfo.GameType = Q3BrowserHandler.GetGameType(info["gametype"].Atoi());
			serverInfo.Pure = info["pure"].Atoi() != 0;
			if (info.TryGetValue("g_humanplayers", out string humanPlayers)) {
				this.NeedStatus = false;
				serverInfo.Clients = humanPlayers.Atoi();
			}
		}
		public virtual void HandleStatusResponse(in ServerInfo serverInfo, in InfoString info) {
			this.NeedStatus = false;
		}
		private static GameType GetGameType(int gameType) {
			switch (gameType) {
			case 0:
				return GameType.FFA;
			case 1:
				return GameType.Duel;
			case 2:
				return GameType.SinglePlayer;
			case 3:
				return GameType.Team;
			case 4:
				return GameType.CTF;
			default:
				return (GameType)(gameType+5);
			}
		}
	}
}
