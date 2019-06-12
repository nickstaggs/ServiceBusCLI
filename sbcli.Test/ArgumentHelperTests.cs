using NUnit.Framework;
using ServiceBusCLI.CommandLineOptions;
using ServiceBusCLI.Helpers;
using System;

namespace sbcli.Test
{

    [TestFixture]
    public class ArgumentHelperTests
    {

        [Test]
        public void ArgumentHelperShouldThrowArgumentExceptionWhenQueueAndTopicNamesDontExist()
        {
            var options = new ReadSubOptions();

            Assert.Throws<ArgumentException>(() => ArgumentHelper.ValidateArguments((CommonSubOptions)options));
        }

        [Test]
        public void ArgumentHelperShouldThrowArgumentExceptionWhenQueueAndTopicNamesBothExist()
        {
            var options = new ReadSubOptions()
            {
                Topic = "topicName",
                Queue = "QueueName"
            };

            Assert.Throws<ArgumentException>(() => ArgumentHelper.ValidateArguments((CommonSubOptions)options));
        }

        [Test]
        public void ArgumentHelperShouldNotThrowExceptionWhenQueueNamesExists()
        {
            var options = new ReadSubOptions()
            {
                Queue = "queueName"
            };

            Assert.DoesNotThrow(() => ArgumentHelper.ValidateArguments((CommonSubOptions)options));
        }

        [Test]
        public void ArgumentHelperShouldThrowArgumentExceptionWhenTopicNameExistsButSubNameDoesNotForRead()
        {
            var options = new ReadSubOptions()
            {
                Topic = "topicName"
            };

            Assert.Throws<ArgumentException>(() => ArgumentHelper.ValidateArguments(options));
        }

        [Test]
        public void ArgumentHelperShouldThrowArgumentExceptionWhenSubNameExistsForWrite()
        {
            var options = new WriteSubOptions()
            {
                Subscription = "subName"
            };

            Assert.Throws<ArgumentException>(() => ArgumentHelper.ValidateArguments(options));
        }
    }
}
