using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusCLI.CommandLineOptions
{
    public class ReadSubOptions : CommonSubOptions
    {
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
    }
}
