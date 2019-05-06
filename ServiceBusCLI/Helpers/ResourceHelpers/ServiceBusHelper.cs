using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusCLI.Helpers.ResourceHelpers
{
    public class ServiceBusHelper
    {
        public static readonly MessageHandlerOptions MessageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
        {
            MaxConcurrentCalls = 1,
            AutoComplete = false
        };

        public static IClientEntity GetClient(string serviceBusConnString, 
            string queueId, 
            string topicId, 
            string subId = null, 
            bool peek = false, 
            bool verbose = false)
        {
            if (!string.IsNullOrEmpty(queueId))
            {
                if (verbose)
                {
                    Console.WriteLine($"Creating queue client for queue {queueId}.");
                }

                return new QueueClient(serviceBusConnString, queueId);
            }
            if (!string.IsNullOrEmpty(topicId) && string.IsNullOrEmpty(subId))
            {
                if (verbose)
                {
                    Console.WriteLine($"Creating topic client for topic {topicId}.");
                }

                return new TopicClient(serviceBusConnString, topicId);
            }
            else
            {

                if (verbose)
                {
                    Console.WriteLine($"Creating subscription client for topic/subscription {topicId}/{subId}.");
                }

                return new SubscriptionClient(serviceBusConnString, topicId, subId);
            }
        }

        public static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }
    }
}
