﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WackEditor.Components;
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
            InspectorView.Instance.DataContext = null;

            if (e.AddedItems.Count >0)
            {
                InspectorView.Instance.DataContext = (sender as ListBox).SelectedItems[0] as GameEntity;
            }
            ListBox listBox = sender as ListBox;
            List<GameEntity> selection = listBox.SelectedItems.Cast<GameEntity>().ToList();
            List<GameEntity> previousSelection = selection.Except(e.AddedItems.Cast<GameEntity>().Concat(e.RemovedItems.Cast<GameEntity>())).ToList();

            ProjectVM.UndoRedoManager.Add(new UndoRedoAction(
                $"Selection changed",
                () => { 
                    listBox.UnselectAll();
                    previousSelection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true); 
                },
                () => {
                    listBox.UnselectAll();
                    selection.ForEach(x => (listBox.ItemContainerGenerator.ContainerFromItem(x) as ListBoxItem).IsSelected = true);
                }
                ));

        }
    }
}
