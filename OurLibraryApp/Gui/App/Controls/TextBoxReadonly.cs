using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Controls
{
    class TextBoxReadonly : TextBox
    {
        public TextBoxReadonly(int FontSize = 0)
        {
            if (FontSize > 0)
            {
                Font = new System.Drawing.Font("Arial", FontSize, System.Drawing.FontStyle.Bold);
            }
            this.ReadOnly = true;
            this.BorderStyle = 0;
            this.BackColor = this.BackColor;
            this.TabStop = false;
        }
    }
}
