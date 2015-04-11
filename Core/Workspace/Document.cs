using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Core.Workspace
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    public class CoreDocument
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public delegate EventHandler FileLoaded(object sender, EventArgs e);

        public Guid Id { get; internal set; }
        
        // Tree of the document
        public SyntaxTree Tree;

        internal bool IsOpen { get; set; }
        internal bool IsSaved { get; set; }
        internal string Name { get; set; }
        internal string Path { get; set; }

        public void Open() { }
        public void Close() { }

        internal void Load()
        {

        }
        
        void LoadTree() {
            var ms = new MemoryStream();
            var fs = File.OpenRead(Path);

            fs.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(ms);

            Tree = SyntaxFactory.ParseSyntaxTree(reader.ReadToEnd());
        }

        internal void Save()
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(Tree.ToString()));
            FileStream fs = new FileStream(Path, FileMode.OpenOrCreate);

            ms.CopyTo(fs);

            fs.Close();
            ms.Close();
        }

        internal void Save(string path) { }
        internal void Rename() { }
        internal void Delete() { }

        public void Update(SyntaxTree tree)
        {
            IsSaved = false;
            this.Tree = tree;
        }

    }
}
