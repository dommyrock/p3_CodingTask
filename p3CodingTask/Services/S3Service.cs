using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Http;
using p3CodingTask.Interfaces;
using p3CodingTask.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace p3CodingTask.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;

        //fileshare s3 bucket
        private const string _bucketName = "fileShareS3";

        public S3Service(IAmazonS3 client)
        {
            _s3Client = client;
        }

        /// <summary>
        /// Creates enw S3 bucket with CORS to my site
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task<S3Response> CreateBucketAsync(string bucketName)
        {
            try
            {
                //Check if bucket already exists
                if (!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName))
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };

                    var response = await _s3Client.PutBucketAsync(putBucketRequest);

                    //Enable CORS For bucket
                    S3Cors cors = new S3Cors(bucketName);

                    return new S3Response
                    {
                        Message = response.ResponseMetadata.RequestId,
                        StatusCode = response.HttpStatusCode
                    };
                }
            }
            catch (AmazonS3Exception ex)
            {
                return new S3Response
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new S3Response
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Message = "Error on the server side"
            };
        }

        /// <summary>
        /// Create empty folder in root bucket
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public async Task<S3Response> CreateFolderAsync(string folderName)
        {
            var s3Response = new S3Response();
            try
            {
                // 1. Put object-specify only key name for the new object.
                var folder = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = folderName,
                    //FilePath = ""NOTE this is only neaded if i want to upload from my local machine (in my case i only want uploads from web)
                };

                PutObjectResponse response = await _s3Client.PutObjectAsync(folder);

                s3Response.StatusCode = response.HttpStatusCode;
                return s3Response;
            }
            catch (AmazonS3Exception e)
            {
                s3Response.Message = e.Message;
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);
            }
            catch (Exception e)
            {
                s3Response.Message = e.Message;
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
            }

            s3Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            return s3Response;
        }

        /// <summary>
        /// Upload files to specified path in s3 bucket
        /// </summary>
        /// <param name="files"></param>
        /// <param name="folderUrl">Relative folder location where to upload files</param>
        /// <returns></returns>
        /// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/dev/HLuploadFileDotNet.html"/>
        public async Task<S3Response> UploadFilesAsync(IFormFileCollection files, string folderUrl)//NOTE:
        {
            var s3Response = new S3Response();

            //NOTE folderUrl == Key in my case
            var fileTransferUtility = new TransferUtility(_s3Client);

            try
            {
                foreach (var file in files)
                {
                    Stream str = file.OpenReadStream();

                    await fileTransferUtility.UploadAsync(str, _bucketName, folderUrl);
                }
            }
            catch (AmazonS3Exception e)
            {
                s3Response.Message = e.Message;

                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                s3Response.Message = e.Message;

                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            s3Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return s3Response;
        }

        /// <summary>
        /// Get all files/folders at specified folder location
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <returns></returns>
        /// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/dev/RetrievingObjectUsingNetSDK.html"/>
        /// <seealso cref="https://docs.aws.amazon.com/AmazonS3/latest/dev/ListingObjectKeysUsingNetSDK.html"/>
        public async Task<FileAttachments> GetFolderContentsAsync(string folderUrl)
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = folderUrl //ex. folder3/nestedfolder1/
                };

                using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Object metadata, Title: {0}", title);
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd(); // Now you process the response body.
                }
                //TODO; mapp read data to my model FileAttachments
            }
            catch (AmazonS3Exception e)
            {
                // If bucket or object does not exist
                Console.WriteLine("Error encountered ***. Message:'{0}' when reading object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when reading object", e.Message);
            }

            return new FileAttachments();
        }

        /// <summary>
        /// Pull out N number of files/folders matching the query param
        /// </summary>
        /// <param name="folderUrl"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <see cref="https://docs.aws.amazon.com/AmazonS3/latest/dev/ListingObjectKeysUsingNetSDK.html"/>
        public async Task<FileAttachments> SearchTopNAsync(string query)
        {
            try
            {
                //TODO: after i fetch all keys (which are file names ) filter them by query , than stream only top N matching files/folders

                //NOTE : this gets all the filles/and folder in bucket
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    //MaxKeys = 10 limit number of returned files/folders
                };
                ListObjectsV2Response response;
                do
                {
                    response = await _s3Client.ListObjectsV2Async(request);

                    var allObjects = response.S3Objects;

                    var filtered = allObjects.Where(x => x.Key == query);

                    // Process the response. NOTE:(single oblect could containt N folders/files)
                    foreach (S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
                    }
                    Console.WriteLine("Next Continuation Token: {0}", response.NextContinuationToken);
                    request.ContinuationToken = response.NextContinuationToken; //paginate all returned objects if needed
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                Console.ReadKey();
            }

            return new FileAttachments();
        }

        public async Task<S3Response> DeleteEntityAsync(string folderUrl)
        {
            var s3Response = new S3Response();

            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = folderUrl
                };

                Console.WriteLine("Deleting an object");
                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                s3Response.Message = e.Message;
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
            catch (Exception e)
            {
                s3Response.Message = e.Message;
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }

            s3Response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            return s3Response;
        }
    }
}