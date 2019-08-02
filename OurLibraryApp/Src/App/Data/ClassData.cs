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
    class ClassData : BaseData {

        private List<@class> @classs = new List<@class>();

        public ClassData(AppUser AppUser) : base("classList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "classList";
            Entity = typeof(@class);
        }
        public ClassData(string Name, AppUser AppUser) : base("classList")
        {
            this.AppUser = AppUser;
            Entity = typeof(@class);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> @classListInfo = ObjMapInfo;
            @classs = (List<@class>)@classListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(@classs);
            EntityTotalCount = (int)@classListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {

            int TotalCount = 0;
            List<@class> @classs = new List<@class>();
            List<Dictionary<string, object>> @classListMap = ObjectList;

            if (@classListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> @classMap in @classListMap)
            {
                if (@classMap.Keys.Count == 1 && @classMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)@classMap["count"];
                    break;
                }
              
                @class @class = (@class)ObjectUtil.FillObjectWithMap(new @class(), @classMap);
                @classs.Add(@class);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",@classs }
            };
        }

       
    }
}
