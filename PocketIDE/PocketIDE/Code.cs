using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using JsonFx.Json;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;

namespace PocketIDE
{
    public class Code
    {
        private const int AnidLength = 32;
        private const int AnidOffset = 2;
        
        public static string GetWindowsLiveAnonymousId()
        {
            string result = string.Empty;
            object anid;
            if (UserExtendedProperties.TryGetValue("ANID", out anid))
            {
                if (anid != null && anid.ToString().Length >= (AnidLength + AnidOffset))
                {
                    result = anid.ToString().Substring(AnidOffset, AnidLength);
                }
            }

            return result;
        }

        public static string ToBase64Json(string code)
        {
            var program = new Program
                              {
                                  AuthorId = GetWindowsLiveAnonymousId(),
                                  Name = string.Empty,
                                  Code = Convert.ToBase64String(Encoding.UTF8.GetBytes(code))
                              };
            program.Hash = Program.CreateHash(program);
            var writer =
                new JsonWriter(
                    new DataWriterSettings(
                        new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase)));
            return writer.Write(program);
        }
    }
}
