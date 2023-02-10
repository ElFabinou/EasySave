using easysave.Views;
using System.Configuration;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(ConfigurationManager.AppSettings["configExtension"]!.ToString());
        MainView mainView = new MainView();
        mainView.mainMenu();
    }
}
