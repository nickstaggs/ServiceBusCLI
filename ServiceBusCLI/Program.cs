using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ServiceBusCLI.CommandLineOptions;
using ServiceBusCLI.Helpers.VerbHelpers;

namespace ServiceBusCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ReadSubOptions, WriteSubOptions>(args)
                .MapResult(
                    (WriteSubOptions opts) => WriteHelper.Write(opts),
                    (ReadSubOptions opts) => ReadHelper.Read(opts),
                    errs => 1);
        }
    }
}
