namespace LogfileReader.Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using FluentAssertions;

    using LogfileReader.Entries;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LogEntryTests
    {
        /// <summary>The expected time stamp.</summary>
        private const string ExpectedTimeStamp = "2018-03-26 12:34:10,100";

        /// <summary>The expected trans action id.</summary>
        private const string ExpectedTransActionId = "0x00C7";

        /// <summary>The sample log entry.</summary>
        private readonly string sampleSxFy = $"{ExpectedTimeStamp} [9] S1F3 ;Transaction ID={ExpectedTransActionId}";

        /// <summary>The log entry created by default, its expected that property values as expected.</summary>
        [TestMethod]
        public void LogEntry_Default_PropertyValuesAsExpected()
        {
            var sut = new LogEntry(new List<string> { this.sampleSxFy });

            sut.Text.Should().Be(this.sampleSxFy);
            sut.TimeStamp.Should().Be(DateTime.ParseExact(ExpectedTimeStamp, LogFileParserExtensions.DateTimeFormat, CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void SxFyEntry_Default_PropertyValuesAsExpected()
        {
            var sut = new SxFyLogEntry(new List<string> { this.sampleSxFy });

            sut.Should().BeAssignableTo<LogEntry>();
            sut.Text.Should().Be(this.sampleSxFy);
            sut.TimeStamp.Should().Be(DateTime.ParseExact(ExpectedTimeStamp, LogFileParserExtensions.DateTimeFormat, CultureInfo.InvariantCulture));
            sut.TransActionId.Should().Be(ExpectedTransActionId);
        }
    }
}