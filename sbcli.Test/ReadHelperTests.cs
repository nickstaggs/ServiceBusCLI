using Microsoft.Azure.ServiceBus.Core;
using NUnit.Framework;
using ServiceBusCLI.CommandLineOptions;
using ServiceBusCLI.Helpers.VerbHelpers;
using Moq;

namespace sbcli.Test
{
    [TestFixture]
    public class ReadHelperTests
    {

        private Mock<IReceiverClient> client;
        private ReadSubOptions options;
        private string connString = "Endpoint=sb://myServiceBus.servicebus.windows.net/;SharedAccessKeyName=KeyName;SharedAccessKey=eraefawefawefwetwetawef";


        [Test, MaxTime(1400)]
        public void ReadVerbShouldOnlyLastForAsLongAsTimerIsSetFor()
        {
            options = new ReadSubOptions() { TimeToWait = 1, Queue = "queueName", NumberOfMessages=10 };
            client = new Mock<IReceiverClient>();
            ReadHelper.Client = client.Object;

            Assert.AreEqual(0, ReadHelper.Read(options));
        }
    }
}
