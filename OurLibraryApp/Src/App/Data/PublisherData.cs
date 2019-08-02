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
    class PublisherData : BaseData {

        private TextBox InputID = new TextBox();
        private TextBox InputPublisherName = new TextBox();
        private TextBox InputContact = new TextBox();
        private TextBox InputAddress = new TextBox();
        private List<publisher> publishers = new List<publisher>();

        public PublisherData(AppUser AppUser) : base("publisherList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "publisherList";
            Entity = typeof(publisher);
        }

        public PublisherData(string Name, AppUser AppUser) : base("publisherList")
        {
            this.AppUser = AppUser;
            Entity = typeof(publisher);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> publisherListInfo = ObjMapInfo;
            publishers = (List<publisher>)publisherListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(publishers);
            EntityTotalCount = (int)publisherListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {

            int TotalCount = 0;
            List<publisher> publishers = new List<publisher>();
            List<Dictionary<string, object>> publisherListMap = ObjectList;

            if (publisherListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> publisherMap in publisherListMap)
            {
                if (publisherMap.Keys.Count == 1 && publisherMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)publisherMap["count"];
                    break;
                }
              
                publisher publisher = (publisher)ObjectUtil.FillObjectWithMap(new publisher(), publisherMap);
                publishers.Add(publisher);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",publishers }
            };
        }

        public override Panel ShowAddForm(object Object = null)
        {
            publisher EditPublisher = (publisher)Object;
            bool EditState = EditPublisher != null;
            ClearAllFields();
            //  UpdateList();
            Button BtnAdd = new Button() { Text = "Add" };

            BtnAdd.Click += (e, o) =>
            {

                publisher Publisher = new publisher()
                {
                    id = EditState ? InputID.Text : null,
                    name = InputPublisherName.Text.Trim(),
                    contact = InputContact.Text.Trim(),
                    address = InputAddress.Text.Trim(),
                    books = null

                };
                if (null != UserClient.AddPublisher(Publisher, AppUser))
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
                InputID.Text = EditPublisher.id;
                InputPublisherName.Text = EditPublisher.name.Trim();
                InputAddress.Text = EditPublisher.address.Trim();
                InputContact.Text = EditPublisher.contact.Trim();

            }

            Control[] Controls = new Control[]
            {
                new TitleLabel(20) {Text="Add Publisher" },new BlankControl(),
                 new Label() {Text="ID" }, InputID,
                new Label() {Text="Name" }, InputPublisherName,
                 new Label() {Text="Contact" }, InputContact,
                   new Label() {Text="Address" }, InputAddress,
                      BtnAdd, null
            };

            return ControlUtil.GeneratePanel(2, Controls, 5, 180, 30, Color.Aqua);
        }

        protected override void ClearAllFields()
        {
            InputID.Clear();
            InputPublisherName.Clear();
            InputAddress.Clear();
            InputContact.Clear();

        }


    }
}
