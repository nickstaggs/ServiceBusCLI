using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using ServiceBusCLI.CommandLineOptions;
using ServiceBusCLI.Helpers.ResourceHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusCLI.Helpers.VerbHelpers
{
    public static class WriteHelper
    {

        public static bool Verbose { get; private set; } = false;
        public static ISenderClient client;

        public static void Init(CommonSubOptions o)
        {
            ArgumentHelper.ValidateArguments(o);

            if (o.Verbose)
            {
                Verbose = true;
            }

            client = (ISenderClient)ServiceBusHelper.GetClient(o.ServiceBusConnString, o.Queue, o.Topic, o.Subscription, o.Peek, Verbose);
        }

        public static int Write(WriteSubOptions opts)
        {
            try
            {
                Init(opts);
                ArgumentHelper.ValidateArguments(opts);
            }
            catch (ArgumentException e)
            {
                return 1;
            }

            Task.Run(async () => {
                var message = CreateSBMessage(opts.Message, opts.UserProperties);

                await WriteMessage(message);
            }).GetAwaiter().GetResult();

            return 0;
        }

        public static async Task WriteMessage(Message message)
        {
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
    }
}
