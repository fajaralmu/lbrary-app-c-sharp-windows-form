using OurLibrary.Models;
using OurLibraryApp.Gui.App.Controls;
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
    class BookData : BaseData
    {
        private TextBox InputID = new TextBox();
        private TextBox InputTitle = new TextBox();
        private TextBox InputISBN = new TextBox();
        private TextBox InputReview = new TextBox();
        private TextBox InputPage = new TextBox();
        private ComboBox AuthorList = new ComboBox();
        private ComboBox PublisherList = new ComboBox();
        private ComboBox CategoryList = new ComboBox();

        private List<publisher> PublisherDataList = new List<publisher>();
        private List<category> CategoryDataList = new List<category>();
        private List<author> AuthorDataList = new List<author>();

        private List<book> Books = new List<book>();

        public BookData(AppUser AppUser) : base("bookList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "bookList";
            Entity = typeof(book);
        }
        public BookData(string Name, AppUser AppUser) : base("bookList")
        {
            this.AppUser = AppUser;
            Entity = typeof(book);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> BookListInfo = ObjMapInfo;
            Books = (List<book>)BookListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Books);
            EntityTotalCount = (int)BookListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {

            int TotalCount = 0;
            List<book> Books = new List<book>();
            List<Dictionary<string, object>> BookListMap = ObjectList;

            if (BookListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> BookMap in BookListMap)
            {
                if (BookMap.Keys.Count == 1 && BookMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)BookMap["count"];
                    break;
                }

                book Book = (book)ObjectUtil.FillObjectWithMap(new book(), BookMap);
                Books.Add(Book);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",Books }
            };
        }

        protected override Panel ShowDetailPanel(object Obj)
        {
            book Book = (book)Obj;
            Panel DetailPanel = new Panel();
            Control[] DetailsCol = new Control[5 * (Book.book_record.Count + 1)];
            //update
            string[] ColumnLabels = { "No", "RecId", "BookCode", "BookId", "Available" };
            for (int i = 0; i < ColumnLabels.Length; i++)
            {
                DetailsCol[i] = new TitleLabel(9) { Text = ColumnLabels[i] };
            }
            int ControlIndex = 5;
            for (int i = 0; i < Book.book_record.Count; i++)
            {
                book_record BR = Book.book_record.ElementAt(i);
                DetailsCol[ControlIndex++] = new Label() { Text = (i + 1).ToString() };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BR.id };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BR.book_code };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BR.book_id };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BR.available == 1 ? "yes" : "-" };

            }
            //
            DetailPanel = ControlUtil.GeneratePanel(5, DetailsCol, 5, 80, 20, Color.Orange, 5, 5, 400, 500);

            //picturebox
            string URL = "http://" + Transaction.Host + "/Assets/Image/App/bookCover.jpg";
            if (Book.img != null)
            {
                URL = "http://" + Transaction.Host + "/Assets/Image/Book/" + Book.img;
            }

            PictureBox Picture = new PictureBox();

            //    Picture.BorderStyle = BorderStyle.Fixed3D;
            Picture.ImageLocation = URL;
            Picture.SizeMode = PictureBoxSizeMode.StretchImage;

            Picture.SetBounds(100, 10, 150, 200);
            Picture.BackColor = Color.Aqua;
            DetailPanel.SetBounds(10, 250, DetailPanel.Width, DetailPanel.Height);

            TitleLabel BookTitle = new TitleLabel(15) { Text = Book.title };

            BookTitle.TextAlign = ContentAlignment.MiddleCenter;
            BookTitle.SetBounds(10, 200, 350, 50);

            Panel Wrapper = new Panel();
            Wrapper.Controls.Add(Picture);
            Wrapper.Controls.Add(BookTitle);
            Wrapper.Controls.Add(DetailPanel);
            Wrapper.SetBounds(5, 5, 500, 1000);
            return Wrapper;
        }

        public override Panel ShowAddForm(object Object = null)
        {
            book EditBook = (book)Object;
            bool EditState = EditBook != null;
            ClearAllFields();
            UpdateList();
            Button BtnAdd = new Button() { Text = "Add" };

            BtnAdd.Click += (e, o) =>
            {
                book Book = new book()
                {
                    id = EditState? InputID.Text :null,
                    title = InputTitle.Text.Trim(),
                    review = InputReview.Text.Trim(),
                    page = int.Parse(InputPage.Text.Trim()),
                    isbn = InputISBN.Text.Trim(),
                    author_id = ((ComboboxItem)AuthorList.SelectedItem).Value.ToString(),
                    publisher_id = ((ComboboxItem)PublisherList.SelectedItem).Value.ToString(),
                    category_id = ((ComboboxItem)CategoryList.SelectedItem).Value.ToString()
                    ,
                    book_record = null
                };
                if (null != UserClient.AddBook(Book, AppUser))
                {
                    MessageBox.Show("Success");
                    EntityForm.Navigate(0, 0);
                }
                else
                {
                    MessageBox.Show("Failed");
                }
            };
            InputID.Enabled = false;
            InputID.Text = "GENERATED";
            if (EditState)
            {
                InputID.Text = EditBook.id;
                InputTitle.Text = EditBook.title;
                InputReview.Text = EditBook.review;
                InputISBN.Text = EditBook.isbn;
                InputPage.Text = EditBook.page.ToString();
                AuthorList.SelectedValue = EditBook.author_id;
                PublisherList.SelectedValue = EditBook.publisher_id;
                CategoryList.SelectedValue = EditBook.category_id;
            }

            Control[] Controls = new Control[]
            {
                new TitleLabel(20) {Text="Add Book" },new BlankControl(),
                 new Label() {Text="ID" }, InputID,
                new Label() {Text="Title" }, InputTitle,
                 new Label() {Text="Author" }, AuthorList,
                  new Label() {Text="Publisher" }, PublisherList,
                   new Label() {Text="Category" }, CategoryList,
                    new Label() {Text="Page" }, InputPage,
                     new Label() {Text="ISBN" }, InputISBN,
                      new Label() {Text="Review" }, InputReview,
                      BtnAdd, null
            };

            return ControlUtil.GeneratePanel(2, Controls, 5, 180, 30, Color.Aqua);
        }

        protected override void UpdateList()
        {
            UserClient UserClient = new UserClient();
            PublisherDataList = UserClient.PublisherList(AppUser, 0, 0, null);
            CategoryDataList = UserClient.CategoryList(AppUser, 0, 0, null);
            AuthorDataList = UserClient.AuthorList(AppUser, 0, 0, null);

            foreach (publisher PUB in PublisherDataList)
            {
                publisher Pub = PUB;
                ComboboxItem Item = new ComboboxItem(Pub.name, Pub.id);
                PublisherList.Items.Add(Item);
            }
            foreach (category CAT in CategoryDataList)
            {
                category Cat = new category() { id = CAT.id, category_name = CAT.category_name };
                ComboboxItem Item2 = new ComboboxItem(Cat.category_name, Cat.id);
                CategoryList.Items.Add(Item2);
            }
            for (int i = 0; i < AuthorDataList.Count; i++)
            {
                author Author = AuthorDataList.ElementAt(i);
                ComboboxItem Item3 = new ComboboxItem(i + Author.name, Author.id);
                AuthorList.Items.Add(Item3);
            }

        }

        protected override void ClearAllFields()
        {
            InputISBN.Clear();
            InputPage.Clear();
            InputReview.Clear();
            CategoryList.Items.Clear();
            AuthorList.Items.Clear();
            PublisherList.Items.Clear();

            AuthorDataList.Clear();
            PublisherDataList.Clear();
            CategoryDataList.Clear();
        }
    }
}
