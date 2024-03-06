using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Xps;
using WackEditor.Utilities;

namespace WackEditor.GameProject
{
    [DataContract]
    public class ProjectTemplate
    {
        [DataMember]
        public string ProjectType { get; set; }
        [DataMember]
        public string ProjectFile { get; set; }

        [DataMember]
        public List<string> FolderNames { get; set; }

        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }
        public string IconFilePath { get; set; }
        public string ScreenShotFilePath { get; set; }
        public string ProjectFilePath { get; set; }

    }


    class NewProjectViewModel : ViewModelBase
    {
        // TODO: Get path from installation location.
        private readonly string _templatePath = @"..\..\WackEditor\ProjectTemplates";

        private string _projectPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\WackEngine\";
        private string _projectName = "NewProject";

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }
        public string ProjectPath
        {
            get => _projectPath;
            set
            {
                if (_projectPath != value)
                {
                    _projectPath = value;
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

        public NewProjectViewModel()
        {
            ProjectTemplates = new ReadOnlyObservableCollection<ProjectTemplate>(_projectTemplates);

            try
            {
                string[] templateFiles = Directory.GetFiles(_templatePath, "template.xml", SearchOption.AllDirectories);
                Debug.Assert(templateFiles.Any());
                foreach (string filename in templateFiles)
                {
                    ProjectTemplate template = Serializer.FromFile<ProjectTemplate>(filename);
                    template.IconFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filename), "ProjectIcon.png"));
                    template.Icon = File.ReadAllBytes(template.IconFilePath);
                    template.ScreenShotFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filename), "Screenshot.png"));
                    template.Screenshot = File.ReadAllBytes(template.ScreenShotFilePath);
                    template.ProjectFilePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filename), template.ProjectFile));
                    _projectTemplates.Add(template);
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                // TODO: log error
            }

        }
    }
}
