using System.Net;

namespace p3CodingTask.Models
{
    public class S3Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}