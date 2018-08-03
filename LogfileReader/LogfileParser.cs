namespace LogfileReader
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using LogfileReader.Entries;

    /// <summary>The log file parser.</summary>
    public class LogfileParser
    {
        /// <summary>Gets the records.</summary>
        public List<LogEntry> Entries { get; private set; }

        /// <summary>Gets the SX FY.</summary>
        public List<SxFyLogEntry> SxFy { get; private set; }

        /// <summary>The parse.</summary>
        /// <param name="fileNames">The file names.</param>
        /// <returns>The <see cref="LogfileParser"/>.</returns>
        public Task<LogfileParser> Parse(string[] fileNames)
        {
            return Task.Run(() => this.ParseInternally(fileNames));
        }

        private LogfileParser ParseInternally(IEnumerable<string> fileNames)
        {
            this.Entries = new List<LogEntry>();
            this.SxFy = new List<SxFyLogEntry>();

            foreach (var fileName in fileNames)
            {
                var linesOfLogEntry = new List<string>();

                using (var file = new StreamReader(fileName))
                {
                    while (file.Peek() >= 0)
                    {
                        var line = file.ReadLine();
                        if (line.IsStartOfNewRecord())
                        {
                            this.BuildLogEntry(linesOfLogEntry);
                        }

                        linesOfLogEntry.Add(line);
                    }

                    this.BuildLogEntry(linesOfLogEntry);
                }
            }

            return this;
        }

        /// <summary>The handle last record.</summary>
        /// <param name="linesOfLogEntry">The lines of log entry.</param>
        private void BuildLogEntry(IList<string> linesOfLogEntry)
        {
            if (linesOfLogEntry.Any())
            {
                this.Entries.Add(new LogEntry(linesOfLogEntry));
                
                if (linesOfLogEntry.ContainsSxFy())
                {
                    this.SxFy.Add(new SxFyLogEntry(linesOfLogEntry));
                }

                linesOfLogEntry.Clear();
            }
        }
    }
}