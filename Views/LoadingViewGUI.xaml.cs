﻿using easysave.Objects;
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

namespace easysave.Views
{
    /// <summary>
    /// Interaction logic for LoadingViewGUI.xaml
    /// </summary>
    public partial class LoadingViewGUI : Window
    {

        private List<item> items = new List<item>();

        private class item
        {
            public string path { get; set; }
            public string size { get; set; }
        }

        public LoadingViewGUI()
        {
            InitializeComponent();
        }

        public void setPercentage(Loader loader)
        {
            progressBar.Value = loader.getPercentage();
            item item = new item();
            item.path = (loader.getIsFile() ? loader.getFile().FullName : loader.getFolder().FullName);
            item.size = (loader.getIsFile() ? loader.getFile().Length+" bytes" : "/");
            percentText.Content = Math.Round(loader.getPercentage(),1)+"%";
            items.Insert(0, item);
            listView.ItemsSource = null;
            listView.ItemsSource = items;
        }

    }
}