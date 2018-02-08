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

        public IList<Google.Apis.Tasks.v1.Data.Task> GetTasks(string taskId)
        {
            var taskRequest = _service.Tasks.List(taskId);
            taskRequest.DueMin = "2010-09-30T10:57:00.000-08:00";
            taskRequest.DueMax = "2010-11-09T10:57:00.000-08:00";
            Tasks tasks = taskRequest.Execute();
            return tasks.Items;
        }

        public Google.Apis.Tasks.v1.Data.Task GetSpecificTask(string taskId, string taskListIdentifier)
        {
            var taskReq = _service.Tasks.Get(taskListIdentifier, taskId);
            var task = taskReq.Execute();
            return task;
        }
    }
}
