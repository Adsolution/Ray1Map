﻿namespace R1Engine
{
    public class GBAVV_NitroKart_NGage_RLEFrame : R1Serializable
    {
        public int Width { get; set; } // Set before serializing
        public int Height { get; set; } // Set before serializing

        public GBAVV_NitroKart_NGage_RLERow[] Rows { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Rows = s.SerializeObjectArray(Rows, Height, x =>
            {
                x.BaseOffset = Offset;
                x.Width = Width;
            }, name: nameof(Rows));
        }
    }
}