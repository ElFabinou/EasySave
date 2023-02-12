using easysave.Objects;
using easysave.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
using static easysave.Objects.LanguageHandler;
namespace easysave.Views
{
    /// <summary>
    /// Logique d'interaction pour SaveWorkListViewGUI.xaml
    /// </summary>
    /// 
    public partial class SaveWorkListViewGUI : Page
    {
        private IList selected;
        public ResourceManager language;

        public SaveWorkListViewGUI()
        {
            InitializeComponent();
            language = Instance.rm;
            RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
            List<RegisteredSaveWork>  registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
            listView.ItemsSource = registeredSaveViewModelList;
        }

        private void initSaveWork_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i<selected.Count;i++)
            {
                RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel((RegisteredSaveWork?)selected[i]);
                viewModel.initRegisteredSaveWork().Print();

            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initSaveWork.IsEnabled = true;
            deleteSaveWork.IsEnabled = true;
            selected = listView.SelectedItems;
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(null);
        }

        private void deleteSaveWork_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i<selected.Count; i++)
            {
                RegisteredSaveViewModel viewModel = new RegisteredSaveViewModel((RegisteredSaveWork?)selected[i]);
                viewModel.initSlotDeletion().Print();
                RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                List<RegisteredSaveWork> registeredSaveViewModelList = registeredSaveViewModel.initSlotSelection();
                listView.ItemsSource = registeredSaveViewModelList;
            }
        }
    }
}
