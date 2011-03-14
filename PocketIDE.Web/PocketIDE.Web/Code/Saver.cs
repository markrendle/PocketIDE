using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Ninject;
using Ninject.Modules;

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