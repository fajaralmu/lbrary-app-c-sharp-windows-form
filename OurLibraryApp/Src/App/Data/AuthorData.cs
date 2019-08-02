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
    class AuthorData : BaseData {

        private TextBox InputID = new TextBox();
        private TextBox InputAuthorName = new TextBox();
        private TextBox InputEmail = new TextBox();
        private TextBox InputPhone = new TextBox();
        private TextBox InputAddress = new TextBox();
        private List<author> authors = new List<author>();

        public AuthorData(AppUser AppUser) : base("authorList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "authorList";
            Entity = typeof(author);
        }
        public AuthorData(string Name, AppUser AppUser) : base("authorList")
        {
            this.AppUser = AppUser;
            Entity = typeof(author);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> authorListInfo = ObjMapInfo;
            authors = (List<author>)authorListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(authors);
            EntityTotalCount = (int)authorListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {

            int TotalCount = 0;
            List<author> authors = new List<author>();
            List<Dictionary<string, object>> authorListMap = ObjectList;

            if (authorListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> authorMap in authorListMap)
            {
                if (authorMap.Keys.Count == 1 && authorMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)authorMap["count"];
                    break;
                }
              
                author author = (author)ObjectUtil.FillObjectWithMap(new author(), authorMap);
                authors.Add(author);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",authors }
            };
        }
        public override Panel ShowAddForm(object Object = null)
        {
            author EditAuthor = (author)Object;
            bool EditState = EditAuthor != null;
            ClearAllFields();
            //  UpdateList();
            Button BtnAdd = new Button() { Text = "Add" };

            BtnAdd.Click += (e, o) =>
            {

                author Author = new author()
                {
                    id = EditState ? InputID.Text : null,
                    name = InputAuthorName.Text.Trim(),
                    phone = InputPhone.Text.Trim(),
                    email = InputEmail.Text.Trim(),
                    address = InputAddress.Text.Trim(),
                    books = null

                };
                if (null != UserClient.AddAuthor(Author, AppUser))
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
                InputID.Text = EditAuthor.id;
                InputAuthorName.Text = EditAuthor.name.Trim();
                InputAddress.Text = EditAuthor.address.Trim();
                InputPhone.Text = EditAuthor.phone.Trim();
                InputEmail.Text = EditAuthor.email.Trim();

            }

            Control[] Controls = new Control[]
            {
                new TitleLabel(20) {Text="Add Author" },new BlankControl(),
                 new Label() {Text="ID" }, InputID,
                new Label() {Text="Name" }, InputAuthorName,
                 new Label() {Text="Email" }, InputEmail,
                  new Label() {Text="Phone" }, InputPhone,
                   new Label() {Text="Address" }, InputAddress,
                      BtnAdd, null
            };

            return ControlUtil.GeneratePanel(2, Controls, 5, 180, 30, Color.Aqua);
        }

        protected override void ClearAllFields()
        {
            InputID.Clear();
            InputAuthorName.Clear();
            InputAddress.Clear();
            InputEmail.Clear();
            InputPhone.Clear();

        }

    }
}
