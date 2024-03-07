using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WackEditor.Utilities;

namespace WackEditor.GameProject
{
    [DataContract]
    class ProjectVM : ViewModelBase
    {
        public static string Extension { get; } = ".wack";

        [DataMember]
        public string ProjectName { get; private set; } = "New Project";

        [DataMember]
        public string ProjectPath { get; private set; }

        public string FullPath => $"{ProjectPath}{ProjectName}{Extension}";

        [DataMember]
        private ObservableCollection<SceneVM> _scenes = new ObservableCollection<SceneVM>();
        public ReadOnlyObservableCollection<SceneVM> Scenes { get; private set; }

        public static ProjectVM CurrentLoadedProject => Application.Current.MainWindow.DataContext as ProjectVM;

        private SceneVM _activeScene;

        public SceneVM ActiveScene
        {
            get => _activeScene;
            set
            {
                if (_activeScene != value)
                {
                    _activeScene = value;
                    OnPropertyChanged(nameof(ActiveScene));
                }
            }
        }


        /// <summary>
        /// Loads a project from a given filename
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static ProjectVM Load(string file)
        {
            Debug.Assert(File.Exists(file));
            return Serializer.FromFile<ProjectVM>(file);
        }

        /// <summary>
        /// Serializes the current project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static void Save(ProjectVM project)
        {
            Serializer.ToFile(project, project.FullPath);
        }

        public void Unload()
        {
        }

        /// <summary>
        /// Runs after the serialization and executes further initialization code
        /// needed bc we don't use the constructor for this class
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_scenes != null)
            {
                Scenes = new ReadOnlyObservableCollection<SceneVM>(_scenes);
                OnPropertyChanged(nameof(Scenes));
                _scenes.Add(new SceneVM("DefaultScene", this));
            }

            ActiveScene = Scenes.FirstOrDefault(x => x.IsActive);
        }

        //TODO : REMOVE THIS CONSTRUCTOR, PROJECTS SHOULD ONLY BE CREATED BY SERIALIZATION
        /// <summary>
        /// Creates a project by code, SHOULD ONLY BE USED FOR DEBUG REASONS
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public ProjectVM(string name, string path)
        {
            ProjectName = name;
            ProjectPath = path;

            OnDeserialized(new StreamingContext());
        }


    }
}
