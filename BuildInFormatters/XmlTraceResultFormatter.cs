﻿using FormatterPluginContract;
using ModelData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BuildInFormatters
{
    public class XmlTraceResultFormatter : ITraceResultFormatter
    {
        public XmlDocument XmlDoc { get; set; }
        public string FlagValue { get => "xml"; }

        public XmlTraceResultFormatter()
        {
            XmlDoc = new XmlDocument();
        }
        public void Format(ITraceResult traceResult)
        {
            XmlElement root = XmlDoc.CreateElement("root");
            XmlElement thread = XmlDoc.CreateElement("thread");
            thread.SetAttribute("id", traceResult.ThreadId.ToString());
            thread.SetAttribute("time", traceResult.OverallTime.ToString("g"));
            root.AppendChild(thread);
            XmlDoc.AppendChild(root);
            CreateMethodNode(thread, traceResult.Root);
            
        }

        private void CreateMethodNode(XmlElement parentNode, IList<IMethodNode> parentMethod)
        {
            int i = 0;
            while (parentMethod.Count > i)
            {
                XmlElement method = XmlDoc.CreateElement("method");
                method.SetAttribute("name", parentMethod[i].MethodName);
                method.SetAttribute("time", parentMethod[i].ExecutionTime.ToString("g"));
                method.SetAttribute("class", parentMethod[i].ClassName);
                method.SetAttribute("params", parentMethod[i].ParametrCounts.ToString());
                parentNode.AppendChild(method);
                if (parentMethod[i].ChildNodes.Count != 0)
                {
                    CreateMethodNode(method, parentMethod[i].ChildNodes);
                }
                i++;
            }
        }


        public string GetFormat()
        {
            using (var stringWriter = new StringWriter())
            {
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    XmlDoc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    return stringWriter.GetStringBuilder().ToString();
                }
            }
        }
    }
}