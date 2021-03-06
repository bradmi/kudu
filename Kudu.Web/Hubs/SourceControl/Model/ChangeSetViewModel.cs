﻿using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Kudu.Core.SourceControl;

namespace Kudu.Web.Model {
    public class ChangeSetViewModel {
        public string Id { get; set; }
        public string ShortId { get; set; }
        public string AuthorName { get; set; }
        public string EmailHash { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
        public string Summary { get; set; }
        public bool Active { get; set; }

        public ChangeSetViewModel(ChangeSet changeSet) {
            Id = changeSet.Id;
            ShortId = changeSet.Id.Substring(0, 12);
            AuthorName = changeSet.AuthorName;
            EmailHash = String.IsNullOrEmpty(changeSet.AuthorEmail) ? null : Hash(changeSet.AuthorEmail);
            Date = changeSet.Timestamp.ToString("u");
            Message = Process(changeSet.Message);
            // Show first line only
            var reader = new StringReader(changeSet.Message);
            Summary = Process(Trim(reader.ReadLine(), 300));
        }

        private string Trim(string value, int max) {
            if (String.IsNullOrEmpty(value)) {
                return value;
            }

            if (value.Length > max) {
                return value.Substring(0, max) + "...";
            }
            return value;
        }

        private string Process(string value) {
            if (String.IsNullOrEmpty(value)) {
                return value;
            }
            return value.Trim().Replace("\n", "<br/>");
        }

        private string Hash(string value) {
            return String.Join(String.Empty, MD5.Create()
                     .ComputeHash(Encoding.Default.GetBytes(value))
                     .Select(b => b.ToString("x2")));
        }
    }
}