using System;
using System.Runtime.Serialization;

namespace PocketIDE.Web.Code
{
    [Serializable]
    [DataContract]
    public class RunResult
    {
        [DataMember]
        public string CompileError { get; set; }
        [DataMember]
        public string Output { get; set; }
    }
}