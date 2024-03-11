using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WackEditor.Utilities;

namespace WackEditor.GameProject
{
    /// <summary>
    /// Basic metadata to keep track of all of the projects created.
    /// </summary>
    [DataContract]
    public class ProjectData
    {
        [DataMember]
        public string ProjectName { get; set; }
        [DataMember]
        public string ProjectPath { get; set; }
        [DataMember]
        public DateTime Date { get; set; }

        public byte[] Icon { get; set; }
        public byte[] Screenshot { get; set; }

        public string FullPath { get => $"{ProjectPath}{ProjectName}{ProjectVM.Extension}"; }
    }

    /// <summary>
    /// Serializable class that contains all of the ProjectData.
    /// </summary>
    [DataContract]
    public class ProjectDataList {
        [DataMember]
        public List<ProjectData> Projects { get; set; } 
    }

    /// <summary>
    /// Keeps track of the created projects and lets you load projects from disk.
    /// </summary>
    class OpenProjectWindowVM  
    {
        private static readonly string _applicationDataPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\WackEditor\";
        private static readonly string _projectDataPath;

        private static readonly ObservableCollection<ProjectData> _projects = new ObservableCollection<ProjectData>();
        public static ReadOnlyObservableCollection<ProjectData> Projects { get; }

        static OpenProjectWindowVM()
        {
            try { 
                //Create the application data directory.
                if(!Directory.Exists(_applicationDataPath)) Directory.CreateDirectory(_applicationDataPath);
                _projectDataPath = $@"{_applicationDataPath}ProjectData.xml";

                Projects = new ReadOnlyObservableCollection<ProjectData>(_projects);
                ReadProjectData();

            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                Logger.Log(MessageTypes.Error, $"Failed to read project data.");
                throw;
            }
        }

        public static ProjectVM Open(ProjectData data)
        {
            ReadProjectData();
            ProjectData project = _projects.FirstOrDefault(x => x.FullPath == data.FullPath);
            if (project != null) {
            }
            else {
                //project that needs to be open is not on the list,
                //means that it was just created.
                project = data;
                _projects.Add(project);
            }
            project.Date = DateTime.Now;
            WriteProjectdata();
            return ProjectVM.Load(project.FullPath);
        }

        private static void WriteProjectdata()
        {
            var projects = _projects.OrderBy(x => x.Date).ToList();
            Serializer.ToFile(new ProjectDataList() { Projects = projects }, _projectDataPath);
        }

        private static void ReadProjectData()
        {
            if(File.Exists(_projectDataPath))
            {
                var projects = Serializer.FromFile<ProjectDataList>(_projectDataPath).Projects.OrderByDescending( x => x.Date);
                _projects.Clear();
                foreach (ProjectData data in projects)
                {
                    if (File.Exists(data.FullPath))
                    {
                        data.Icon = File.ReadAllBytes($@"{data.ProjectPath}\.wack\Icon.png");
                        data.Screenshot = File.ReadAllBytes($@"{data.ProjectPath}\.wack\Screenshot.png");
                        _projects.Add(data);
                    }
                }
            }
        }
    }
}
