using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Workspace;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Core
{
    [Obsolete]
    public class Compiler
    {
        public Workspace.Project BuildProject;

        public string AssemblyName;
        public string ExecutableName;
        public IEnumerable<SyntaxTree> SourceTrees;
        public IEnumerable<MetadataReference> References;
        public OutputKind Output { get; set; }

        void LoadFiles()
        {
            foreach (var doc in BuildProject.Documents)
            {
               SyntaxFactory.ParseSyntaxTree(doc.ToString());
            }
        }

        void LoadReferences()
        {


        }

        public void Compile()
        {
            if (BuildProject.Type == OutputKind.ConsoleApplication || BuildProject.Type == OutputKind.DynamicallyLinkedLibrary)
            {
                LoadFiles();
                LoadReferences();

                if (string.IsNullOrEmpty(AssemblyName))
                {
                    AssemblyName = "HelloWorld";
                }

                var options = new CSharpCompilationOptions(Output);
               
                var compilation = CSharpCompilation.Create(AssemblyName)
                    .AddReferences(new MetadataFileReference(typeof(object).Assembly.Location))
                    .AddSyntaxTrees(SourceTrees)                    
                    .WithOptions(options);

                var emitResult = compilation.Emit(ExecutableName);

                Debug.WriteLineIf(emitResult.Success, "Emitting IL passed");
                Debug.WriteLineIf(!emitResult.Success, "Emitting IL failed");

                foreach(var message in emitResult.Diagnostics)
                {
                    Debug.WriteLine(message);
                }
            }else
            {
                throw new NotSupportedException();
            }
        }

        public void Run()
        {
            if (File.Exists(ExecutableName))
            {
                Process p = Process.Start(ExecutableName);
            }
        }
    }
}
