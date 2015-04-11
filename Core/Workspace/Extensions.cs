using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;

namespace Core.Workspace
{
    public static class Extensions
    {
        public static void WriteDebug(this SolutionInfo info)
        {
            var sb = new StringBuilder();

            sb.AppendLine("--- Solution Info ---");
            sb.Append("Id: ");
            sb.AppendLine(info.Id.ToString());
            sb.Append("Path: ");
            sb.AppendLine(info.FilePath);

            foreach(var p in info.Projects)
            {
                
            }

            Debug.Write(sb.ToString());
        }

        public static void WriteDebug(this ProjectInfo info)
        {
            var sb = new StringBuilder();

            sb.AppendLine("--- Project Info ---");
            sb.Append("Id: ");
            sb.Append("Path: ");
        }

        /// <summary>
        /// Delete a document
        /// </summary>
        /// <param name="document"></param>
        public static void Delete(this Document document) { }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="document"></param>
        public static void Delete(this Project project) { }

        /// <summary>
        /// Delete a solution
        /// </summary>
        /// <param name="solution"></param>
        public static void Delete(this Solution solution) { }

        /// <summary>
        /// Evaluate a project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static ImmutableArray<Diagnostic> Evaluate(this Project project)
        {
            return project.GetCompilationAsync().Result.GetDiagnostics();
        }

        /// <summary>
        /// Evaluate all projects in a solution
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static ImmutableArray<Diagnostic> Evaluate(this Solution solution)
        {
            var builder = ImmutableArray.CreateBuilder<Diagnostic>();

            if (solution != null)
            {
                foreach (var project in solution.Projects)
                {
                    builder.AddRange(project.Evaluate());
                }
            }

            return builder.ToImmutableArray();
        }

        /// <summary>
        /// Evaluate all projects in the current solution
        /// </summary>
        /// <param name="workspace"></param>
        /// <returns></returns>
        public static ImmutableArray<Diagnostic> Evaluate(this IWorkspace workspace)
        {
            return workspace.CurrentSolution.Evaluate();
        }

        public static void Emit(this Solution solution)
        {
            foreach(var id in solution.ProjectIds)
            {
                var project = solution.GetProject(id);
                var compile = project.GetCompilationAsync().Result;
                var fileInfo = new FileInfo(project.OutputFilePath);
                var dirInfo = fileInfo.Directory;
                var pdbPath = Path.Combine(dirInfo.FullName, project.Name + ".pdb");

                // Make sure output directory exists
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                // Emit fill and PDB (used for debugging)
                var result = compile.Emit(fileInfo.FullName, pdbPath);

                Debug.WriteLine("" + (result.Success ? "Success" : "Failure"));
                Debug.WriteLine(string.Format("Errors: {0}", result.Diagnostics.Length));
            }
        }

        /// <summary>
        /// Rename a project
        /// </summary>
        /// <param name="project"></param>
        /// <param name="name"></param>
        public static void Rename(this Project project, string name)
        {
            // A new project has to be created
        }

        /// <summary>
        /// Rename a document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name"></param>
        public static void Rename(this Document document, string name)
        {
            var old = new FileInfo(document.FilePath);

            // position of file name
            var index = old.FullName.LastIndexOf(old.Name);

            //old.Name
            //old.CopyTo()

            var oldFile = document.FilePath;
            //var newFile = "";            
        }
        
        public static bool Try(this Action action)
        {
            string message;

            return Try(action, out message);
        }

        public static bool Try(this Action action, out string message)
        {
            message = string.Empty;
            string stack = string.Empty;

            return Try(action, out message, out stack);
        }

        public static bool Try(this Action action, out string message, out string stack)
        {
            message = string.Empty;
            stack = string.Empty;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                message = ex.Message;
                stack = ex.StackTrace;
                return false;
            }

            return true;
        }
    }
}