using OurLibrary.Models;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Data;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{
    class BooksForm : BaseForm
    {
        private BaseData EntityData;
        private Panel ListPanel;
        private Panel DetailPanel;
        private Panel NavPanel;
        private TextBox InputOffset = new TextBox() { Text = "0" };
        private TextBox InputLimit = new TextBox() { Text = "10" };
        private Label InfoOffsetLimit = new Label() { };
        private Button BtnFilterPagination = new Button() { Text = "OK" };
        private int Offset = 0;
        private int Limit = 10;

        public BooksForm(AppForm RefForm, BaseData entityData)
        {
            this.RefForm = RefForm;
            entityData.SetEntityForm(this);
            this.EntityData = entityData;
            RefForm.Enabled = false;
            Init();
            Show();
        }

        private void Init()
        {
            Width = 1300;
            Height = 700;
            Name = EntityData.Name + "Form";
            Text = @"" + EntityData.Name + "Form";
            BtnFilterPagination.Click += (o, e) =>
            {
                try
                {
                    Offset = int.Parse(InputOffset.Text);
                    Limit = int.Parse(InputLimit.Text);
                    Navigate(Offset, Limit);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Input tidak valid", "Error");
                    Console.WriteLine(ex.Message);
                    return;
                }
            };
            Control[] FilterControls =
            {
                new Label() {Text="Page(from 0)" }, InputOffset,
                new Label() {Text="Record Per Page" }, InputLimit,
                BtnFilterPagination, InfoOffsetLimit
            };
            Panel PagingPanel = ControlUtil.PopulatePanel(6, FilterControls, 0, 110, 30, Color.Azure);
            Controls.Add(PagingPanel);
            GenerateTable();

        }

        public void ShowDetail(object OBJ)
        {

            Controls.Remove(DetailPanel);
            DetailPanel = EntityData.ShowDetail(OBJ);
            Controls.Add(DetailPanel);
        }

        private void Navigate(int Offset, int Limit)
        {
            Console.WriteLine("Navigating {0}, {1}", Offset, Limit);
            GenerateTable(Offset, Limit);
        }

        private void GenerateTable(int Offset = 0, int Limit = 0)
        {
            Controls.Remove(ListPanel);
            this.Offset = Offset;
            this.Limit = Limit == 0 ? this.Limit : Limit;
            ListPanel = EntityData.UpdateListPanel(Offset, Limit);
            Controls.Add(ListPanel);
            GenerateNavButton();
        }

        private void GenerateNavButton()
        {
            Controls.Remove(NavPanel);
            int Page = EntityData.EntityTotalCount / Limit + (EntityData.EntityTotalCount % Limit > 0 ? 1 : 0);

            Control[] NavButtons = new Control[Page];
            for (int i = 0; i < Page; i++)
            {
                Button NavBtn = new Button() { Text = (i + 1).ToString() };
                if (i == Offset)
                {
                    NavBtn.Text = "&" + NavBtn.Text;
                }
                int page = i;
                NavBtn.Click += (o, e) =>
                {
                    Navigate(page, Limit);
                };
                NavButtons[i] = NavBtn;
            }
            NavPanel = ControlUtil.PopulatePanel(10, NavButtons, 5, 50, 50, Color.AliceBlue, 5, 65, 760, 60);
            Controls.Add(NavPanel);
            InfoOffsetLimit.Text = "Page: " + Offset + ", Rec per Page: " + Limit;
        }


        protected override void OnClosed(EventArgs e)
        {
            RefForm.Enabled = true;
            base.OnClosed(e);
        }

        public void UpdatePanel(Panel P)
        {
            Controls.Remove(P);
            Controls.Add(P);
        }

    }
}
