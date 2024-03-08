using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
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

        public static UndoRedo UndoRedoManager { get; } = new UndoRedo();

        public ICommand AddScene { get; private set; }

        public ICommand RemoveScene { get; private set; }

        public ICommand Undo {  get; private set; }

        public ICommand Redo { get; private set; }


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
        /// Initializes all of the editor commands
        /// </summary>
        private void InitializeCommands()
        {            
            AddScene = new RelayCommand<object>(x =>
            {
                AddSceneInternal($"New Scene {_scenes.Count}");
                SceneVM newScene = _scenes.Last();
                int sceneIndex = _scenes.Count - 1;
                UndoRedoManager.Add(new UndoRedoAction(
                    $"Add {newScene.Name}",
                    () => RemoveSceneInternal(newScene),
                    () => _scenes.Insert(sceneIndex, newScene)
                    ));
            });

            RemoveScene = new RelayCommand<SceneVM>(
            x =>
            {
                int sceneIndex = _scenes.IndexOf(x);
                RemoveSceneInternal(x);
                UndoRedoManager.Add(new UndoRedoAction(
                    $"Remove {x.Name}",
                    () => _scenes.Insert(sceneIndex, x),
                    () => RemoveSceneInternal(x)
                    ));
            }, x => !x.IsActive);

            Undo = new RelayCommand<object>(
                x => UndoRedoManager.Undo()
                ); 

            Redo = new RelayCommand<object>(
                x=> UndoRedoManager.Redo()
                );
        }

        /// <summary>
        /// Runs after the serialization (project instance is created)
        /// and executes further initialization code,
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
            }

            ActiveScene = Scenes.FirstOrDefault(x => x.IsActive);

            InitializeCommands();
        }

        /// <summary>
        /// Creates a new SceneVM and adds it to the _scenes list
        /// DOESN'T have undo/redo functionality
        /// </summary>
        /// <param name="sceneName"></param>
        private void AddSceneInternal(string sceneName)
        {
            Debug.Assert(!string.IsNullOrEmpty(sceneName));
            _scenes.Add(new SceneVM(sceneName, this));
        }

        /// <summary>
        /// removes a given SceneVM from _scenes lists
        /// DOESN'T have undo/redo functionality
        /// </summary>
        /// <param name="scene"></param>
        private void RemoveSceneInternal(SceneVM scene)
        {
            Debug.Assert(_scenes.Contains(scene));
            _scenes.Remove(scene);
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
