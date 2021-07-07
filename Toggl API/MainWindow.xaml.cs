﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Toggl;
using Toggl.Extensions;
using Toggl.QueryObjects;
using Toggl_API;
using Toggl_API.APIHelper;
using Toggl_API.APIHelper.ChartClasses;

namespace Toggl_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {

            InitializeComponent();

         

            var helper = new Helper();
            Debug.WriteLine(helper.GetClientProjectTimeTrack("Klient 1", "07/05/2021", "07/08/2021"));
            //Debug.WriteLine("");
            //Debug.WriteLine(helper.GetTotalProjectWorkTime(helper.Projects[0], "07/05/2021", "07/08/2021"));
            LoadBarChartData(helper.GetProjectChart(helper.Projects[0]));

        }


        private void LoadBarChartData(ProjectChart projectChart)
        {

            List<KeyValuePair<string, double>> keyValuePairs = new List<KeyValuePair<string, double>>();
            foreach (var item in projectChart.TimePerTasks)
            {
                keyValuePairs.Add(new KeyValuePair<string, double>(item.Description,item.Time));
            }

            ((BarSeries)mcChart.Series[0]).ItemsSource = keyValuePairs;
            mcChart.Title = projectChart.ProjectName;


        }




        // xmlns:DVC="clr-namespace:System.Windows.Controls.DataVisualization.Charting.Compatible;assembly=DotNetProjects.DataVisualization.Toolkit"
        // xmlns:DVC="clr-namespace:System.Windows.Controls.DataVisualization.Charting.Primitives;assembly=DotNetProjects.DataVisualization.Toolkit"

        //WorkSpace
        //var WSService = new Toggl.Services.WorkspaceService(apiKey);
        //var listwork = WSService.List();
        //var workspaceID = listwork[0].Id;


        //Project
        //var project = new Project
        //{
        //    IsBillable = true,
        //    WorkspaceId = WorkspaceID,
        //    Name = "Project 2",
        //    IsAutoEstimates = false,
        //    ClientId = clients[0].Id,
        //    IsActive = true,
        //};
        //var act = PService.Add(project);


        //Add Task
        //TService.Add(new Toggl.Task
        //{
        //    IsActive = true,
        //    Name = "Test 1",
        //    EstimatedSeconds = 3600,
        //    WorkspaceId = WorkspaceID,
        //    ProjectId = ProjectID
        //});



        //Lastjob
        //var TService = new Toggl.Services.TaskService(apiKey);


        //var tasks = TService.ForProject((int)ProjectID);

        //var TEService = new Toggl.Services.TimeEntryService(apiKey);
        //var timeEntry = TEService.Add(new TimeEntry()
        //{
        //    IsBillable = true,
        //    CreatedWith = "TogglAPI.Net",
        //    Duration = 900,
        //    Start = DateTime.Now.ToIsoDateStr(),
        //    WorkspaceId = WorkspaceID,
        //    ProjectId=ProjectID,
        //    TaskId = tasks[1].Id
        //});



        //var TService = new Toggl.Services.TaskService(apiKey);
        //var task = TService.ForProject((int)ProjectID);
        //var loadedTask = TService.Get((int)task[0].Id);
        //loadedTask.IsActive = false;
        //var editedTask = TService.Edit(loadedTask);

    }
}
