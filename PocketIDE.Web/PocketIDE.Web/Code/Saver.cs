using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.StorageClient;
using Ninject;
using Ninject.Modules;
using PocketIDE.Web.Data;

namespace PocketIDE.Web.Code
{
    public class Saver
    {
        private readonly IBlobHelper _blobHelper;

        [Inject]
        public Saver(IBlobHelper blobHelper)
        {
            _blobHelper = blobHelper;
        }

        public void Save(string userId, string saveName, string code)
        {
            if (!saveName.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
            {
                saveName = saveName + ".cs";
            }

            _blobHelper.SaveText(userId.ToToken(), saveName.ToToken(), code);
        }
    }

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

        public void SaveText(string containerName, string name, string text)
        {
            ExceptionHelper.AssertParameterIsNotNull(() => containerName);
            ExceptionHelper.AssertParameterIsNotNull(() => name);
            ExceptionHelper.AssertParameterIsNotNull(() => text);
            SaveTextImpl(containerName, name, text);
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

        private static void SaveTextImpl(string containerName, string name, string text)
        {
            var blob = GetBlobReference(containerName, name);
            blob.Properties.ContentType = "text/text";
            blob.UploadText(text);
        }

        private static string LoadTextImpl(string containerName, string name)
        {
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

    public interface IBlobHelper
    {
        void SaveText(string containerName, string name, string text);
        void SaveTextAsync(string containerName, string name, string text);

        void SaveObject(string containerName, string name, object obj);
        void SaveObjectAsync(string containerName, string name, object obj);

        string LoadText(string containerName, string name);
        Task<string> LoadTextAsync(string containerName, string name);

        T LoadObject<T>(string containerName, string name) where T : class;
    }

    internal static class ExceptionHelper
    {
        public static T AssertParameterIsNotNull<T>(Expression<Func<T>> expression)
            where T : class
        {
            var value = expression.Compile()();
            if (value == null)
            {
                throw new ArgumentNullException(((MemberExpression)expression.Body).Member.Name);
            }
            return value;
        }

        public static void AssertParameterIsNotNull(Expression<Func<string>> expression)
        {
            var value = AssertParameterIsNotNull<string>(expression);
            if (string.IsNullOrWhiteSpace(value))
            {
                var name = ((MemberExpression)expression.Body).Member.Name;
                throw new ArgumentException(name + " cannot be empty", name);
            }
        }
    }

    public class AzureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlobHelper>().To<BlobHelper>();
        }
    }
}