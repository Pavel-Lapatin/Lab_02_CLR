using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using NetMastery.Lab02CLR.TracedConsoleApp.Properties;

namespace NetMastery.Lab02CLR.TracedConsoleApp
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
            ArgFormat = Cmd.Option(Strings.FormatOptions, Strings.FormatHelp, CommandOptionType.SingleValue);
            ArgOutput = Cmd.Option(Strings.OutputOptions, Strings.OutputHelp, CommandOptionType.SingleValue);
            ArgHelp = Cmd.Option(Strings.HelpOptions, Strings.HelpHelp, CommandOptionType.NoValue);

            var allowedFormats = new StringBuilder();
            allowedFormats.Append(Strings.AllowedFormattsBegin);
            foreach (var key in formatters.Keys)
            {
                allowedFormats.Append($"{key}, ");
            }
            allowedFormats.Length = (allowedFormats.Length - 2);
            Cmd.ExtendedHelpText = allowedFormats.ToString();

            Cmd.OnExecute(() =>
            {
                if (ArgHelp.HasValue())  return 0;
                if (ArgFormat.Value() == null)
                {
                    Cmd.ShowHelp();
                    throw new CommandParsingException(Cmd, "");
                }
                if (ArgFormat.Value().Equals("console") && ArgOutput.Values.Count != 0) ArgOutput.Values[0]= null;
                if (!formatters.Keys.Contains(ArgFormat.Value()))
                {
                    throw new CommandParsingException(Cmd, $"{allowedFormats}");
                }
                return 0;
            });
        }
    }
}
