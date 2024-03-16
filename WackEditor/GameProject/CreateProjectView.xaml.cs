using System.Windows;
using System.Windows.Controls;

namespace WackEditor.GameProject
{
    /// <summary>
    /// Interaction logic for CreateProjectControl.xaml
    /// </summary>
    public partial class CreateProjectView : UserControl
    {
        public CreateProjectView()
        {
            InitializeComponent();
        }

        private void OnCreateButtonClick(object sender, RoutedEventArgs e)
        {
            CreateProjectWindowVM vm = DataContext as CreateProjectWindowVM;
            string projectPath = vm.CreateProject(templateListBox.SelectedItem as ProjectTemplate);

            bool dialogResult = false;

            Window win = Window.GetWindow(this);
            if (!string.IsNullOrEmpty(projectPath))
            {
                dialogResult = true;
                ProjectVM project = OpenProjectWindowVM.Open(new ProjectData() { ProjectName = vm.ProjectName, ProjectPath = projectPath });
                win.DataContext = project;
            }
            win.DialogResult = dialogResult;
            win.Close();
        }
    }
}
