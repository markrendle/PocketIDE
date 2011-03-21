using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.StorageClient;
using PocketIDE.Web.Data;

namespace PocketIDE.Web.Code
{
    public class BlobHelper : IBlobHelper
    {
        public string LoadText(string containerName, string name)
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            return LoadTextImpl(containerName, name);
        }

        public Task<string> LoadTextAsync(string containerName, string name)
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            return Task.Factory.StartNew(() => LoadTextImpl(containerName, name));
        }

        public T LoadObject<T>(string containerName, string name)
            where T : class
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            return LoadObjectImpl<T>(containerName, name);
        }

        public string SaveText(string containerName, string name, string text, string contentType = "text/text")
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            ExceptionHelper.AssertParameterIsNotNull(() => text);
            return SaveTextImpl(containerName, name, text, contentType);
        }

        public void SaveTextAsync(string containerName, string name, string text)
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            ExceptionHelper.AssertParameterIsNotNull(() => text);

            Task.Factory.StartNew(() => SaveTextImpl(containerName, name, text));
        }

        public void SaveObject(string containerName, string name, object obj)
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            ExceptionHelper.AssertParameterIsNotNull(() => obj);
            SaveObjectImpl(containerName, name, obj);
        }

        public void SaveObjectAsync(string containerName, string name, object obj)
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            ExceptionHelper.AssertParameterIsNotNull(() => obj);
            Task.Factory.StartNew(() => SaveObjectImpl(containerName, name, obj));
        }

        private static string SaveTextImpl(string containerName, string name, string text, string contentType = "text/text")
        {
            var blob = GetBlobReference(containerName, name);
            blob.Properties.ContentType = contentType;
            blob.UploadText(text);
            return blob.Uri.ToString();
        }

        private static string LoadTextImpl(string containerName, string name)
        {
            if (!name.EndsWith(".cs")) name += ".cs";
            var blob = GetBlobReference(containerName, name);
            return blob.DownloadText();
        }

        private static CloudBlockBlob GetBlobReference(string containerName, string name)
        {
            var blobClient = CloudStorageHelper.CreateBlobClient();
            var codeContainer = blobClient.GetContainerReference(containerName);
            codeContainer.CreateIfNotExist();
            return codeContainer.GetBlockBlobReference(name);
        }

        private static void SaveObjectImpl(string containerName, string name, object obj)
        {
            var blob = GetBlobReference(containerName, name);
            var serializer = new XmlSerializer(typeof (Program));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, obj);
                blob.Properties.ContentType = "text/xml";
                blob.UploadText(stringWriter.ToString());
            }
        }

        private static T LoadObjectImpl<T>(string containerName, string name)
            where T : class
        {
            var blob = GetBlobReference(containerName, name);
            var serializer = new XmlSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);
                ms.Position = 0;
                return serializer.Deserialize(ms) as T;
            }
        }
    }
}