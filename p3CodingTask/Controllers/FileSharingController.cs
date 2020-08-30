using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using p3CodingTask.Interfaces;
using p3CodingTask.Models;
using System.Threading.Tasks;

namespace p3CodingTask.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/fileshare")]
    [Produces("application/josn")]
    public class FileSharingController
    {
        private readonly IS3Service _s3Service;

        public FileSharingController(IS3Service service)
        {
            _s3Service = service;
        }

        //POST: api/fileshare/create-bucket/name
        [HttpPost("create-bucket/{name}", Name = "CreateBucket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<S3Response> CreateBucket([FromRoute] string name)
        {
            var response = await _s3Service.CreateBucketAsync(name);

            return response;
        }

        //POST: api/fileshare/new-folder?path=""
        [HttpPost("new-folder", Name = "CreateFolder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<S3Response> CreateFolder([FromQuery] string path)
        {
            var response = await _s3Service.CreateFolderAsync(path);

            return response;
        }

        //POST: api/fileshare/files
        [HttpPost("files", Name = "UploadFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<S3Response> UploadFiles([FromForm] FileModel model)
        {
            var response = await _s3Service.UploadFilesAsync(model.Files, model.FolderUrl);

            return response;
        }

        // GET: api/fileshare/folder?url=""
        [HttpGet("folder", Name = "GetFolderContents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<FileAttachments> GetFolderContents([FromQuery]string url)
        {
            var result = await _s3Service.GetFolderContentsAsync(url);

            return result;
        }

        // DELETE: api/fileshare/delete?url=""
        [HttpDelete("delete", Name = "DeleteEntity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<S3Response> DeleteContent([FromQuery]string url)
        {
            var result = await _s3Service.DeleteEntityAsync(url);

            return result;
        }

        // GET: api/fileshare/query?url=""
        [HttpGet("query", Name = "SearchFiles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<FileAttachments> SearchTopN([FromQuery]string query)
        {
            var result = await _s3Service.SearchTopNAsync(query);

            return result;
        }

        // GET: api/fileshare/share?url=""
        [HttpGet("share", Name = "ShareResource")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string ShareResource([FromQuery]string url)
        {
            var result = _s3Service.ShareResource(url);

            return result;
        }
    }
}

/*
   > Create folders and subfolders
    ● Create new files in the folders
    ● Delete folders and files
    ● Search files by its exact name within a parent folder or across all files
    List the top 10 files that start with a search string.

    This will be used in the search box to show possible matches when the user is
    typing. Only “start with” logic is required.

    For the purpose of this exercise, you can assume that a file is simply its name and
    does not contain any other content.
    Please provide a README with instructions on how to deploy your application!!!

 *--------------------------------------------------
 * Publish to  AWS Elastic Beanstalk:
 * 1 Right click WEb App --> Publiish to  AWS Elastic Beanstalk
 * 2 Follow deployment wizard
 *----------------------------------------------------
 *
 * Resources:
 * https://docs.aws.amazon.com/AmazonS3/latest/user-guide/using-folders.html
 * An object that is named with a trailing "/" appears as a folder in the Amazon S3 console. ( examplekeyname/)
 *
 * Upload File/folder
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/UploadObjSingleOpNET.html
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadFileDotNet.html
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/LLuploadFileDotNet.html
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadDirDotNet.html
 *
 * Get
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/RetrievingObjectUsingNetSDK.html
 *
 * Delete
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/DeletingMultipleObjectsUsingNetSDK.html
 *
 * CORS
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/ManageCorsUsingDotNet.html
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/cors.html
 * https://docs.aws.amazon.com/AmazonS3/latest/user-guide/add-cors-configuration.html
 *
 * Storage classes
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/storage-class-intro.html
 *
 * Versioning
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/ObjectVersioning.html
 *
 * multipart/Upload limits
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/qfacts.html
 *
 * Performance
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/optimizing-performance.html
 *------------------------------------
 * https://docs.aws.amazon.com/AmazonS3/latest/dev/transfer-acceleration-examples.html
 * https://stacks.wellcomecollection.org/creating-a-data-store-from-s3-and-dynamodb-8bb9ecce8fc1
 */