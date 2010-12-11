using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using PocketIDE.Web.Data;

namespace PocketIDE.Web.Msdn
{
    public class ContentSaver
    {
        private readonly CloudBlobClient _blobClient = CloudStorageHelper.CreateBlobClient();

        public void SaveContentsAsync(string msdnUrl, IEnumerable<Tuple<string, byte>> contents)
        {
            var folderName = MsdnUrlToFolderName(msdnUrl);
            var msdnContainer = _blobClient.GetContainerReference("msdn");
            var urlContainerReference = msdnContainer.GetDirectoryReference(folderName);

            foreach (var content in contents)
            {
                var blob = urlContainerReference.GetPageBlobReference(content.Item1);
                var memoryStream = new MemoryStream(content.Item2);
                blob.BeginUploadFromStream(memoryStream, EndUploadFromStream, Tuple.Create(blob, memoryStream));
            }
        }

        private static void EndUploadFromStream(IAsyncResult asyncResult)
        {
            var state = asyncResult.AsyncState as Tuple<CloudPageBlob, MemoryStream>;
            if (state != null)
            {
                try
                {
                    state.Item1.EndUploadFromStream(asyncResult);
                    state.Item2.Dispose();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                {
                }
                // ReSharper restore EmptyGeneralCatchClause
            }
        }

        private static string MsdnUrlToFolderName(string msdnUrl)
        {
            return Regex.Replace(msdnUrl.ToLowerInvariant(), "[^a-z]", "");
        }
    }
}