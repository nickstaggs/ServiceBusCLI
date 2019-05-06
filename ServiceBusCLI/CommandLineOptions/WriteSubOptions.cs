using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusCLI.CommandLineOptions
{
    [Verb("write", HelpText = "Writes messages to a service bus queue or topic")]
    public class WriteSubOptions : CommonSubOptions
    {
        [Option(
            shortName: 'm',
            longName: "message",
            Required = true,
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
