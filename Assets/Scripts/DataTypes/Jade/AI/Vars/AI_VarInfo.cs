﻿using BinarySerializer;

namespace R1Engine.Jade {
	public class AI_VarInfo : BinarySerializable {
		public int BufferOffset { get; set; }
		public int ArrayDimensionsCount { get; set; }
		public int ArrayLength { get; set; }
		public short Type { get; set; }
		public short Flags { get; set; }
		public override void SerializeImpl(SerializerObject s) {
			BufferOffset = s.Serialize<int>(BufferOffset, name: nameof(BufferOffset));
			s.SerializeBitValues<int>(bitFunc => {
				ArrayLength = bitFunc(ArrayLength, 30, name: nameof(ArrayLength));
				ArrayDimensionsCount = bitFunc(ArrayDimensionsCount, 2, name: nameof(ArrayDimensionsCount));
			});
			Type = s.Serialize<short>(Type, name: nameof(Type));
			Flags = s.Serialize<short>(Flags, name: nameof(Flags));
		}
	}
}
