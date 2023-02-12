using easysave.Objects;
using System;
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
using System.Windows.Shapes;

namespace easysave.Views
{
    /// <summary>
    /// Interaction logic for LoadingViewGUI.xaml
    /// </summary>
    public partial class LoadingViewGUI : Window
    {

        public LoadingViewGUI()
        {
            InitializeComponent();
        }

        public void sendPercentage(Loader loader)
        {
            if (loader.getIsFile())
            {
                progressBar.Value = Math.Round(loader.getPercentage(), 0);
            }
            else
            {
                progressBar.Value = Math.Round(loader.getPercentage(), 0);
            }
        }
    }
}
