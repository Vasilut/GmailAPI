using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailAPI
{
    public class GmailServiceBuilder
    {
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "LucianMockProduct";
        GmailService _service = null;
        public GmailServiceBuilder()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret_gmail.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            _service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public IList<Label> GetLabelList()
        {
            // Define parameters of request.
            UsersResource.LabelsResource.ListRequest request = _service.Users.Labels.List("me");

            // List labels.
            IList<Label> labels = request.Execute().Labels;
            return labels;
        }
    }
}
