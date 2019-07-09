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
    class StudentData : BaseData {

        private List<student> Students = new List<student>();

        public StudentData()
        {
            Entity = typeof(student);
        }
        public StudentData(string Name)
        {
            Entity = typeof(student);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit)
        {
            Dictionary<string, object> studentListInfo = Transaction.StudentList(Offset, Limit);
            Students = (List<student>)studentListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Students);
            EntityTotalCount = (int)studentListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

       
    }
}
