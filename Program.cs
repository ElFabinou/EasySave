using easysave;
using System.Configuration;
class Program
{
    static void Main(string[] args)
    {
        View view = new View(View.mode.Console);
        view.mainMenu();
    }
}