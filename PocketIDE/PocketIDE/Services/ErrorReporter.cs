using System;
using System.Net;
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
using PocketIDE.Extensions;

namespace PocketIDE.Services
{
    public class ErrorReporter
    {
        public void ReportError(Exception ex)
        {
            var report = new ErrorReport {Text = ex.ToString().Base64Encode()};
            report.Hash = ErrorReport.CreateHash(report);
            var writer =
                new JsonWriter(
                    new DataWriterSettings(
                        new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase)));
            var json = writer.Write(report);

            var webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            Uri address = UriFactory.Create("exceptions/report");
            webClient.UploadStringCompleted += UploadStringCompleted;
            webClient.UploadStringAsync(address, json);
        }

        void UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            
        }
    }
}
