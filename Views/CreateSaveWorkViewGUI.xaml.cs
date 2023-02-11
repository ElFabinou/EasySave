using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using easysave.Objects;
using Ookii.Dialogs.Wpf;

namespace easysave.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateSaveWorkGUI.xaml
    /// </summary>
    public partial class CreateSaveWorkViewGUI : Page
    {

        public CreateSaveWorkViewGUI()
        {
            InitializeComponent();
            cbType.ItemsSource = Enum.GetValues(typeof(RegisteredSaveWork.Type)).Cast<RegisteredSaveWork.Type>();
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private string openFileExplorer()
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Select a directory";
            dialog.UseDescriptionForTitle = true;
            if (dialog.ShowDialog() == true)
            {
                string selectedPath = dialog.SelectedPath;
                return selectedPath;
            }
            return "";
        }

        private void initSlotCreation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fileExplorerSource_Click(object sender, RoutedEventArgs e)
        {
            sourcePath.Text = openFileExplorer();
        }

        private void fileExplorerTarget_Click(object sender, RoutedEventArgs e)
        {
            targetPath.Text = openFileExplorer();
        }
    }
}
