namespace LogfileReader.Test
{
    using System.Linq;

    using FluentAssertions;

    using LogfileReader.Enumerable;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LogEntriesTests
    {
        private const string FileName = "test.log";

        private const int ExpectedEntryCount = 999;

        [TestInitialize]
        public void Setup()
        {
            Fixture.SetupFile(FileName, ExpectedEntryCount);
        }

        [TestMethod]
        public void Create()
        {
            using (var sut = new LogEntries(FileName))
            {
                sut.Count().Should().Be(ExpectedEntryCount);
                sut.ToList();
            }
        }
    }
}