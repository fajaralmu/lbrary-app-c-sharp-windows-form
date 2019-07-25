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
    class VisitData : BaseData
    {

        private List<visit> visits = new List<visit>();

        public VisitData(AppUser AppUser) : base("visitList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "visitList";
            Entity = typeof(visit);
        }
        public VisitData(string Name, AppUser AppUser) : base("visitList")
        {
            this.AppUser = AppUser;
            Entity = typeof(visit);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> visitListInfo = ObjMapInfo;
            visits = (List<visit>)visitListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(visits);
            EntityTotalCount = (int)visitListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {
            int TotalCount = 0;
            List<Dictionary<string, object>> visitListMap = ObjectList;
            List<visit> visits = new List<visit>();
            if (visitListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> visitMap in visitListMap)
            {
                if (visitMap.Keys.Count == 1 && visitMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)visitMap["count"];
                    break;
                }
                visit visit = (visit)ObjectUtil.FillObjectWithMap(new visit(), visitMap);
                visits.Add(visit);
            }
            
            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",visits }
            };
        }


    }
}
