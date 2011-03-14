using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using PocketIDE.Web.Data;

namespace PocketIDE.Web.Msdn
{
    public class ContentSaver
    {
        private readonly CloudBlobClient _blobClient = CloudStorageHelper.CreateBlobClient();

        public Task SaveContentsAsync(string msdnUrl, IEnumerable<Tuple<string, byte[]>> contents)
        {
            return Task.Factory.StartNew(() =>
                                             {
                                                 var folderName = Content.GetBlobFolderName(msdnUrl);
                                                 var msdnContainer = _blobClient.GetContainerReference("msdn");
                                                 var urlContainerReference =
                                                     msdnContainer.GetDirectoryReference(folderName);

                                                 foreach (var content in contents)
                                                 {
                                                     var blob =
                                                         urlContainerReference.GetBlockBlobReference(content.Item1 +
                                                                                                     ".html");
                                                     blob.Properties.ContentType = "text/html";
                                                     using (var memoryStream = new MemoryStream(content.Item2))
                                                     {
                                                         blob.UploadFromStream(memoryStream);
                                                     }
                                                 }
                                             });
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