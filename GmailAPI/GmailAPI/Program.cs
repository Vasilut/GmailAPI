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
    class Program
    {

        static void Main(string[] args)
        {


            //ListLabels();
            //ListContacts();
            //ListTasks();
            ListMessages();

            Console.WriteLine("Press any key to continue..");
            Console.ReadLine();
        }

        static void ListLabels()
        {
            GmailServiceBuilder gmailServiceBuilder = new GmailServiceBuilder();
            var labels = gmailServiceBuilder.GetLabelList();

            Console.WriteLine("Labels:");
            if (labels != null && labels.Count > 0)
            {
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            else
            {
                Console.WriteLine("No labels found.");
            }
            Console.Read();
        }

        static void ListContacts()
        {
            ContactServiceBuilder contactServiceBuilder = new ContactServiceBuilder();
            contactServiceBuilder.GetContactList();
        }

        static void ListTasks()
        {
            TaskServiceBuilder taskServiceBuilder = new TaskServiceBuilder();
            var taskLists = taskServiceBuilder.GetTaskList();
            Console.WriteLine("Task Lists:");
            if (taskLists != null && taskLists.Count > 0)
            {
                foreach (var taskList in taskLists)
                {
                    
                    Console.WriteLine("{0} ({1})", taskList.Title, taskList.Id);
                    var tsk = taskServiceBuilder.GetTasks(taskList.Id);
                }
            }
            else
            {
                Console.WriteLine("No task lists found.");
            }
            Console.Read();
        }

        static void ListMessages()
        {
            GmailServiceBuilder gmailServiceBuilder = new GmailServiceBuilder();
            var listOfMessages = gmailServiceBuilder.ListMessages("me","is:unread");

            foreach (Message item in listOfMessages)
            {
                var message = gmailServiceBuilder.GetMessage("me", item.Id);
                var listOfLabels = gmailServiceBuilder.GetLabelsForMessage(message);
                bool isImportant = gmailServiceBuilder.MessageHasImportantLabel(message);
                var x = 2;
            }
        }
    }
}
