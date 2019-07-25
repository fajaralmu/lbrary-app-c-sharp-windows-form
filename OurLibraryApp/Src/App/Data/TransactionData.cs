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
    class TransactionData : BaseData
    {

        private List<issue> Issues = new List<issue>();

        public TransactionData(AppUser AppUser) : base("issuesList")
        {
            this.AppUser= AppUser;
            ListObjServiceName = "issuesList";
            Entity = typeof(issue);
        }
        public TransactionData(string Name, AppUser AppUser) : base("issuesList")
        {
            this.AppUser = AppUser;
            Entity = typeof(issue);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> issueListInfo = ObjMapInfo;
            Issues = (List<issue>)issueListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Issues);
            EntityTotalCount = (int)issueListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {
            int TotalCount = 0;
            List<issue> issues = new List<issue>();
            List<Dictionary<string, object>> issueListMap = ObjectList;

            if (issueListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> issueMap in issueListMap)
            {
                if (issueMap.Keys.Count == 1 && issueMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)issueMap["count"];
                    break;
                }
               
                issue issue = (issue)ObjectUtil.FillObjectWithMap(new issue(), issueMap);
                issue.appUser = AppUser;
                issues.Add(issue);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",issues }
            };
        }
    }
}
