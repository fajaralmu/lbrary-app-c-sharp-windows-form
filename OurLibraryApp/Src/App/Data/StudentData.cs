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
    class StudentData : BaseData
    {

        private List<student> Students = new List<student>();

        public StudentData(AppUser AppUser) : base("studentList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "studentList";
            Entity = typeof(student);
        }
        public StudentData(string Name, AppUser AppUser) : base("studentList")
        {
            this.AppUser = AppUser;
            Entity = typeof(student);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> studentListInfo = ObjMapInfo;
            Students = (List<student>)studentListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Students);
            EntityTotalCount = (int)studentListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {
            int TotalCount = 0;
            List<Dictionary<string, object>> StudentListMap = ObjectList;
            List<student> Students = new List<student>();
            if (StudentListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> StudentMap in StudentListMap)
            {
                if (StudentMap.Keys.Count == 1 && StudentMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)StudentMap["count"];
                    break;
                }
                student Student = (student)ObjectUtil.FillObjectWithMap(new student(), StudentMap);
                Student.AppUser = AppUser;
                Students.Add(Student);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",Students }
            };
        }


    }
}
