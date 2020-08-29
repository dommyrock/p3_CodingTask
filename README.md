# p3_CodingTask
### coding task issued by company

## Project setup instructions:

#### Backend: (.NET Core 3.1 Web App)

### DB:
AWS S3

### To deploy project :
Instal .NET AWS SDK

#### Right click Web app --> Publish to AWS Elastic Beanstalk --> Follow publish wizard
*setup App Environment
*setup options
*setup permisions(aws Roles)
*setup tracing options
*Review Deployment options


### Useful Resources

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
