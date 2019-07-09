using OurLibraryApp.Gui.App.Controls;
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
        private VisitForm  FormVisit;
        private EntityForm FormBooks;
        private Panel MainPanel = new Panel();

        public AppForm(AppUser User)
        {
            TheUser = User;
            Init();
            Update();
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
            Update();
        }

        public void Update()
        {
            this.Show();
            MainPanel.Controls.Clear();
            this.Menu = null;
            if (null != TheUser && TheUser.User != null)
            {
                Console.WriteLine("User OK");
                TitleLabel WelcomeLabel = new TitleLabel(20) { Text = "Welcome, " + TheUser.User.name };
                TitleLabel AppLabel = new TitleLabel(30) { Text = "Our Library PC APP" };
                Control[] ControlParam = { AppLabel, WelcomeLabel };
                Panel HeaderPanel = ControlUtil.PopulatePanel(1, ControlParam, 5, 400, 50, System.Drawing.Color.Blue);
                MainPanel.Controls.Add(HeaderPanel);
                AddMenus();
                this.Enabled = true;
            }
            else
            {
                FormLogin = new LoginForm(this);
                FormLogin.Show();
                FormLogin.Focus();
                Console.WriteLine("User Not OK");
                this.Enabled = false;
            }
        }

        private void AddMenus()
        {

            MenuItem LogoutMenu = new MenuItem("&LogOut");
            LogoutMenu.Click += new EventHandler(LogOutClick);

            MainMenu Menus = new MainMenu();
            Menus.Name = "Master Data";

            MenuItem MasterMenu = Menus.MenuItems.Add("&Master");
            MasterMenu.MenuItems.Add(new MenuItem("&Book Record", new EventHandler(ShowBookRecord)));
            MasterMenu.MenuItems.Add(new MenuItem("&Student", new EventHandler(ShowStudentRecord)));
            
            MenuItem AdminMenu = Menus.MenuItems.Add("&Admin");
            AdminMenu.MenuItems.Add(new MenuItem("&Transactions"));
            AdminMenu.MenuItems.Add(new MenuItem("&Visit Recorder", new EventHandler(ShowVisit)));
           
                       
            Menus.MenuItems.Add(LogoutMenu);

            this.Menu = Menus;
        }

        private void ShowBookRecord(object sender, EventArgs e)
        {
            BookData Data = new BookData("Book");
            FormBooks = new EntityForm(this, Data);

        }

        private void ShowStudentRecord(object sender, EventArgs e)
        {
            StudentData Data = new StudentData("Student");
            FormBooks = new EntityForm(this, Data);

        }

        protected void LogOutClick(object Sender, EventArgs ev)
        {
            var Confirm = MessageBox.Show("Yakin akan keluar ??",
                                     "Confirm Logout!!",
                                     MessageBoxButtons.YesNo);
            if (Confirm == DialogResult.Yes)
            {
                this.TheUser = null;
                Update();
            }

        }

        protected void ShowVisit(object Sender, EventArgs ev)
        {
            FormVisit = new VisitForm(this);
        }
    }
}

