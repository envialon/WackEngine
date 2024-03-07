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

namespace WackEditor.GameProject
{
    /// <summary>
    /// Interaction logic for OpenProjectControl.xaml
    /// </summary>
    public partial class OpenProjectControl : UserControl
    {
        public OpenProjectControl()
        {
            InitializeComponent();
        }
        private void OnOpenButtonClick(object sender, RoutedEventArgs e)
        {
            OpenSelectedProject();
        }
        private void OnListBoxItemMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            OpenSelectedProject();
        }
            
        private void OpenSelectedProject()
        {
            ProjectVM project = OpenProjectWindowVM.Open(projectsListBox.SelectedItem as ProjectData);

            bool dialogResult = false;

            Window win = Window.GetWindow(this);
            if (project != null)
            {
                dialogResult = true;
                win.DataContext = project;
            }
            win.DialogResult = dialogResult;
            win.Close();
        }
    }
}
