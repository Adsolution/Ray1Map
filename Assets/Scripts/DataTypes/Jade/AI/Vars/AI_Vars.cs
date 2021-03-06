﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinarySerializer;

namespace R1Engine.Jade {
	public class AI_Vars : Jade_File {
		public uint RewindVarEndOffset { get; set; }
		public bool HasRewindZones { get; set; }
		public AI_Vars_RewindZone[] RewindZones { get; set; }

		public uint VarInfosBufferSize { get; set; }
		public AI_VarInfo[] VarInfos { get; set; }
		public uint NameBufferSize { get; set; }
		public AI_VarName[] Names { get; set; }
		public uint VarEditorInfoBufferSize { get; set; }
		public int VarEditorInfoStringBufferSize { get; set; }
		public AI_VarEditorInfo[] VarEditorInfos { get; set; }
		public uint VarValueBufferSize { get; set; }
		public AI_VarValue[] Values { get; set; }

		public Jade_Reference<AI_Function>[] Functions { get; set; }
		public uint ExtraFunctionsCount { get; set; }
		public Jade_Reference<AI_Function>[] ExtraFunctions { get; set; }

		// Custom
		public AI_Var[] Vars { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			if (s.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_Montreal)) {
				if (s.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_PoP_T2T_20051002)) {
					s.SerializeBitValues<int>(bitFunc => {
						RewindVarEndOffset = (uint)bitFunc((int)RewindVarEndOffset, 31, name: nameof(RewindVarEndOffset));
						HasRewindZones = bitFunc(HasRewindZones ? 1 : 0, 1, name: nameof(HasRewindZones)) == 1;
					});
					if (HasRewindZones) RewindZones = s.SerializeObjectArray<AI_Vars_RewindZone>(RewindZones, RewindVarEndOffset, name: nameof(RewindZones));
				} else {
					RewindVarEndOffset = s.Serialize<uint>(RewindVarEndOffset, name: nameof(RewindVarEndOffset));
				}
			}

			// Normal var data
			VarInfosBufferSize = s.Serialize<uint>(VarInfosBufferSize, name: nameof(VarInfosBufferSize));
			VarInfos = s.SerializeObjectArray<AI_VarInfo>(VarInfos, VarInfosBufferSize / 12, name: nameof(VarInfos));

			NameBufferSize = s.Serialize<uint>(NameBufferSize, name: nameof(NameBufferSize));
			if (NameBufferSize > 0) {
				Names = s.SerializeObjectArray<AI_VarName>(Names, VarInfos.Length, name: nameof(Names));
			}

			// Generate main Vars array
			Vars = new AI_Var[VarInfos.Length];
			for (int i = 0; i < Vars.Length; i++) {
				Vars[i] = new AI_Var() {
					Index = i,
					Info = VarInfos[i],
					Name = Names?[i]?.Name,
				};
				Vars[i].Init();
			}

			// Editor var data
			if (!Loader.IsBinaryData) {
				VarEditorInfoBufferSize = s.Serialize<uint>(VarEditorInfoBufferSize, name: nameof(VarEditorInfoBufferSize));
				VarEditorInfoStringBufferSize = s.Serialize<int>(VarEditorInfoStringBufferSize, name: nameof(VarEditorInfoStringBufferSize));
				if (VarEditorInfoBufferSize > 0) {
					VarEditorInfos = s.SerializeObjectArray<AI_VarEditorInfo>(VarEditorInfos, VarEditorInfoBufferSize / 0x14, name: nameof(VarEditorInfos));
					for (int i = 0; i < VarEditorInfos.Length; i++) {
						var var = VarEditorInfos[i];
						s.Log($"Strings for {nameof(VarEditorInfos)}[{i}]");
						var.SerializeStrings(s);

						var match = Vars.FirstOrDefault(v => v.Info.BufferOffset == var.BufferOffset);
						if (match != null) match.EditorInfo = var;
					}
				}
			}

			// Var values
			VarValueBufferSize = s.Serialize<uint>(VarValueBufferSize, name: nameof(VarValueBufferSize));
			var sortedVars = Vars.OrderBy(v => v.Info.BufferOffset).ToArray();
			if(Values == null) Values = new AI_VarValue[sortedVars.Length];
			for(int i = 0; i < Values.Length; i++) {
				var variable = sortedVars[i];
				Values[i] = s.SerializeObject<AI_VarValue>(Values[i], onPreSerialize: v => v.Var = variable, name: $"{nameof(Values)}[{i}]");
				
				variable.Value = Values[i];
			}

			if(Functions == null) Functions = new Jade_Reference<AI_Function>[5];
			for (int i = 0; i < Functions.Length; i++) {
				Functions[i] = s.SerializeObject<Jade_Reference<AI_Function>>(Functions[i], name: $"{nameof(Functions)}[{i}]")?.Resolve();
			}

			if (!s.GetR1Settings().EngineVersionTree.HasParent(EngineVersion.Jade_KingKong)) {
				if (s.CurrentPointer.AbsoluteOffset < (Offset + FileSize).AbsoluteOffset) {
					ExtraFunctionsCount = s.Serialize<uint>(ExtraFunctionsCount, name: nameof(ExtraFunctionsCount));
					ExtraFunctions = s.SerializeObjectArray<Jade_Reference<AI_Function>>(ExtraFunctions, ExtraFunctionsCount, name: nameof(ExtraFunctions));
					foreach (var extraFunction in ExtraFunctions) {
						extraFunction?.Resolve();
					}
				}
			}
			//ExportVarsOverview(s);
		}

		public void ExportVarsOverview(SerializerObject s) {
			StringBuilder b = new StringBuilder();
			b.AppendLine($"VARS COUNT: {Vars.Length}");

			for (int i = 0; i < Vars.Length; i++) {
				b.AppendLine($"Vars[{i}]: {Vars[i].Name}" +
					$"\n\t\tDescription: {Vars[i].EditorInfo?.Description?.Trim() ?? "null"}" +
					$"\n\t\tToggle text: {Vars[i].EditorInfo?.SelectionString?.Trim() ?? "null"}" +
					$"\n\t\tValue: {Vars[i].Value}" +
					$"\n\t\tBuffer value offset: {Vars[i].Info.BufferOffset:X8}" +
					$"\n\t\tBF Value offset: {Vars[i].Value?.Offset}" +
					$"\n\t\tValue type: {Vars[i].Type} ({Vars[i].Link.Key})" +
					$"\n\t\tValue element size: {Vars[i].Link.Size}" +
					$"\n\t\tValue count: {Vars[i].Info.ArrayLength}" +
					$"\n\t\tValue dimensions count: {Vars[i].Info.ArrayDimensionsCount}" +
					$"\n\t\tVariable flags: {Vars[i].Info.Flags:X4}" +
					$"\n\t\tCopy to instance buffer: {(Vars[i].Info.Flags & 0x20) != 0}");
			}
			string basePath = Context.BasePath;
			string path = basePath + "vars/" + Key + "_" + s.GetR1Settings().Platform + ".vardec";
			System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
			System.IO.File.WriteAllText(path, b.ToString());

		}
	}
}
