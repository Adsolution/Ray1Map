﻿using System.Linq;
using UnityEngine;

namespace R1Engine
{
    public class GBACrash_ObjData : R1Serializable
    {
        public ushort Ushort_00 { get; set; }
        public ushort ObjGroupsCount { get; set; }
        public Pointer ObjGroupsPointer { get; set; }
        public Pointer ObjParamsOffsetsPointer { get; set; }
        public Pointer ObjParamsPointer { get; set; }
        public Pointer Pointer_10 { get; set; } // Only referenced from function at 08023d8c in Crash 2?

        // Serialized from pointers

        public GBACrash_ObjGroups[] ObjGroups { get; set; }
        public ushort[] ObjParamsOffsets { get; set; }
        public byte[][] ObjParams { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));
            ObjGroupsCount = s.Serialize<ushort>(ObjGroupsCount, name: nameof(ObjGroupsCount));
            ObjGroupsPointer = s.SerializePointer(ObjGroupsPointer, name: nameof(ObjGroupsPointer));
            ObjParamsOffsetsPointer = s.SerializePointer(ObjParamsOffsetsPointer, name: nameof(ObjParamsOffsetsPointer));
            ObjParamsPointer = s.SerializePointer(ObjParamsPointer, name: nameof(ObjParamsPointer));
            Pointer_10 = s.SerializePointer(Pointer_10, name: nameof(Pointer_10));

            ObjGroups = s.DoAt(ObjGroupsPointer, () => s.SerializeObjectArray<GBACrash_ObjGroups>(ObjGroups, ObjGroupsCount, name: nameof(ObjGroups)));
            ObjParamsOffsets = s.DoAt(ObjParamsOffsetsPointer, () => s.SerializeArray<ushort>(ObjParamsOffsets, ObjGroups.SelectMany(x => x.Objects).Max(x => x.ObjParamsIndex) + 1, name: nameof(ObjParamsOffsets)));

            s.DoAt(ObjParamsPointer, () =>
            {
                if (ObjParams == null)
                    ObjParams = new byte[ObjParamsOffsets.Length][];

                for (int i = 0; i < ObjParams.Length; i++)
                {
                    var length = i < ObjParams.Length - 1 ? ObjParamsOffsets[i + 1] - ObjParamsOffsets[i] : ObjParamsOffsetsPointer - (ObjParamsPointer + ObjParamsOffsets[i]);

                    // Make sure the length is reasonable (only really used for the last entry)
                    if (length < 0 || length > 64)
                    {
                        length = 8; // Default to 8

                        Debug.LogWarning($"Obj params length is invalid for entry {i}/{ObjParams.Length - 1}");
                    }

                    ObjParams[i] = s.SerializeArray<byte>(ObjParams[i], length, name: $"{nameof(ObjParams)}[{i}]");
                }
            });
        }
    }
}