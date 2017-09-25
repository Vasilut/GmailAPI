using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Contacts;
using Google.GData.Client;
using Google.GData.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GmailAPI
{
    public class ContactServiceBuilder
    {
        static string clientId = "619233939360-jc00amt1cqmapi1shqs7ijkl1pitgh5o.apps.googleusercontent.com";
        static string clientSecretId = "kZ96AHcZkcQSNHnoLDCEom-L";
        static string[] scopes = new string[] { "https://www.google.com/m8/feeds/", "https://www.googleapis.com/auth/contacts.readonly" };     // view your basic profile info.
        OAuth2Parameters parameters;

        public ContactServiceBuilder()
        {
            try
            {
                // Use the current Google .net client library to get the Oauth2 stuff.
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecretId }
                                                                                             , scopes
                                                                                             , "test"
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore("test")).Result;

                // Translate the Oauth permissions to something the old client libray can read
                parameters = new OAuth2Parameters();
                parameters.AccessToken = credential.Token.AccessToken;
                parameters.RefreshToken = credential.Token.RefreshToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetContactList()
        {
            RequestSettings settings = new RequestSettings("Contact API .NET Quickstart", parameters);
            ContactsRequest cr = new ContactsRequest(settings);

            Feed<Contact> f = cr.GetContacts();
            foreach (Contact entry in f.Entries)
            {
                if (entry.Name != null)
                {
                    Name name = entry.Name;
                    if (!string.IsNullOrEmpty(name.FullName))
                        Console.WriteLine("\t\t" + name.FullName);
                    else
                        Console.WriteLine("\t\t (no full name found)");
                    if (!string.IsNullOrEmpty(name.NamePrefix))
                        Console.WriteLine("\t\t" + name.NamePrefix);
                    else
                        Console.WriteLine("\t\t (no name prefix found)");
                    if (!string.IsNullOrEmpty(name.GivenName))
                    {
                        string givenNameToDisplay = name.GivenName;
                        if (!string.IsNullOrEmpty(name.GivenNamePhonetics))
                            givenNameToDisplay += " (" + name.GivenNamePhonetics + ")";
                        Console.WriteLine("\t\t" + givenNameToDisplay);
                    }
                    else
                        Console.WriteLine("\t\t (no given name found)");
                    if (!string.IsNullOrEmpty(name.AdditionalName))
                    {
                        string additionalNameToDisplay = name.AdditionalName;
                        if (string.IsNullOrEmpty(name.AdditionalNamePhonetics))
                            additionalNameToDisplay += " (" + name.AdditionalNamePhonetics + ")";
                        Console.WriteLine("\t\t" + additionalNameToDisplay);
                    }
                    else
                        Console.WriteLine("\t\t (no additional name found)");
                    if (!string.IsNullOrEmpty(name.FamilyName))
                    {
                        string familyNameToDisplay = name.FamilyName;
                        if (!string.IsNullOrEmpty(name.FamilyNamePhonetics))
                            familyNameToDisplay += " (" + name.FamilyNamePhonetics + ")";
                        Console.WriteLine("\t\t" + familyNameToDisplay);
                    }
                    else
                        Console.WriteLine("\t\t (no family name found)");
                    if (!string.IsNullOrEmpty(name.NameSuffix))
                        Console.WriteLine("\t\t" + name.NameSuffix);
                    else
                        Console.WriteLine("\t\t (no name suffix found)");
                }
                else
                    Console.WriteLine("\t (no name found)");
                foreach (EMail email in entry.Emails)
                {
                    Console.WriteLine("\t" + email.Address);
                }
            }
        }
    }
}
