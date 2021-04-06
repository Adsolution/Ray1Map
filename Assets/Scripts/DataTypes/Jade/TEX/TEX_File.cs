﻿using System;
using BinarySerializer;

namespace R1Engine.Jade 
{
	// See: GDI_l_AttachWorld & TEX_l_File_Read
	// TEX_l_File_Read reads the texture header, after that GDI_l_AttachWorld resolves some references (for animated textures) and adds palette references
	public class TEX_File : Jade_File 
    {
		public uint Uint_00 { get; set; } // Always 0xFFFFFFFF in files
		public ushort Ushort_04 { get; set; }
		public TexFileFormat FileFormat { get; set; }
		public byte Byte_07 { get; set; }
		public ushort Ushort_08 { get; set; }
		public ushort Ushort_0A { get; set; }
		public uint Uint_0C { get; set; }
		public uint Uint_10 { get; set; }
		public uint Uint_14 { get; set; } // Usually CAD01234
        public uint Uint_18 { get; set; } // Checked for 0xFF00FF
        public uint Uint_1C { get; set; } // Checked for 0xC0DEC0DE

        public override void SerializeImpl(SerializerObject s) 
        {
			if (!Loader.IsBinaryData)
				s.Goto(s.CurrentPointer + FileSize - 32);

			Uint_00 = s.Serialize<uint>(Uint_00, name: nameof(Uint_00));
			Ushort_04 = s.Serialize<ushort>(Ushort_04, name: nameof(Ushort_04));
            FileFormat = s.Serialize<TexFileFormat>(FileFormat, name: nameof(FileFormat));
            Byte_07 = s.Serialize<byte>(Byte_07, name: nameof(Byte_07));
            Ushort_08 = s.Serialize<ushort>(Ushort_08, name: nameof(Ushort_08));
            Ushort_0A = s.Serialize<ushort>(Ushort_0A, name: nameof(Ushort_0A));
            Uint_0C = s.Serialize<uint>(Uint_0C, name: nameof(Uint_0C));
            Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
            Uint_14 = s.Serialize<uint>(Uint_14, name: nameof(Uint_14));
            Uint_18 = s.Serialize<uint>(Uint_18, name: nameof(Uint_18));
            Uint_1C = s.Serialize<uint>(Uint_1C, name: nameof(Uint_1C));

            if (!Loader.IsBinaryData)
                s.Goto(Offset);
        }

        public enum TexFileFormat : byte
        {
            Tga = 1,
            Bmp = 2,
            Jpeg = 3,
            SpriteGen = 4,
            Procedural = 5,
            Raw_1 = 6,
            Raw_2 = 7,
            Animated = 9
        }
	}
}