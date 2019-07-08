using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Controls
{
    class TitleLabel:Label
    {
        public TitleLabel(int em_ten_hi)//10 is big
        {
            this.Font = new System.Drawing.Font("Arial", em_ten_hi);
        }

        public TitleLabel()//10 is big
        {
            this.Font = new System.Drawing.Font("Arial", 20);
        }
    }
}
