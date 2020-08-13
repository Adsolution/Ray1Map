﻿namespace R1Engine
{
    public class GBA_Actor : R1Serializable
    {
        #region Actor Data

        public ushort XPos { get; set; }
        public ushort YPos { get; set; }

        public byte Byte_04 { get; set; }
        
        public GBA_R3_ActorID ActorID { get; set; }
        
        public byte GraphicsDataIndex { get; set; }

        // Seems to determine the state/animation/palette - this value determines the color of a butterfly for example
        public byte Byte_07 { get; set; }

        // Almost always -1
        public int Int_08 { get; set; }

        // Only in Prince of Persia
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public byte[] ExtraData { get; set; }

        #endregion

        #region Parsed

        public GBA_ActorGraphicData GraphicData { get; set; }

        #endregion

        #region Public Methods

        public override void SerializeImpl(SerializerObject s)
        {

            XPos = s.Serialize<ushort>(XPos, name: nameof(XPos));
            YPos = s.Serialize<ushort>(YPos, name: nameof(YPos));

            Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
            ActorID = (GBA_R3_ActorID)s.Serialize<byte>((byte)ActorID, name: nameof(ActorID));
            GraphicsDataIndex = s.Serialize<byte>(GraphicsDataIndex, name: nameof(GraphicsDataIndex));
            Byte_07 = s.Serialize<byte>(Byte_07, name: nameof(Byte_07));

            if (s.GameSettings.EngineVersion != EngineVersion.BatmanVengeanceGBA) {
                Int_08 = s.Serialize<int>(Int_08, name: nameof(Int_08));
            }

            if (s.GameSettings.EngineVersion == EngineVersion.PrinceOfPersiaGBA)
            {
                Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
                int len = Short_0E & 0xF;
                ExtraData = s.SerializeArray<byte>(ExtraData, len, name: nameof(ExtraData));
            }
        }

        #endregion
    }
}