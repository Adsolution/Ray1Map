﻿namespace R1Engine
{
    public class GBA_Animation : GBA_BaseBlock
    {
        public byte Flags { get; set; }
        public byte Byte_01 { get; set; }
        public byte Byte_02 { get; set; }
        public byte Byte_03 { get; set; }
        public byte[] LayersPerFrame { get; set; }

        // Parsed
        public int FrameCount { get; set; }

        public GBA_AnimationLayer[][] Layers { get; set; }

        public override void SerializeImpl(SerializerObject s) {
            Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
            FrameCount = Byte_03 & 0x3F;

            LayersPerFrame = s.SerializeArray<byte>(LayersPerFrame, FrameCount, name: nameof(LayersPerFrame));

            s.Align();

            if (Layers == null) 
                Layers = new GBA_AnimationLayer[FrameCount][];

            for (int i = 0; i < FrameCount; i++)
                Layers[i] = s.SerializeObjectArray<GBA_AnimationLayer>(Layers[i], LayersPerFrame[i], name: $"{nameof(Layers)}[{i}]");
        }
    }
}