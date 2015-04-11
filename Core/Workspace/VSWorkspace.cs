using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace Core.Workspace
{
    /*
    /// <summary>
    /// Capable of utilizing Visual Studio formats in a read-only capacity. Source code documents are read/write
    /// </summary>
    public class VSWorkspace : Microsoft.CodeAnalysis.Workspace, IWorkspace
    {
        public override event EventHandler<WorkspaceChangeEventArgs> AdditionalDocumentAdded;
        public override event EventHandler<WorkspaceChangeEventArgs> AdditionalDocumentChanged;
        public override event EventHandler<WorkspaceChangeEventArgs> AdditionalDocumentReloaded;
        public override event EventHandler<WorkspaceChangeEventArgs> AdditionalDocumentRemoved;
        //public override event EventHandler<DocumentEventArgs> DocumentOpened;
        //public override event EventHandler<DocumentEventArgs> DocumentClosed;
        public override event EventHandler<WorkspaceChangeEventArgs> DocumentAdded;
        public override event EventHandler<WorkspaceChangeEventArgs> DocumentChanged;
        public override event EventHandler<WorkspaceChangeEventArgs> DocumentReloaded;
        public override event EventHandler<WorkspaceChangeEventArgs> DocumentRemoved;
        public override event EventHandler<WorkspaceChangeEventArgs> ProjectAdded;
        public override event EventHandler<WorkspaceChangeEventArgs> ProjectChanged;
        public override event EventHandler<WorkspaceChangeEventArgs> ProjectReloaded;
        public override event EventHandler<WorkspaceChangeEventArgs> ProjectRemoved;
        public override event EventHandler<WorkspaceChangeEventArgs> SolutionAdded;
        public override event EventHandler<WorkspaceChangeEventArgs> SolutionChanged;
        public override event EventHandler<WorkspaceChangeEventArgs> SolutionCleared;
        public override event EventHandler<WorkspaceChangeEventArgs> SolutionReloaded;
        public override event EventHandler<WorkspaceChangeEventArgs> SolutionRemoved;
        //public override event EventHandler<WorkspaceChangeEventArgs> WorkspaceChanged;
        //public override event EventHandler<WorkspaceDiagnosticEventArgs> WorkspaceFailed;

        private MSBuildWorkspace workspace;

        public VSWorkspace()
        {
            workspace = MSBuildWorkspace.Create();

            workspace.DocumentOpened += Workspace_DocumentOpened;
            workspace.DocumentClosed += Workspace_DocumentClosed;
            workspace.WorkspaceChanged += Workspace_WorkspaceChanged;
            workspace.WorkspaceFailed += Workspace_WorkspaceFailed;
        }

        /* event handlers

        private void Workspace_WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Workspace_WorkspaceChanged(object sender, WorkspaceChangeEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Workspace_DocumentClosed(object sender, DocumentEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Workspace_DocumentOpened(object sender, DocumentEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        /* properties 
        public override bool CanCreateSolutions { get { return false; } }
        public override bool CanCreateProjects { get { return false; } }
        //public override bool CanOpenDocuments { get { return workspace.CanOpenDocuments; } }
        //public override Solution CurrentSolution { get { return workspace.CurrentSolution; } }

        /* solution methods

        public override Solution CreateSolution(string name)
        {
            throw new NotSupportedException("Cannot create Visual Studio Solution Files");
        }

        public override Solution OpenSolution(string solutionFilePath)
        {
            return OpenSolutionAsync(solutionFilePath).Result;
        }

        public override async Task<Solution> OpenSolutionAsync(string solutionFilePath)
        {
            if (!File.Exists(solutionFilePath))
            {
                throw new FileNotFoundException();
            }

            var solution = await workspace.OpenSolutionAsync(solutionFilePath);

            return solution;
        }

        public override void CloseSolution()
        {
            workspace.CloseSolution();
        }

        /*public override void ClearSolution()
        {
            throw new NotSupportedException();
        }

        /* project methods 

        public override Project OpenProject(string projectFilePath)
        {
            throw new NotImplementedException();
        }

        public override async Task<Project> OpenProjectAsync(string projectFilePath)
        {
            if (!File.Exists(projectFilePath))
            {
                throw new FileNotFoundException();
            }

            return await workspace.OpenProjectAsync(projectFilePath);
        }

        /*public override IEnumerable<DocumentId> GetOpenDocumentIds(ProjectId projectId)
        {
            return workspace.GetOpenDocumentIds(projectId);
        }

        internal override Project CreateProject(Solution solution, string name, string assemblyName)
        {
            throw new NotSupportedException("Cannot create Visual Studio Project Files. Use DUWorkspace instead");
        }

        public override Project CreateClassLibraryProject(Solution solution, string name, string assemblyName)
        {
            throw new NotSupportedException("Cannont creates Visual Studio Project Files. Use DUWorkspace instead");
        }

        public override Project CreateConsoleApplicationProject(Solution solution, string name, string assemblyName)
        {
            throw new NotSupportedException("Cannot create Visual Studio projects. Use DUWorkspace instead");
        }

        /* document methods

        public override Document CreateDocument(ProjectId id, string name)
        {
            throw new NotImplementedException();
        }

        public override Document CreateDocument(ProjectId id, string name, string text)
        {
            throw new NotImplementedException();
        }

        public override void OpenDocument(DocumentId documentId)
        {
            workspace.OpenDocument(documentId);
        }

        /*public override void CloseDocument(DocumentId documentId)
        {
            workspace.CloseDocument(documentId);
        }

        public override void SaveDocument(DocumentId documentId)
        {
            throw new NotImplementedException();
        }
    }*/
}