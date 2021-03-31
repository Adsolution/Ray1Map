﻿
using System;
using BinarySerializer;

namespace R1Engine.Jade {
	public class Jade_TextureReference : BinarySerializable {
		public Jade_Key Key { get; set; }
		public bool IsNull => Key.IsNull;

		public override void SerializeImpl(SerializerObject s) {
			Key = s.SerializeObject<Jade_Key>(Key, name: nameof(Key));
		}

		public Jade_TextureReference() { }
		public Jade_TextureReference(Context c, Jade_Key key) {
			Context = c;
			Key = key;
		}

		// Dummy resolve method for now
		public Jade_TextureReference Resolve() {
			return this;
		}
	}
}
