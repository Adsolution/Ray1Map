﻿namespace R1Engine
{
    /// <summary>
    /// A map block for Rayman 3 (GBA)
    /// </summary>
    public class GBA_R3_MapBlock : GBA_R3_BaseBlock
    {
        // Flags?
        public uint Unk1 { get; set; }

        public ushort Width { get; set; }
        public ushort Height { get; set; }

        // Two ushorts?
        public uint Unk2 { get; set; }

        // Always 1?
        public uint Unk3 { get; set; }

        // The tile indexes
        public ushort[] MapData { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize block header
            base.SerializeImpl(s);

            Unk1 = s.Serialize<uint>(Unk1, name: nameof(Unk1));
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Unk2 = s.Serialize<uint>(Unk2, name: nameof(Unk2));
            Unk3 = s.Serialize<uint>(Unk3, name: nameof(Unk3));

            // TODO: It seems the compressed block contains more data than just the tile indexes for BG_2 & 3?
            s.DoEncoded(new LZSSEncoder(), () => MapData = s.SerializeArray<ushort>(MapData, Width * Height, name: nameof(MapData)));
        }
    }
}