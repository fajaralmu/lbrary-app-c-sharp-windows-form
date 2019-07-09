using OurLibraryApp.Gui.App.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Src.App.Data
{
    class BaseData
    {
        public Panel EntityListPanel;
        public List<object> EntityList { get; set; }
        public Panel DetailPanel;
        public int EntityTotalCount = 0;
        public BooksForm EntityForm;
        public string Name { get; set; }
        public BaseData()
        {

        }

        public void SetEntityForm(BooksForm EntityForm)
        {
            this.EntityForm = EntityForm;
        }

        public virtual Panel UpdateListPanel(int Offset, int Limit)
        {
            return new Panel();
        }

        public virtual Panel ShowDetail(object OBJ)
        {
            return new Panel();
        }
    }
}
