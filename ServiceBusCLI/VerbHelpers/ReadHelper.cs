using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServiceBusCLI.VerbHelpers
{
    public static class ReadHelper
    {

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
    }
}
