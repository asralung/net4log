namespace Net4Log.RoslynHost
{
    using System.Collections.Generic;

    using LogfileReader.Entries;

    public class Globals
    {
        public IEnumerable<SxFyLogEntry> SxFy;

        public IEnumerable<LogEntry> Entries;
    }
}