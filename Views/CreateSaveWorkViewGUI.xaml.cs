using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    }
}
