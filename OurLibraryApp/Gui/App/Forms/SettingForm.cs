using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Gui.App.Home;
using OurLibraryApp.Src.App.Data;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Forms
{
    class SettingForm : BaseForm
    {
        private TitleLabel Title = new TitleLabel(20) { Text = "Setting" };
        private Label LabelServerName = new Label() { Text = "Server Name:" };
        private TextBox InputServerName = new TextBox();
        private Button SubmitBtn = new Button() { Text = "SAVE" };

        public SettingForm(AppForm RefForm)
        {
            Text = @"Setting";
            InputServerName.Text = GeneralSetting.ServerAddress;
            this.RefForm = RefForm;
            RefForm.Enabled = false;
            Init();
            Show();
        }

        protected void Init()
        {
            Width = 300;
            Height = 300;
            Controls.Clear();
            SubmitBtn.Click += new EventHandler(Submit);
            Panel FormPanel = ControlUtil.GeneratePanel(2, new Control[]
            {
                Title, new BlankControl() {Reserved = ReservedFor.BEFORE_HOR },LabelServerName,InputServerName,SubmitBtn
            }, 5, 150, 30, Color.Beige);
            Controls.Add(FormPanel);
        }

        private void Submit(object Sender, EventArgs ev)
        {
            GeneralSetting.ServerAddress = InputServerName.Text.Trim();
            MessageBox.Show("ServerName Changed!");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.RefForm.Enabled = true;
        }
    }
}
