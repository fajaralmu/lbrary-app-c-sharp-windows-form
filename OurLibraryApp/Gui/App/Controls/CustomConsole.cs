using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Gui.App.Controls
{
    class CustomConsole
    {
        public static string DateString()
        {
            string date = "";

            DateTime Now = DateTime.Now;

            date = Now.Day + "-" + Now.Month + "-" + Now.Year + " " + Now.Hour + ":" + Now.Minute + ":" + Now.Second + "." + Now.Millisecond;

            return date;
        }

        public static void WriteLine(string str, params object[] args)
        {
            Console.WriteLine(DateString() + "  " + str, args);
        }
        public static void WriteLine(string str)
        {
            Console.WriteLine(DateString() + "  " + str);
        }
    }
}
