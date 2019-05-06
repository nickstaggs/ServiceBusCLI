using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusCLI.CommandLineOptions
{
    public abstract class CommonSubOptions
    {
        [Option(
            shortName: 'c',
            longName: "serviceBusConnString",
            Required = true,
            HelpText = "Connection string to the service bus")]
        public string ServiceBusConnString { get; set; }

        [Option(
            shortName: 'v',
            longName: "verbose",
            Required = false,
            Default = false,
            HelpText = "Prints all logging to standard output.")]
        public bool Verbose { get; set; }

        [Option(
            shortName: 't',
            longName: "topic",
            Required = false,
            HelpText = "Topic ID")]
        public string Topic { get; set; }

        [Option(
            shortName: 'q',
            longName: "queue",
            Required = false,
            HelpText = "Queue ID")]
        public string Queue { get; set; }

        [Option(
            shortName: 's',
            longName: "subscription",
            Required = false,
            HelpText = "Subscription ID")]
        public string Subscription { get; set; }

        [Option(
            shortName: 'p',
            longName: "peek",
            Required = false,
            Default = false,
            HelpText = "Peek messages instead of reading and taking them off the queue/topic/subscription")]
        public bool Peek { get; set; }
    }
}
