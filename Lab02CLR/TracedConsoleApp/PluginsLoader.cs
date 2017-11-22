using NetMastery.Lab02CLR.Formatters.FormatterPluginContract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using NetMastery.Lab02CLR.TracedConsoleApp.Properties;


namespace NetMastery.Lab02CLR.TracedConsoleApp
{
    internal class PluginsLoader
    {
        public static IDictionary<string, ITraceResultFormatter> LoadPlugins(string path, IDictionary<string, ITraceResultFormatter> availableFormatters)
        {
            if (availableFormatters == null)
            {
                availableFormatters = new ConcurrentDictionary<string, ITraceResultFormatter>();
            }
            if (Directory.Exists(path))
            {
                try
                {
                    var dllFileNames = Directory.GetFiles(path, "*.dll");
                    ICollection<Assembly> assemblies = new List<Assembly>();
                    foreach (var dllFile in dllFileNames)
                    {
                        try
                        {
                            var assemblyName = AssemblyName.GetAssemblyName(dllFile);
                            var assembly = Assembly.Load(assemblyName);
                            assemblies.Add(assembly);
                        }
                        catch (FileLoadException e)
                        {
                            Console.WriteLine($"{Strings.AssemblyLoadException} {e.Source}");
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine($"{Strings.AssemblyLoadException} {e.Source}");
                        }
                        catch (BadImageFormatException e)
                        {
                            Console.WriteLine($"{Strings.AssemblyLoadException} {e.Source}");
                        }
                        catch (SecurityException e)
                        {
                            Console.WriteLine($"{Strings.UnathorizedException} {e.Source}");
                        }
                    }
                    var pluginType = typeof(ITraceResultFormatter);
                    ICollection<Type> pluginTypes = new List<Type>();
                    foreach (var assembly in assemblies)
                    {
                        if (assembly != null)
                        {
                            try
                            {
                                foreach (var type in assembly.GetTypes())
                                {
                                    try
                                    {
                                        if (!(type.IsInterface || type.IsAbstract) &&
                                            type.GetInterface(pluginType.FullName) != null)
                                        {
                                            pluginTypes.Add(type);
                                        }
                                    }
                                    catch (ReflectionTypeLoadException)
                                    {
                                        Console.WriteLine(Strings.PluginTypeAddException);
                                    }
                                    catch (AmbiguousMatchException)
                                    {
                                        Console.WriteLine(Strings.PluginTypeAddException);
                                    }
                                }
                            }
                            catch(ReflectionTypeLoadException)
                            {
                                Console.WriteLine(Strings.ReflectionTypesException);
                            };
                        }
                    }
                    foreach (var type in pluginTypes)
                    {
                        try
                        {
                            var plugin = (ITraceResultFormatter) Activator.CreateInstance(type);
                            availableFormatters.Add(plugin.FlagValue, plugin);
                            
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine();
                        }
                        catch (MemberAccessException e)
                        {
                            Console.WriteLine($"{Strings.PluginTypeAddException} {e.Source}");
                        }
                        catch (TypeLoadException e)
                        {
                            Console.WriteLine($"{Strings.PluginTypeAddException} {e.Source}");
                        }
                        catch (TargetException e)
                        {
                            Console.WriteLine($"{Strings.PluginTypeAddException} {e.Source}");
                        }
                        catch (InvalidComObjectException e)
                        {
                            Console.WriteLine($"{Strings.PluginTypeAddException} {e.Source}");
                        }
                    }
                    return availableFormatters;
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine(Strings.UnathorizedException);
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine(Strings.DirectoryException);
                }
                catch (PathTooLongException)
                {
                    Console.WriteLine(Strings.PathTooLong);
                }
            }
            return availableFormatters;
        }
    }
}
