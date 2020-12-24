﻿namespace R1Engine
{
    public enum Unity_MapCollisionTypeGraphic
    {
        None = 0,
        Reactionary = 1,
        Hill_Steep_Left = 2,
        Hill_Steep_Right = 3,
        Hill_Slight_Left_1 = 4,
        Hill_Slight_Left_2 = 5,
        Hill_Slight_Right_2 = 6,
        Hill_Slight_Right_1 = 7,
        Damage = 8,
        Bounce = 9,
        Water = 10,
        Exit = 11,
        Climb = 12,
        Water_NoSplash = 13,
        Passthrough = 14,
        Solid = 15,
        Seed = 16,
        Unknown0 = 17,
        Slippery_Steep_Left = 18,
        Slippery_Steep_Right = 19,
        Slippery_Slight_Left_1 = 20,
        Slippery_Slight_Left_2 = 21,
        Slippery_Slight_Right_2 = 22,
        Slippery_Slight_Right_1 = 23,
        Spikes = 24,
        Cliff = 25,
        Unknown1 = 26,
        Unknown2 = 27,
        Unknown3 = 28,
        Unknown4 = 29,
        Slippery = 30,

        Direction_Up,
        Direction_Down,
        Direction_Left,
        Direction_Right,
        Direction_UpLeft,
        Direction_UpRight,
        Direction_DownLeft,
        Direction_DownRight,
        Climb_Full,
        Climb_Hang,
        Solid_Hangable,
        Solid_NotHangable,
        Slippery_Hangable,

        Angle_Top_Left,
        Angle_Top_Right,


        EnemyDirection_Up,
        EnemyDirection_Down,
        EnemyDirection_Left,
        EnemyDirection_Right,
        EnemyDirection_UpLeft,
        EnemyDirection_UpRight,
        EnemyDirection_DownLeft,
        EnemyDirection_DownRight,
        Climb_Spider,
        Climb_Walls,
        Rotate_Clockwise45,
        Rotate_Clockwise90,
        Rotate_CounterClockwise45,
        Rotate_CounterClockwise90,
        Lava,
        
        Trigger_StopMovement,
        SpikePin,
        DetectionZone,
        Race_Bumper,
        Race_SpeedUp,
        Race_Finish1,
        Race_Finish2,
        Race_Finish3,
        CannonTarget_Valid,
        CannonTarget_Invalid,
    }
}