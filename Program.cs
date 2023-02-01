using easysave;
using System.Configuration;
class Program
{
    static void Main(string[] args)
    {
        string s = ConfigurationManager.AppSettings["logPath"]!.ToString();
        Console.WriteLine(s);
        View view = new View(View.mode.Console);
        view.mainMenu();
    }
}