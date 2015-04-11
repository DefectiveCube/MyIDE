using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;

namespace Core.Workspace
{
    public abstract class CoreWorkspace
    {
        public static string UserDirectory { get; internal set; }
        public static string ProjectDirectory { get; internal set; }
        public static string WorkspaceDirectory { get; internal set; }
        public static string GACDirectory { get; internal set; }

        static CoreWorkspace()
        {                        
            // Easy way to determine the folder of the GAC
            GACDirectory = new FileInfo(typeof(object).Assembly.Location).DirectoryName;

            // Assign values for user directories
            UserDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\IDE";
            ProjectDirectory = UserDirectory + @"\Projects";
            WorkspaceDirectory = UserDirectory + @"\Workspace";

            // Verify each directory and create if it doesn't exist
            foreach (var str in new[] { UserDirectory, ProjectDirectory, WorkspaceDirectory })
            {
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }
            }
        }

        internal static string CreateSolutionDirectory(string name)
        {
            var path = Path.Combine(ProjectDirectory, name);

            // Verify path is valid
            if (string.IsNullOrEmpty(path) && path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("Invalid Characters in Path");
            }

            var di = new DirectoryInfo(path);

            if (!di.Exists)
            {
                // Create Solution Directory
                di.Create();

                // Create Solution File
                path = Path.Combine(path, name + ".dusln");
                
                using(var writer = XmlWriter.Create(File.OpenWrite(path)))
                {
                    var doc = new XDocument(
                        new XElement("Solution",
                            new XAttribute("Path", path)));

                    doc.WriteTo(writer);
                }
            }
            else
            {
                throw new Exception("Solution already exists");
            }

            return path;
        }

        internal static string CreateProjectDirectory(string name, string solutionDirectory)
        {
            var solutionFile = new FileInfo(solutionDirectory);

            if (!solutionFile.Exists)
            {
                throw new FileNotFoundException("Solution File Not Found");
            }        

            var solutionDir = solutionFile.Directory;

            if (!solutionDir.Exists)
            {
                throw new DirectoryNotFoundException("Solution Directory Not Found");
            }

            // Create Project Directory in Solution Directory
            var projectDir = solutionDir.CreateSubdirectory(name);

            var path = Path.Combine(projectDir.FullName, name + ".duproj");

            // Create Project File in Project Directory
            var writer = File.CreateText(path);

            writer.Write("project file");
            writer.Close();

            return path;
        }

        public abstract bool IsSolutionPathValid(string path);

        public abstract bool IsProjectPathValid(string path, string dirPath);
    }
}
