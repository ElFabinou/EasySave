using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using easysave.Objects;
using easysave.ViewModels;
using Microsoft.VisualBasic.Devices;
using Ookii.Dialogs.Wpf;

namespace easysave.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateSaveWorkGUI.xaml
    /// </summary>
    public partial class CreateSaveWorkViewGUI : Page
    {

        private void enableButton()
        {
            if (!pathExists(targetPath.Text))
            {
                targetPath.Background = System.Windows.Media.Brushes.Red;
            }
            else
            {
                targetPath.Background = System.Windows.Media.Brushes.White;
            }
            if (!pathExists(sourcePath.Text))
            {
                sourcePath.Background = System.Windows.Media.Brushes.Red;
            }
            else
            {
                sourcePath.Background = System.Windows.Media.Brushes.White;
            }
            if(!pathExists(sourcePath.Text) || !pathExists(sourcePath.Text) || saveName.Text == "" || cbType == null)
            {
                initSlotCreation.IsEnabled = false;
                return;

            }
            initSlotCreation.IsEnabled = true;

        }

        public CreateSaveWorkViewGUI()
        {
            InitializeComponent();
            cbType.ItemsSource = Enum.GetValues(typeof(RegisteredSaveWork.Type)).Cast<RegisteredSaveWork.Type>();
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(null);
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
            RegisteredSaveWork registeredSaveWork = new RegisteredSaveWork();
            registeredSaveWork.setType(RegisteredSaveWork.Type.Complet);
            if (cbType.SelectedIndex == 1) registeredSaveWork.setType(RegisteredSaveWork.Type.Differentiel);
            registeredSaveWork.setSaveName(saveName.Text);
            registeredSaveWork.setSourcePath(sourcePath.Text);
            registeredSaveWork.setTargetPath(targetPath.Text);
            RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel(registeredSaveWork);
            viewModel.initSlotCreation().Print();
            NavigationService.Navigate(null);
        }

        private void fileExplorerSource_Click(object sender, RoutedEventArgs e)
        {
            sourcePath.Text = openFileExplorer();
        }

        private void fileExplorerTarget_Click(object sender, RoutedEventArgs e)
        {
            targetPath.Text = openFileExplorer();
        }

        private void saveName_TextChanged(object sender, TextChangedEventArgs e)
        {
            enableButton();
        }

        private void sourcePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            enableButton();
        }

        private void targetPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            enableButton();
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            enableButton();
        }

        public bool pathExists(string path)
        {
            Console.WriteLine(Directory.Exists(path));
            return Directory.Exists(path);
        }
    }
}
