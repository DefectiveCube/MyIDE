using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

using Microsoft.CodeAnalysis;

using Core.Workspace;
using System.Diagnostics;

namespace IDE_WPF
{
    /// <summary>
    /// Interaction logic for ProjectDialog.xaml
    /// </summary>
    public partial class ProjectDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SolutionDirectoryCreated;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ProjectDirectoryCreated;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SolutionCreationFailed;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ProjectCreationFailed;

        private string solutionPath;
        private string projectPath;
        private string projectName;

        public string SolutionPath {
            get { return solutionPath; }
            private set
            {
                solutionPath = value;
                OnPropertyChanged("SolutionPath");
            }
        }

        public string ProjectPath
        {
            get { return projectPath; }
            set
            {
                projectPath = value;
                OnPropertyChanged("ProjectPath");
            }
        }

        public string ProjectName
        {
            get { return projectName; }
            set
            {
                projectName = value;
                OnPropertyChanged("ProjectName");
            }
        }

        public OutputKind Kind { get; private set; }

        public bool IsValid { get; private set; }

        public ProjectDialog()
        {
            InitializeComponent();

            CreateButton.IsEnabled = false;
            CreateButton.Click += CreateButton_Click;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var message = string.Empty;
            var di = new DirectoryInfo(ProjectPath);

            if (!(Application.Current as App).Workspace.TryCreateSolution(Name, out message))
            {
                MessageBox.Show(message);
            }
        }

        void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var message = string.Empty;
            var path = string.Empty;

            CreateButton.IsEnabled = (Application.Current as App).Workspace.IsValidSolution(ProjectName, out path, out message);

            if (string.IsNullOrEmpty(message))
            {
                ProjectPath = path;
            }
            else
            {
                Debug.WriteLine(message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
