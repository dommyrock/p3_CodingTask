using Microsoft.AspNetCore.Http;

namespace p3CodingTask.Models
{
    public class FileModel
    {
        //NOTE : when using [FromForm], names of props == param names in FormData( on client side !)

        public IFormFileCollection Files { get; set; }
        public string FolderPath { get; set; }
    }
}