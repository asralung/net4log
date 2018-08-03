namespace LogfileReader.Enumerable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using LogfileReader.Entries;

    public sealed class LogEntries : IEnumerable<LogEntry>, IDisposable
    {
        private readonly LogEntryEnumerator enumerator;

        public LogEntries(string fileName)
        {
            this.enumerator = new LogEntryEnumerator(fileName);
        }

        public IEnumerator<LogEntry> GetEnumerator()
        {
            return this.enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            this.enumerator?.Dispose();
        }
    }
}