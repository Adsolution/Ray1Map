﻿using System;
using BinarySerializer;

namespace R1Engine.Jade {
	public class WAY_Network : Jade_File {
		public Jade_Reference<OBJ_GameObject> Root { get; set; }
		public uint Flags { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			Root = s.SerializeObject<Jade_Reference<OBJ_GameObject>>(Root, name: nameof(Root))?.Resolve();
			Flags = s.Serialize<uint>(Flags, name: nameof(Flags));
		}
	}
}
