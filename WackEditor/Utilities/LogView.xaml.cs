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

namespace WackEditor.Utilities
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            LoggerVM.Clear();
        }

        private void OnFilterButtonCLicked(object sender, RoutedEventArgs e)
        {
            int mask = 0;
            mask |= toggleErrors.IsChecked == true ? (int)MessageTypes.Error : 0;
            mask |= toggleWarns.IsChecked == true ? (int)MessageTypes.Warning : 0;
            mask |= toggleInfo.IsChecked == true ? (int)MessageTypes.Info : 0;
            LoggerVM.SetMessageFilter(mask);
        }
    }
}
