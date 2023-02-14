using easysave.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using static easysave.Objects.LanguageHandler;

namespace easysave.Views
{
    /// <summary>
    /// Interaction logic for SettingsViewGUI.xaml
    /// </summary>
    public partial class SettingsViewGUI : Page
    {
        public SettingsViewGUI()
        {

            InitializeComponent();
            Array allLanguages = Enum.GetNames(typeof(Language));
            int languageIndex = 0;
            for (int i = 0; i<allLanguages.Length; i++)
            {
                if (allLanguages.GetValue(i)!.ToString() == Instance.getLanguage().ToString())
                {
                    languageIndex = i;
                }
            }
            cbLanguage.ItemsSource = allLanguages;
            cbLanguage.SelectedIndex = languageIndex;
            List<string> extensions = new List<string>();
            extensions.Add("json");
            extensions.Add("xml");
            cbFormat.SelectedIndex = 0;
            if (ConfigurationManager.AppSettings["configExtension"]!.ToString() == "xml")
            {
                cbFormat.SelectedIndex = 1;
            }
            cbFormat.ItemsSource = extensions;
        }

        private void cbFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoggerHandlerViewModel loggerHandlerViewModel = new LoggerHandlerViewModel();
            switch (cbFormat.SelectedIndex)
            {
                case 0:
                    loggerHandlerViewModel.setLoggerExtension("json").Print();
                    break;
                case 1:
                    loggerHandlerViewModel.setLoggerExtension("xml").Print();
                    break;
            }
        }

        private void cbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LanguageHandlerViewModel languageHandlerViewModel = new LanguageHandlerViewModel();
            languageHandlerViewModel.setLanguage((Language)(cbLanguage.SelectedIndex));
            languageHandlerViewModel.initLanguageSelection().Print();
        }

        private void changeLanguage_Click(object sender, RoutedEventArgs e)
        {
            tabSelector.SelectedIndex = 0;
        }

        private void changeLogsFormat_Click(object sender, RoutedEventArgs e)
        {
            tabSelector.SelectedIndex = 1;
        }

        private void blacklist_Click(object sender, RoutedEventArgs e)
        {
            tabSelector.SelectedIndex = 2;
        }
    }
}