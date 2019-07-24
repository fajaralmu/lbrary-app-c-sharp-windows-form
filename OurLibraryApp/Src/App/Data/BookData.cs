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
    class BookData : BaseData {

        private List<book> Books = new List<book>();

        public BookData() : base("bookList")
        {
            ListObjServiceName = "bookList";
            Entity = typeof(book);
        }
        public BookData(string Name) : base("bookList")
        {
            Entity = typeof(book);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> BookListInfo = ObjMapInfo;
            Books = (List<book>)BookListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Books);
            EntityTotalCount = (int)BookListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {

            int TotalCount = 0;
            List<book> Books = new List<book>();
            List<Dictionary<string, object>> BookListMap = ObjectList;

            if (BookListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> BookMap in BookListMap)
            {
                if (BookMap.Keys.Count == 1 && BookMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)BookMap["count"];
                    break;
                }
              
                book Book = (book)ObjectUtil.FillObjectWithMap(new book(), BookMap);
                Books.Add(Book);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",Books }
            };
        }



    }
}
