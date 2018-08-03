

namespace LogfileReader.Test
{
    using FluentAssertions;

    using LogfileReader.Entries;
    using LogfileReader.Enumerable;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LogEntryEnumeratorTests
    {
        private const string FileName = "test.log";

        private const int ExpectedEntryCount = 999;

        [TestInitialize]
        public void Setup()
        {
            Fixture.SetupFile(FileName, expectedEntryCount: 2);
        }

        [TestMethod]
        public void Create()
        {
            using (var sut = new LogEntryEnumerator(FileName))
            {
                sut.MoveNext();
                sut.Current.Should().BeOfType<SxFyLogEntry>();
                ((SxFyLogEntry)sut.Current).TransActionId.Should().Be("0x00000000");

                sut.MoveNext();
                sut.Current.Should().BeOfType<SxFyLogEntry>();
                ((SxFyLogEntry)sut.Current).TransActionId.Should().Be("0x00000001");
            }
        }
    }
}
