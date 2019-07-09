using OurLibrary.Models;
using OurLibraryApp.Gui.App.Home;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Src.App.Data
{
    class BookData : BaseData {

        private List<book> Books = new List<book>();

        public BookData()
        {

        }
        public BookData(string Name)
        {
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit)
        {
            Dictionary<string, object> BookListInfo = Transaction.BookList(Offset, Limit);
            Books = (List<book>)BookListInfo["data"];
            EntityTotalCount = (int)BookListInfo["totalCount"];

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
                ICollection<book_record> BR = Book.book_record;
                BtnDetail.Click += (o, e) =>
                {
                    EntityForm.ShowDetail(BR);
                   
                };
                TableControls[ControlIndex++] = new Label() { Text = ((Offset * Limit) + i + 1).ToString() };
                TableControls[ControlIndex++] = new Label() { Text = Book.title };
                TableControls[ControlIndex++] = new Label() { Text = Book.category.category_name };
                TableControls[ControlIndex++] = new Label() { Text = Book.isbn };
                TableControls[ControlIndex++] = new Label() { Text = Book.author.name };
                TableControls[ControlIndex++] = new Label() { Text = Book.publisher.name };
                TableControls[ControlIndex++] = new Label() { Text = Book.book_record.Count.ToString() };
                TableControls[ControlIndex++] = BtnDetail;
            }
            EntityListPanel = ControlUtil.PopulatePanel(8, TableControls, 5, 90, 30, Color.White, 5, 130, 760, 500);

            return EntityListPanel;
        }

        public override Panel ShowDetail(object Object)
        {
            List<book_record> Records = (List<book_record>)Object;
            Control[] DetailsCol = new Control[5 * (Records.Count + 1)];
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
                DetailsCol[ControlIndex++] = new Label() { Text = BR.available == 1 ? "yes" : "-" };


            }
            //
            DetailPanel = ControlUtil.PopulatePanel(5, DetailsCol, 5, 70, 20, Color.Orange, 780, 100, 400, 500);
            return DetailPanel;
        }
    }
}
