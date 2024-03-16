using System.Windows.Controls;

namespace WackEditor.Editors
{
    /// <summary>
    /// Interaction logic for Inspector.xaml
    /// </summary>
    public partial class InspectorView : UserControl
    {
        public static InspectorView Instance { get; private set; }

        public InspectorView()
        {
            InitializeComponent();
            DataContext = null;
            Instance = this;
        }
    }
}
