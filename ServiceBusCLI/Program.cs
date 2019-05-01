using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServiceBusCLI
{
    class Options
    {
        [Option(
            shortName: 'r',
            longName: "read", 
            Required = false,
            Default = false, 
            HelpText = "Read from service bus")]
        public bool Read { get; set; }

        [Option(
            shortName: 'w', 
            longName: "write", 
            Required = false, 
            Default = false, 
            HelpText = "Write to service bus")]
        public bool Write { get; set; }

        [Option(
            shortName: 't', 
            longName: "topic", 
            Required = false,
            HelpText = "Topic ID")]
        public string Topic { get; set; }

        [Option(
            shortName: 's', 
            longName: "subscription", 
            Required = false,
            HelpText = "Subscription ID")]
        public string Subscription { get; set; }

        [Option(
            shortName: 'q', 
            longName: "queue",
            Required = false,
            HelpText = "Queue ID")]
        public string Queue { get; set; }

        [Option(
            shortName: 'n',
            longName: "numberOfMessages",
            Required = false,
            Default = 10,
            HelpText = "Number of messages to read off the queue/topic/subscription")]
        public int NumberOfMessages { get; set; }

        [Option(
            shortName: 'x',
            longName: "timeToWait",
            Required = false,
            Default = 10,
            HelpText = "Time in seconds to wait to read messages off the queue/topic/subscription, -1 to wait indefinitely")]
        public int TimeToWait { get; set; }

        [Option(
            shortName: 'p', 
            longName: "peek", 
            Required = false, 
            Default = false, 
            HelpText = "Peek messages instead of reading and taking them off the queue/topic/subscription")]
        public bool Peek { get; set; }

        [Option(
            shortName: 'm', 
            longName: "message", 
            Required = false,
            HelpText = "Message to send to queue/topic")]
        public string Message { get; set; }

        [Option(
            shortName: 'c',
            longName: "serviceBusConnString",
            Required = true,
            HelpText = "Connection string to the service bus")]
        public string ServiceBusConnString { get; set; }

        [Option(
            shortName: 'u',
            longName: "userProperties",
            Required = false,
            HelpText = "User properties to be placed on message to be written to service bus, format: <k1>:<v1>,<k2>:<v2>,...,<kn>:<vn> no spaces")]
        public string UserProperties { get; set; }

        [Option(
            shortName: 'v',
            longName: "verbose",
            Required = false,
            Default = false,
            HelpText = "Prints all logging to standard output.")]
        public bool Verbose { get; set; }
    }

    class Program
    {

        public static bool Verbose { get; private set; } = false;
        private static IClientEntity client;

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       if (o.Read && o.Write)
                       {
                           Console.WriteLine("Please use read or write argument. Not both.");
                           return;
                       }

                       if (!o.Read && !o.Write)
                       {
                           Console.WriteLine("Please use read or write argument.");
                           return;
                       }

                       if (string.IsNullOrEmpty(o.Queue) && string.IsNullOrEmpty(o.Topic))
                       {
                           Console.WriteLine("Please provide queue or topic name");
                           return;
                       }

                       if (!string.IsNullOrEmpty(o.Queue) && !string.IsNullOrEmpty(o.Topic))
                       {
                           Console.WriteLine("Please provide queue or topic name. Not both.");
                           return;
                       }

                       if (o.Verbose)
                       {
                           Verbose = true;
                       }

                       client = GetClient(o.ServiceBusConnString, o.Queue, o.Topic, o.Subscription, o.Peek);

                       if (o.Read)
                       {
                           Read((IReceiverClient)client, o.Peek, o.NumberOfMessages, o.TimeToWait);
                       }
                       else
                       {
                           Task.Run(async () =>
                           {
                               var message = CreateSBMessage(o.Message, o.UserProperties);

                               await Write((ISenderClient)client, message);
                           }).GetAwaiter().GetResult();
                       }


                   });
        }

        static string Read(IReceiverClient client, bool peek, int numMessages, int time)
        {

            System.Timers.Timer timer = new System.Timers.Timer();
            System.Timers.Timer logTimer = new System.Timers.Timer() { Interval = 2000, AutoReset = true };

            client.PrefetchCount = numMessages;

            var messages = new StringBuilder();
            int n = 0;
            bool isTimeUp = false;

            timer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) => { isTimeUp = true; };
            logTimer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) => {
                if (Verbose)
                {
                    Console.WriteLine("Waiting for messages...");
                }
            };

            if (time >= 0)
            {
                timer.Interval = time * 1000;
                timer.Start();
            }
            
            logTimer.Start();

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            try
            {
                client.RegisterMessageHandler(async (Message message, CancellationToken token) =>
                {
                    if (Verbose)
                    {
                        Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber}");
                    }

                    Console.WriteLine(Encoding.UTF8.GetString(message.Body));

                    messages.Append(Encoding.UTF8.GetString(message.Body));

                    if (!peek)
                    {
                        await client.CompleteAsync(message.SystemProperties.LockToken);
                    }

                    n++;

                }, messageHandlerOptions);

                while (n < numMessages && !isTimeUp) { }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ooops, trying to read messages failed: {e}");
            }
            finally
            {
                client.CloseAsync();
                timer.Stop();
                logTimer.Stop();
            }

            return messages.ToString();
        }

        static async Task Write(ISenderClient client, Message message)
        {
            Type clientType = client.GetType();

            if (client.GetType() == typeof(SubscriptionClient))
            {
                Console.WriteLine("Cannot write message to subscription only topic or queue, please remove subscription Id");
                return;
            }

            try
            {
                await client.SendAsync(message);
            } 
            catch (Exception e)
            {
                Console.WriteLine($"Ooops, sending messsage failed: {e}");
                return;
            }

            if (Verbose)
            {
                Console.WriteLine($"Sent message: {message}");
            }
        }

        static IClientEntity GetClient(string serviceBusConnString, string queueId, string topicId, string subId, bool peek)
        {
            if (!string.IsNullOrEmpty(queueId))
            {
                if (Verbose)
                {
                    Console.WriteLine($"Creating queue client for queue {queueId}.");
                }

                return new QueueClient(serviceBusConnString, queueId);
            }
            if (!string.IsNullOrEmpty(topicId) && string.IsNullOrEmpty(subId))
            {
                if (Verbose)
                {
                    Console.WriteLine($"Creating topic client for topic {topicId}.");
                }

                return new TopicClient(serviceBusConnString, topicId);
            }
            else
            {

                if (Verbose)
                {
                    Console.WriteLine($"Creating subscription client for topic/subscription {topicId}/{subId}.");
                }

                return new SubscriptionClient(serviceBusConnString, topicId, subId);
            }
        }

        static Message CreateSBMessage(string message, string userPropsString)
        {
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Message required to write to service bus");
            }

            var sbMessage = new Message(Encoding.UTF8.GetBytes(message));


            if (!string.IsNullOrEmpty(userPropsString))
            {
                var userPropStrings = userPropsString.Split(',');

                IDictionary<string, object> userPropKVs = new Dictionary<string, object>();

                foreach (var userPropString in userPropStrings)
                {
                    var keyAndVal = userPropString.Split(':');
                    if (keyAndVal.Length != 2)
                    {
                        throw new ArgumentException("User Properties argument is malformed please check format and try again.");
                    }

                    userPropKVs.Add(keyAndVal[0], keyAndVal[1]);
                }

                foreach (var keyVal in userPropKVs)
                {
                    sbMessage.UserProperties[keyVal.Key] = keyVal.Value;
                }
            }

            return sbMessage;
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }
    }
}
