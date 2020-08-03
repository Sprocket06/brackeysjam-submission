using System;

namespace Projection.DialogSystem
{
    public class LineEventArgs : EventArgs
    {
        public string Line { get; private set; }
        public bool AutoContinue { get; set; }

        public LineEventArgs(string line)
        {
            Line = line;
            AutoContinue = false;
        }
    }
}
