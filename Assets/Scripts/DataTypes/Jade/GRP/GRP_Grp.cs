﻿using System;
using BinarySerializer;

namespace R1Engine.Jade {
	public class GRP_Grp : Jade_File {
		public Jade_Reference<OBJ_World_GroupObjectList> GroupObjectList { get; set; }
		public uint UInt_04 { get; set; }
		public uint Editor_UInt_08 { get; set; }
		public uint Editor_UInt_0C { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			GroupObjectList = s.SerializeObject<Jade_Reference<OBJ_World_GroupObjectList>>(GroupObjectList, name: nameof(GroupObjectList))?.Resolve();
			UInt_04 = s.Serialize<uint>(UInt_04, name: nameof(UInt_04));
			if (!Loader.IsBinaryData) {
				Editor_UInt_08 = s.Serialize<uint>(Editor_UInt_08, name: nameof(Editor_UInt_08));
				Editor_UInt_0C = s.Serialize<uint>(Editor_UInt_0C, name: nameof(Editor_UInt_0C));
			}
		}
	}
}
