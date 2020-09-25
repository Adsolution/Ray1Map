﻿namespace R1Engine
{
    public class R1_Poing : R1Serializable
    {
        public byte Byte_00 { get; set; }
        public byte Byte_01 { get; set; }
        public byte Byte_02 { get; set; }
        public byte Byte_03 { get; set; }
        public byte Byte_04 { get; set; }
        public byte Byte_05 { get; set; }
        public byte Byte_06 { get; set; }
        public byte Byte_07 { get; set; }
        public byte Byte_08 { get; set; }
        public byte Byte_09 { get; set; }
        public byte Byte_0A { get; set; }
        public byte Byte_0B { get; set; }
        public byte Byte_0C { get; set; }
        public bool IsGoldFist { get; set; }
        public byte FistSpeed { get; set; }
        public bool IsNormalFist { get; set; }
        public byte Byte_0D { get; set; }
        public byte Byte_0E { get; set; }
        public byte Byte_0F { get; set; }
        public byte Byte_10 { get; set; }
        public byte Byte_11 { get; set; }
        public byte Byte_12 { get; set; }
        public byte Byte_13 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte_00 = s.Serialize<byte>(Byte_00, name: nameof(Byte_00));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            Byte_05 = s.Serialize<byte>(Byte_05, name: nameof(Byte_05));
            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            Byte_07 = s.Serialize<byte>(Byte_07, name: nameof(Byte_07));
            Byte_08 = s.Serialize<byte>(Byte_08, name: nameof(Byte_08));
            Byte_09 = s.Serialize<byte>(Byte_09, name: nameof(Byte_09));
            Byte_0A = s.Serialize<byte>(Byte_0A, name: nameof(Byte_0A));
            Byte_0B = s.Serialize<byte>(Byte_0B, name: nameof(Byte_0B));
            s.SerializeBitValues<byte>(bitFunc =>
            {
                IsNormalFist = bitFunc(IsNormalFist ? 1 : 0, 1, name: nameof(IsNormalFist)) == 1;
                FistSpeed = (byte)bitFunc(FistSpeed, 2, name: nameof(FistSpeed));
                IsGoldFist = bitFunc(IsGoldFist ? 1 : 0, 1, name: nameof(IsGoldFist)) == 1;
                Byte_0C = (byte)bitFunc(Byte_0C, 4, name: nameof(Byte_0C));
            });
            Byte_0D = s.Serialize<byte>(Byte_0D, name: nameof(Byte_0D));
            Byte_0E = s.Serialize<byte>(Byte_0E, name: nameof(Byte_0E));
            Byte_0F = s.Serialize<byte>(Byte_0F, name: nameof(Byte_0F));
            Byte_10 = s.Serialize<byte>(Byte_10, name: nameof(Byte_10));
            Byte_11 = s.Serialize<byte>(Byte_11, name: nameof(Byte_11));
            Byte_12 = s.Serialize<byte>(Byte_12, name: nameof(Byte_12));
            Byte_13 = s.Serialize<byte>(Byte_13, name: nameof(Byte_13));
        }
    }
}