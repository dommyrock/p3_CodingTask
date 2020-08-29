using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace p3CodingTask.Models
{
    public class FileAttachments
    {
        public ICollection<FileAttachment> Files { get; set; }
        public ICollection<FolderModel> Folders { get; set; }
    }
}