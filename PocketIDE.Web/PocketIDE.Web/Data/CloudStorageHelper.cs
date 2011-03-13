using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace PocketIDE.Web.Data
{
    public static class CloudStorageHelper
    {
        private static readonly CloudStorageAccount StorageAccount =
            CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("ConnectionString"));

        public static CloudTableClient CreateTableClient()
        {
            return StorageAccount.CreateCloudTableClient();
        }

        public static CloudQueueClient CreateQueueClient()
        {
            return StorageAccount.CreateCloudQueueClient();
        }

        public static CloudBlobClient CreateBlobClient()
        {
            return StorageAccount.CreateCloudBlobClient();
        }
    }
}