﻿using System;
using System.Linq;
using BinarySerializer;
using UnityEngine;

namespace R1Engine
{
    public class GAX2_InstrumentVolumeEnvelope : BinarySerializable {
        public byte NumPoints { get; set; }
        public byte NumPointsWithUShort_02 { get; set; }
        public byte Byte_02 { get; set; }
        public byte Byte_03 { get; set; }
        public Point[] Points { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            NumPoints = s.Serialize<byte>(NumPoints, name: nameof(NumPoints));
            NumPointsWithUShort_02 = s.Serialize<byte>(NumPointsWithUShort_02, name: nameof(NumPointsWithUShort_02));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
            Points = s.SerializeObjectArray<Point>(Points, NumPoints, name: nameof(Points));
        }

		public class Point : BinarySerializable {
            public ushort X { get; set; }
            public ushort UShort_02 { get; set; }
            public ushort Y { get; set; }
            public ushort UShort_06 { get; set; }

			public override void SerializeImpl(SerializerObject s) {
				X = s.Serialize<ushort>(X, name: nameof(X));
                UShort_02 = s.Serialize<ushort>(UShort_02, name: nameof(UShort_02));
                Y = s.Serialize<ushort>(Y, name: nameof(Y));
                UShort_06 = s.Serialize<ushort>(UShort_06, name: nameof(UShort_06));
			}
		}
	}
}