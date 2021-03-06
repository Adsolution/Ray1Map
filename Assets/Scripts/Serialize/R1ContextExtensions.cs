﻿using BinarySerializer;

namespace R1Engine
{
    public static class R1ContextExtensions
    {
        public static GameSettings GetR1Settings(this SerializerObject s) => s.Context.GetR1Settings();
        public static GameSettings GetR1Settings(this Context c) => (c as R1Context)?.GameSettings;
    }
}