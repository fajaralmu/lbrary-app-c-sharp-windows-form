using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Gui.App.Forms;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Data;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{
    partial class AppForm : BaseForm
    {
        private LoginForm FormLogin;
        private SettingForm SettingForm;
        private VisitForm  FormVisit;
        private EntityForm EntityForm;
        private IssueForm IssueForm;
        private Panel MainPanel = new Panel();

        public AppForm(AppUser User)
        {
            TheUser = User;
            Init();
            UpdateForm();
        }

        public void Init()
        {
            base.Name = "Main Form";
            base.Text = @"Our Library";
            base.Width = 800;
            base.Height = 600;
            MainPanel.SetBounds(0, 0, 800, 600);
            base.Controls.Add(MainPanel);

        }

        protected void BtnLogn_Click(object Sender, EventArgs ev)
        {
            TheUser = null;
            UpdateForm();
        }

        public void UpdateForm()
        {
            this.Show();
            MainPanel.Controls.Clear();
            this.Menu = null;
            if (null != TheUser && TheUser.User != null)
            {
                CustomConsole.WriteLine("User OK");
                TitleLabel WelcomeLabel = new TitleLabel(20) { Text = "Welcome, " + TheUser.User.name };
                TitleLabel AppLabel = new TitleLabel(30) { Text = "Our Library PC APP" };
                Control[] ControlParam = { AppLabel, WelcomeLabel };
                Panel HeaderPanel = ControlUtil.GeneratePanel(1, ControlParam, 5, 400, 50, System.Drawing.Color.Blue);
                MainPanel.Controls.Add(HeaderPanel);
                AddMenus(true);
                this.Enabled = true;
            }
            else
            {
                AddMenus(false);
               
                CustomConsole.WriteLine("User Not OK");
               
            }
        }



        private void AddMenus(bool UserLoggedIn)
        {
            MainMenu Menus = new MainMenu();

            if (UserLoggedIn)
            {
                MenuItem LogoutMenu = new MenuItem("&LogOut");
                LogoutMenu.Click += new EventHandler(LogOutClick);

                Menus.Name = "Master Data";

                MenuItem MasterMenu = Menus.MenuItems.Add("&Master");
                MasterMenu.MenuItems.Add(new MenuItem("&Book Record", new EventHandler(ShowBookRecord)));
                MasterMenu.MenuItems.Add(new MenuItem("&Student", new EventHandler(ShowStudentRecord)));

                MenuItem HistoryMenu = Menus.MenuItems.Add("&History");
                HistoryMenu.MenuItems.Add(new MenuItem("&Transaction History", new EventHandler(ShowTransactionRecord)));
                HistoryMenu.MenuItems.Add(new MenuItem("&Visit Recorder", new EventHandler(ShowVisit)));
                HistoryMenu.MenuItems.Add(new MenuItem("&Visit History", new EventHandler(ShowVisitRecord)));

                MenuItem AdminMenu = Menus.MenuItems.Add("&Transaction");
                AdminMenu.MenuItems.Add(new MenuItem("&Issue Book", new EventHandler(ShowIssueBook)));
               
                Menus.MenuItems.Add(LogoutMenu);

                
            }else
            {
                MenuItem Login = new MenuItem("&LOGIN");
                Login.Click += new EventHandler(LoginClick);
                Menus.MenuItems.Add(Login);
            }
            MenuItem Setting = new MenuItem("&Setting");
            Setting.Click += new EventHandler(SettingClick);
            Menus.MenuItems.Add(Setting);
            this.Menu = Menus;
        }

        private void ShowIssueBook(object sender, EventArgs e)
        {
            IssueForm = new IssueForm(this);
        }

        protected void SettingClick(object Sender, EventArgs ev)
        {
            SettingForm Setting = new SettingForm(this);
        }

        private void ShowVisitRecord(object sender, EventArgs e)
        {
            VisitData Data = new VisitData("Visit History", TheUser);
            EntityForm = new EntityForm(this, Data);
        }

        private void ShowTransactionRecord(object sender, EventArgs e)
        {
            TransactionData Data = new TransactionData("Transaction", TheUser);
            EntityForm = new EntityForm(this, Data);
        }

        private void ShowBookRecord(object sender, EventArgs e)
        {
            BookData Data = new BookData("Book", TheUser);
            EntityForm = new EntityForm(this, Data);

        }

        private void ShowStudentRecord(object sender, EventArgs e)
        {
            StudentData Data = new StudentData("Student", TheUser);
            EntityForm = new EntityForm(this, Data);

        }

        protected void LoginClick(object s, EventArgs ev)
        {
            FormLogin = new LoginForm(this);
            FormLogin.Show();
            FormLogin.Focus();
            this.Enabled = false;
        }

        protected void LogOutClick(object Sender, EventArgs ev)
        {
            var Confirm = MessageBox.Show("Yakin akan keluar ??",
                                     "Confirm Logout!!",
                                     MessageBoxButtons.YesNo);
            if (Confirm == DialogResult.Yes)
            {
                this.TheUser = null;
                UpdateForm();
            }

        }

        protected void ShowVisit(object Sender, EventArgs ev)
        {
            FormVisit = new VisitForm(this);
        }
    }
}

