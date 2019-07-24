using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Controls
{
    class TitleLabel : Label
    {
        private int fontSize = 0;
        public TitleLabel(int em_ten_hi)//10 is big
        {
            fontSize = em_ten_hi;
            this.Font = new System.Drawing.Font("Arial", em_ten_hi);
            Init();
        }

        private void Init()
        {

        }

        public TitleLabel()//10 is big
        {
            this.Font = new System.Drawing.Font("Arial", 20);
            Init();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Width = Text.Length * (fontSize == 0 ? 20 : fontSize);
            Height = 100;
            base.OnTextChanged(e);
        }
    }
}
