using FormatterPluginContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TracedConsoleApp
{
    class PluginsLoader
    {
        public static IDictionary<string, ITraceResultFormatter> LoadPlugins(string path, IDictionary<string, ITraceResultFormatter> awailableFormatters)
        {
            string[] dllFileNames = null;

            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(assemblyName);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(ITraceResultFormatter);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        try
                        {
                            foreach (Type type in assembly.GetTypes())
                            {
                                if (type.IsInterface || type.IsAbstract)
                                {
                                    continue;
                                }
                                else
                                {
                                    if (type.GetInterface(pluginType.FullName) != null)
                                    {
                                        pluginTypes.Add(type);
                                    }
                                }
                            }
                        }
                        catch (ReflectionTypeLoadException e)
                        {
                            Console.WriteLine($"Error is raised in {e.Source}");
                            continue;
                        }
                    }
                }

                foreach (Type type in pluginTypes)
                {
                    ITraceResultFormatter plugin = (ITraceResultFormatter)Activator.CreateInstance(type);
                    awailableFormatters.Add(plugin.FlagValue, plugin);
                }
                return awailableFormatters;
            }
            return null;
        }
    }
}
