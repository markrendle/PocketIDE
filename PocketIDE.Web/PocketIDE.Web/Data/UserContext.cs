using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using PocketIDE.Web.Models;

namespace PocketIDE.Web.Data
{
    public class UserContext
    {
        private const string EntitySetName = "users";
        private static readonly CloudTableClient TableClient;
        private static ManualResetEvent _initialized = new ManualResetEvent(false);

        private TableServiceContext _context;

        static UserContext()
        {
            TableClient = CloudStorageHelper.CreateTableClient();
            TableClient.BeginCreateTableIfNotExist(EntitySetName, UserTableCreated, null);
        }

        private static void UserTableCreated(IAsyncResult asyncResult)
        {
            TableClient.EndCreateTableIfNotExist(asyncResult);
            _initialized.Set();
        }

        private static TableServiceContext GetContext()
        {
            if (_initialized != null)
            {
                _initialized.WaitOne();
                _initialized.Dispose();
                _initialized = null;
            }
            var context = TableClient.GetDataServiceContext();
            return context;
        }

        protected TableServiceContext Context
        {
            get { return _context ?? (_context = GetContext()); }
        }

        public User GetByWindowsLiveAnonymousId(string windowsLiveAnonymousId)
        {
            return CreateQuery()
                .Where(u => u.PartitionKey == windowsLiveAnonymousId)
                .SingleOrDefault();
        }

        public IQueryable<User> CreateQuery()
        {
            return Context.CreateQuery<User>(EntitySetName);
        }

        public User GetOrAdd(string windowsLiveAnonymousId)
        {
            var user = GetByWindowsLiveAnonymousId(windowsLiveAnonymousId);
            if (user == null)
            {
                user = new User
                           {
                               WindowsLiveAnonymousId = windowsLiveAnonymousId,
                               UserId = Guid.NewGuid(),
                           };
                try
                {
                    Context.AddObject(EntitySetName, user);
                    Context.SaveChangesWithRetries();
                }
                catch (StorageException)
                {
                    user = GetByWindowsLiveAnonymousId(windowsLiveAnonymousId);
                }
            }

            return user;
        }
    }
}