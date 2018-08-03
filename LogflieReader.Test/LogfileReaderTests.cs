// ReSharper disable InconsistentNaming
namespace LogfileReader.Test
{
    using System.Linq;

    using LogfileReader;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>The log file reader tests.</summary>
    [TestClass]
    public class LogfileReaderTests
    {
        /// <summary>The file names.</summary>
        private readonly string[] fileNames =
            {
                "c:\\Tmp\\A011 NASP14-01 (negative TimePrev)\\EAF_EDA-20180123_095631.log"
            };

        /// <summary>The system under test.</summary>
        private LogfileParser sut;

        /// <summary>The setup.</summary>
        [TestInitialize]
        public void Setup()
        {
            this.sut = new LogfileParser();
        }

        /// <summary>Parsing a given log file it is expected that the record count is as expected.</summary>
        [TestMethod]
        public void Parse_GivenLogFile_RecordCountIsAsExpected()
        {
            this.sut.Parse(this.fileNames);

            Assert.AreEqual(20527, this.sut.Entries.Count);
        }

        /// <summary>The get S1F3 respond statistics.</summary>
        [TestMethod]
        public void GetS1F3RespondStatistics()
        {
            this.sut.Parse(this.fileNames);

            var s1f3s = from entry in this.sut.SxFy
                        where entry.Text.Contains("S1F3")
                        select entry;

            var s1f4s = from entry in this.sut.SxFy
                        where entry.Text.Contains("S1F4")
                        select entry;

            var query = (from s1f3 in s1f3s
                         join s1f4 in s1f4s on s1f3.TransActionId equals s1f4.TransActionId
                         select new
                                    {
                                        RespondTime = (s1f4.TimeStamp - s1f3.TimeStamp).TotalMilliseconds,
                                        TransactionId = s1f3.TransActionId
                                    })
                         .ToList();

            var max = query.Select(x => x.RespondTime).Max();
            var average = query.Select(x => x.RespondTime).Average();

            Assert.AreEqual(17436, max);
            Assert.AreEqual(123.49522957662492, average);
        }
    }
}
