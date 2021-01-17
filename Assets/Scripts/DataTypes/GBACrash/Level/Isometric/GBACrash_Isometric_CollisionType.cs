﻿namespace R1Engine
{
    public class GBACrash_Isometric_CollisionType : R1Serializable
    {
        public Pointer FunctionPointer_0 { get; set; }
        public Pointer FunctionPointer_1 { get; set; }
        public Pointer FunctionPointer_2 { get; set; }
        public byte[] Bytes_10 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FunctionPointer_0 = s.SerializePointer(FunctionPointer_0, name: nameof(FunctionPointer_0));
            FunctionPointer_1 = s.SerializePointer(FunctionPointer_1, name: nameof(FunctionPointer_1));
            FunctionPointer_2 = s.SerializePointer(FunctionPointer_2, name: nameof(FunctionPointer_2));
            Bytes_10 = s.SerializeArray<byte>(Bytes_10, 28, name: nameof(Bytes_10));
        }
    }
}