using System;
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
