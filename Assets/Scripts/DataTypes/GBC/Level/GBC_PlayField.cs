﻿namespace R1Engine
{
    public class GBC_PlayField : GBC_BaseBlock
    {
        public uint PointersCount { get; set; }
        public GBC_Pointer[] Pointers { get; set; }
        public byte[] UnkData { get; set; } // This data is identical across GBC and PalmOS, so most likely no 16/32-bit values

        // Parsed
        public GBC_Map Map { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize header
            base.SerializeImpl(s);
            SerializeOffsetTable(s);

            // TODO: figure out format

            // Parse data from pointers
            Map = s.DoAt(OffsetTable.GetPointer(0), () => s.SerializeObject<GBC_Map>(Map, name: nameof(Map)));
        }
    }
}