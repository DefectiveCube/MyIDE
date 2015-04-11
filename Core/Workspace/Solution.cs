using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Core.Workspace
{
    /*
    [DataContract(Name = "Solution", Namespace = "http://davenport.edu")]
    [Obsolete]
    public class CoreSolution
    {
        public event EventHandler Opened;
        public event EventHandler Closed;

        //[DataMember(Name = "SolutionId", IsRequired = true)]
        //internal SolutionId solutionId;

        /// <summary>
        /// 
        /// </summary>
        internal CoreSolution() {
            Projects = new List<CoreProject>();

            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Create an empty solution in the default location
        /// </summary>
        internal CoreSolution(string name) : this()
        {
            // Name can't be null, empty, or whitespace
            // TODO: check for invalid characters

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }

            // Solutions must have a name
            Name = name;

            // Set the path the "My Documents" folder and use the IDE subfolder
            Path = BaseWorkspace.UserProjectsDirectory + @"\" + name;

            // Can't use a directory that already exists
            if (Directory.Exists(Path))
            {
                throw new Exception("Directory already exists!");
            }
            else
            {
                Directory.CreateDirectory(Path);

                // Create a subdirectory to store project in
                Directory.CreateDirectory(Path + @"\" + name);
            }
        }

        /// <summary>
        /// Create an empty solution in a specific location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        internal CoreSolution(string name, string path) : this()
        {
            // Do not create a project

            // Verify the path is valid
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Invalid Solution Path");
            }

            // Solutions must have a name
            Name = name;

            // Set the path of the solution
            Path = path + @"\" + Name;
        }

        /// <summary>
        /// Create a new solution with a new specific project in the default location
        /// </summary>
        /// <param name="name">The name of the solution</param>
        /// <param name="Kind">The type of project to add</param>
        internal CoreSolution(string name, OutputKind kind) : this(name)
        {
            // Create a new project
            var project = new CoreProject(name, kind);

            // Assign the directory
            project.directory = Path + @"\" + name;
        }

        /// <summary>
        /// Create a new solution with a new specific type of project in a specific location
        /// </summary>
        /// <param name="name"></param>
        /// <param name="kind"></param>
        /// <param name="path"></param>
        internal CoreSolution(string name, OutputKind kind, string path) : this(name, path)
        {
            // Create a new project
            var project = new CoreProject(name, kind);

            // Assign the directory
            project.directory = Path + @"\" + name;
        }

        /// <summary>
        /// Open an existing solution
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        internal CoreSolution(Guid id)
        {
            Id = id;

            // determine solution's directory

            // deserialize XML file
        }

        [DataMember]
        public List<CoreProject> Projects { get; set; }

        [IgnoreDataMember]
        public CoreProject this[Guid key]
        {
            get {
                return Projects.Where(p => p.Id == key).First();
            }
        }

        [IgnoreDataMember]
        public IEnumerable<CoreProject> this[string name]
        {
            get
            {
                return Projects.Where(p => p.Name == name);
            }
        }

        [DataMember]
        public Guid Id { get; internal set; }       

        [DataMember]
        public string Name { get; internal set; }

        [DataMember]
        public string Path { get; internal set; }        

        protected virtual void OnOpened(EventArgs e)
        {
            if(Opened != null)
            {
                Opened(this, e);
            }
        }

        protected virtual void OnClosed(EventArgs e)
        {
            if(Closed != null)
            {
                Closed(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="kind"></param>
        internal void AddNewProject(string name, OutputKind kind) {
            var project = new CoreProject(name, kind);

            // Set the directory of the project
            project.directory = Path + @"\" + project.Name;

            // Write project file
            project.WriteTo(project.directory + @"\" + string.Format("{0}.proj.xml", project.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        internal void AddExistingProject(string path)
        {
            // TODO deserialize project file
        }

        /// <summary>
        /// Open a solution
        /// </summary>
        public void Open()
        {
            OnOpened(new EventArgs());
        }

        /// <summary>
        /// Close a solution
        /// </summary>
        public void Close()
        {
            OnClosed(new EventArgs());
        }

        /// <summary>
        /// Rename a solution
        /// </summary>
        /// <param name="name"></param>
        public void Rename(string name) { }

        /// <summary>
        /// Save all open files
        /// </summary>
        public void Save()
        {
            foreach(var project in Projects)
            {
                project.Save();
            }
        }
              
        public void Build(object sender, EventArgs e)
        {
            Build();
        }

        public void Run(object sender, EventArgs e)
        {
            Run();
        }

        public void Package(object sender, EventArgs e)
        {
            Package();
        }

        public void Build()
        {
            // Save all open documents

            // Compile all project files in the solution

            // Solutions with multiple projects will have a specific build path

            if(Projects.Count == 1)
            {
                //Projects[0].Build();
            }
        }

        public void Run()
        {            
            //Process process = Process.Start("");
        }

        /// <summary>
        /// Compress solution into ZIP and place in the top directory of solution
        /// </summary>
        public void Package()
        {
            Package(Path);
        }

        /// <summary>
        /// Compress solution into ZIP and place at a specific location
        /// </summary>
        /// <param name="path"></param>
        public void Package (string path)
        {
            // Set output file with gzipstream
            var fs = new FileStream(Path + @"\" + Name + ".zip", FileMode.CreateNew);
            var gzip = new GZipStream(fs, CompressionLevel.Optimal);
            var dirInfo = new DirectoryInfo(Path);

            byte[] data;

            foreach(var file in dirInfo.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                using (FileStream f = new FileStream(file.FullName, FileMode.Open))
                {
                    if (file.Extension.EndsWith("zip")) // don't add zip files
                    {
                        continue;
                    }

                    data = new byte[f.Length];

                    // Read file into buffer
                    f.Read(data, 0, (int)f.Length);

                    // Write buffer to memory
                    gzip.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// Saves a solution file using the default naming scheme
        /// </summary>
        internal void WriteTo()
        {
            WriteTo(string.Format(@"{0}\{1}.sln.xml", Path, Name));
        }

        /// <summary>
        /// Saves a solution file to the specified path
        /// </summary>
        /// <param name="path">The file path being written to</param>
        internal void WriteTo(string path)
        {
            var fs = new FileStream(path, FileMode.Create);
            var settings = new XmlWriterSettings()
            {
                Indent = true
            };

            var writer = XmlWriter.Create(fs, settings);
            var serializer = new DataContractSerializer(typeof(CoreSolution));

            serializer.WriteObject(writer, this);
            writer.Close();
            fs.Close();
        }
    }
}
*/
}