using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Src.App.Utils
{
    class ControlUtil
    {
        public static Panel PopulatePanel(int Col, Control[] Controls, int Margin, int W, int H, Color c)
        {
            Panel Panel = new Panel();
            int CurrentCol = 0;
            int CurrentRow = 0;
            for (int i = 0; i < Controls.Count(); i++)
            {
                Control C = Controls[i];
                if (null != C)
                {
                    C.SetBounds(CurrentCol * Margin + (W * CurrentCol), CurrentRow * Margin + H * CurrentRow, W, H);
                }
                CurrentCol++;
                if (CurrentCol == Col)
                {
                    CurrentCol = 0;
                    CurrentRow++;

                }
                Panel.Controls.Add(C);
            }

            Panel.BackColor = c;
            Panel.SetBounds(Margin, Margin, Col * W + Col * Margin, (CurrentRow + 1) * H + (CurrentRow + 1) * Margin);
            return Panel;
        }
    }
}
