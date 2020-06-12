﻿using System.Collections.Generic;
using System.Linq;

namespace R1Engine
{
    /// <summary>
    /// Event block data for Rayman 1 (Jaguar)
    /// </summary>
    public class Jaguar_R1_EventBlock : R1Serializable
    {
        // Is this correct?
        public bool HasEvents { get; set; }

        // Event map dimensions, always the map size divided by 4
        public ushort Width { get; set; }
        public ushort Height { get; set; }

        // Mapped to a 2D plane based on width and height
        public ushort[] EventIndexMap { get; set; }

        // Indexed, with offsets to the data table
        public ushort[] EventOffsetTable { get; set; }

        public Jaguar_R1_EventData[][] EventData { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            HasEvents = s.Serialize<bool>(HasEvents, name: nameof(HasEvents));
            s.SerializeArray<byte>(new byte[3], 3, name: "Padding");
            
            // Serialize event map dimensions
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));

            EventIndexMap = s.SerializeArray<ushort>(EventIndexMap, Width * Height, name: nameof(EventIndexMap));

            // Serialize next data block, skipping the padding
            s.DoAt(Offset + 0x1208, () => EventOffsetTable = s.SerializeArray<ushort>(EventOffsetTable, EventIndexMap.Max(), name: nameof(EventIndexMap)));

            if (EventData == null)
                EventData = new Jaguar_R1_EventData[EventOffsetTable.Length][];

            // Serialize the events based on the offsets
            for (int i = 0; i < EventData.Length; i++)
            {
                s.DoAt(Offset + 0x1608 + EventOffsetTable[i], () =>
                {
                    if (EventData[i] == null)
                    {
                        var temp = new List<Jaguar_R1_EventData>();

                        var index = 0;
                        while (s.Serialize<ushort>(default, name: "ReadEvent") == 1)
                        {
                            temp.Add(s.SerializeObject<Jaguar_R1_EventData>(default, name: $"{nameof(EventData)}[{i}][{index}]"));
                            index++;
                        }

                        EventData[i] = temp.ToArray();
                    }
                    else
                    {
                        for (int j = 0; j < EventData[i].Length; j++)
                        {
                            s.Serialize<ushort>(1, name: "ReadEvent");
                            EventData[i][j] = s.SerializeObject<Jaguar_R1_EventData>(EventData[i][j], name: $"{nameof(EventData)}[{i}][{j}]");
                        }

                        s.Serialize<ushort>(0, name: "ReadEvent");
                    }
                });
            }
        }
    }
}