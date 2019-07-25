using OurLibraryApp.Gui.App.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp
{
    class Program
    {
        static bool run = true;
       
        static void Main(string[] args)
        {
            Gui.App.Controls.CustomConsole.WriteLine("BEGIN LIBRARY APP");
            Application.Run(new AppForm(null));
            Console.ReadLine();
        }

       
    }
}
