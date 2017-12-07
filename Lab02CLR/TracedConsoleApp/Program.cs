using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using NetMastery.Lab02CLR.Formatters.BuiltInFormatters;
using NetMastery.Lab02CLR.TracedConsoleApp.Properties;
using NetMastery.Lab02CLR.TracerLibrary;

namespace NetMastery.Lab02CLR.TracedConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(Assembly.GetCallingAssembly().FullName);
                Console.WriteLine();
                SetCurrentDirectory();
                string filePath = null;
                try
                {
                    filePath = ConfigurationManager.AppSettings["FilePathForPlugin"];
                }
                catch (ConfigurationErrorsException)
                {
                    Console.WriteLine(Strings.DirectoryNotFoundExceptionForPlugins);
                }
                var formatters = GetAvailableFormatters(filePath);
                var cmdOptions = new Options(formatters);
                while (true)
                {
                    cmdOptions.Cmd.Execute(args);
                    if (cmdOptions.ArgHelp.HasValue())
                    {
                        cmdOptions.Cmd.ShowHelp();
                        char[] separators = {' '};
                        Console.WriteLine(Strings.InputNote);
                        var arguments = Console.ReadLine();
                        args = arguments.Split(separators);
                    }
                    else
                    {
                        cmdOptions.ArgOutput.Value();
                        TestMethod1();
                        var secondThread = new Thread(TestMethod1);
                        secondThread.Start();
                        secondThread.Join();
                        TestMethod3("Finished");
                        try
                        {
                            if (cmdOptions.ArgFormat.Value() == "console")
                            {
                                var writer = new ConsoleWriter(formatters["console"]);
                                writer.WriteResult(Tracer.Instance.GetTraceResult());
                            }
                            else
                            {
                                var writer = new FileWriter(formatters[cmdOptions.ArgFormat.Value()],
                                      cmdOptions.ArgOutput.Value());
                                writer.WriteResult(Tracer.Instance.GetTraceResult());
                                Console.WriteLine(Strings.SuccessWriting);
                            }
                        }
                        catch (IOException)
                        {
                            Console.WriteLine(Strings.DirectoryNotFoundExceptionForOutput);
                        }
                        break;
                    }
                }
            }
            catch (CommandParsingException e)
            {
                e.Command.ShowHelp();
                Console.WriteLine(e.Message);
            }
            catch (Exception)
            {
                Console.WriteLine(Strings.UndefinedExceptionForMethod);
            }
            Console.WriteLine(Strings.FinishedAppNote);
            Console.ReadKey();
        }

        public static void TestMethod1()
        {
            Tracer.Instance.StartTrace();
            double result = 0;
            for (var i = 0; i < 1000000; i++)
            {
                result += Math.Sqrt(i);
            }
            TesMethod2(result);
            Tracer.Instance.StopTrace();
        }

        public static void TesMethod2(double res)
        {
            Tracer.Instance.StartTrace();
            for (var i = 0; i < 1000000; i++)
            {
                res -= Math.Pow(i, 2);
            }
            Tracer.Instance.StopTrace();
        }

        public static void TestMethod3(string text)
        {
            Tracer.Instance.StartTrace();
            Thread.Sleep(5000);
            Tracer.Instance.StopTrace();

        }

        public static IDictionary<string, ITraceResultFormatter> GetAvailableFormatters(string path)
        {
            IDictionary<string, ITraceResultFormatter> availableFormatters = new Dictionary<string, ITraceResultFormatter>();
            availableFormatters.Add("console", new ConsoleTraceResultFormatter());
            availableFormatters.Add("xml", new XmlTraceResultFormatter());
            if (path == null) return availableFormatters;
            try
            {
                PluginsLoader.LoadPlugins(path, availableFormatters);
            }
            catch (Exception)
            {
                Console.WriteLine(Strings.UndefinedExceptionForPluginsLoader);
            }
            return availableFormatters;
        }

        private static void SetCurrentDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var directoryPath = Path.GetDirectoryName(path);
            if (directoryPath != null)
            {
                Directory.SetCurrentDirectory(directoryPath);
            }
        }
    }
}
