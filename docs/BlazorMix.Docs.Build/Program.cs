using CommandLine.Text;
using CommandLine;
using System;
using System.IO;
using System.Collections.Generic;
using GTranslate.Translators;
using GTranslate;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

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

        [Option('t', "type", Required = false, HelpText = "output type")]
        public string Type { get; set; } = "full";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            //var translator = new AggregateTranslator();
            //var result = await translator.TranslateAsync("是否渲染为块级元素", "en", "zh-cn");
            //Console.WriteLine($"Translation: {result.Translation}");
            //Console.WriteLine($"Source Language: {result.SourceLanguage}");
            //Console.WriteLine($"Target Language: {result.TargetLanguage}");
            //Console.WriteLine($"Service: {result.Service}");

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
