﻿using System;
using BinarySerializer;

namespace R1Engine.Jade {
	public class SND_Insert : Jade_File {
		public byte[] WaveData { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			// TODO: Maybe properly parse this later on
			WaveData = s.SerializeArray<byte>(WaveData, FileSize, name: nameof(WaveData));
		}
	}
}
