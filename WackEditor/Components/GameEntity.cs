using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using WackEditor.GameProject;

namespace WackEditor.Components
{
    [DataContract]
    [KnownType(typeof(Transform))]
    public class GameEntity : ViewModelBase
    {
        private int _entityID = Utilities.IdUtils.INVALID_ID;
        public int EntityID
        {
            get => _entityID;
            set
            {
                if (_entityID != value)
                {
                    _entityID = value;
                    OnPropertyChanged(nameof(EntityID));
                }
            }

        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                    if(_isActive)
                    {
                        EntityID = DLLWrapper.EngineAPI.CreateGameEntity(this);
                        //Debug.Assert(Utilities.IdUtils.IsValid(EntityID));
                    }
                    else
                    {
                        DLLWrapper.EngineAPI.RemoveGameEntity(this);
                    }
                    OnPropertyChanged(nameof(IsActive));
                }
            }

        }

        private bool _isEnabled = true;
        [DataMember]
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }

        }

        private string _name;
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }

        }

        [DataMember]
        public SceneVM ParentScene { get; private set; }

        [DataMember]
        private ObservableCollection<Component> _components = new ObservableCollection<Component>();
        public ReadOnlyObservableCollection<Component> Components { get; private set; }

        //public ICommand RenameCommand { get; set; }
        //public ICommand EnableCommand { get; set; }
        private void InitializeCommands()
        {
            //RenameCommand = new RelayCommand<string>(x =>
            //{
            //    string oldName = Name;
            //    Name = x;

            //    ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
            //        $"Renamed {oldName} to {Name}",
            //        nameof(Name),
            //        this,
            //        oldName,
            //        x
            //        ));
            //},
            //x => x != _name
            //);

            //EnableCommand = new RelayCommand<bool>(x =>
            //{
            //    bool oldValue = IsEnabled;
            //    IsEnabled = x;

            //    ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
            //    x ? $"Enabled {Name}" : $"Disabled {Name}",
            //    nameof(IsEnabled),
            //    this,
            //    oldValue,
            //    x
            //    ));
            //}
            //);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_components != null)
            {
                Components = new ReadOnlyObservableCollection<Component>(_components);
                OnPropertyChanged(nameof(Components));
            }
            InitializeCommands();
        }

        public GameEntity(SceneVM scene)
        {
            Debug.Assert(scene != null);
            ParentScene = scene;
            _components.Add(new Transform(this));
            Components = new ReadOnlyObservableCollection<Component>(_components);

            OnDeserialized(new StreamingContext());
        }

        public Component GetComponent(Type type) => Components.FirstOrDefault(c => c.GetType() == type);

        public T GetComponent<T>() where T : Component => GetComponent(typeof(T)) as T;
    }
}
