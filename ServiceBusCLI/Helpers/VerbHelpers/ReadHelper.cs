using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using ServiceBusCLI.CommandLineOptions;
using ServiceBusCLI.Helpers.ResourceHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServiceBusCLI.Helpers.VerbHelpers
{
    public static class ReadHelper
    {
        public static bool Verbose { get; private set; } = false;
        public static IReceiverClient Client { get; set; }

        public static void Init(CommonSubOptions o)
        {
            ArgumentHelper.ValidateArguments(o);

            if (o.Verbose)
            {
                Verbose = true;
            }

            if (Client == null)
            {
                Client = (IReceiverClient)ServiceBusHelper.GetClient(o.ServiceBusConnString, o.Queue, o.Topic, o.Subscription, o.Peek, Verbose);
            }
        }

        public static int Read(ReadSubOptions opts)
        {
            try
            {
                Init(opts);
                ArgumentHelper.ValidateArguments(opts);
            }
            catch(ArgumentException e)
            {
                return 1;
            }

            System.Timers.Timer timer = new System.Timers.Timer();
            System.Timers.Timer logTimer = new System.Timers.Timer() { Interval = 2000, AutoReset = true };

            Client.PrefetchCount = opts.NumberOfMessages;

            int n = 0;
            bool isTimeUp = false;

            timer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) => { isTimeUp = true; };
            logTimer.Elapsed += (object source, System.Timers.ElapsedEventArgs e) => {
                if (Verbose)
                {
                    Console.WriteLine("Waiting for messages...");
                }
            };

            if (opts.TimeToWait >= 0)
            {
                timer.Interval = opts.TimeToWait * 1000;
                timer.Start();
            }

            logTimer.Start();
            
            try
            {
                Client.RegisterMessageHandler(async (Message message, CancellationToken token) =>
                {
                    if (Verbose)
                    {
                        Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber}");
                    }

                    Console.WriteLine(Encoding.UTF8.GetString(message.Body));

                    if (!opts.Peek)
                    {
                        await Client.CompleteAsync(message.SystemProperties.LockToken);
                    }

                    n++;

                }, ServiceBusHelper.MessageHandlerOptions);

                while (n < opts.NumberOfMessages && !isTimeUp) { }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ooops, trying to read messages failed: {e}");
                return 1;
            }
            finally
            {
                Client.CloseAsync();
                timer.Stop();
                logTimer.Stop();
            }

            return 0;
        }
    }
}
