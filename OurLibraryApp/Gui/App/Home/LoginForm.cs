using OurLibrary.Models;
using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{
    class LoginForm : BaseForm
    {
        TextBox TxtUsername = new TextBox();
        TextBox TxtPwd = new TextBox();
        UserClient userClient = new UserClient();

        Button BtnLogin = new Button();
        AppForm RefForm;


        public LoginForm(AppForm RefForm)
        {
            Init();

            this.RefForm = RefForm;
        }

        private void Init()
        {
            Width = 600;
            Height = 400;
            Text = @"Login User";
            Name = "LoginForm";
            BtnLogin.Click += new EventHandler(BtnLogn_Click);
            BtnLogin.Text = "LOGIN";

            Control[] LoginControls =
            {
                new TitleLabel() {Text="LOGIN FORM"},null,
                new Label() {Text="Username" }, TxtUsername,
                new Label() {Text="Password" }, TxtPwd,
                BtnLogin
            };
            Panel LoginPanel = ControlUtil.PopulatePanel(2, LoginControls, 5, 200, 50, System.Drawing.Color.White);
            LoginPanel.SetBounds(50, 50, LoginPanel.Width, LoginPanel.Height);
            Controls.Add(LoginPanel);
            TxtUsername.Focus();
        }

        protected void BtnLogn_Click(object Sender, EventArgs ev)
        {
            string Username = TxtUsername.Text;
            string Password = TxtPwd.Text;
            user LoggedUser = userClient.UserLogin(Username, Password);
            if (null != LoggedUser)
            {
                MessageBox.Show("Login Berhasil!","Info");
                RefForm.TheUser = new AppUser() { User = LoggedUser };
                RefForm.Update();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Login Gagal!","Info");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            RefForm.Dispose();
            base.OnClosed(e);
            Console.WriteLine("Press Enter to Exit");
        }
    }
}
