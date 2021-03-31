﻿using BinarySerializer;

namespace R1Engine.Jade {
	public class BIG_FatFile : BinarySerializable {
		public static uint HeaderLength => 0x18;
		
		public uint FilesCount { get; set; }
		public uint DirectoriesCount { get; set; }
		public Pointer FilesOffset { get; set; }
		public int NextFatFileOffset { get; set; }
		public uint UInt_14 { get; set; }
		public uint UInt_18 { get; set; }

		public FileReference[] Files { get; set; }
		public FileInfo[] FileInfos { get; set; }
		public DirectoryInfo[] DirectoryInfos { get; set; }
		public uint MaxEntries { get; set; } // Set in OnPreSerialize
		public uint Version { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			FilesCount = s.Serialize<uint>(FilesCount, name: nameof(FilesCount));
			DirectoriesCount = s.Serialize<uint>(DirectoriesCount, name: nameof(DirectoriesCount));
			FilesOffset = s.SerializePointer(FilesOffset, name: nameof(FilesOffset));
			NextFatFileOffset = s.Serialize<int>(NextFatFileOffset, name: nameof(NextFatFileOffset));
			UInt_14 = s.Serialize<uint>(UInt_14, name: nameof(UInt_14));
			UInt_18 = s.Serialize<uint>(UInt_18, name: nameof(UInt_18));

			s.DoAt(FilesOffset, () => {
				Files = s.SerializeObjectArray<FileReference>(Files, FilesCount, name: nameof(Files));
			});
			s.DoAt(FilesOffset + MaxEntries * FileReference.StructSize, () => {
				FileInfos = s.SerializeObjectArray<FileInfo>(FileInfos, FilesCount, onPreSerialize: fi => fi.Version = Version, name: nameof(FileInfos));
			});
			s.DoAt(FilesOffset + MaxEntries * FileReference.StructSize + MaxEntries * FileInfo.StructSize(Version), () => {
				DirectoryInfos = s.SerializeObjectArray<DirectoryInfo>(DirectoryInfos, DirectoriesCount, name: nameof(DirectoryInfos));
			});
			if (NextFatFileOffset != -1) {
				s.Goto(s.CurrentPointer.File.StartPointer + NextFatFileOffset - HeaderLength);
			}
		}

		public class FileReference : BinarySerializable {
			public static uint StructSize => 0x8;
			public Pointer FileOffset { get; set; }
			public Jade_Key Key { get; set; }
			public bool IsCompressed  => Key.IsCompressed;

			public override void SerializeImpl(SerializerObject s) {
				FileOffset = s.SerializePointer(FileOffset, name: nameof(FileOffset));
				Key = s.SerializeObject<Jade_Key>(Key, name: nameof(Key));
			}
		}
		/// <summary>
		/// File names. Not read by the engine
		/// </summary>
		public class FileInfo : BinarySerializable {
			public static uint StructSize(uint version) => version == 34 ? (uint)0x54 : 0x58;
			public uint Version { get; set; }

			public string Name { get; set; }
			public uint UInt_00 { get; set; }
			public int NextFile { get; set; }
			public int PreviousFile { get; set; }
			public int ParentDirectory { get; set; }
			public uint UInt_10 { get; set; }
			public uint UInt_14 { get; set; }
			public uint UInt_54 { get; set; }
			public override void SerializeImpl(SerializerObject s) {
				bool hasName = false;
				if (s.GetXOR() != null) {
					s.DoXOR(null, () => {
						hasName = s.Serialize<uint>(default, "NameCheck") != 0;
					});
					s.Goto(s.CurrentPointer - 4);
				} else {
					hasName = true;
				}
				if (hasName) {
					UInt_00 = s.Serialize<uint>(UInt_00, name: nameof(UInt_00));
					NextFile = s.Serialize<int>(NextFile, name: nameof(NextFile));
					PreviousFile = s.Serialize<int>(PreviousFile, name: nameof(PreviousFile));
					ParentDirectory = s.Serialize<int>(ParentDirectory, name: nameof(ParentDirectory));
					UInt_10 = s.Serialize<uint>(UInt_10, name: nameof(UInt_10));
					Name = s.SerializeString(Name, 0x40, encoding: Jade_BaseManager.Encoding, name: nameof(Name));
					if (Version != 34) {
						UInt_54 = s.Serialize<uint>(UInt_54, name: nameof(UInt_54));
					}
				} else {
					s.Goto(s.CurrentPointer + StructSize(Version));
				}
			}
		}


		/// <summary>
		/// Directories. Not read by the engine
		/// </summary>
		public class DirectoryInfo : BinarySerializable {
			public static uint StructSize => 0x54;

			public int FirstFileID { get; set; }
			public int FirstDirectoryID { get; set; }
			public int NextDirectory { get; set; }
			public int PreviousDirectory { get; set; }
			public int ParentDirectory { get; set; }
			public string Name { get; set; }
			public override void SerializeImpl(SerializerObject s) {
				FirstFileID = s.Serialize<int>(FirstFileID, name: nameof(FirstFileID));
				FirstDirectoryID = s.Serialize<int>(FirstDirectoryID, name: nameof(FirstDirectoryID));
				NextDirectory = s.Serialize<int>(NextDirectory, name: nameof(NextDirectory));
				PreviousDirectory = s.Serialize<int>(PreviousDirectory, name: nameof(PreviousDirectory));
				ParentDirectory = s.Serialize<int>(ParentDirectory, name: nameof(ParentDirectory));
				Name = s.SerializeString(Name, 0x40, encoding: Jade_BaseManager.Encoding, name: nameof(Name));
			}
		}

	}
}
