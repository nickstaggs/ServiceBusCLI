using ServiceBusCLI.CommandLineOptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusCLI.Helpers
{
    public static class ArgumentHelper
    {
        public static void ValidateArguments(CommonSubOptions options)
        {
            if (string.IsNullOrEmpty(options.Queue) && string.IsNullOrEmpty(options.Topic))
            {
                Console.WriteLine("Please provide queue or topic name");
                throw new ArgumentException();
            }

            if (!string.IsNullOrEmpty(options.Queue) && !string.IsNullOrEmpty(options.Topic))
            {
                Console.WriteLine("Please provide queue or topic name. Not both.");
                throw new ArgumentException();
            }
        }

        public static void ValidateArguments(ReadSubOptions options)
        {
            if (!string.IsNullOrEmpty(options.Topic) && string.IsNullOrEmpty(options.Subscription))
            {
                Console.WriteLine("You can not read straight from a topic, please provide subscription listening on that topic with -s arg");
                throw new ArgumentException();
            }
        }

        public static void ValidateArguments(WriteSubOptions options)
        {
            if (!string.IsNullOrEmpty(options.Subscription))
            {
                Console.WriteLine("You can not write to a subscription, please remove subscription argument");
                throw new ArgumentException();
            }
        }
    }
}
