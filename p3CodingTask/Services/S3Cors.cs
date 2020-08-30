using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace p3CodingTask.Services
{
    public class S3Cors
    {
        private static IAmazonS3 _s3Client;
        private static string _bucketName { get; set; }
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.EUCentral1;

        public S3Cors(string bucketName)
        {
            _bucketName = bucketName;
            _s3Client = new AmazonS3Client(bucketRegion);
            CreateCORSConfig().Wait();
        }

        /// <summary>
        /// Set CORS for specific bucket
        /// </summary>
        /// <returns></returns>
        private static async Task CreateCORSConfig()
        {
            try
            {
                // Create a new configuration request and add two rules
                CORSConfiguration configuration = new CORSConfiguration
                {
                    Rules = new List<CORSRule>
                    {
                        new CORSRule
                        {
                            Id = "S3CORS_Rule",
                            AllowedMethods = new List<string> { "PUT", "POST", "DELETE" },
                            AllowedOrigins = new List<string> { "https://aws-hosted-parser-app.vercel.app" }
                        //    MaxAgeSeconds = 3000,
                        //    ExposeHeaders = new List<string> {"x-amz-server-side-encryption"}
                        },
                    }
                };

                // Add the configuration to the bucket.
                await PutCORSConfigurationAsync(configuration);

                // Retrieve an existing configuration.
                //configuration = await RetrieveCORSConfigurationAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        static async Task PutCORSConfigurationAsync(CORSConfiguration configuration)
        {
            PutCORSConfigurationRequest request = new PutCORSConfigurationRequest
            {
                BucketName = _bucketName,
                Configuration = configuration
            };

            var response = await _s3Client.PutCORSConfigurationAsync(request);
        }

        static async Task<CORSConfiguration> RetrieveCORSConfigurationAsync()
        {
            GetCORSConfigurationRequest request = new GetCORSConfigurationRequest
            {
                BucketName = _bucketName
            };
            var response = await _s3Client.GetCORSConfigurationAsync(request);
            var configuration = response.Configuration;
            PrintCORSRules(configuration);
            return configuration;
        }

        static async Task DeleteCORSConfigurationAsync()
        {
            DeleteCORSConfigurationRequest request = new DeleteCORSConfigurationRequest
            {
                BucketName = _bucketName
            };
            await _s3Client.DeleteCORSConfigurationAsync(request);
        }

        static void PrintCORSRules(CORSConfiguration configuration)
        {
            Console.WriteLine();

            if (configuration == null)
            {
                Console.WriteLine("\nConfiguration is null");
                return;
            }

            Console.WriteLine("Configuration has {0} rules:", configuration.Rules.Count);
            foreach (CORSRule rule in configuration.Rules)
            {
                Console.WriteLine("Rule ID: {0}", rule.Id);
                Console.WriteLine("MaxAgeSeconds: {0}", rule.MaxAgeSeconds);
                Console.WriteLine("AllowedMethod: {0}", string.Join(", ", rule.AllowedMethods.ToArray()));
                Console.WriteLine("AllowedOrigins: {0}", string.Join(", ", rule.AllowedOrigins.ToArray()));
                Console.WriteLine("AllowedHeaders: {0}", string.Join(", ", rule.AllowedHeaders.ToArray()));
                Console.WriteLine("ExposeHeader: {0}", string.Join(", ", rule.ExposeHeaders.ToArray()));
            }
        }
    }
}