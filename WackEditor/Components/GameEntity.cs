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

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if(_components != null)
            {
                Components = new ReadOnlyObservableCollection<Component>(_components);
                OnPropertyChanged(nameof(Components));
            }
        }

        public GameEntity(SceneVM scene)
        {
            Debug.Assert(scene != null);
            ParentScene = scene;
            _components.Add(new Transform(this));
            Components = new ReadOnlyObservableCollection<Component>(_components);
        }
    }
}
