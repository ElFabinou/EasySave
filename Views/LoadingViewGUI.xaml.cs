using easysave.Objects;
using easysave.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static easysave.Objects.LanguageHandler;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace easysave.Views
{
    /// <summary>
    /// Interaction logic for LoadingViewGUI.xaml
    /// </summary>
    public partial class LoadingViewGUI : Window
    {

        private List<item> items = new List<item>();
        public bool closeAllowed;
        public ResourceManager language;
        public Loader loader;
        public RegisteredSaveViewModel registeredSaveViewModel;
        private class item
        {
            public string path { get; set; }
            public string size { get; set; }
        }

        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register("Percentage", typeof(double), typeof(LoadingViewGUI), new PropertyMetadata(default(double)));

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closeAllowed)
            {
                e.Cancel = true; // this will prevent to close
            }
        }


        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }


    public LoadingViewGUI(Loader loader)
        {
            InitializeComponent();
            this.language = Instance.rm;
            translateAllItems();
            this.loader=loader;
        }

        public void setPercentage(Loader loader)
        {
            progressBar.Value = Math.Round(loader.getPercentage(), 1);
            item item = new item();
            item.path = (loader.getIsFile() ? loader.getFile().FullName : loader.getFolder().FullName);
            item.size = (loader.getIsFile() ? loader.getFile().Length+" bytes" : "/");
            percentText.Content = Math.Round(loader.getPercentage(), 1)+"%";
            items.Insert(0, item);
            listView.ItemsSource = null;
            listView.ItemsSource = items;
            if(loader.getPercentage() == 100)
            {
                closeAllowed= true;
            }
        }

        private void translateAllItems()
        {
            title.Text = language.GetString("gui_LoadingViewGUI_title");
            tabHeader_path.Header = language.GetString("gui_LoadingViewGUI_tabHeader_path");
            tabHeader_size.Header = language.GetString("gui_LoadingViewGUI_tabHeader_size");
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
            registeredSaveViewModel.initPause(loader.getSaveModel());
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to stop?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                registeredSaveViewModel.initStop(loader.getSaveModel());
                // Close the form
                closeAllowed = true;
                this.Close();
            }
        }

        public void displayInterruptBlacklist()
        {
            MessageBoxResult result = MessageBox.Show("Blacklisted process has been started.", "Close it and continue", MessageBoxButton.OK, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                RegisteredSaveViewModel registeredSaveViewModel = new RegisteredSaveViewModel();
                registeredSaveViewModel.initPause(loader.getSaveModel());
                // Close the form
            }
        }
    }
}