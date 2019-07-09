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
        public static Panel PopulatePanel(int Col, Control[] Controls, int Margin, int W, int H, Color c, int panelX = 0, int panelY =0, int panelW =0, int panelH=0)
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
            int X = panelX == 0 ? Margin : panelX;
            int Y = panelY == 0 ? Margin : panelY;
            int finalW = panelW != 0 ? panelW : Col * W + Col * Margin;
            int finalH = panelH != 0 ? panelH : (CurrentRow + 1) * H + (CurrentRow + 1) * Margin;
            Panel.SetBounds(X, Y, finalW, finalH);
            Panel.AutoScroll = false;
            Panel.VerticalScroll.Visible = true;
            Panel.VerticalScroll.Enabled = true;
            Panel.AutoScroll = true;
            Console.WriteLine("Generated Panel x:{0}, y:{1}, width:{2}, height:{3}", X, Y, finalW, finalH);
            return Panel;
        }
    }
}
