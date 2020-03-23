﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace R1Engine
{
    /// <summary>
    /// The available game modes to select from
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GameModeSelection
    {
        [GameMode(GameMode.RayPS1, "Rayman 1 (PS1)", typeof(PS1_R1_Manager))]
        RaymanPS1,

        [GameMode(GameMode.RayPS1JP, "Rayman 1 (PS1 - Japan)", typeof(PS1_R1JP_Manager))]
        RaymanPS1Japan,

        [GameMode(GameMode.RayPC, "Rayman 1 (PC)", typeof(PC_R1_Manager))]
        RaymanPC,

        [GameMode(GameMode.RayKit, "Rayman Gold (PC - Demo)", typeof(PC_RD_Manager))]
        RaymanGoldPCDemo,

        [GameMode(GameMode.RayKit, "Rayman Designer (PC)", typeof(PC_RD_Manager))]
        RaymanDesignerPC,

        [GameMode(GameMode.RayKit, "Rayman Mapper (PC)", typeof(PC_Mapper_Manager))]
        MapperPC,

        [GameMode(GameMode.RayKit, "Rayman by his Fans (PC)", typeof(PC_RD_Manager))]
        RaymanByHisFansPC,

        [GameMode(GameMode.RayKit, "Rayman 60 Levels (PC)", typeof(PC_RD_Manager))]
        Rayman60LevelsPC,

        [GameMode(GameMode.RayEduPC, "Rayman Educational (PC)", typeof(PC_EDU_Manager))]
        RaymanEducationalPC,

        [GameMode(GameMode.RayEduPC, "Rayman Quiz (PC)", typeof(PC_EDU_Manager))]
        RaymanQuizPC,

        [GameMode(GameMode.RayPocketPC, "Rayman Ultimate (Pocket PC)", typeof(PocketPC_R1_Manager))]
        RaymanPocketPC,

        //[GameMode(GameMode.RayPocketPC, "Rayman Classic (Mobile)", typeof(PocketPC_R1_Manager))]
        //RaymanClassicMobile,
    }
}