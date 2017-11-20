using FormatterPluginContract;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TracedConsoleApp
{
    public class Options
    {
        public CommandOption ArgFormat { get; set; }
        public CommandOption ArgOutput { get; set; }
        public CommandOption ArgHelp { get; set; }
        public CommandLineApplication Cmd { get; set; }

        public Options(IDictionary<string, ITraceResultFormatter> formatters)
        {
            Cmd = new CommandLineApplication();
            ArgFormat = Cmd.Option("--f | -format <format>", "Format used for represent programm result.", CommandOptionType.SingleValue);
            ArgOutput = Cmd.Option("--o | -output <output>", "The path to the file for saving program result.", CommandOptionType.SingleValue);
            ArgHelp = Cmd.Option("--h | -help", "Gets help for options", CommandOptionType.NoValue);
            StringBuilder allowedFormats = new StringBuilder();
            allowedFormats.Append($"Following output formats are allowed: ");
            foreach (var key in formatters.Keys)
            {
                allowedFormats.Append($"{key}, ");
            }
            allowedFormats[allowedFormats.Length - 2] = ' ';
            Cmd.ExtendedHelpText = allowedFormats.ToString();

            Cmd.OnExecute(() =>
            {
                if (ArgHelp.HasValue())  return 0;
                if (ArgFormat.Value() != null && ArgFormat.Value().Equals("console") && ArgOutput.Values.Count != 0) ArgOutput.Values[0]= null;
                if (ArgFormat.Value() == null) throw new CommandParsingException(Cmd, "Format haven't been entered");
                if (!formatters.Keys.Contains(ArgFormat.Value()))
                {
                    throw new CommandParsingException(Cmd, $"{ArgFormat.Value()} doesn't present in allowed formats\n" +
                        $"{allowedFormats.ToString()}");
                }
                return 0;
            });
        }
    }
}
