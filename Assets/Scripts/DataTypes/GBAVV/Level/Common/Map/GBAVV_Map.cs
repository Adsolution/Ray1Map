﻿namespace R1Engine
{
    public class GBAVV_Map : R1Serializable
    {
        public bool SerializeData { get; set; } = true; // Set before serializing

        public Pointer TilePalettePointer { get; set; }
        public Pointer[] MapLayerPointers { get; set; }
        public Pointer MapCollisionPointer { get; set; }
        public Pointer ObjDataPointer { get; set; }
        public Pointer TileSetsPointer { get; set; }

        // Serialized from pointers

        public RGBA5551Color[] TilePalette { get; set; }
        public GBAVV_MapLayer[] MapLayers { get; set; }
        public GBAVV_Map2D_ObjData ObjData { get; set; }
        public GBAVV_TileSets TileSets { get; set; }
        public GBAVV_MapCollision MapCollision { get; set; }
        public GBAVV_LineCollisionSector LineCollision { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TilePalettePointer = s.SerializePointer(TilePalettePointer, name: nameof(TilePalettePointer));
            MapLayerPointers = s.SerializePointerArray(MapLayerPointers, 4, name: nameof(MapLayerPointers));
            MapCollisionPointer = s.SerializePointer(MapCollisionPointer, name: nameof(MapCollisionPointer));
            ObjDataPointer = s.SerializePointer(ObjDataPointer, name: nameof(ObjDataPointer));
            TileSetsPointer = s.SerializePointer(TileSetsPointer, name: nameof(TileSetsPointer));

            if (!SerializeData)
                return;

            TilePalette = s.DoAt(TilePalettePointer, () => s.SerializeObjectArray<RGBA5551Color>(TilePalette, 256, name: nameof(TilePalette)));

            if (MapLayers == null)
                MapLayers = new GBAVV_MapLayer[MapLayerPointers.Length];

            for (int i = 0; i < MapLayers.Length; i++)
                MapLayers[i] = s.DoAt(MapLayerPointers[i], () => s.SerializeObject<GBAVV_MapLayer>(MapLayers[i], name: $"{nameof(MapLayers)}[{i}]"));

            ObjData = s.DoAt(ObjDataPointer, () => s.SerializeObject<GBAVV_Map2D_ObjData>(ObjData, name: nameof(ObjData)));
            TileSets = s.DoAt(TileSetsPointer, () => s.SerializeObject<GBAVV_TileSets>(TileSets, name: nameof(TileSets)));

            if (s.GameSettings.EngineVersion < EngineVersion.GBAVV_BrotherBear && 
                s.GameSettings.EngineVersion != EngineVersion.GBAVV_ThatsSoRaven && 
                s.GameSettings.EngineVersion != EngineVersion.GBAVV_KidsNextDoorOperationSODA)
                s.DoAt(MapCollisionPointer, () =>
                {
                    s.DoEncoded(new GBA_LZSSEncoder(), () => MapCollision = s.SerializeObject<GBAVV_MapCollision>(MapCollision, name: nameof(MapCollision)));
                });
            else
                LineCollision = s.DoAt(MapCollisionPointer, () => s.SerializeObject<GBAVV_LineCollisionSector>(LineCollision, name: nameof(LineCollision)));
        }
    }
}