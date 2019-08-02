using OurLibrary.Models;
using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Gui.App.Forms;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Data;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{
    class EntityForm : BaseForm
    {
        private PictureBox LoadingImg = new PictureBox() { Width = 300, Height = 300, SizeMode = PictureBoxSizeMode.StretchImage, ImageLocation = "https://i.giphy.com/3oEjI6SIIHBdRxXI40.gif" };
        private BaseData EntityData;
        public static AddForm addForm;
        private Panel ListPanel;
        private Panel DetailPanel = new Panel();
        private Panel NavPanel;
        private Panel MiscPanel = new Panel();

        private Label InfoFilter = new Label();
        private TextBox InputOffset = new TextBox() { Text = "0" };
        private TextBox InputLimit = new TextBox() { Text = "10" };
        private Label InfoOffsetLimit = new Label() { };
        private Button BtnFilterPagination = new Button() { Text = "OK" };
        private int Offset = 0;
        private int Limit = 10;

        public EntityForm(AppForm RefForm, BaseData entityData)
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
            //form
            Width = 1300;
            Height = 700;
            Font = new System.Drawing.Font("Arial", 10);
            Name = EntityData.Name + "_Form";
            Text = @"" + EntityData.Name + " Form";

            //components
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
                    CustomConsole.WriteLine(ex.Message);
                    return;
                }
            };

            Button ClearBtn = new Button() { Text = "Clear" };
            ClearBtn.Click += new EventHandler((o, e) =>
            {
                EntityData.FilterParmas.Clear();
                Navigate(0, 0);

            });

            Control[] FilterControls =
            {
                new Label() {Text="Page(from 0)" }, InputOffset,
                new Label() {Text="Record Per Page" }, InputLimit,
                BtnFilterPagination, InfoOffsetLimit, new Label() {Text="Filter Info:" },
                InfoFilter, new BlankControl() {Reserved = ReservedFor.BEFORE_HOR },
                new BlankControl() {Reserved = ReservedFor.BEFORE_HOR },
                ClearBtn
            };
            Panel PagingPanel = ControlUtil.GeneratePanel(6, FilterControls, 0, 110, 30, Color.Azure);
            Controls.Add(PagingPanel);

            Button BtnAdd = new Button() { Text = "ADD" };
            BtnAdd.Click += (o, e) =>
            {
                addForm = new AddForm(EntityData.ShowAddForm(), EntityData);
                addForm.Show();
            };

            MiscPanel = ControlUtil.GeneratePanel(3, new Control[]
            {
                new TitleLabel(13) {Text="Buttons Panel" }, BtnAdd
            }, 5, 130, 20, Color.Azure, 850, 10);

            Controls.Add(MiscPanel);

            DetailPanel.SetBounds(850, 130, Constant.DETAIL_PANEL_HEIGHT, Constant.DETAIL_PANEL_WIDTH);
            DetailPanel.AutoScroll = false;
            DetailPanel.VerticalScroll.Visible = true;
            DetailPanel.VerticalScroll.Enabled = true;
            DetailPanel.AutoScroll = true;
            Controls.Add(DetailPanel);

            GenerateTable();

        }

        public void ShowDetail(Panel _DetailPanel, Loading L)
        {
            ISyncInvoke.InvokeAsync(this, (f) =>
            {
                try
                {
                    DetailPanel.Controls.Clear();
                    DetailPanel.Controls.Add(_DetailPanel);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Server Error");
                }
                L.Dispose();
            });
        }

        public void Navigate(int Offset, int Limit)
        {
            CustomConsole.WriteLine("Navigating {0}, {1}", Offset, Limit);
            GenerateTable(Offset, Limit);
        }

        private void UpdateInfoFilter()
        {

            string Text = "";
            foreach (string key in EntityData.FilterParmas.Keys)
            {
                if (EntityData.FilterParmas[key] == null || EntityData.FilterParmas[key].ToString() == "")
                {
                    continue;
                }
                Text += key + ":" + EntityData.FilterParmas[key] + "|";
            }
            InfoFilter.Text = Text;

        }

        private void GenerateTable(int Offset = 0, int Limit = 0)
        {

            this.Offset = Offset;
            this.Limit = Limit == 0 ? this.Limit : Limit;
            Loading LoadingMsg = new Loading("LOADING");

            ISyncInvoke.InvokeAsync(this, (f) =>
            {
                UpdateData();

                if (this.ListPanel == null)
                {
                    LoadingMsg.Dispose();
                    MessageBox.Show("Server error / data kosong\nTekan Clear untuk reset filter", "Error");
                }
                else
                {
                    SetListPanel();
                    GenerateNavButton();
                    UpdateInfoFilter();
                    LoadingMsg.Dispose();
                }
            });



        }

        public void UpdateData()
        {
            Controls.Remove(ListPanel);
            ListPanel = EntityData.UpdateData(this.Offset, this.Limit);
        }

        public void SetListPanel()
        {
            Controls.Add(ListPanel);
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
                    NavBtn.Text = "(" + NavBtn.Text + ")";
                }
                int page = i;
                NavBtn.Click += (o, e) =>
                {
                    Navigate(page, Limit);
                };
                NavButtons[i] = NavBtn;
            }
            NavPanel = ControlUtil.GeneratePanel(17, NavButtons, 5, 40, 30, Color.AliceBlue, 5, 65, 760, 60);
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
