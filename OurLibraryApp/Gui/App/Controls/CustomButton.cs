using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Controls
{
    class CustomButton:Button
    {
        public CustomButton()
        {
            Init();
            this.Show();
        }

        public void Init()
        {
            GraphicsPath p = new GraphicsPath();
            p.AddEllipse(1, 1, this.Width - 4, this.Height - 4);
            this.Region = new Region(p);
        }

    }
}
