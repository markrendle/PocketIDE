using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using PocketIDE.Web.Data;
using PocketIDE.Web.Models;

namespace PocketIDE.Web.Code
{
    public class Loader
    {
        private readonly IBlobHelper _blobHelper;

        public Loader(IBlobHelper blobHelper)
        {
            _blobHelper = blobHelper;
        }

        public IEnumerable<string> List(Guid userId)
        {
            var blobClient = CloudStorageHelper.CreateBlobClient();
            var codeContainer = blobClient.GetContainerReference(userId.ToToken());
            return codeContainer.ListBlobs().Select(blobItem => blobItem.Uri.PathAndQuery.Split('/').LastOrDefault());
        }

        public Program Load(User user, string name)
        {
            var code = _blobHelper.LoadText(user.UserId.ToToken(), name.ToToken());
            var program = new Program
                              {
                                  AuthorId = user.WindowsLiveAnonymousId,
                                  Name = name,
                                  Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(code)),
                              };
            program.Hash = Program.CreateHash(program);
            return program;
        }
    }

    public static class StringEx
    {
        public static string ToToken(this string source)
        {
            return new string(source.Where(c => c == '.' || char.IsLetterOrDigit(c)).ToArray());
        }

        public static string ToToken(this Guid source)
        {
            return source.ToString("N");
        }
    }
}