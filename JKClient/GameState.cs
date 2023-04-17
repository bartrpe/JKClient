﻿using System.Runtime.InteropServices;

namespace JKClient {
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal struct GameState {
		private const int MaxConfigstrings = 2200;
		public const int MaxGameStateChars = 16000;
		public const int ServerInfo = 0;
		public const int SystemInfo = 1;
		public unsafe fixed int StringOffsets[GameState.MaxConfigstrings];
		public unsafe fixed sbyte StringData[GameState.MaxGameStateChars];
		public int DataCount;
	}
}
