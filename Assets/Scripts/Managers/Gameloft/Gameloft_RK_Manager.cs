﻿using Cysharp.Threading.Tasks;
using R1Engine.Serialize;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace R1Engine
{
	public class Gameloft_RK_Manager : Gameloft_BaseManager {
		public override string[] ResourceFiles => new string[] {
			"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
		};

		public virtual string GetLevelPath(GameSettings settings) => "20";
		public virtual string GetRoadTexturesPath(GameSettings settings) => "2";
		public virtual int GetLevelResourceIndex(GameSettings settings) => settings.Level;

		public override string[] SingleResourceFiles => new string[] {
			"s"
		};

		public override GameInfo_Volume[] GetLevels(GameSettings settings) => GameInfo_Volume.SingleVolume(new GameInfo_World[]
		{
			new GameInfo_World(0, Enumerable.Range(0, 16).ToArray()),
		});

		public override async UniTask LoadFilesAsync(Context context) {
			await context.AddLinearSerializedFileAsync(GetLevelPath(context.Settings));
			await context.AddLinearSerializedFileAsync(GetRoadTexturesPath(context.Settings));
			foreach (var fileIndex in Enumerable.Range(0, PuppetCount).Select(i => GetPuppetFileIndex(i)).Distinct()) {
				await context.AddLinearSerializedFileAsync(fileIndex.ToString());
			}
		}

		public virtual int BasePuppetsResourceFile => 10;
		public virtual int PuppetsPerResourceFile => 15;

		public virtual int GetPuppetFileIndex(int puppetIndex) => BasePuppetsResourceFile + puppetIndex / PuppetsPerResourceFile;
		public virtual int GetPuppetResourceIndex(int puppetIndex) => puppetIndex % PuppetsPerResourceFile;
		public virtual PuppetReference[] PuppetReferences => Enumerable.Range(0,PuppetCount-2).Select(pi => new PuppetReference() {
			FileIndex = GetPuppetFileIndex(pi),
			ResourceIndex = GetPuppetResourceIndex(pi)
		}).Append(new PuppetReference() {
			FileIndex = GetPuppetFileIndex(PuppetCount-3),
			ResourceIndex = GetPuppetResourceIndex(PuppetCount-3) + 1
		}).Append(new PuppetReference() {
			FileIndex = GetPuppetFileIndex(PuppetCount - 3),
			ResourceIndex = GetPuppetResourceIndex(PuppetCount - 3) + 2
		}).ToArray();
		public virtual int PuppetCount => 62;

		public class PuppetReference {
			public int FileIndex { get; set; }
			public int ResourceIndex { get; set; }
		}

		public virtual Unity_ObjectManager_GameloftRK.PuppetData[] LoadPuppets(Context context) {

			var s = context.Deserializer;
			var refs = PuppetReferences;
			Gameloft_Puppet[] puppets = new Gameloft_Puppet[refs.Length];
			Unity_ObjectManager_GameloftRK.PuppetData[] models = new Unity_ObjectManager_GameloftRK.PuppetData[refs.Length];
			for (int i = 0; i < refs.Length; i++) {
				int fileIndex = refs[i].FileIndex;
				int resIndex = refs[i].ResourceIndex;
				var resf = FileFactory.Read<Gameloft_ResourceFile>(fileIndex.ToString(), context);
				puppets[i] = resf.SerializeResource<Gameloft_Puppet>(s, default, resIndex, name: $"Puppets[{i}]");
				models[i] = new Unity_ObjectManager_GameloftRK.PuppetData(i, fileIndex, resIndex, GetCommonDesign(puppets[i]));
			}
			return models;
		}

		public class MeshInProgress {
			public List<Vector3> vertices = new List<Vector3>();
			public List<Vector3> normals = new List<Vector3>();
			public List<Vector2> uvs = new List<Vector2>();
			public List<Color> colors = new List<Color>();
			public List<int> triangles = new List<int>();
		}

		public void CreateTrackMesh(Gameloft_RK_Level level, Context context) {
			// Load road textures
			var resf = FileFactory.Read<Gameloft_ResourceFile>(GetRoadTexturesPath(context.Settings), context);
			var roadTex0 = resf.SerializeResource<Gameloft_Puppet>(context.Deserializer, default, level.RoadTextureID_0, name: $"RoadTexture0");
			var roadTex1 = resf.SerializeResource<Gameloft_Puppet>(context.Deserializer, default, level.RoadTextureID_1, name: $"RoadTexture1");


			Vector3 curPos = Vector3.zero;
			float curAngle = 0f;
			float curHeight = 0f;
			float roadSize = 1f;
			float groundSize = 2f;
			float groundDisplacement = -0.2f;
			var heightMultiplier = 0.025f;

			var t = level.TrackBlocks;

			// Road
			var road0 = new MeshInProgress();
			var road1 = new MeshInProgress();
			var bridge = new MeshInProgress();
			MeshInProgress[] roadMeshes = new MeshInProgress[] { road0, road1, bridge };

			// Ground
			Vector3[] verticesGround = new Vector3[(t.Length) * 8];
			Vector3[] normalsGround = new Vector3[verticesGround.Length];
			Vector2[] uvsGround = new Vector2[verticesGround.Length];
			Color[] colorsGround = new Color[verticesGround.Length];
			int[] trianglesGround = new int[(t.Length) * 12];

			for (int i = 0; i < t.Length; i++) {
				var curBlock = t[i];
				float roadWidthFactor = ((level.Types[curBlock.Type].Flags & 1) == 1 ? level.Types[curBlock.Type].Short2 : 1000) / 1000f;
				float roadSizeCur = roadSize * roadWidthFactor;
				bool isBridge = (level.Types[curBlock.Type].Flags & 1) == 1;
				var road = isBridge ? bridge : i % 2 == 0 ? road0 : road1;
				int roadVertexCount = road.vertices.Count;
				Quaternion q = Quaternion.Euler(0, curAngle, 0);
				var curPosH = curPos + Vector3.up * curHeight;
				road.vertices.Add(curPosH + q * Vector3.left * roadSizeCur);
				road.vertices.Add(curPosH + q * Vector3.right * roadSizeCur);
				road.vertices.Add(curPosH + q * Vector3.left * roadSizeCur);
				road.vertices.Add(curPosH + q * Vector3.right * roadSizeCur);

				verticesGround[(i * 8) + 0] = curPosH + q * Vector3.left * groundSize;
				verticesGround[(i * 8) + 1] = curPosH + q * Vector3.right * groundSize;
				verticesGround[(i * 8) + 2] = curPosH + q * Vector3.left * groundSize;
				verticesGround[(i * 8) + 3] = curPosH + q * Vector3.right * groundSize;

				curPos += q * Vector3.forward;
				curHeight += curBlock.DeltaHeight * heightMultiplier;
				curAngle -= curBlock.DeltaRotation;

				q = Quaternion.Euler(0, curAngle, 0);
				curPosH = curPos + Vector3.up * curHeight;
				road.vertices.Add(curPosH + q * Vector3.left * roadSizeCur);
				road.vertices.Add(curPosH + q * Vector3.right * roadSizeCur);
				road.vertices.Add(curPosH + q * Vector3.left * roadSizeCur);
				road.vertices.Add(curPosH + q * Vector3.right * roadSizeCur);

				verticesGround[(i * 8) + 4] = curPosH + q * Vector3.left * groundSize;
				verticesGround[(i * 8) + 5] = curPosH + q * Vector3.right * groundSize;
				verticesGround[(i * 8) + 6] = curPosH + q * Vector3.left * groundSize;
				verticesGround[(i * 8) + 7] = curPosH + q * Vector3.right * groundSize;

				// Up
				road.triangles.Add(roadVertexCount + 0);
				road.triangles.Add(roadVertexCount + 4);
				road.triangles.Add(roadVertexCount + 1);
				road.triangles.Add(roadVertexCount + 1);
				road.triangles.Add(roadVertexCount + 4);
				road.triangles.Add(roadVertexCount + 5);
				// Down
				road.triangles.Add(roadVertexCount + 2);
				road.triangles.Add(roadVertexCount + 3);
				road.triangles.Add(roadVertexCount + 7);
				road.triangles.Add(roadVertexCount + 2);
				road.triangles.Add(roadVertexCount + 7);
				road.triangles.Add(roadVertexCount + 6);

				// Up
				trianglesGround[(i * 12) + 0] = (i * 8) + 0;
				trianglesGround[(i * 12) + 1] = (i * 8) + 4;
				trianglesGround[(i * 12) + 2] = (i * 8) + 1;
				trianglesGround[(i * 12) + 3] = (i * 8) + 1;
				trianglesGround[(i * 12) + 4] = (i * 8) + 4;
				trianglesGround[(i * 12) + 5] = (i * 8) + 5;
				// Down
				trianglesGround[(i * 12) + 6] = (i * 8) + 2;
				trianglesGround[(i * 12) + 7] = (i * 8) + 3;
				trianglesGround[(i * 12) + 8] = (i * 8) + 7;
				trianglesGround[(i * 12) + 9] = (i * 8) + 2;
				trianglesGround[(i * 12) + 10] = (i * 8) + 7;
				trianglesGround[(i * 12) + 11] = (i * 8) + 6;

				// Test colors
				var roadCol = isBridge ? (level.Color_dk_BridgeLight ?? level.Types[curBlock.Type].ColorGround).GetColor() : Color.white;
				road.colors.Add(roadCol);
				road.colors.Add(roadCol);
				road.colors.Add(Color.grey);
				road.colors.Add(Color.grey);

				road.colors.Add(roadCol);
				road.colors.Add(roadCol);
				road.colors.Add(Color.grey);
				road.colors.Add(Color.grey);

				// UVs
				road.uvs.Add(new Vector2(0, 1));
				road.uvs.Add(new Vector2(2, 1));
				road.uvs.Add(new Vector2(0, 1));
				road.uvs.Add(new Vector2(2, 1));
				road.uvs.Add(new Vector2(0, 0));
				road.uvs.Add(new Vector2(2, 0));
				road.uvs.Add(new Vector2(0, 0));
				road.uvs.Add(new Vector2(2, 0));

				// Ground colors
				var groundCol = (level.Types[curBlock.Type].Flags & 1) == 1 ? level.Types[curBlock.Type].Color8.GetColor() : level.Types[curBlock.Type].ColorGround.GetColor();
				colorsGround[(i * 8) + 0] = groundCol;
				colorsGround[(i * 8) + 1] = groundCol;
				colorsGround[(i * 8) + 4] = groundCol;
				colorsGround[(i * 8) + 5] = groundCol;
				colorsGround[(i * 8) + 2] = groundCol;
				colorsGround[(i * 8) + 3] = groundCol;
				colorsGround[(i * 8) + 6] = groundCol;
				colorsGround[(i * 8) + 7] = groundCol;

				// Normals
				//normalsRoad[(i * 8) + 0] =
			}

			GameObject gaoParent = new GameObject();
			gaoParent.transform.position = Vector3.zero;

			// Road
			foreach (var road in roadMeshes) {
				Mesh roadMesh = new Mesh();
				roadMesh.SetVertices(road.vertices);
				roadMesh.SetTriangles(road.triangles, 0);
				roadMesh.SetColors(road.colors);
				roadMesh.SetUVs(0, road.uvs);
				roadMesh.RecalculateNormals();
				GameObject gao = new GameObject("Road mesh");
				MeshFilter mf = gao.AddComponent<MeshFilter>();
				MeshRenderer mr = gao.AddComponent<MeshRenderer>();
				gao.layer = LayerMask.NameToLayer("3D Collision");
				gao.transform.SetParent(gaoParent.transform);
				gao.transform.localScale = Vector3.one;
				gao.transform.localPosition = Vector3.zero;
				mf.mesh = roadMesh;
				mr.material = Controller.obj.levelController.controllerTilemap.unlitMaterial;
				if (road != bridge) {
					Texture2D tex = null;
					if (road == road0) tex = GetPuppetImages(roadTex0)?[0][0];
					if (road == road1) tex = GetPuppetImages(roadTex1)?[0][0];
					if (tex != null) {
						tex.wrapModeU = TextureWrapMode.Mirror;
						mr.material.SetTexture("_MainTex", tex);
					}
				}
			}

			// Ground
			Mesh groundMesh = new Mesh();
			groundMesh.vertices = verticesGround;
			groundMesh.triangles = trianglesGround;
			groundMesh.colors = colorsGround;
			groundMesh.RecalculateNormals();
			GameObject g_gao = new GameObject("Ground mesh");
			MeshFilter g_mf = g_gao.AddComponent<MeshFilter>();
			MeshRenderer g_mr = g_gao.AddComponent<MeshRenderer>();
			g_gao.layer = LayerMask.NameToLayer("3D Collision");
			g_gao.transform.SetParent(gaoParent.transform);
			g_gao.transform.localScale = Vector3.one;
			g_gao.transform.localPosition = Vector3.zero + Vector3.up * groundDisplacement;
			g_mf.mesh = groundMesh;
			g_mr.material = Controller.obj.levelController.controllerTilemap.unlitMaterial;

			gaoParent.transform.localScale = Vector3.one * 8;

		}

		public Mesh GetObject3DMesh(Gameloft_RK_Level.Object3D o) {
			Mesh m = new Mesh();
			Color currentColor = Color.white;
			int curCount = 0;
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			List<Color> colors = new List<Color>();
			Vector3 pt0, pt1, pt2;
			int curTri = 0;


			foreach (var c in o.Commands) {
				switch (c.Type) {
					case Gameloft_RK_Level.Object3D.Command.CommandType.Color:
						currentColor = c.Color.GetColor();
						break;
					case Gameloft_RK_Level.Object3D.Command.CommandType.DrawTriangle:
						pt0 = new Vector3(c.Positions[0].X, c.Positions[0].Y, curTri);
						pt1 = new Vector3(c.Positions[1].X, c.Positions[1].Y, curTri);
						pt2 = new Vector3(c.Positions[2].X, c.Positions[2].Y, curTri);
						vertices.Add(pt0 / 1000f);
						vertices.Add(pt2 / 1000f);
						vertices.Add(pt1 / 1000f);
						//Controller.print(expectedNormal);
						colors.Add(currentColor);
						colors.Add(currentColor);
						colors.Add(currentColor);

						// Clockwise winding order
						Vector3 expectedNormal = Vector3.Cross(pt1 - pt0, pt2 - pt1);
						if (expectedNormal.z >= 0) {
							triangles.Add(curCount);
							triangles.Add(curCount + 1);
							triangles.Add(curCount + 2);
						} else {
							triangles.Add(curCount);
							triangles.Add(curCount + 2);
							triangles.Add(curCount + 1);
						}
						curCount += 3;
						curTri -= 1;
						break;
					case Gameloft_RK_Level.Object3D.Command.CommandType.DrawLine:
						pt0 = new Vector3(c.Positions[0].X, c.Positions[0].Y, curTri);
						pt1 = new Vector3(c.Positions[1].X, c.Positions[1].Y, curTri);
						if (c.Positions[1].Y > 0 && BitHelpers.ExtractBits(c.Positions[1].Y, 1, 14) == 1) {
							var newY = BitHelpers.ExtractBits(c.Positions[1].Y, 14, 0);
							pt1 = new Vector3(c.Positions[1].X, newY, curTri + 10000);
						}
						var diff = pt1 - pt0;
						var lineThickness = (Quaternion.Euler(0, 0, 90) * diff).normalized * 5;
						if (lineThickness.x == 0 && lineThickness.y == 0) {
							lineThickness = new Vector3(1,1,0) * 5f;
						}
						vertices.Add((pt0 - lineThickness) / 1000f);
						vertices.Add((pt0 + lineThickness) / 1000f);
						vertices.Add((pt1 - lineThickness) / 1000f);
						vertices.Add((pt1 + lineThickness) / 1000f);
						colors.Add(currentColor);
						colors.Add(currentColor);
						colors.Add(currentColor);
						colors.Add(currentColor);
						triangles.Add(curCount);
						triangles.Add(curCount + 1);
						triangles.Add(curCount + 2);
						triangles.Add(curCount + 1);
						triangles.Add(curCount + 3);
						triangles.Add(curCount + 2);
						curTri -= 1;
						curCount += 4;
						break;
					case Gameloft_RK_Level.Object3D.Command.CommandType.DrawRectangle:
						vertices.Add(new Vector3(c.Rectangle.X, c.Rectangle.Y, curTri) / 1000f);
						vertices.Add(new Vector3(c.Rectangle.X, c.Rectangle.Y + c.Rectangle.Height, curTri) / 1000f);
						vertices.Add(new Vector3(c.Rectangle.X + c.Rectangle.Width, c.Rectangle.Y, curTri) / 1000f);
						vertices.Add(new Vector3(c.Rectangle.X + c.Rectangle.Width, c.Rectangle.Y + c.Rectangle.Height, curTri) / 1000f);
						colors.Add(currentColor);
						colors.Add(currentColor);
						colors.Add(currentColor);
						colors.Add(currentColor);
						triangles.Add(curCount);
						triangles.Add(curCount + 1);
						triangles.Add(curCount + 2);
						triangles.Add(curCount + 1);
						triangles.Add(curCount + 3);
						triangles.Add(curCount + 2);
						curCount += 4;
						curTri -= 1;
						break;
				}
			}
			m.SetVertices(vertices);
			m.SetColors(colors);
			m.SetTriangles(triangles, 0);
			m.RecalculateNormals();
			return m;
		}



		public override async UniTask<Unity_Level> LoadAsync(Context context, bool loadTextures) {
			await UniTask.CompletedTask;
			var resf = FileFactory.Read<Gameloft_ResourceFile>(GetLevelPath(context.Settings), context);
			var ind = GetLevelResourceIndex(context.Settings);
			var level = resf.SerializeResource<Gameloft_RK_Level>(context.Deserializer, default, ind, name: $"Level_{ind}");

			CreateTrackMesh(level, context);

			// Load objects
			Mesh[] meshes = level.Objects3D.Select(o => GetObject3DMesh(o)).ToArray();
			var objManager = new Unity_ObjectManager_GameloftRK(context, LoadPuppets(context));
			List<Unity_Object> objs = new List<Unity_Object>();

			UnityEngine.Debug.Log("Sum rotation: " + level.TrackBlocks.Sum(o => o.DeltaRotation));
			UnityEngine.Debug.Log("Sum height: " + level.TrackBlocks.Sum(o => o.DeltaHeight));
			Vector3 curPos = Vector3.zero;
			float curAngle = 0f;
			float curHeight = 0f;
			int curBlockIndex = 0;
			GameObject gaoParent = new GameObject();
			gaoParent.transform.position = Vector3.zero;
			gaoParent.transform.localRotation = Quaternion.identity;
			gaoParent.transform.localScale = Vector3.one * 8;
			var heightMultiplier = 0.025f;
			foreach (var o in level.TrackBlocks) {
				var sphere = new GameObject();//GameObject.CreatePrimitive(PrimitiveType.Cube);
				sphere.transform.position = curPos + Vector3.up * curHeight;
				sphere.transform.rotation = Quaternion.Euler(0, curAngle, 0);

				var lumsForCurrentBlock = level.Lums?.Where(s12 => s12.TrackBlockIndex == curBlockIndex);
				if (lumsForCurrentBlock != null) {
					foreach (var lum in lumsForCurrentBlock) {
						var pos = sphere.transform.TransformPoint(new Vector3(lum.XPosition * 0.001f, 0.05f, 0));
						// TODO: Add Lum object here. As waypoint? As lum/coin (load puppet first)?
						// Maybe have a system for objects with custom puppets, so player puppets can be displayed? 
						/*objs.Add(new Unity_Object_GameloftRK(objManager, s8, level.ObjectTypes[s8.ObjectType]) {
							Position = new Vector3(pos.x, -pos.z, pos.y),
							Instance = blk
						});*/
					}
				}
				var cj = level.TrackObjectCollections[curBlockIndex];
				for (int i = 0; i < cj.Count; i++) {
					var ci_ind = cj.InstanceIndex + i;
					var blk = level.TrackObjectInstances[ci_ind];
					var s8Ind = blk.TrackObjectIndex;
					var s8 = level.TrackObjects[s8Ind];
					var type = level.ObjectTypes[s8.ObjectType];
					var pos = sphere.transform.TransformPoint(new Vector3(s8.XPosition * 0.001f, 0.05f + type.YPosition * 0.001f, 0));
					if(blk.ObjType == 4) continue;
					// TODO: Create obj types 4. These are hardcoded it seems.
					// Usually they don't show up, but if Byte2 == 2, they show up as speed boosts
					if (blk.ObjType == 5) {
						GameObject gp = new GameObject();
						gp.transform.position = Vector3.zero;
						var m = meshes[blk.TrackObjectIndex];
						GameObject gao = new GameObject();
						MeshFilter mf = gao.AddComponent<MeshFilter>();
						MeshRenderer mr = gao.AddComponent<MeshRenderer>();
						gao.layer = LayerMask.NameToLayer("3D Collision");
						gao.transform.SetParent(gp.transform);
						gao.transform.localScale = new Vector3(blk.FlipX ? -1 : 1, 1, 0.1f);
						gao.transform.localRotation = Quaternion.Euler(0, curAngle, 0);
						gao.transform.localPosition = sphere.transform.position;
						mf.mesh = m;
						mr.material = Controller.obj.levelController.controllerTilemap.unlitMaterial;
						gp.transform.localScale = Vector3.one * 8;
						gp.transform.name = s8.ObjectType.ToString();
					} else {
						objs.Add(new Unity_Object_GameloftRK(objManager, s8, type) {
							Position = new Vector3(pos.x, -pos.z, pos.y),
							Instance = blk
						});
					}
				}

				curPos += Quaternion.Euler(0,curAngle,0) * Vector3.forward;
				curHeight += o.DeltaHeight * heightMultiplier;
				curAngle -= o.DeltaRotation;
				sphere.gameObject.name = $"{curBlockIndex}: {o.Type} | {o.Flags} | {o.Unknown} | {level.Types[o.Type].Flags}";
				sphere.gameObject.transform.SetParent(gaoParent.transform);
				curBlockIndex++;
			}

			curPos = Vector3.zero + Vector3.up * 100;
			//curAngle = 0;
			for(int i = 0; i < level.MapSpriteMapping.Length; i++) {
				var b = level.MapSpriteMapping[i];
				var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.transform.position = curPos;
				var v = b.Vector2;

				curPos += new Vector3(v.x,0,-v.y) / 2f;

				/*UnityEngine.Random.InitState((int)o.Unknown);
				var color = UnityEngine.Random.ColorHSV(0, 1, 0.2f, 1f, 0.8f, 1.0f);*/
				curBlockIndex++;
			}

			// Load objects
			var unityObjs = objs;

			// Return level
			return new Unity_Level(
				maps: new Unity_Map[] {
					new Unity_Map() {
						TileSet = new Unity_TileSet[] {
							new Unity_TileSet(8)
						},
						
					}
				},
				objManager: objManager,
				isometricData: new Unity_IsometricData {
					CollisionWidth = 0,
					CollisionHeight = 0,
					TilesWidth = 38,
					TilesHeight = 24,
					Collision = null,
					Scale = Vector3.one * 8,
					ViewAngle = Quaternion.Euler(90,0,0),
					CalculateYDisplacement = () => 0,
					CalculateXDisplacement = () => 0,
					ObjectScale = Vector3.one,
				},
				eventData: unityObjs,
				//localization: LoadLocalization(context),
				defaultMap: 0,
				defaultCollisionMap: 0,
				cellSize: 8);

			///throw new NotImplementedException();
		}
	}
}