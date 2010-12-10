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

        public static CloudQueueClient CreateQueueClient()
        {
            return StorageAccount.CreateCloudQueueClient();
        }
    }
}