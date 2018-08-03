namespace LogfileReader.Test
{
    using System.Collections.Generic;
    using System.IO;

    using LogfileReader.Entries;

    internal static class Fixture
    {
        public static void SetupFile(string fileName, int expectedEntryCount)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var file = new StreamWriter(fileName))
            {
                for (var i = 0; i < expectedEntryCount; i++)
                {
                    var logEntry = new SxFyLogEntry(
                        new List<string> { $"2018-03-26 12:34:10,{i:D3} [9] S1F3 ;Transaction ID=0x{i:X8}", $"{i}" });

                    file.WriteLine(logEntry.ToString());
                }
            }
        }
    }
}