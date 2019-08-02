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
    class CategoryData : BaseData {

        private List<category> categorys = new List<category>();

        public CategoryData(AppUser AppUser) : base("categoryList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "categoryList";
            Entity = typeof(category);
        }
        public CategoryData(string Name, AppUser AppUser) : base("categoryList")
        {
            this.AppUser = AppUser;
            Entity = typeof(category);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> categoryListInfo = ObjMapInfo;
            categorys = (List<category>)categoryListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(categorys);
            EntityTotalCount = (int)categoryListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {

            int TotalCount = 0;
            List<category> categorys = new List<category>();
            List<Dictionary<string, object>> categoryListMap = ObjectList;

            if (categoryListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> categoryMap in categoryListMap)
            {
                if (categoryMap.Keys.Count == 1 && categoryMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)categoryMap["count"];
                    break;
                }
              
                category category = (category)ObjectUtil.FillObjectWithMap(new category(), categoryMap);
                categorys.Add(category);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",categorys }
            };
        }

       
    }
}
