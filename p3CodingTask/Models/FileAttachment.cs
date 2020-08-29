using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace p3CodingTask.Models
{
    public class FileAttachment
    {
        public string FileName { get; set; }
        public int? FileSize { get; set; }
        public string MimeType { get; set; }
        public string FileUrl { get; set; }
        public string ServerRelativeUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string Body { get; set; }
    }
}