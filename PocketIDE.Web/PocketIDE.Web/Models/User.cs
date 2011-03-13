using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PocketIDE.Web.Models
{
    [DataContract]
    [DataServiceEntity]
    [DataServiceKey("PartitionKey","RowKey")]
    public class User
    {
        public string PartitionKey
        {
            get { return _windowsLiveAnonymousId; }
            set { _windowsLiveAnonymousId = value; }
        }

        public string RowKey
        {
            get { return _userId.ToString("N"); }
            set
            {
                if (_userId.ToString("N") != value)
                    _userId = Guid.Parse(value);
            }
        }

        public DateTime Timestamp { get; set; }

        private string _windowsLiveAnonymousId;

        [DataMember]
        public string WindowsLiveAnonymousId
        {
            get { return _windowsLiveAnonymousId; }
            set { _windowsLiveAnonymousId = value; }
        }

        private Guid _userId;

        [DataMember]
        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
    }
}