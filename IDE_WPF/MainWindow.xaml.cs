using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

using Core;
using Core.Workspace;
using Core.Text;

using IDE_WPF.Controls;

namespace IDE_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Controls.Window
    {
        private IWorkspace Workspace
        {
            get { return (Application.Current as App).Workspace; }
        }

        private Log Log
        {
            get { return (Application.Current as App).Log; }
        }

        public MainWindow() : base()
        {
            //Target = this;

            InitializeComponent();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);

            // Resize window

            //MinimalWindow w = new MinimalWindow();

            // Workspace events
            Workspace.DocumentOpened += Workspace_DocumentOpened;
            Workspace.DocumentClosed += Workspace_DocumentClosed;

            Workspace.DocumentAdded += Workspace_Document;
            Workspace.DocumentChanged += Workspace_Document;
            Workspace.DocumentReloaded += Workspace_Document;
            Workspace.DocumentRemoved += Workspace_Document;

            //Workspace.SolutionAdded += Workspace_Solution;
            //Workspace.SolutionChanged += Workspace_Solution;
            //Workspace.SolutionCleared += Workspace_Solution;
            //Workspace.SolutionReloaded += Workspace_Solution;
            //Workspace.SolutionRemoved += Workspace_Solution;

            //Workspace.ProjectAdded += Workspace_Project;
            //Workspace.ProjectChanged += Workspace_Project;
            //Workspace.ProjectReloaded += Workspace_Project;
            //Workspace.ProjectRemoved += Workspace_Project;

            Closing += (s, e) => { Application.Current.Shutdown(); };            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Height = 768;
            Width = 1024;

//            WindowState = WindowState.Maximized;

            var box = new CodeBox();
            var menu = new Menu();
            var button = new Button();

            button.Height = 10;
            button.Width = 40;
            button.Text = "Hello";

            box.Text = "public class Test { ... }";
            box.Margin = new Thickness(0, 20, 0, 0);

            menu.Width = 1024;
            menu.Height = 20;

            menu.Add(new MenuItem() { Text = "File" });
            menu.Add(new MenuItem() { Text = "Edit" });
            menu.Add(new MenuItem() { Text = "Project" });

            Add(menu);
            Add(box);
            Add(button);
        }

        #region Workspace Event Handlers
        private void Workspace_Document(object sender, WorkspaceChangeEventArgs e)
        {                        
            Log.Event(e, e.DocumentId);
            
            //UpdateWorkspaceTreeNodes();
        }

        /*private void Workspace_Project(object sender, WorkspaceChangeEventArgs e)
        {
            UpdateWorkspaceTreeNodes();
        }

        private void Workspace_Solution(object sender, WorkspaceChangeEventArgs e)
        {
            UpdateWorkspaceTreeNodes();
        }

        private void Workspace_WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
        }*/

        private void Workspace_DocumentOpened(object sender, DocumentEventArgs e)
        {
            /*if (e != null && e.Document is Document)
            {
                var doc = e.Document;

                // Add button to the FileControlPanel
                var button = new Button();

                // TODO: don't hard code styling values here
                button.Text = doc.Name;
                button.Width = 75;
                button.Height = 30;
                button.Tag = doc.Id;
                //button.ContextMenu = 

                // TODO: Change OpenDocument to ActivateDocument (after it's created/implemented)
                button.Click += (_sender, _e) => { workspace.OpenDocument(doc.Id); };

                //FileControlPanel.Controls.Add(button);

            
            }*/

            //Editor.SourceTree = e.Document.GetSyntaxTreeAsync().Result;
            
            UpdateWorkspaceTreeNodes();
        }

        private void Workspace_DocumentClosed(object sender, DocumentEventArgs e)
        {
            /*if (e != null && e.Document is Document)
            {
                // Remove button in FileControlPanel that correspondes to this document
                var result = FileControlPanel.Controls.OfType<Button>().Where(b => b.Tag as DocumentId == e.Document.Id).FirstOrDefault();

                if (!string.IsNullOrEmpty(result.Text))
                {
                    FileControlPanel.Controls.Remove(result);
                }

                // Clear TextEditor out (or activate a different document)
                TextEditor.Clear();
                TextEditor.ClearUndo();
            }*/
        }
        #endregion

        private void Editor_TextChanged(object sender, dynamic e)
        {
            // Send text from editor to parser
            // parser.Text = SourceText.From(new TextRange(Editor.Document.ContentStart, Editor.Document.ContentEnd).Text);

            if (Workspace != null && Workspace.CurrentDocument != null)
            {
                //parser.UpdateTree(workspace.CurrentSolution.WithDocumentText)
            }
            else
            {
                // TODO: unintrusively notify user that they are typing in an empty buffer that isn't associated with a file
            }
        }

        private void UpdateWorkspaceTreeNodes()
        {
            /*TreeViewItem root = new TreeViewItem() { Header = string.Format("Solution | {0} Project(s)", Workspace.CurrentSolution.Projects.Count()) };
            root.Tag = Workspace.CurrentSolution.Id;

            TreeViewItem node = null;

            foreach(var project in Workspace.CurrentSolution.Projects)
            {
                node = new TreeViewItem() { Header = project.Name };
                node.Tag = project.Id;

                foreach(var doc in project.Documents)
                {
                    TreeViewItem child = new TreeViewItem() { Header = doc.Name };
                    child.Tag = doc.Id;
                    child.MouseDoubleClick += TreeView_OpenDocument;
                    // ContextMenu
                    node.Items.Add(child);
                }

                root.Items.Add(node);
            }*/

            //solutionExplorer.Tree.Items.Clear();
            //solutionExplorer.Tree.Items.Add(root);            
        }

        private void TreeView_OpenDocument(object sender, MouseButtonEventArgs e)
        {
            /*var item = sender as TreeViewItem;

            if (item != null)
            {
                Workspace.OpenDocument(item.Tag as DocumentId);
            }*/
        }
    }
}