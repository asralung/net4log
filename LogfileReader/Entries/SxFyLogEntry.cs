namespace LogfileReader.Entries
{
    using System.Collections.Generic;
    using System.Linq;

    public class SxFyLogEntry : LogEntry
    {
        public SxFyLogEntry(IList<string> lines)
            : base(lines)
        {
            this.TransActionId = lines.First().GetTransActionId();
        }

        /// <summary>Gets the trans action id.</summary>
        public string TransActionId { get; }
    }
}