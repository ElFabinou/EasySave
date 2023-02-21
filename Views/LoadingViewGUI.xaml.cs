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
        private bool closeAllowed;
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


        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);



        const uint MF_BYCOMMAND = 0x00000000;
        const uint MF_GRAYED = 0x00000001;
        const uint MF_ENABLED = 0x00000000;

        const uint SC_CLOSE = 0xF060;

        const int WM_SHOWWINDOW = 0x00000018;
        const int WM_CLOSE = 0x10;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.hwndSourceHook));
            }
        }


        IntPtr hwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_SHOWWINDOW)
            {
                IntPtr hMenu = GetSystemMenu(hwnd, false);
                if (hMenu != IntPtr.Zero)
                {
                    EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                }
            }
            else if (msg == WM_CLOSE)
            {
                handled = false;
            }
            return IntPtr.Zero;
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