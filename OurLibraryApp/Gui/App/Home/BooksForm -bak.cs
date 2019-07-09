using OurLibrary.Models;
using OurLibraryApp.Src.App.Access;
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
    class BooksFormbak : BaseForm
    {

        private List<book> Books = new List<book>();
        private Panel DetailPanel;
        private Panel TablePanel;
        private Panel NavPanel;
        private TextBox InputOffset = new TextBox() { Text = "0" };
        private TextBox InputLimit = new TextBox() { Text = "10" };
        private Label InfoOffsetLimit = new Label() { };
        private Button BtnFilterPagination = new Button() { Text = "OK" };
        private int BookTotalCount = 0;
        private int Offset = 0;
        private int Limit = 10;

        public BooksFormbak(AppForm RefForm)
        {
            this.RefForm = RefForm;
            RefForm.Enabled = false;
            Init();
            Show();
        }

        private void Init()
        {
            Width = 1300;
            Height = 700;
            Name = "BooksForm";
            Text = @"Books Form";
            BtnFilterPagination.Click += (o, e)=> {
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

        

        private void Navigate(int Offset, int Limit)
        {
            Console.WriteLine("Navigating {0}, {1}", Offset, Limit);
            GenerateTable(Offset, Limit);
        }

        private void GenerateTable(int Offset = 0, int Limit = 0)
        {
            this.Offset = Offset;
            this.Limit = Limit == 0 ? this.Limit : Limit;
            Controls.Remove(TablePanel);
            Dictionary<string, object> BookListInfo = Transaction.BookList(this.Offset, this.Limit);
            Books = (List<book>)BookListInfo["data"];
            BookTotalCount = (int)BookListInfo["totalCount"];

            string[] ColumnLabels = { "No", "Title", "Category", "ISBN", "Author", "Publisher", "Records", "Option" };
            Control[] TableControls = new Control[8 * (Books.Count + 1)];
            for (int i = 0; i < ColumnLabels.Length; i++)
            {
                TableControls[i] = new Label() { Text = ColumnLabels[i] };
            }
            int ControlIndex = 8;
            for (int i = 0; i < Books.Count; i++)
            {
                book Book = Books.ElementAt(i);
                Button BtnDetail = new Button() { Text = "Detail" };
                BtnDetail.Click += (o, e) =>
                {
                    ShowDetail(Book.book_record);
                };
                TableControls[ControlIndex++] = new Label() { Text = ((this.Offset * this.Limit) + i + 1).ToString() };
                TableControls[ControlIndex++] = new Label() { Text = Book.title };
                TableControls[ControlIndex++] = new Label() { Text = Book.category.category_name };
                TableControls[ControlIndex++] = new Label() { Text = Book.isbn };
                TableControls[ControlIndex++] = new Label() { Text = Book.author.name };
                TableControls[ControlIndex++] = new Label() { Text = Book.publisher.name };
                TableControls[ControlIndex++] = new Label() { Text = Book.book_record.Count.ToString() };
                TableControls[ControlIndex++] = BtnDetail;
            }
            TablePanel = ControlUtil.PopulatePanel(8, TableControls, 5, 90, 30, Color.White, 5, 130, 760, 500);


            Controls.Add(TablePanel);
            GenerateNavButton();
        }

        private void GenerateNavButton()
        {
            Controls.Remove(NavPanel);
            int Page = BookTotalCount / Limit + (BookTotalCount % Limit > 0 ? 1 : 0);

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
            NavPanel = ControlUtil.PopulatePanel(10, NavButtons, 5, 50, 50, Color.AliceBlue, 5, 65, 760,60);
            Controls.Add(NavPanel);
            InfoOffsetLimit.Text = "Page: " + Offset + ", Rec per Page: " + Limit;
        }

        private void ShowDetail(object Object)
        {
            Controls.Remove(DetailPanel);

            List<book_record> Records = (List<book_record>)Object;
            Control[] DetailsCol = new Control[5 * (Records.Count+1)];
            //update
            string[] ColumnLabels = { "No", "RecId", "BookCode", "BookId", "Available" };
            for (int i = 0; i < ColumnLabels.Length; i++)
            {
                DetailsCol[i] = new Label() { Text = ColumnLabels[i] };
            }
            int ControlIndex = 5;
            for (int i = 0; i < Records.Count; i++)
            {
                book_record BR = Records.ElementAt(i);
                DetailsCol[ControlIndex++] = new Label() { Text = (i + 1).ToString() };
                DetailsCol[ControlIndex++] = new Label() { Text = BR.id };
                DetailsCol[ControlIndex++] = new Label() { Text = BR.book_code };
                DetailsCol[ControlIndex++] = new Label() { Text = BR.book_id };
                DetailsCol[ControlIndex++] = new Label() { Text = BR.available == 1?"yes":"-" };


            }
            //
            DetailPanel = ControlUtil.PopulatePanel(5, DetailsCol, 5, 70, 20, Color.Orange, 780, 100, 400, 500);
            Controls.Add(DetailPanel);
        }

        protected override void OnClosed(EventArgs e)
        {
            RefForm.Enabled = true;
            base.OnClosed(e);
        }


    }
}
