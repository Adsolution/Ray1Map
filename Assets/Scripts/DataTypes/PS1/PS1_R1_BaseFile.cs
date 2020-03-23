﻿namespace R1Engine
{
    /// <summary>
    /// Base file for Rayman 1 (PS1)
    /// </summary>
    public class PS1_R1_BaseFile : R1Serializable
    {
        /// <summary>
        /// The amount of pointers in the header
        /// </summary>
        public uint PointerCount { get; set; }

        /// <summary>
        /// The block pointers
        /// </summary>
        public Pointer[] BlockPointers { get; set; }
        
        /// <summary>
        /// The length of the file in bytes
        /// </summary>
        public uint FileSize { get; set; }

        /// <summary>
        /// Serializes the data
        /// </summary>
        /// <param name="serializer">The serializer</param>
        public override void SerializeImpl(SerializerObject s) {
            Pointer BaseAddress = s.CurrentPointer;
            PointerCount = s.Serialize<uint>(PointerCount, name: "PointerCount");

            // Serialize the block pointers. These aren't memory pointers but file pointers, so subtract the base address
            BlockPointers = s.SerializePointerArray(BlockPointers, PointerCount, anchor: BaseAddress, name: "Pointers");
            FileSize = s.Serialize<uint>(FileSize, name: "FileSize");
        }
    }
}