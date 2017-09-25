using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
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
    public class TaskServiceBuilder
    {
        static string[] Scopes = { TasksService.Scope.TasksReadonly };
        static string ApplicationName = "Task Api App";
        TasksService _service = null;
        public TaskServiceBuilder()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret_task.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/tasks-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Tasks API service.
            _service = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public IList<TaskList> GetTaskList()
        {
            // Define parameters of request.
            TasklistsResource.ListRequest listRequest = _service.Tasklists.List();
            listRequest.MaxResults = 10;

            // List task lists.
            IList<TaskList> taskLists = listRequest.Execute().Items;
            return taskLists;
        }

        public IList<Google.Apis.Tasks.v1.Data.Task> GetTasks(string taskList)
        {
            Tasks tasks = _service.Tasks.List(taskList).Execute();
            return tasks.Items;
        }
    }
}
