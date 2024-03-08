using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Input;
using WackEditor.Components;
using WackEditor.Utilities;
using WackEditor;

namespace WackEditor.GameProject
{
    [DataContract]
    public class SceneVM : ViewModelBase
    {

        private string _sceneName;
        [DataMember]
        public string Name
        {
            get { return _sceneName; }
            set
            {
                if (value != _sceneName)
                {
                    _sceneName = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [DataMember]
        public ProjectVM Project { get; private set; }

        private bool _isActive;

        [DataMember]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        #region Commands
        public ICommand AddGameEntityCommand { get; private set; }
        public ICommand RemoveGameEntityCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        #endregion

        [DataMember]
        private ObservableCollection<GameEntity> _entities = new ObservableCollection<GameEntity>();

        public ReadOnlyObservableCollection<GameEntity> Entities { get; private set; }

        private void AddGameEntity(GameEntity entity)
        {
            Debug.Assert(!_entities.Contains(entity));
            _entities.Add(entity);
        }
        private void RemoveGameEntity(GameEntity entity)
        {
            Debug.Assert(_entities.Contains(entity));
            _entities.Remove(entity);
        }

        private void InitializeCommands()
        {
            AddGameEntityCommand = new RelayCommand<GameEntity>(
                 x =>
                 {
                     AddGameEntity(x);
                     int index = _entities.IndexOf(x);
                     ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                         $"Add {x.Name} to {Name}",
                         () => { RemoveGameEntity(x); },
                         () => { _entities.Insert(index, x); }
                         ));
                 }
                 );

            RemoveGameEntityCommand = new RelayCommand<GameEntity>(
                x =>
                {
                    int index = _entities.IndexOf(x);
                    RemoveGameEntity(x);
                    ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                        $"Remove {x.Name} from {Name}",
                        () => { _entities.Insert(index, x); },
                        () => { RemoveGameEntity(x); }
                        ));
                }
                );

            UndoCommand = new RelayCommand<object>(
                x => ProjectVM.UndoRedoManager.Undo()
                );

            RedoCommand = new RelayCommand<object>(
                x => ProjectVM.UndoRedoManager.Redo()
                );    
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            //if(_entities == null)
            //{
            //    _entities = new ObservableCollection<GameEntity>();
            //}
            if (_entities != null)
            {
                Entities = new ReadOnlyObservableCollection<GameEntity>(_entities);
                OnPropertyChanged(nameof(Entities));
            }

            InitializeCommands();
        }


        public SceneVM(string name, ProjectVM project)
        {
            Debug.Assert(project != null);
            Project = project;
            Name = name;

            OnDeserialized(new StreamingContext());
        }


    }
}
