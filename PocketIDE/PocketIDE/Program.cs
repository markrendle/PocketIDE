using System;
using System.Security.Cryptography;
using System.Text;

namespace PocketCSharp
{
    public class Program
    {
        private const string Salt = "58D0CA96DFBC4A42A080E151FAC2A2A4";

        public string AuthorId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }

        public override string ToString()
        {
            return AuthorId + Code;
        }

        public static string CreateHash(Program program)
        {
            var sha1 = new SHA1Managed();
            return Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(program + Salt)));
        }
    }
}