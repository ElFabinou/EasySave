using easysave;
using easysave.Views;
using System;
using System.Configuration;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}