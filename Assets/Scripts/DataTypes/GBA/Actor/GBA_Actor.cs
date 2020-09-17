﻿namespace R1Engine
{
    public class GBA_Actor : R1Serializable
    {
        #region Actor Data

        public ushort XPos { get; set; }
        public ushort YPos { get; set; }

        public byte Byte_04 { get; set; }
        
        public byte ActorID { get; set; }
        
        public byte GraphicsDataIndex { get; set; }

        public byte StateIndex { get; set; }

        public byte Link_0 { get; set; } = 255;
        public byte Link_1 { get; set; } = 255;
        public byte Link_2 { get; set; } = 255;
        public byte Link_3 { get; set; } = 255;

        // For specific actor types
        public ActorType Type { get; set; }
        // BoxActor
        public byte BoxActorBlockOffsetIndex { get; set; }
        public byte[] UnkData1 { get; set; }
        public byte[] UnkData2 { get; set; }
        public ushort BoxMinY { get; set; }
        public ushort BoxMinX { get; set; }
        public ushort BoxMaxY { get; set; }
        public ushort BoxMaxX { get; set; }
        public byte BoxID { get; set; }

        // Unk2
        public byte Index { get; set; }
        public byte Unk_01 { get; set; }

        // Only in Prince of Persia
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public byte[] ExtraData { get; set; }

        // Star Wars Trilogy & above
        public ushort ActorSize { get; set; }

        #endregion

        #region Parsed

        public GBA_ActorGraphicData GraphicData { get; set; }

        #endregion

        #region Public Methods

        public override void SerializeImpl(SerializerObject s)
        {
            if (Type == ActorType.Unk) {
                Index = s.Serialize<byte>(Index, name: nameof(Index));
                Unk_01 = s.Serialize<byte>(Unk_01, name: nameof(Unk_01));
                ActorSize = s.Serialize<ushort>(ActorSize, name: nameof(ActorSize));
                if (ActorSize >= 2) {
                    Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
                    GraphicsDataIndex = s.Serialize<byte>(GraphicsDataIndex, name: nameof(GraphicsDataIndex));
                }
                ExtraData = s.SerializeArray<byte>(ExtraData, ActorSize - (s.CurrentPointer - Offset), name: nameof(ExtraData));
            } else {
                XPos = s.Serialize<ushort>(XPos, name: nameof(XPos));
                YPos = s.Serialize<ushort>(YPos, name: nameof(YPos));

                if (Type != ActorType.BoxTrigger) {
                    Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
                    ActorID = s.Serialize<byte>(ActorID, name: nameof(ActorID));

                    if (s.GameSettings.EngineVersion < EngineVersion.GBA_SplinterCellPandoraTomorrow || Type == ActorType.Normal || Type == ActorType.Always) {
                        GraphicsDataIndex = s.Serialize<byte>(GraphicsDataIndex, name: nameof(GraphicsDataIndex));
                        StateIndex = s.Serialize<byte>(StateIndex, name: nameof(StateIndex));
                    }

                    if (s.GameSettings.EngineVersion > EngineVersion.GBA_BatmanVengeance && s.GameSettings.EngineVersion < EngineVersion.GBA_SplinterCellPandoraTomorrow) {
                        Link_0 = s.Serialize<byte>(Link_0, name: nameof(Link_0));
                        Link_1 = s.Serialize<byte>(Link_1, name: nameof(Link_1));
                        Link_2 = s.Serialize<byte>(Link_2, name: nameof(Link_2));
                        Link_3 = s.Serialize<byte>(Link_3, name: nameof(Link_3));
                    }

                    if (s.GameSettings.EngineVersion >= EngineVersion.GBA_SplinterCell
                        && s.GameSettings.EngineVersion < EngineVersion.GBA_SplinterCellPandoraTomorrow) {
                        Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                        Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
                        int len = Short_0E & 0xF;
                        ExtraData = s.SerializeArray<byte>(ExtraData, len, name: nameof(ExtraData));
                    } 
                    else if (s.GameSettings.EngineVersion >= EngineVersion.GBA_SplinterCellPandoraTomorrow) 
                    {
                        if (Type == ActorType.Trigger || Type == ActorType.Unk) {
                            ActorSize = s.Serialize<ushort>(ActorSize, name: nameof(ActorSize));
                            ExtraData = s.SerializeArray<byte>(ExtraData, ActorSize - 8, name: nameof(ExtraData));
                        } else {
                            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                            ActorSize = s.Serialize<ushort>(ActorSize, name: nameof(ActorSize));
                            ExtraData = s.SerializeArray<byte>(ExtraData, ActorSize - 12, name: nameof(ExtraData));
                        }
                    }
                } else {
                    Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
                    BoxID = s.Serialize<byte>(BoxID, name: nameof(BoxID));
                    if (s.GameSettings.EngineVersion >= EngineVersion.GBA_PrinceOfPersia) {
                        UnkData1 = s.SerializeArray<byte>(UnkData1, 2, name: nameof(UnkData1));
                    }
                    BoxActorBlockOffsetIndex = s.Serialize<byte>(BoxActorBlockOffsetIndex, name: nameof(BoxActorBlockOffsetIndex));

                    if (s.GameSettings.EngineVersion >= EngineVersion.GBA_SplinterCellPandoraTomorrow) {
                        UnkData2 = s.SerializeArray<byte>(UnkData2, 1, name: nameof(UnkData2));
                        ActorSize = s.Serialize<ushort>(ActorSize, name: nameof(ActorSize));
                    } else if (s.GameSettings.EngineVersion < EngineVersion.GBA_PrinceOfPersia) {
                        UnkData2 = s.SerializeArray<byte>(UnkData2, 1, name: nameof(UnkData2));
                    } else {
                        UnkData2 = s.SerializeArray<byte>(UnkData2, 3, name: nameof(UnkData2));
                    }
                    BoxMinY = s.Serialize<ushort>(BoxMinY, name: nameof(BoxMinY));
                    BoxMinX = s.Serialize<ushort>(BoxMinX, name: nameof(BoxMinX));
                    BoxMaxY = s.Serialize<ushort>(BoxMaxY, name: nameof(BoxMaxY));
                    BoxMaxX = s.Serialize<ushort>(BoxMaxX, name: nameof(BoxMaxX));
                    if (s.GameSettings.EngineVersion >= EngineVersion.GBA_SplinterCellPandoraTomorrow) {
                        ExtraData = s.SerializeArray<byte>(ExtraData, ActorSize - 20, name: nameof(ExtraData));
                    }
                }
            }
        }

        #endregion


        public enum ActorType {
            Main,
            Always,
            Normal,
            BoxTrigger,
            Trigger,
            Unk
        }
    }
}