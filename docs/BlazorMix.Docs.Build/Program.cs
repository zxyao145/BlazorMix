using CommandLine.Text;
using CommandLine;
using System;
using System.IO;
using System.Collections.Generic;

namespace BlazorMix.Docs.Build
{
    public class Options
    {
        [Option('c', "command", Required = true, HelpText = "command.")]
        public string Cmd { get; set; }


        [Option('s', "src", Required = true, HelpText = "source directory.")]
        public string Src { get; set; }

        [Option('o', "out", Required = true, HelpText = "output directory.")]
        public string Out { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(RunOptions)
               .WithNotParsed(HandleParseError);
        }

        static void RunOptions(Options opts)
        {
            var cmd = new GenDemoDocs(opts);
            cmd.Run();
            //handle options
        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            //handle errors
        }
    }
}
