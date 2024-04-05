using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WackEditor.Dictionaries
{
    public partial class ControlTemplates : ResourceDictionary
    {
        private void OnTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            var exp = textBox.GetBindingExpression(TextBox.TextProperty);
            if (exp == null) { return; }
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (textBox.Tag is ICommand command && command.CanExecute(textBox.Text))
                {
                    command.Execute(textBox.Text);
                }
                else { exp.UpdateSource(); }
                Keyboard.ClearFocus();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                exp.UpdateTarget();
                Keyboard.ClearFocus();
            }

        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }

        private void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e)
        {

            Window window = (Window)((FrameworkElement)sender).TemplatedParent;
            if(window.WindowState == WindowState.Normal)
            {
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowState = WindowState.Normal;   
            }
        }

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            Window window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
        }
    }
}
