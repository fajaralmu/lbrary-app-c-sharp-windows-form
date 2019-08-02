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

       
    }
}
