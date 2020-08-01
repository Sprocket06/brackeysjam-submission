using System;
using Chroma;
using Chroma.Diagnostics.Logging;

namespace Projection
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        internal GameCore()
        {
            Log.Info("Hello, world!");
        }
    }
}
