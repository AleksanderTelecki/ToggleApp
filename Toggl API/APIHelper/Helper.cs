﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toggl;
using Toggl.Extensions;
using Toggl.QueryObjects;

namespace Toggl_API.APIHelper
{
    public class Helper
    {
        public static string APIToken { get => "c99e059b3a3bb073c14097be588d2cbd"; }

        public Toggl.Services.ProjectService ProjectService { get; private set; }

        public Toggl.Services.TaskService TaskService { get ; private set; }

        public Toggl.Services.TimeEntryService TimeEntryService { get ; private set; }

        public  Toggl.Services.ClientService ClientService { get ; private set; }

        public  Toggl.Services.WorkspaceService WorkspaceService { get ; private set; }

        public int? WorkSpaceID { get; private set; }

        public List<Toggl.Project> Projects { get => ProjectService.List(); }




        public Helper()
        {

            ProjectService = new Toggl.Services.ProjectService(APIToken);
            TaskService = new Toggl.Services.TaskService(APIToken);
            TimeEntryService = new Toggl.Services.TimeEntryService(APIToken);
            ClientService = new Toggl.Services.ClientService(APIToken);
            WorkspaceService = new Toggl.Services.WorkspaceService(APIToken);
            WorkSpaceID = WorkspaceService.List()[0].Id;



        }

        public string GetClientProjectTimeTrack(string Name, string startDate, string endDate)
        {
            StringBuilder result = new StringBuilder();

            var prams = new TimeEntryParams();

            prams.StartDate = Convert.ToDateTime(startDate);
            prams.EndDate = Convert.ToDateTime(endDate);

            var hours = TimeEntryService.List(prams);

            var client = ClientService.GetByName(Name);
            var clientproj = ProjectService.List().Where(w => w.ClientId == client.Id);




            result.AppendLine($"{client.Name}");
            foreach (var item in clientproj)
            {
                result.AppendLine($"   {item.Name}");
                var choosedtimestamp = hours.Where(w => w.ProjectId == item.Id).ToList();
                var choosedtasks = TaskService.ForProject((int)item.Id);
                foreach (var timestamp in choosedtimestamp)
                {
                    DateTime start = DateTime.Parse(timestamp.Start, null, System.Globalization.DateTimeStyles.RoundtripKind);
                    DateTime stop = DateTime.Parse(timestamp.Stop, null, System.Globalization.DateTimeStyles.RoundtripKind);
                    timestamp.TaskName = GetTaskNameByID((int)timestamp.TaskId, choosedtasks);
                  
                    result.AppendLine($"({timestamp.TaskName}) Duration: {TimeSpan.FromSeconds((double)timestamp.Duration).TotalHours} h | {start.ToString("HH:mm")} - {stop.ToString("HH:mm")} | {timestamp.Description}");
          

                }


            }


            return result.ToString();


        }

        public string GetTotalProjectWorkTime(Toggl.Project project, string startDate, string endDate)
        {

            StringBuilder result = new StringBuilder();

            var prams = new TimeEntryParams();

            prams.StartDate = Convert.ToDateTime(startDate);
            prams.EndDate = Convert.ToDateTime(endDate);

            var hours = TimeEntryService.List(prams);

            var choosedtimestamp = hours.Where(w => w.ProjectId == project.Id).ToList();
            double totaltime = 0;


            result.AppendLine($"{project.Name}");
            

            foreach (var item in choosedtimestamp)
            {
                totaltime += TimeSpan.FromSeconds((double)item.Duration).TotalHours;
            }

            result.AppendLine($"   Total time: {totaltime} h");

            result.AppendLine($"       Task Description:");
            foreach (var item in choosedtimestamp)
            {
                result.AppendLine($"          {item.Description}");
            }







            return result.ToString();

        }

        private string GetTaskNameByID(int Id, List<Toggl.Task> tasks)
        {
            var task = tasks.Where(w => w.Id == Id).ToList();
            return task[0].Name;

        }

        public void AddTask(int projectId,string taskName,bool isActive,int estimatedseconds)
        {
            
            TaskService.Add(new Toggl.Task
            {
                IsActive = isActive,
                Name = taskName,
                EstimatedSeconds = estimatedseconds,
                WorkspaceId = WorkSpaceID,
                ProjectId = projectId
            });



        }

        
        public void AddTimeEntries(int projectId, int taskId, int duration,string description)
        {

            TimeEntryService.Add(new TimeEntry()
            {
                IsBillable = true,
                CreatedWith = "TogglAPI.Net",
                Duration = duration,
                Start = DateTime.Now.ToIsoDateStr(),
                WorkspaceId = WorkSpaceID,
                ProjectId = projectId,
                TaskId = taskId,
                Description= description
            });

        }




    }
}
