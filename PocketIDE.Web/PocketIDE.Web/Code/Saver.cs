using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PocketIDE.Web.Data;

namespace PocketIDE.Web.Code
{
    public class Saver
    {
        public void Save(string saveName, string code)
        {
            if (!saveName.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
            {
                saveName = saveName + ".cs";
            }

            var blobClient = CloudStorageHelper.CreateBlobClient();
            var codeContainer = blobClient.GetContainerReference("code");
            var blob = codeContainer.GetBlockBlobReference(saveName);
            blob.Properties.ContentType = "text/text";
            blob.UploadText(code);
        }
    }
}