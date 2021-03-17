﻿using Cysharp.Threading.Tasks;
using R1Engine.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R1Engine.Jade;
using System.IO;

namespace R1Engine {
	public class Jade_BaseManager : IGameManager {
		public GameAction[] GetGameActions(GameSettings settings) => new GameAction[]
		{
			new GameAction("Extract BF file(s)", false, true, (input, output) => ExtractFilesAsync(settings, output, true)),
		};


        public async UniTask ExtractFilesAsync(GameSettings settings, string outputDir, bool decompressBIN = false) {
            using (var context = new Context(settings)) {
				var s = context.Deserializer;
                await LoadFilesAsync(context);
				var bf = await LoadBF(context);
				try {
					for (int fatIndex = 0; fatIndex < bf.FatFiles.Length; fatIndex++) {
						var fat = bf.FatFiles[fatIndex];
						string[] directories = new string[fat.DirectoryInfos.Length];
						for (int i = 0; i < directories.Length; i++) {
							var dir = fat.DirectoryInfos[i];
							var dirName = dir.Name;
							var curDir = dir;
							while (curDir.ParentDirectory != -1) {
								curDir = fat.DirectoryInfos[curDir.ParentDirectory];
								dirName = Path.Combine(curDir.Name, dirName);
							}
							directories[i] = Path.Combine(outputDir, dirName);
							Directory.CreateDirectory(directories[i]);
						}
						for (int i = 0; i < fat.Files.Length; i++) {
							var f = fat.Files[i];
							byte[] fileBytes = null;
							await bf.SerializeFile(s, fatIndex, i, (fileSize) => {
								fileBytes = s.SerializeArray<byte>(fileBytes, fileSize, name: "FileBytes");
							});
							var fi = fat.FileInfos[i];
							string fileName = null;
							if (fi.Name != null) {
								fileName = fi.Name;
								Util.ByteArrayToFile(Path.Combine(directories[fi.ParentDirectory], fileName), fileBytes);
							} else {
								fileName = $"no_name_{fat.Files[i].Key:X8}.dat";
								Util.ByteArrayToFile(Path.Combine(outputDir, fileName), fileBytes);
							}
						}
					}
				} catch (Exception ex) {
					UnityEngine.Debug.LogError(ex);
				}
            }
        }

        public virtual GameInfo_Volume[] GetLevels(GameSettings settings) => GameInfo_Volume.SingleVolume(new GameInfo_World[]
		{
			new GameInfo_World(0, Enumerable.Range(0, 1).ToArray()),
		});

		public virtual string BFFile => "Rayman4.bf";

		public async UniTask<BIG_BigFile> LoadBF(Context context) {
			var s = context.Deserializer;
			s.Goto(context.GetFile(BFFile).StartPointer);
			await s.FillCacheForRead((int)BIG_BigFile.HeaderLength);
			var bfFile = FileFactory.Read<BIG_BigFile>(BFFile, context);
			await s.FillCacheForRead((int)bfFile.TotalFatFilesLength);
			bfFile.SerializeFatFiles(s);
			return bfFile;
		}

		public async UniTask<Unity_Level> LoadAsync(Context context, bool loadTextures) {
			var bf = await LoadBF(context);
			throw new NotImplementedException();
		}

		public async UniTask LoadFilesAsync(Context context) {
			await context.AddLinearSerializedFileAsync(BFFile, bigFileCacheLength: 8);
		}

		public async UniTask SaveLevelAsync(Context context, Unity_Level level) {
			await Task.CompletedTask;
			throw new NotImplementedException();
		}
	}
}