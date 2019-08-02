using OurLibraryApp.Gui.App.Home;
using OurLibraryApp.Src.App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Forms
{
    class AddForm : BaseForm
    {
        private Panel InnerPanel;
        public AddForm(Panel InnerPanel, BaseData BaseData)
        {

            this.InnerPanel = InnerPanel;
            Init();
            Text = @"Add Form";
            Controls.Add(InnerPanel);
        }

        private void Init()
        {
            Height = InnerPanel.Height * 5 / 4;
            Width = InnerPanel.Width * 5 / 4;
        }

        protected override void OnClosed(EventArgs e)
        {
            Dispose();
            base.OnClosed(e);
        }
    }
}
