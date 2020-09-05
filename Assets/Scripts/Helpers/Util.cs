﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace R1Engine {
    public static class Util {
        public static bool ByteArrayToFile(string fileName, byte[] byteArray) {
			if (byteArray == null) return false;
            if (FileSystem.mode == FileSystem.Mode.Web) return false;
            try {
				Directory.CreateDirectory(new System.IO.FileInfo(fileName).Directory.FullName);
                using (var fs = new FileStream(fileName, System.IO.FileMode.Create, FileAccess.Write)) {
                    fs.Write(byteArray, 0, byteArray.Length);
                    return true;
                }
            } catch (Exception ex) {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }

		private static readonly string[] SizeSuffixes =
				  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
		public static string SizeSuffix(Int64 value, int decimalPlaces = 1) {
			if (value < 0) { return "-" + SizeSuffix(-value); }

			int i = 0;
			decimal dValue = value;
			while (Math.Round(dValue, decimalPlaces) >= 1000) {
				dValue /= 1024;
				i++;
			}

			return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
		}

		public static string NormalizePath(string path, bool isFolder) {
			string newPath = path.Replace("\\", "/");
			if (isFolder && !newPath.EndsWith("/")) newPath += "/";
			return newPath;
		}

		// For debugging
		public static void ExportPointerArray(SerializerObject s, string path, IEnumerable<Pointer> pointers)
        {
            var p1 = pointers.Where(x => x != null).Distinct().OrderBy(x => x.AbsoluteOffset).ToArray();
            var output = new List<string>();

            for (int i = 0; i < p1.Length - 1; i++)
            {
                var length = p1[i + 1] - p1[i];

                s.DoAt(p1[i], () => output.Add(String.Join(" ", s.SerializeArray<byte>(null, length).Select(x => x.ToString("X2")))));
            }

            File.WriteAllLines(path, output);
        }

		/// <summary>
		/// Convert a byte array to a hex string
		/// </summary>
		/// <param name="Bytes">The byte array to convert</param>
		/// <param name="Align">Should the byte array be split in different lines, this defines the length of one line</param>
		/// <param name="NewLinePrefix">The prefix to add to each new line</param>
		/// <returns></returns>
		public static string ByteArrayToHexString(byte[] Bytes, int? Align = null, string NewLinePrefix = null, int? MaxLines = null) {
			StringBuilder Result = new StringBuilder(Bytes.Length * 2);
			string HexAlphabet = "0123456789ABCDEF";
			int curLine = 0;
			for(int i = 0; i < Bytes.Length; i++) {
				if (i > 0 && Align.HasValue && i % Align == 0) {
					curLine++;
					if (MaxLines.HasValue && curLine >= MaxLines.Value) {
						Result.Append("...");
						return Result.ToString();
					}
					Result.Append("\n" + NewLinePrefix ?? "");
				}
				byte B = Bytes[i];
				Result.Append(HexAlphabet[(int)(B >> 4)]);
				Result.Append(HexAlphabet[(int)(B & 0xF)]);
				if(i < Bytes.Length-1) Result.Append(' ');
			}

			return Result.ToString();
		}

        public static void OutputJSONForWeb(string outputDir)
        {
            foreach (var mode in EnumHelpers.GetValues<GameModeSelection>().Where(x => Settings.GameDirectories.ContainsKey(x) && Directory.Exists(Settings.GameDirectories[x])))
            {
                var s = new GameSettings(mode, Settings.GameDirectories[mode], 0, 0);
                var m = (IGameManager)Activator.CreateInstance(mode.GetAttribute<GameModeAttribute>().ManagerType);

                foreach (var vol in m.GetLevels(s))
                {
                    s.EduVolume = vol.Name;
                    OutputJSONForWeb(Path.Combine(outputDir, $"{mode}{vol.Name}.json"), s);
                }
            }
        }

        public static void OutputJSONForWeb(string outputPath, GameSettings s)
        {
            var manager = s.GetGameManager;
            var attr = s.GameModeSelection.GetAttribute<GameModeAttribute>();
            var settings = s;
            var worlds = manager.GetLevels(settings).First(x => x.Name == null || x.Name == s.EduVolume).Worlds.ToArray();
            var names = MapNames.GetMapNames(attr.Game);

            var lvlWorldIndex = 0;

            var jsonObj = new
            {
                name = attr.DisplayName,
                mode = s.GameModeSelection.ToString(),
                folder = (string)null,
                icons = worlds.Select(x =>
                {
                    var icon = new
                    {
                        image = (string)null,
                        level = lvlWorldIndex
                    };

                    lvlWorldIndex += x.Maps.Length;

                    return icon;
                }),
                levels = worlds.Select(w => w.Maps.OrderBy(x => x).Select(lvl => new
                {
                    world = w.Index,
                    level = lvl,
                    nameInternal = s.MajorEngineVersion == MajorEngineVersion.GBA ? lvl.ToString() : (string)null,
                    name = names?.TryGetItem(w.Index)?.TryGetItem(lvl) ?? (s.MajorEngineVersion == MajorEngineVersion.GBA ? $"Map {lvl}" : $"Map {w.Index}-{lvl}")
                })).SelectMany(x => x)
            };

            JsonHelpers.SerializeToFile(jsonObj, outputPath);
        }

        public static async UniTask EnumerateLevelsAsync(Func<GameSettings, UniTask> action)
        {
            var manager = Settings.GetGameManager;
            var settings = Settings.GetGameSettings;

            foreach (var vol in manager.GetLevels(settings))
            {
                settings.EduVolume = vol.Name;

                foreach (var world in vol.Worlds)
                {
                    settings.World = world.Index;

                    foreach (var map in world.Maps)
                    {
                        settings.Level = map;
                        await action(settings);
                    }
                }
            }
        }

        public static void FindMatchingEncoding(params KeyValuePair<string, byte[]>[] input)
        {
            if (input.Length < 2)
                throw new Exception("Too few strings to check!");

            // Get all possible encodings
            var encodings = Encoding.GetEncodings().Select(x => Encoding.GetEncoding(x.CodePage)).ToArray();

            // Keep a list of all matching ones
            var matches = new List<Encoding>();

            // Helper method for getting all matching encodings
            IEnumerable<Encoding> GetMatches(KeyValuePair<string, byte[]> str)
            {
                var m = encodings.Where(enc => enc.GetString(str.Value).Equals(str.Key, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                Debug.Log($"Matching encodings for {str.Key}: {String.Join(", ", m.Select(x => $"{x.EncodingName} ({x.CodePage})"))}");
                return m;
            }

            // Add matches for the first one
            matches.AddRange(GetMatches(input.First()));

            // Check remaining ones, removing any which don't match
            foreach (var str in input.Skip(1))
            {
                var ma = GetMatches(str);
                matches.RemoveAll(x => !ma.Contains(x));
            }

            // Log the result
            Debug.Log($"Matching encodings for all: {String.Join(", ", matches.Select(x => $"{x.EncodingName} ({x.CodePage})"))}");
        }
    }
}
