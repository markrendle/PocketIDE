using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PocketIDE.Web.Data;

namespace PocketIDE.Web.Code
{
    public class Loader
    {
        public IEnumerable<string> List()
        {
            var blobClient = CloudStorageHelper.CreateBlobClient();
            var codeContainer = blobClient.GetContainerReference("code");
            return codeContainer.ListBlobs().Select(blobItem => blobItem.Uri.PathAndQuery.Split('/').LastOrDefault());
        }

        public string Load(string name)
        {
            var blobClient = CloudStorageHelper.CreateBlobClient();
            var codeContainer = blobClient.GetContainerReference("code");
            var blob = codeContainer.GetBlockBlobReference(name);
            return blob.DownloadText();
        }
    }
}