﻿using Cysharp.Threading.Tasks;

namespace R1Engine
{
    public class GBA_Sabrina_Manager : GBA_Manager
    {
        public override int LevelCount => 50;
        public override UniTask ExtractVignetteAsync(GameSettings settings, string outputDir) => throw new System.NotImplementedException();
    }
}