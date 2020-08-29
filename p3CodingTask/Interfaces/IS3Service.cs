using Microsoft.AspNetCore.Http;
using p3CodingTask.Models;
using System.Threading.Tasks;

namespace p3CodingTask.Interfaces
{
    public interface IS3Service
    {
        public Task<S3Response> CreateBucketAsync(string bucketName);

        public Task<S3Response> CreateFolderAsync(string folderName);

        public Task<S3Response> UploadFilesAsync(IFormFileCollection files, string folderUrl);

        public Task<FileAttachments> GetFolderContentsAsync(string folderUrl);

        public Task<S3Response> DeleteEntityAsync(string folderUrl);

        public Task<FileAttachments> SearchTopNAsync(string query);
    }
}