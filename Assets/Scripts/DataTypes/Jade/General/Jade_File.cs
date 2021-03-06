﻿using BinarySerializer;
using System;

namespace R1Engine.Jade {
	public abstract class Jade_File : BinarySerializable {
		public LOA_Loader Loader { get; set; }
		public Jade_Key Key { get; set; }
		public uint FileSize { get; set; }
		public bool UnknownFileSize { get; set; }
		public uint ReferencesCount { get; set; } = 1;
		public uint CachedCount { get; set; } = 1;
		public LOA_BinFileHeader BinFileHeader { get; set; }

		protected override void OnPostSerialize(SerializerObject s) {
			CheckFileSize(s);
		}

		public virtual T ConvertType<T>() where T : Jade_File {
			return (T)this; // Override this method for more complex types
		}
		public void CheckFileSize(SerializerObject s) {
			long readSize = s.CurrentPointer - Offset;
			if (Loader.IsBinaryData && FileSize != readSize && !UnknownFileSize) {
				UnityEngine.Debug.LogWarning($"File {Key} with type {GetType()} was not fully serialized: File Size: {FileSize:X8} / Serialized: {readSize:X8}");
			} else if(UnknownFileSize) FileSize = (uint)readSize;
		}
	}
}
