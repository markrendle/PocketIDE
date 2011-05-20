using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace PocketIDE.Web
{
    [DataContract]
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            
        }

        public ServiceResponse(string message)
        {
            Message = message;
        }
        [DataMember]
        public string Message { get; set; }
    }
}