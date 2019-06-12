using Microsoft.Azure.ServiceBus;
using NUnit.Framework;
using ServiceBusCLI.Helpers.ResourceHelpers;

namespace sbcli.Test
{

    [TestFixture]
    public class ServiceBusHelperTests
    {
        private string subName = null;
        private string queueName = null;
        private string topicName = null;
        private string connString = "Endpoint=sb://myServiceBus.servicebus.windows.net/;SharedAccessKeyName=KeyName;SharedAccessKey=eraefawefawefwetwetawef";

        [Test]
        public void ServiceBusHelperShouldReturnQueueClientWhenProvidedQueueName()
        {
            var client = ServiceBusHelper.GetClient(connString, "queueName", topicName, subName);

            Assert.IsInstanceOf<QueueClient>(client);
        }

        [Test]
        public void ServiceBusHelperShouldReturnTopicClientWhenProvidedTopicName()
        {
            var client = ServiceBusHelper.GetClient(connString, queueName, "topicName", subName);

            Assert.IsInstanceOf<TopicClient>(client);
        }

        [Test]
        public void ServiceBusHelperShouldReturnSubscriptionClientWhenProvidedTopicNameAndSubName()
        {
            var client = ServiceBusHelper.GetClient(connString, queueName, "topicName", "subName");

            Assert.IsInstanceOf<SubscriptionClient>(client);
        }
    }
}
