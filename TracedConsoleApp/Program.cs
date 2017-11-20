using BuildInFormatters;
using FormatterPluginContract;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection.Emit;
using TracerLib;
using System.IO;

namespace TracedConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(AssemblyBuilder.GetCallingAssembly().FullName);
            Console.WriteLine();
            string filePath = $@"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName}\Plugins\";
            IDictionary<string, ITraceResultFormatter> formatters = GetAwailableFormatters(filePath);
            Options CmdOptions;
            while (true)
            {
                CmdOptions = new Options(formatters);
                try
                {
                    CmdOptions.Cmd.Execute(args);
                    if (CmdOptions.ArgHelp.HasValue())
                    {
                        CmdOptions.Cmd.ShowHelp();
                        char[] separators = { ' ' };
                        Console.WriteLine("Please, enter options");
                        string arguments = Console.ReadLine();
                        args = arguments.Split(separators);
                        continue;
                    } 
                }
                catch (CommandParsingException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
                    Console.ReadKey();
                    return;
                }
                break;
            }
            Writer writer = new Writer(formatters[CmdOptions.ArgFormat.Value()], CmdOptions.ArgOutput.Value());
            TestMethod1();
            Thread SecondThread = new Thread(TestMethod1);
            SecondThread.Start();
            TestMethod3("Finished");
            writer.WriteResult(Tracer.Instance.GetTraceResult());
            Console.WriteLine("Нажмите любую клавишу для завершения работы приложения.");
            Console.ReadKey();
        }

        public static void TestMethod1()
        {
            Tracer.Instance.StartTrace();
            double result = 0;
            for (int i = 0; i < 10000; i++)
            {
                result += Math.Sqrt(i);
            }
            TesMethod2(result);
            Tracer.Instance.StopTrace();
        }

        public static void TesMethod2(double res)
        {
            Tracer.Instance.StartTrace();
            for (int i = 0; i < 10000; i++)
            {
                res -= Math.Pow(i, 2);
            }
            Tracer.Instance.StopTrace();
        }

        public static string TestMethod3(string text)
        {
            Tracer.Instance.StartTrace();
            try
            {
                return $"{text}";
            }
            finally
            {
                Tracer.Instance.StopTrace();
            }
        }

        public static IDictionary<string, ITraceResultFormatter> GetAwailableFormatters(string path)
        {
            IDictionary<string, ITraceResultFormatter> AwailableFormatters = new Dictionary<string, ITraceResultFormatter>();
            AwailableFormatters.Add("console", new ConsoleTraceResultFormatter());
            AwailableFormatters.Add("xml", new XmlTraceResultFormatter());
            PluginsLoader.LoadPlugins(path, AwailableFormatters);
            return AwailableFormatters;
        }

    }
}
