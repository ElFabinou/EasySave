using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using easysave.Objects;
using easysave.ViewModels;
using Ookii.Dialogs.Wpf;
using System.Resources;
using static easysave.Objects.LanguageHandler;

namespace easysave.Views
{
    /// <summary>
    /// Logique d'interaction pour CreateSaveWorkGUI.xaml
    /// </summary>
    public partial class CreateSaveWorkViewGUI : Page
    {
        public ResourceManager language;

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
            if(!pathExists(sourcePath.Text) || !pathExists(sourcePath.Text) || saveName.Text == "" || cbType.SelectedItem == null)
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
            this.language = Instance.rm;
            translateAllItems();
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
            var result = viewModel.initSlotCreation();
            if(result.GetReturnTypeEnum() == ReturnHandler.ReturnTypeEnum.Error)
            {
                MessageBox.Show("Erreur : la save existe déjà.");
                return;
            }
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

        private void translateAllItems()
        {
            title.Text = language.GetString("gui_CreateSaveWorkViewGUI_title");
            title_workName.Text = language.GetString("gui_CreateSaveWorkViewGUI_title_workName");
            title_folderToCopy.Text = language.GetString("gui_CreateSaveWorkViewGUI_title_folderToCopy");
            title_targetPath.Text = language.GetString("gui_CreateSaveWorkViewGUI_title_targetPath");
            title_saveType.Text = language.GetString("gui_CreateSaveWorkViewGUI_title_saveType");
            initSlotCreation.Content = language.GetString("gui_CreateSaveWorkViewGUI_initSlotCreation");
            mainMenu.Content = language.GetString("gui_CreateSaveWorkViewGUI_mainMenu");
        }
    }
}
