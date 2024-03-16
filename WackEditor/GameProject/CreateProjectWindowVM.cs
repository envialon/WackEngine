using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
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


    class CreateProjectWindowVM : ViewModelBase
    {
        // TODO: Get path from installation location.
        private readonly string _templatePath = @"..\..\WackEditor\ProjectTemplates\";

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
                    ValidateProjectPath();
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
                    ValidateProjectPath();
                    OnPropertyChanged(nameof(ProjectPath));
                }
            }
        }

        //indicates wether the project path is valid
        private bool _isValid;

        public bool IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private string _errorMsg;
        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                if (_errorMsg != value)
                {
                    _errorMsg = value;
                    OnPropertyChanged(nameof(ErrorMsg));
                }
            }
        }


        private ObservableCollection<ProjectTemplate> _projectTemplates = new ObservableCollection<ProjectTemplate>();
        public ReadOnlyObservableCollection<ProjectTemplate> ProjectTemplates { get; }

        public CreateProjectWindowVM()
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
                ValidateProjectPath();

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                LoggerVM.Log(MessageTypes.Error, $"Failed to open the CreateProject window.");
            }

        }

        /// <summary>
        /// Checks for errors on ProjectName and ProjectPath and if it does,
        /// modifies the ErrorMsg field.
        /// </summary>
        /// <returns></returns>
        private bool ValidateProjectPath()
        {
            var fullPath = ProjectPath;
            if (!Path.EndsInDirectorySeparator(fullPath))
            {
                fullPath += @"\";
            }
            fullPath += $@"{ProjectName}\";

            IsValid = false;

            if (string.IsNullOrEmpty(ProjectName.Trim()))
            {
                ErrorMsg = "Type in a project name.";
            }
            else if (ProjectName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
            {
                ErrorMsg = "Invalid character(s) used in project name.";
            }
            else if (string.IsNullOrWhiteSpace(ProjectPath.Trim()))
            {
                ErrorMsg = "Select a valid project folder";
            }
            else if (ProjectPath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                ErrorMsg = "Invalid character(s) used in project path.";
            }
            else if (Directory.Exists(fullPath) && Directory.EnumerateFileSystemEntries(fullPath).Any())
            {
                ErrorMsg = "Target project folder already exists and its not empty.";
            }
            else
            {
                ErrorMsg = string.Empty;
                IsValid = true;
            }

            return IsValid;
        }

        /// <summary>
        /// Returns the path of the created Project
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public string CreateProject(ProjectTemplate template)
        {
            ValidateProjectPath();
            if (!IsValid)
            {
                return string.Empty;
            }

            if (!Path.EndsInDirectorySeparator(ProjectPath)) ProjectPath += @"\";
            string fullPath = $@"{ProjectPath}{ProjectName}\";

            try
            {
                //Create project dir if it doesn't exist, then create all of the subfolders
                if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
                foreach (string folderName in template.FolderNames)
                {
                    Directory.CreateDirectory(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(fullPath), folderName)));
                }


                //Copy icon and screenshot to the hidden .Wack dirs
                DirectoryInfo dirInfo = new DirectoryInfo(fullPath + @".Wack\");
                dirInfo.Attributes = FileAttributes.Hidden;
                File.Copy(template.IconFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Icon.png")));
                File.Copy(template.ScreenShotFilePath, Path.GetFullPath(Path.Combine(dirInfo.FullName, "Screenshot.png")));

                //open the template project file and create the new one
                string projectXml = File.ReadAllText(template.ProjectFilePath);
                projectXml = string.Format(projectXml, ProjectName, ProjectPath); //put the correct name and path on the xml fields
                string projectFilePath = Path.GetFullPath(Path.Combine(fullPath, $"{ProjectName}{ProjectVM.Extension}"));

                File.WriteAllText(projectFilePath, projectXml);


                return fullPath;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                LoggerVM.Log(MessageTypes.Error, $"Failed to create new project {ProjectName}");
                throw;
            }


        }

    }
}
