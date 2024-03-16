using System.Windows;
using System.Windows.Controls;
using WackEditor.Components;
using WackEditor.Components.Multiselection;
using WackEditor.GameProject;
using WackEditor.Utilities;

namespace WackEditor.Editors
{
    /// <summary>
    /// Interaction logic for ProjectLayout.xaml
    /// </summary>
    public partial class ProjectLayoutView : UserControl
    {
        public ProjectLayoutView()
        {
            InitializeComponent();
        }

        private void OnAddGameEntityButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            SceneVM vm = button.DataContext as SceneVM;
            vm.AddGameEntityCommand.Execute(new GameEntity(vm) { Name = "EmptyGameEntity" });
        }

        private void OnEntitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            #region Undo/redo selection
            ListBox listBox = sender as ListBox;
            List<GameEntity> selection = listBox.SelectedItems.Cast<GameEntity>().ToList();
            List<GameEntity> previousSelection = selection.Except(e.AddedItems.Cast<GameEntity>().Concat(e.RemovedItems.Cast<GameEntity>())).ToList();

            ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                $"Selection changed",
                () =>
                {
                    listBox.UnselectAll();
                    previousSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                },
                () =>
                {
                    listBox.UnselectAll();
                    selection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                }
                ));
            #endregion


            MultiselectGameEntity msEntity = null;

            if (e.AddedItems.Count > 0)
            {
                msEntity = new MultiselectGameEntity(selection);
            }
            InspectorView.Instance.DataContext = msEntity;
        }
    }
}
