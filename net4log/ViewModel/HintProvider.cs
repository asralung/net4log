namespace Net4Log.ViewModel
{
    using System.Text;

    internal static class HintProvider
    {
        public static string GetUsageHint()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Usage:");
            sb.AppendLine("- Drag one ore more logfiles to this window and drop.");
            sb.AppendLine("- Use the following predefined variables to write your query :");
            sb.AppendLine();
            sb.AppendLine("     - IEnumerable<SxFyLogEntry> SxFy");
            sb.AppendLine();
            sb.AppendLine("         Contains all entries of the loaded log files that represents an SxFy.");
            sb.AppendLine("         The type SxFyLogEntry provides the properties Timestamp, Text and TransactionId.");
            sb.AppendLine();
            sb.AppendLine("     - IEnumerable<LogEntry> Entries");
            sb.AppendLine();
            sb.AppendLine("         Contains all entries of the loaded log files.");
            sb.AppendLine("         The type LogEntry provides the properties Timestamp and Text.");
            sb.AppendLine();
            sb.AppendLine("     Explore all available properties with IntelliSense.");
            sb.AppendLine();
            sb.AppendLine("- Return the result of your query as a number or formatted string to display it in this output pane");
            sb.AppendLine("- Hit Ctrl+Enter to run your query");
            sb.AppendLine();
            sb.AppendLine("Example: Select Timestamp and Transaction Id of all S1F3");
            sb.AppendLine();
            sb.AppendLine("         var query = from s1f3 in SxFy");
            sb.AppendLine("                     where s1f3.Text.Contains(\"S1F3\")");
            sb.AppendLine("                     select $\"{s1f3.TimeStamp} - {s1f3.TransActionId}\";");
            sb.AppendLine();
            sb.AppendLine("         return string.Join(Environment.NewLine, query);");

            return sb.ToString();
        }

        public static string NoLogFilesLoadedYetText()
        {
            return "No log files loaded yet. Drop one ore more to this Window.";
        }

        public static string GetInitalScriptText()
        {
            return string.Empty;
        }
    }
}