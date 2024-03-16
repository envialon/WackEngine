using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Input;
using WackEditor.GameProject;
using WackEditor.Utilities;

namespace WackEditor.Components.Multiselection
{
    /// <summary>
    /// Acts as the new DataContext for the InspectorView and handles the 
    /// property modification for a selection of GameEntities, instead of one.
    /// </summary>
    public abstract class MultiSelectEntity : ViewModelBase
    {
        #region Attributes
        protected bool _enableUpdates = true;

        protected bool? _isEnabled = true;
        [DataMember]
        public bool? IsEnabled
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

        protected string _name;
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

#nullable enable

        [DataMember]
        private ObservableCollection<IMSComponent>? _components = new ObservableCollection<IMSComponent>();

        public ReadOnlyObservableCollection<IMSComponent>? Components { get; private set; }
#nullable disable
        #endregion 

        #region commands
        public ICommand RenameSelectionCommand { get; set; }
        public ICommand EnableSelectionCommand { get; set; }
        #endregion

        private void InitializeCommands()
        {
            //TODO: if this grows, it might be posible to do this with reflexion or generic methods.

            RenameSelectionCommand = new RelayCommand<string>(x =>
            {
                _enableUpdates = false;
                Dictionary<GameEntity, string> changedNames = new Dictionary<GameEntity, string>();

                string oldName = Name;
                Name = x;

                for (int i = 0; i < selectedEntities.Count; i++)
                {
                    GameEntity entity = selectedEntities[i];
                    changedNames.Add(entity, (string)entity.Name.Clone());
                    entity.Name = Name;
                }

                ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                    $"Renamed selection to {Name}",
                    () => //UNDO
                    {
                        Name = oldName;
                        foreach (var pair in changedNames)
                        {
                            pair.Key.Name = pair.Value;
                        }
                    },
                    () => //REDO
                    {
                        Name = x;

                        foreach (var pair in changedNames)
                        {
                            pair.Key.Name = Name;
                        }
                    }
                    ));
                _enableUpdates = true;
            },
            x => x != _name
            );

            EnableSelectionCommand = new RelayCommand<bool>(x =>
            {
                Dictionary<GameEntity, bool> changedEnable = new Dictionary<GameEntity, bool>();

                bool oldValue = (bool)IsEnabled;
                IsEnabled = x;

                for (int i = 0; i < selectedEntities.Count; i++)
                {
                    GameEntity entity = (GameEntity)selectedEntities[i];
                    changedEnable.Add(entity, entity.IsEnabled);
                    entity.IsEnabled = (bool)IsEnabled;
                }

                ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                x ? $"Enabled Selection" : $"Disabled Selection",
                () => //undo
                {
                    foreach (var pair in changedEnable)
                    {
                        pair.Key.IsEnabled = pair.Value;
                    }
                },
                () => //redo
                {
                    IsEnabled = x;

                    foreach (var pair in changedEnable)
                    {
                        pair.Key.Name = Name;
                    }
                }
                ));
            }
            );
        }

        public List<GameEntity> selectedEntities { get; private set; }

        public MultiSelectEntity(List<GameEntity> entities)
        {
            InitializeCommands();
            Debug.Assert(entities?.Any() == true);
            Components = new ReadOnlyObservableCollection<IMSComponent>(_components);
            selectedEntities = entities;
            //PropertyChanged += (s, e) =>
            //{
            //    if (_enableUpdates)
            //    {

            //        UpdateGameEntitiesProperties(e.PropertyName);
            //    }
            //};

        }

        public void Refresh()
        {
            _enableUpdates = false;
            UpdateProperties();
            _enableUpdates = true;
        }

        //TODO: these two update methods might need renaming to avoid confusion:

        /// <summary>
        /// Updates the given property of all of the selected GameEntities
        /// </summary>
        /// <param name="propertyName">The name of the property to update for the selected GameEntities</param>
        /// <returns></returns>
        protected virtual bool UpdateGameEntitiesProperties(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IsEnabled): selectedEntities.ForEach(x => x.IsEnabled = IsEnabled.Value); return true;
                case nameof(Name): selectedEntities.ForEach(x => x.Name = Name); return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the properties of the MultiSelectedEntity object (self)
        /// </summary>
        /// <returns></returns>
        protected virtual bool UpdateProperties()
        {
            IsEnabled = GetMixedValue(selectedEntities, new Func<GameEntity, bool>(x => x.IsEnabled));
            Name = GetMixedValue(selectedEntities, new Func<GameEntity, string>(x => x.Name));
            return true;
        }


#nullable enable
        /// <summary>
        /// If all the selected entitites have the same value for the given property
        /// it will return it, if not, null.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="selectedEntities">List of selected GameEntities</param>
        /// <param name="getProperty">Lambda for getting the property, provided a GameEntity</param>
        /// <returns></returns>
        static protected T? GetMixedValue<T>(List<GameEntity> selectedEntities, Func<GameEntity, T> getProperty) where T : IComparable<T>
        {
            T value = getProperty(selectedEntities.First());
            foreach (var entity in selectedEntities.Skip(1))
            {
                if (!value.Equals(getProperty(entity)))
                {
                    return default(T);
                }
            }
            return value;
        }
#nullable disable

        /// <summary> 
        /// Special case for GetMixedValue if the property is a float.
        /// </summary>
        /// <param name="selectedEntities">List of selected GameEntities</param>
        /// <param name="getProperty">Lambda for getting the property, provided a GameEntity</param>
        /// <returns></returns>
        static protected float? GetMixedValue(List<GameEntity> selectedEntities, Func<GameEntity, float> getProperty)
        {
            float value = getProperty(selectedEntities.First());
            foreach (var entity in selectedEntities.Skip(1))
            {
                if (!value.IsTheSameAs(getProperty(entity)))
                {
                    return null;
                }
            }
            return value;
        }
    }


    public class MultiselectGameEntity : MultiSelectEntity
    {
        public MultiselectGameEntity(List<GameEntity> entities) : base(entities)
        {
            Refresh();
        }

    }
}
