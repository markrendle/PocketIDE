using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace PocketIDE.Web.Code
{
    [Serializable]
    [DataContract]
    public class Program
    {
        private const string Salt = "58D0CA96DFBC4A42A080E151FAC2A2A4";

        private string _authorId;
        [DataMember]
        public string AuthorId
        {
            get { return _authorId; }
            set { _authorId = value; }
        }

        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _code;
        [DataMember]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _hash;
        [DataMember]
        public string Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        public override string ToString()
        {
            return AuthorId + Code;
        }

        public static string CreateHash(Program program)
        {
            var sha1 = new SHA1Managed();
            return Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(program + Salt)));
        }

        public bool IsTrusted()
        {
            return (Hash ?? string.Empty).Equals(CreateHash(this),StringComparison.OrdinalIgnoreCase);
        }
    }
}