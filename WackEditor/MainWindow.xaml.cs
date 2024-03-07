using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WackEditor.GameProject;

namespace WackEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
            Closing += OnMainWindowClosing;
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            Closing -= OnMainWindowClosing;
            ProjectVM.CurrentLoadedProject?.Unload();
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnMainWindowLoaded;
            OpenProjectBrowserDialog();

        }

        private void OpenProjectBrowserDialog()
        {
            ProjectBrowser projectBrowser = new ProjectBrowser();
            if(projectBrowser.ShowDialog() == false || projectBrowser.DataContext ==null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                ProjectVM.CurrentLoadedProject?.Unload();
                DataContext = projectBrowser.DataContext;
                //TODO: open the project
            }
        }
    }
}