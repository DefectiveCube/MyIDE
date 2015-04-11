using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Core;
using Core.Workspace;

using Microsoft.CodeAnalysis;

namespace IDE_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IWorkspace Workspace { get; private set; }

        public Parser Parser { get; private set; } 

        public Log Log { get; private set; }

        public Notifier Notifier { get; private set; }

        public App()
        {
            this.LoadCompleted += App_LoadCompleted;
            this.Startup += App_Startup;
            this.SessionEnding += App_SessionEnding;
            this.Exit += App_Exit;
        }

        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            Workspace = new DUWorkspace();
            Parser = new Parser();
            Log = new Log();
            Notifier = new Notifier();
            
            Workspace.WorkspaceFailed += Log_Workspace;
            Workspace.DocumentOpened += Log_Document;
            Workspace.DocumentClosed += Log_Document;
            Workspace.DocumentAdded += Log_Document;
            Workspace.DocumentChanged += Log_Document;
            Workspace.DocumentReloaded += Log_Document;
            Workspace.DocumentRemoved += Log_Document;

            Workspace.ProjectAdded += Log_Project;
            Workspace.ProjectChanged += Log_Project;
            Workspace.ProjectReloaded += Log_Project;
            Workspace.ProjectRemoved += Log_Project;

            Workspace.SolutionAdded += Log_Solution;
            Workspace.SolutionChanged += Log_Solution;
            Workspace.SolutionCleared += Log_Solution;
            Workspace.SolutionReloaded += Log_Solution;
            Workspace.SolutionRemoved += Log_Solution;
        }

        private void Log_Workspace(object sender, WorkspaceDiagnosticEventArgs e)
        {
            Log.Error(string.Format("{0} | {1}", e.Diagnostic.Kind, e.Diagnostic.Message));
        }

        private void Log_Document(object sender, DocumentEventArgs e)
        {
            Log.Event(e, e.Document.Id);
        }

        private void Log_Document(object sender, WorkspaceChangeEventArgs e)
        {
            Log.Event(e, e.DocumentId);
        }

        private void Log_Project(object sender, WorkspaceChangeEventArgs e)
        {
            Log.Event(e, e.ProjectId);
        }

        private void Log_Solution(object sender, WorkspaceChangeEventArgs e)
        {
            Log.Event(e, e.OldSolution, e.NewSolution);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            var en = Windows.GetEnumerator();
            Window w;
       
            while (en.MoveNext())
            {
                if (en.Current is Window)
                {
                    w = en.Current as Window;


                    w.Close();
                }
            }
        }    
        

        private void App_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
        }
    }
}