﻿using System;
using R1Engine.Serialize;

namespace R1Engine
{
    public class GBAIsometric_Spyro_DataTable : R1Serializable
    {
        public GBAIsometric_Spyro_DataTableEntry[] DataEntries { get; set; }

        public T DoAtBlock<T>(Context context, long index, Func<uint, T> action)
            where T : class
        {
            var entry = DataEntries[index];
            var pointer = entry.DataPointer;
            var s = context.Deserializer;

            switch (entry.Compression)
            {
                case GBAIsometric_Spyro_DataTableEntry.CompressionType.None:
                    return s.DoAt<T>(pointer, () => action(entry.DataLength));

                case GBAIsometric_Spyro_DataTableEntry.CompressionType.Huffman:
                    return s.DoAt<T>(pointer, () =>
                    {
                        T data = null;
                        s.DoEncoded(new HuffmanEncoder(), () => data = action(s.CurrentLength));
                        return data;
                    });

                case GBAIsometric_Spyro_DataTableEntry.CompressionType.LZSS:
                    return s.DoAt<T>(pointer, () =>
                    {
                        T data = null;
                        s.DoEncoded(new GBA_LZSSEncoder(), () => data = action(s.CurrentLength));
                        return data;
                    });

                case GBAIsometric_Spyro_DataTableEntry.CompressionType.RL:
                    throw new NotImplementedException("RL encoding is not implemented");

                default:
                    throw new ArgumentOutOfRangeException(nameof(entry.Compression), entry.Compression, null);
            }
        }

        public override void SerializeImpl(SerializerObject s)
        {
            DataEntries = s.SerializeObjectArray<GBAIsometric_Spyro_DataTableEntry>(DataEntries, 2180, name: nameof(DataEntries));
        }
    }
}