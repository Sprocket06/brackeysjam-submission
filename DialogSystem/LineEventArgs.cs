using System;

namespace Projection.DialogSystem
{
    public class LineEventArgs : EventArgs
    {
        public string Line { get; private set; }
        public bool AutoContinue { get; set; } = true;

        public LineEventArgs(string line)
        {
            Line = line;
        }
    }
}
