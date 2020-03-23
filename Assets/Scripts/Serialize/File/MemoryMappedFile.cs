﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R1Engine.Serialize {
	public class MemoryMappedFile : BinaryFile {
		public MemoryMappedFile(Context context, uint baseAddress) : base(context) {
			this.baseAddress = baseAddress;
		}

		public override Pointer StartPointer => new Pointer((uint)baseAddress, this);

		public override Reader CreateReader() {
			Stream s = FileSystem.GetFileReadStream(AbsolutePath);
			length = (uint)s.Length;
			Reader reader = new Reader(s, isLittleEndian: true);
			return reader;
		}

		public override Writer CreateWriter() {
			CreateBackupFile();
			Stream s = FileSystem.GetFileWriteStream(AbsolutePath);
			length = (uint)s.Length;
			Writer writer = new Writer(s, isLittleEndian: true);
			return writer;
		}

		private uint length = 0;
		public uint Length {
			get {
				if (length == 0) {
					Stream s = FileSystem.GetFileReadStream(AbsolutePath);
					length = (uint)s.Length;
					s.Close();
				}
				return length;
			}
			set {
				length = value;
			}
		}

		public virtual Pointer GetPointerInThisFileOnly(uint serializedValue, Pointer anchor = null) {
			uint anchorOffset = anchor?.AbsoluteOffset ?? 0;
			if (serializedValue + anchorOffset >= baseAddress && serializedValue + anchorOffset < baseAddress + Length) {
				return new Pointer(serializedValue, this, anchor: anchor);
			}
			return null;
		}

		public override Pointer GetPointer(uint serializedValue, Pointer anchor = null) {
			Pointer ptr = GetPointerInThisFileOnly(serializedValue, anchor: anchor);
			if (ptr != null) return ptr;
			foreach (BinaryFile f in Context.MemoryMap.Files) {
				if (f is MemoryMappedFile && f != this) {
					Pointer p = ((MemoryMappedFile)f).GetPointerInThisFileOnly(serializedValue, anchor: anchor);
					if (p != null) return p;
				}
			}
			return null;
		}
	}
}
