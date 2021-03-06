﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Kudu.Core.Deployment {
    public class XmlLogger : ILogger {
        private readonly string _path;

        public XmlLogger(string path) {
            _path = path;
        }

        public void Log(string value, LogEntryType type) {
            XDocument document = GetDocument();

            document.Root.Add(new XElement("entry",
                                  new XAttribute("time", DateTime.Now),
                                  new XAttribute("type", (int)type),
                                  value));

            document.Save(_path);
        }

        public IEnumerable<LogEntry> GetLogEntries() {
            XDocument document = GetDocument();

            return from e in document.Root.Elements("entry")
                   let time = DateTime.Parse(e.Attribute("time").Value)
                   let type = (LogEntryType)Int32.Parse(e.Attribute("type").Value)
                   select new LogEntry(time, e.Value, type);
        }

        private XDocument GetDocument() {
            if (!File.Exists(_path)) {
                return new XDocument(new XElement("entries"));
            }

            XDocument document;
            using (var stream = File.OpenRead(_path)) {
                document = XDocument.Load(stream);
            }
            return document;
        }
    }
}
