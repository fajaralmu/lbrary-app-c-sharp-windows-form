using OurLibraryApp.Src.App.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{

    class BaseForm : Form
    {
        public AppUser TheUser { get; set; }
        protected AppForm RefForm { get; set; }
        public readonly int margin = 10;
        public BaseForm()
        {
            Name = "Default";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        protected override void OnCreateControl()
        {
            this.CenterToScreen();
            Console.WriteLine("Show " + this.Name + " Form");
            base.OnCreateControl();
        }

        protected override void OnClosed(EventArgs e)
        {
            Console.WriteLine("Close " + this.Name + " Form");
          
            base.OnClosed(e);
        }
    }
}
