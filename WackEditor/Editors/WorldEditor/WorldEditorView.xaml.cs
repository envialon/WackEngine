using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using WackEditor.GameProject;

namespace WackEditor.Editors
{
    /// <summary>
    /// Interaction logic for WorldEditorView.xaml
    /// </summary>
    public partial class WorldEditorView : UserControl
    {
        public WorldEditorView()
        {
            InitializeComponent();
            Loaded += OnWorldEditorViewLoaded;
        }

        private void OnWorldEditorViewLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnWorldEditorViewLoaded;
            Focus();
            //Add a delegate to whenever the undo list changes to regain focus 
            //in case undoing an action loses focus on the controller.
            ((INotifyCollectionChanged)ProjectVM.UndoRedoManager.UndoList).CollectionChanged += (s, e) => Focus();
        }
    }
}
