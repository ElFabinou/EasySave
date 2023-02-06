using easysave;
using easysave.Views;
using System.Configuration;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Environment.UserName);
        MainView mainView = new MainView();
        mainView.mainMenu();
    }
}