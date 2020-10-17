﻿using System;
using UnityEngine;

namespace R1Engine
{
    public class GBARRR_Actor : R1Serializable
    {
        public short YPosition { get; set; }
        public short XPosition { get; set; }
        public byte[] Data1 { get; set; }
        public ushort Runtime_Ushort_08 { get; set; }
        public ushort Ushort_0A { get; set; }
        public ushort Ushort_0C { get; set; } // 2 bytes?
        public ushort Ushort_0E { get; set; }
        public ushort Ushort_10 { get; set; }
        public ushort RuntimeStateIndex { get; set; } // Is this correct? Changing this changes the animation.
        public uint Runtime_Uint_14 { get; set; }
        public uint Runtime_Uint_18 { get; set; }
        public uint RuntimeFunctionPointer { get; set; }
        public ushort Runtime_Ushort_20 { get; set; }
        public short RuntimeXPosition { get; set; }
        public short RuntimeYPosition { get; set; }
        public byte RuntimeAnimIndex { get; set; } // Is this correct? This can only be changed when changing the state index.
        public byte Runtime_Byte_27 { get; set; }
        public byte RuntimeSpriteWidth { get; set; }
        public byte RuntimeSpriteHeight { get; set; }
        public GBARRR_ActorType ObjectType { get; set; } // Used as index (-1) in function table at 0x0800e86c in ROM
        public byte Runtime_Byte_2B { get; set; }
        public ushort Runtime_Ushort_2C { get; set; }
        public ushort Runtime_Ushort_2E { get; set; }
        public uint Uint_30 { get; set; }
        public uint Runtime_Uint_34 { get; set; }
        public int RuntimeAnimFrame { get; set; }
        public uint RuntimeAnimOffset { get; set; } // Is this correct? Changes with the anim index.

        // Parsed
        public uint P_GraphicsIndex { get; set; }
        public uint P_GraphicsOffset { get; set; }
        public uint P_30 { get; set; }
        public uint P_FrameCount { get; set; }
        public uint P_PaletteIndex { get; set; }
        public uint P_FunctionPointer { get; set; }
        public uint P_Field12 { get; set; }
        public uint P_Field14 { get; set; }
        public uint P_OtherGraphicsOffset { get; set; }
        public uint P_Field20 { get; set; }
        public short P_RuntimeXPos { get; set; }
        public short P_RuntimeAnimFrame { get; set; }
        public short P_Field2E { get; set; }
        public uint P_Field2C { get; set; }
        public short P_Field10 { get; set; }
        public uint P_Field34 { get; set; }
        public uint P_Field0E { get; set; }
        public uint P_Field08 { get; set; }
        public uint P_SpriteHeight { get; set; }
        public uint P_SpriteWidth { get; set; }
        
        public GBARRR_GraphicsBlock GraphicsBlock { get; set; }
        public GBARRR_Palette Palette { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            YPosition = s.Serialize<short>(YPosition, name: nameof(YPosition));
            XPosition = s.Serialize<short>(XPosition, name: nameof(XPosition));
            Data1 = s.SerializeArray<byte>(Data1, 4, name: nameof(Data1));
            Runtime_Ushort_08 = s.Serialize<ushort>(Runtime_Ushort_08, name: nameof(Runtime_Ushort_08));
            Ushort_0A = s.Serialize<ushort>(Ushort_0A, name: nameof(Ushort_0A));
            Ushort_0C = s.Serialize<ushort>(Ushort_0C, name: nameof(Ushort_0C));
            Ushort_0E = s.Serialize<ushort>(Ushort_0E, name: nameof(Ushort_0E));
            Ushort_10 = s.Serialize<ushort>(Ushort_10, name: nameof(Ushort_10));
            RuntimeStateIndex = s.Serialize<ushort>(RuntimeStateIndex, name: nameof(RuntimeStateIndex));
            Runtime_Uint_14 = s.Serialize<uint>(Runtime_Uint_14, name: nameof(Runtime_Uint_14));
            Runtime_Uint_18 = s.Serialize<uint>(Runtime_Uint_18, name: nameof(Runtime_Uint_18));
            RuntimeFunctionPointer = s.Serialize<uint>(RuntimeFunctionPointer, name: nameof(RuntimeFunctionPointer));
            Runtime_Ushort_20 = s.Serialize<ushort>(Runtime_Ushort_20, name: nameof(Runtime_Ushort_20));
            RuntimeXPosition = s.Serialize<short>(RuntimeXPosition, name: nameof(RuntimeXPosition));
            RuntimeYPosition = s.Serialize<short>(RuntimeYPosition, name: nameof(RuntimeYPosition));
            RuntimeAnimIndex = s.Serialize<byte>(RuntimeAnimIndex, name: nameof(RuntimeAnimIndex));
            Runtime_Byte_27 = s.Serialize<byte>(Runtime_Byte_27, name: nameof(Runtime_Byte_27));
            RuntimeSpriteWidth = s.Serialize<byte>(RuntimeSpriteWidth, name: nameof(RuntimeSpriteWidth));
            RuntimeSpriteHeight = s.Serialize<byte>(RuntimeSpriteHeight, name: nameof(RuntimeSpriteHeight));
            ObjectType = s.Serialize<GBARRR_ActorType>(ObjectType, name: nameof(ObjectType));
            Runtime_Byte_2B = s.Serialize<byte>(Runtime_Byte_2B, name: nameof(Runtime_Byte_2B));
            Runtime_Ushort_2C = s.Serialize<ushort>(Runtime_Ushort_2C, name: nameof(Runtime_Ushort_2C));
            Runtime_Ushort_2E = s.Serialize<ushort>(Runtime_Ushort_2E, name: nameof(Runtime_Ushort_2E));
            Uint_30 = s.Serialize<uint>(Uint_30, name: nameof(Uint_30));
            Runtime_Uint_34 = s.Serialize<uint>(Runtime_Uint_34, name: nameof(Runtime_Uint_34));
            RuntimeAnimFrame = s.Serialize<int>(RuntimeAnimFrame, name: nameof(RuntimeAnimFrame));
            RuntimeAnimOffset = s.Serialize<uint>(RuntimeAnimOffset, name: nameof(RuntimeAnimOffset));

            if (!Enum.IsDefined(typeof(GBARRR_ActorType), ObjectType))
                Debug.LogWarning($"Actor type {ObjectType} is not defined");
        }
    }
}