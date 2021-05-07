﻿namespace R1Engine
{
    public class Jade_RRR2_Wii_Manager : Jade_BaseManager 
    {
        // Levels
        public override LevelInfo[] LevelInfos => new LevelInfo[] {
            new LevelInfo(0x00000004, "ROOT/EngineDatas/02 Modelisation Bank/Creation_Graphistes/Temp_Polo", "Temp_Polo.wol"),
            new LevelInfo(0x0000C452, "ROOT/EngineDatas/06 Levels/__old/_main/_main_fix", "_main_fix.wol"),
            new LevelInfo(0x00000D41, "ROOT/EngineDatas/06 Levels/Compil", "Compil.wol"),
            new LevelInfo(0x000010C2, "ROOT/EngineDatas/06 Levels/PROTOTYPE_Levels_RIV/RIV_test_bunker/RIV_FPP_Bunker2", "RIV_FPP_Bunker2.wol"),
            new LevelInfo(0x00002838, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Danse/Danse_01/Dance_01_LD", "Dance_01_LD.wol"),
            new LevelInfo(0x00000A7B, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Danse/Danse_02/Danse_02_LD", "Danse_02_LD.wol"),
            new LevelInfo(0x000012DD, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Danse/Danse_03/Danse_03_LD", "Danse_03_LD.wol"),
            new LevelInfo(0x00001539, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Danse/Danse_04/Danse_04_LD", "Danse_04_LD.wol"),
            new LevelInfo(0x00001759, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Danse/Danse_05/Danse_05_LD", "Danse_05_LD.wol"),
            new LevelInfo(0x0000175A, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Danse/Danse_06/Danse_06_LD", "Danse_06_LD.wol"),
            new LevelInfo(0x00002026, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/FPS/FPS1_PARIS/FPS1_PARIS_LD", "FPS1_PARIS_LD.wol"),
            new LevelInfo(0x00000161, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/FPS/FPS2_ASIE/FPS2_ASIE_LD", "FPS2_ASIE_LD.wol"),
            new LevelInfo(0x00001CF9, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/FPS/FPS3_USA/FPS3_USA_LD", "FPS3_USA_LD.wol"),
            new LevelInfo(0x00000044, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/FPS/FPS4_BestOf/FPS4_BestOf_LD", "FPS4_BestOf_LD.wol"),
            new LevelInfo(0x000001A6, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/FPS/FPS5_ParisBis/FPS5_ParisBis_LD", "FPS5_ParisBis_LD.wol"),
            new LevelInfo(0x00000530, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Briefing_Podium/Amerique_Nord/Amerique_Nord_LD", "Amerique_Nord_LD.wol"),
            new LevelInfo(0x000004D9, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Briefing_Podium/Amerique_Sud/Amerique_Sud_LD", "Amerique_Sud_LD.wol"),
            new LevelInfo(0x0000070B, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Briefing_Podium/Asie/Asie_LD", "Asie_LD.wol"),
            new LevelInfo(0x0000054D, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Briefing_Podium/Europe/Europe_LD", "Europe_LD.wol"),
            new LevelInfo(0x000004F5, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Briefing_Podium/Tour_du_monde/Tour_du_monde_LD", "Tour_du_monde_LD.wol"),
            new LevelInfo(0x00000512, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Briefing_Podium/Tropique/Tropique_LD", "Tropique_LD.wol"),
            new LevelInfo(0x0000030A, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/customise/Custom_Rabbits_LD", "Custom_Rabbits_LD.wol"),
            new LevelInfo(0x00000163, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/Menu/Menu_LD", "Menu_LD.wol"),
            new LevelInfo(0x000007C8, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/Interface/RRR2_Boot", "RRR2_Boot.wol"),
            new LevelInfo(0x000006C6, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG000_Dummy", "MG000_Dummy.wol"),
            new LevelInfo(0x00001F4A, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG018_Jonglerie/MG018_Jonglerie_LD", "MG001_Jonglerie.wol"),
            new LevelInfo(0x00002008, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG037_Chimiste/MG037_Chimiste_LD", "MG011_Chimiste.wol"),
            new LevelInfo(0x00000018, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG041_LapinOlympique/MG008_Lapin_Olympic_SOUND", "MG008_Lapin_Olympic_SOUND.wol"),
            new LevelInfo(0x00002F5F, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG041_LapinOlympique/MG256_TrashRace_LD", "MG256_TrashRace_LD.wol"),
            new LevelInfo(0x0000016B, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG056_Natation/MG056_Natation_LD", "MG056_Natation_LD.wol"),
            new LevelInfo(0x0000208F, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG074_Crachat/MG074_Crachat_LD", "MG017_Crachat.wol"),
            new LevelInfo(0x000002AA, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG099_CellPhone/MG099_CellPhone_LD", "MG099_CellPhone_LD.wol"),
            new LevelInfo(0x00000128, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG117_Burger/MG117_Burger_LD", "MG117_Burger.wol"),
            new LevelInfo(0x00000042, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG121_UsualSuspects/MG121_UsualSuspects_LD", "MG121_UsualSuspects_LD.wol"),
            new LevelInfo(0x00000E97, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG161_Rot/MG161_Rot_LD", "MG161_Rot_LD.wol"),
            new LevelInfo(0x000003F7, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG168_SawWood/MG168_SawWood_LD", "MG168_SawWood_LD.wol"),
            new LevelInfo(0x000000E1, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG174_Baseball/MG174_Baseball_LD", "MG174_Baseball_LD.wol"),
            new LevelInfo(0x000004FB, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG183_Spiderman/MG183_Spiderman_LD", "MG183_Spiderman_LD.wol"),
            new LevelInfo(0x00000126, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG186_MereDenis/MG186_MereDenis_LD", "MG186_MereDenis_LD.wol"),
            new LevelInfo(0x0000067F, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG190_PouletFlambe/MG190_PouletFlambe_LD", "MG190_PouletFlambe_LD.wol"),
            new LevelInfo(0x0000235A, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG191_Anesthesie/MG191_Anesthesie_LD", "MG191_Anesthesie_LD.wol"),
            new LevelInfo(0x00000892, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG200_Football/MG200_Football_LD", "MG200_Football_LD.wol"),
            new LevelInfo(0x0000037B, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG205_PetitsTrous/MG205_PetitsTrous_LD", "MG205_PetitsTrous_LD.wol"),
            new LevelInfo(0x000006D1, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG208_GobePapillon/MG208_GobePapillon_LD", "MG208_GobePapillon_LD.wol"),
            new LevelInfo(0x000003C2, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG209_GeyserCaca/MG209_GeyserCaCa_LD", "MG209_GeyserCaCa_LD.wol"),
            new LevelInfo(0x00000C97, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG211_Caddy/MG211_Caddy_LD", "MG211_Caddy_LD.wol"),
            new LevelInfo(0x00000F2E, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG215_SnakeCharmer/MG215_SnakeCharmer_LD", "MG215_SnakeCharmer_LD.wol"),
            new LevelInfo(0x00000E8C, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG217_Photomaton/MG217_Photomaton_LD", "MG217_Photomaton_LD.wol"),
            new LevelInfo(0x00000373, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG222_Classe/MG222_Classe_LD", "MG222_Classe_LD.wol"),
            new LevelInfo(0x00000E85, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG224_BurgerBis/MG224_BurgerBis_LD", "MG224_BurgerBis_LD.wol"),
            new LevelInfo(0x00002DAF, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG225_Bureau/MG225_Bureau_LD", "MG225_Bureau_LD.wol"),
            new LevelInfo(0x00000227, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG226_DodgeCity/MG226_DodgeCity_LD", "MG226_DodgeCity_LD.wol"),
            new LevelInfo(0x00002D68, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG228_Dynamite/MG228_Dynamite_LD", "MG228_Dynamite_LD.wol"),
            new LevelInfo(0x0000126C, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG231_Duel/MG231_Duel_LD", "MG231_Duel_LD.wol"),
            new LevelInfo(0x00000E1D, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG233_HotCake/MG233_HotCake_LD", "MG233_HotCake_LD.wol"),
            new LevelInfo(0x000005D3, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG234_IndianaJohns/MG234_IndianaJohns_LD", "MG234_IndianaJohns_LD.wol"),
            new LevelInfo(0x00000261, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG237_Echec/MG237_Echec_LD", "MG237_Echec_LD.wol"),
            new LevelInfo(0x000003DF, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG244_BumperCars/MG244_BumperCars_LD", "MG244_BumperCars_LD.wol"),
            new LevelInfo(0x00000716, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG245_FartWars/MG245_FartWars_LD", "MG245_FartWars_LD.wol"),
            new LevelInfo(0x0000071B, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG246_SingingInTheRain/MG246_LD", "MG246_SingingInTheRain_LD.wol"),
            new LevelInfo(0x00000094, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG257_IceOnIce/MG257_IceOnIce_LD", "MG257_IceOnIce_LD.wol"),
            new LevelInfo(0x0000076A, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG258_DizzyRace/MG258_DizzyRace_LD", "MG258_DizzyRace_LD.wol"),
            new LevelInfo(0x00000F91, "ROOT/EngineDatas/06 Levels/PROTOTYPE_RRR2/MiniGame/MG259_SnailRace/MG259_SnailRace_LD", "MG259_SnailRace_LD.wol"),
        };

        // Version properties
        public override string[] BFFiles => new string[] {
            "Rayman4.bf"
        };
    }
}