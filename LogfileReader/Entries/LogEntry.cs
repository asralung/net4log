namespace LogfileReader.Entries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>The record.</summary>
    public class LogEntry
    {
        public LogEntry()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="LogEntry"/> class.</summary>
        /// <param name="linesOfLogEntry">The lines of the record to be created.</param>
        public LogEntry(IList<string> linesOfLogEntry)
        {
            this.TimeStamp = linesOfLogEntry.First().GetRecordTimeStamp();
            this.Text = string.Join(Environment.NewLine, linesOfLogEntry);
        }

        /// <summary>Gets or sets the text.</summary>
        public string Text { get; set; }

        /// <summary>Gets or sets the time stamp.</summary>
        public DateTime TimeStamp { get; set; }

        public string Thread { get; set; }

        public string Level { get; set; }

        public string Logger { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Text;
        }
    }
}