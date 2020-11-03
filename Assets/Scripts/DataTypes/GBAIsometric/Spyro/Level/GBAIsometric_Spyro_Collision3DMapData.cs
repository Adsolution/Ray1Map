﻿namespace R1Engine
{
    public class GBAIsometric_Spyro_Collision3DMapData : R1Serializable
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }

        public GBAIsometric_TileCollision[] Collision { get; set; }
        
        public override void SerializeImpl(SerializerObject s)
        {            
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));

            Collision = s.SerializeObjectArray<GBAIsometric_TileCollision>(Collision, Width * Height, name: nameof(Collision));
        }
    }
}