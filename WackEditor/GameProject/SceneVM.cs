using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Input;
using WackEditor.Components;
using WackEditor.Utilities;

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

        private void AddGameEntity(GameEntity entity, int index = -1)
        {
            Debug.Assert(!_entities.Contains(entity));
            entity.IsActive = this.IsActive; //is active only if the scene is active.
            if(index == -1)
            {
                _entities.Add(entity);
            }
            else {
                _entities.Insert( index, entity);
            }
        }
        private void RemoveGameEntity(GameEntity entity)
        {
            Debug.Assert(_entities.Contains(entity));
            entity.IsActive = false;
            _entities.Remove(entity);
        }

        private void InitializeCommands()
        {
            AddGameEntityCommand = new RelayCommand<GameEntity>(
                 x =>
                 {
                     int index = _entities.IndexOf(x);
                     AddGameEntity(x, index);
                     ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                         $"Add {x.Name} to {Name}",
                         () => { RemoveGameEntity(x); },
                         () => { AddGameEntity(x, index); }
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
                        () => { AddGameEntity(x,index); },
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

            foreach(GameEntity entity in _entities)
            {
                entity.IsActive = this.IsActive;
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
