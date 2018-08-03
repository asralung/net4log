namespace LogfileReader.Enumerable
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using LogfileReader.Entries;

    public sealed class LogEntryEnumerator : IEnumerator<LogEntry>
    {
        private readonly string fileName;

        private StreamReader file;

        private List<string> linesOfLogEntry;

        private LogEntry current;

        public LogEntryEnumerator(string fileName)
        {
            this.fileName = fileName;
            this.Reset();
        }

        public LogEntry Current => this.current;

        object IEnumerator.Current => this.Current;

        public void Dispose()
        {
            this.file.Dispose();
        }

        public bool MoveNext()
        {
            while (this.file.Peek() >= 0)
            {
                var line = this.file.ReadLine();
                try
                {
                    if (line.IsStartOfNewRecord())
                    {
                        this.current = this.BuildLogEntryOrDefault();
                        if (this.current != default(LogEntry))
                        {
                            return true;
                        }
                    }
                }
                finally
                {
                    this.linesOfLogEntry.Add(line);
                }
            }

            this.current = this.BuildLogEntryOrDefault();

            return this.Current != default(LogEntry);
        }

        public void Reset()
        {
            this.file = new StreamReader(fileName);
            this.linesOfLogEntry = new List<string>();
        }

        private LogEntry BuildLogEntryOrDefault()
        {
            try
            {
                if (!this.linesOfLogEntry.Any())
                {
                    return default(LogEntry);
                }

                if (this.linesOfLogEntry.ContainsSxFy())
                {
                    return new SxFyLogEntry(this.linesOfLogEntry);
                }

                return new LogEntry(this.linesOfLogEntry);
            }
            finally
            {
                this.linesOfLogEntry.Clear();
            }
        }
    }
}