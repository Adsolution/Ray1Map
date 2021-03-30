﻿using System;

namespace R1Engine.Jade {
	public class GEO_Object : Jade_File {
		public GRO_Struct GRO { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			GRO = s.SerializeObject<GRO_Struct>(GRO, name: nameof(GRO));
			throw new NotImplementedException($"TODO: Implement {GetType()}");
		}
	}
}
