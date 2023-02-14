﻿using System;
using System.Collections.Generic;
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
using easysave.ViewModels;

namespace easysave.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void initSaveWork_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Content = new SaveWorkListViewGUI();
        }

        private void initSlotCreation_Click(object sender, RoutedEventArgs e)
        {
            MainMenu.Content = new CreateSaveWorkViewGUI();
        }

        private void initSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void initLeave_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Environment.Exit(0);

        }

        private void MainMenu_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel MainViewModelobj = new MainViewModel();
            MainViewModelobj.StartProcessMonitor();
        }
    }
}
