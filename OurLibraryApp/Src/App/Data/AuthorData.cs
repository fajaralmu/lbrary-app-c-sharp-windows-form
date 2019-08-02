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

       
    }
}
