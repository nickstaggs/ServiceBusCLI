using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusCLI.CommandLineOptions
{
    public class WriteSubOptions
    {
        [Option(
            shortName: 'm',
            longName: "message",
            Required = false,
            HelpText = "Message to send to queue/topic")]
        public string Message { get; set; }

        [Option(
            shortName: 'u',
            longName: "userProperties",
            Required = false,
            HelpText = "User properties to be placed on message to be written to service bus, format: <k1>:<v1>,<k2>:<v2>,...,<kn>:<vn> no spaces")]
        public string UserProperties { get; set; }
    }
}
