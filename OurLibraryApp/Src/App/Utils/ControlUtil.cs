using OurLibraryApp.Gui.App.Controls;
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
        public static Panel GeneratePanel(int Col, Control[] Controls, int Margin, int W, int H, Color c, int panelX = 0, int panelY = 0, int panelW = 0, int panelH = 0)
        {
            return PopulatePanel(true, Col, Controls, Margin, W, H, c, panelX, panelY, panelW, panelH);
        }

        public static Panel PopulatePanel(bool autoscroll, int Col, Control[] Controls, int Margin, int W, int H, Color c, int panelX = 0, int panelY = 0, int panelW = 0, int panelH = 0)
        {
            Panel Panel = new Panel();
            int CurrentCol = 0;
            int CurrentRow = 0;
            int Size = Controls.Count();
            Control[] ControlsClone = Controls;
            for (int i = 0; i < Size; i++)
            {
                Control C = ControlsClone[i];
                if (null != C)
                {
                    C.SetBounds(CurrentCol * Margin + (W * CurrentCol), CurrentRow * Margin + H * CurrentRow, W, H);
                    if (C.GetType().Equals(typeof(BlankControl)))
                    {
                        BlankControl blankC = (BlankControl)C;
                        switch (blankC.Reserved)
                        {
                            case ReservedFor.BEFORE_HOR:
                                Control beforeContHor = Controls[i - 1];
                                beforeContHor.Width += blankC.Width;
                                Panel.Controls.Remove(beforeContHor);
                                Controls[i] = beforeContHor;
                                C = beforeContHor;
                                break;
                            case ReservedFor.BEFORE_VER:
                                Control beforeContVer = Controls[i - Col];
                                beforeContVer.Height += blankC.Height;
                                Panel.Controls.Remove(beforeContVer);
                                Controls[i] = beforeContVer;
                                C = beforeContVer;
                                break;
                            case ReservedFor.AFTER_HOR:

                                break;
                            case ReservedFor.AFTER_VER:

                                break;
                            default:
                                break;
                        }
                    }

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
            if (autoscroll)
            {
                Panel.AutoScroll = false;
                Panel.VerticalScroll.Visible = true;
                Panel.VerticalScroll.Enabled = true;
                Panel.AutoScroll = true;
            }
            Gui.App.Controls.CustomConsole.WriteLine("Generated Panel x:{0}, y:{1}, width:{2}, height:{3}", X, Y, finalW, finalH);
            return Panel;
        }


        public static float NewFontSize(Graphics graphics, Size size, Font font, string str)
        {
            SizeF stringSize = graphics.MeasureString(str, font);
            float wRatio = size.Width / stringSize.Width;
            float hRatio = size.Height / stringSize.Height;
            float ratio = Math.Min(hRatio, wRatio);
            return font.Size * ratio;
        }

        public static ComboBox FillComboInteger(int begin, int end)
        {

            ComboBox Cbx = new ComboBox();
            for(int i = begin; i <= end; i++)
            {
                Cbx.Items.Add(i);
            }
            return Cbx;
        }
    }
}
