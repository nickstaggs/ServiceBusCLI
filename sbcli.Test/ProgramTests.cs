using NUnit.Framework;
using ServiceBusCLI;

namespace sbcli.Test
{

    [TestFixture]
    public class ProgramTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MainShouldReturn1WhenNoOptionsAreCalledWithRead()
        {
            int result = Program.Main(new string[] { "read" });
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void MainShouldReturn1WhenNoOptionsAreCalledWithWrite()
        {
            int result = Program.Main(new string[] { "write" });
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void MainShouldReturn1WhenMissingConnStringCalledWithRead()
        {
            int result = Program.Main(new string[] { "read", "-q", "queue" });
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void MainShouldReturn1WhenMissingQueueOrTopicNameCalledWithRead()
        {
            int result = Program.Main(new string[] { "read", "-c", "connString" });
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void MainShouldReturn1WhenMissingQueueOrTopicNameCalledWithWrite()
        {
            int result = Program.Main(new string[] { "write", "-c", "connString", "-m", "message" });
            Assert.AreEqual(result, 1);
        }
    }
}