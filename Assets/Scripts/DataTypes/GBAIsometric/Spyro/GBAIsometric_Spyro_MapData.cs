﻿using System.Linq;

namespace R1Engine
{
    public class GBAIsometric_Spyro_MapData : R1Serializable
    {
        public long BlockSize { get; set; }

        public ushort Width { get; set; }
        public ushort Height { get; set; }

        public uint Uint_04 { get; set; }

        public ushort[] MapData { get; set; }
        
        public override void SerializeImpl(SerializerObject s)
        {
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Uint_04 = s.Serialize<uint>(Uint_04, name: nameof(Uint_04));

            if (BlockSize == (Width * Height * 2) + 8)
                MapData = s.SerializeArray<ushort>(MapData, Width * Height, name: nameof(MapData));
            else
                MapData = s.SerializeArray<byte>(MapData?.Select(x => (byte)x).ToArray(), Width * Height, name: nameof(MapData)).Select(x => (ushort)x).ToArray();

            //s.Log($"Max index: {MapData.Max()}");
        }
    }
}