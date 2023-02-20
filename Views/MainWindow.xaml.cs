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
using System.Windows.Navigation;
using System.Resources;
using static easysave.Objects.LanguageHandler;
using System.Windows.Shapes;
using System.Resources;
using static easysave.Objects.LanguageHandler;
using System.Threading;

namespace easysave.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string UniqueName = "EasySave";
        private static Mutex mutex;

        public ResourceManager language;

        public MainWindow()
        {
            mutex = new Mutex(true, UniqueName, out bool createdNew);
            if (!createdNew)
            {
                MessageBox.Show("L'application est déjà en cours d'exécution", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                mutex.Dispose();
                Application.Current.Shutdown();
                return;
            }
            InitializeComponent();
            this.language = Instance.rm;
            translateAllItems();
        }

        //Méthode appelé automatiquement quand l'utilisateur ferme l'application
        protected override void OnClosed(EventArgs e)
        {
            mutex?.Dispose();//Si le mutex n'est pas null, on le libère (pour éviter l'erreur s'il est null)
            base.OnClosed(e);//Call la méthode de la class base. Permet de bien gérer la fermeture
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
            MainMenu.Content = new SettingsViewGUI();
        }

        private void initLeave_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Environment.Exit(0);

        }

        private void MainMenu_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void translateAllItems()
        {
            initSaveWork.Content = language.GetString("gui_MainWindow_initSaveWork");
            initSettings.Content = language.GetString("gui_MainWindow_initSettings");
            initLeave.Content = language.GetString("gui_MainWindow_initLeave");
            initSlotCreation.Content = language.GetString("gui_MainWindow_initSlotCreation");
        }
    }
}
