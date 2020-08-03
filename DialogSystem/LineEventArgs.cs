using System;

namespace Projection.DialogSystem
{
    public class LineEventArgs : EventArgs
    {
        public String Line { get; private set; }
        public bool autoContinue;

        public LineEventArgs(String line)
        {
            Line = line;
            autoContinue = false;
        }
    }
}
