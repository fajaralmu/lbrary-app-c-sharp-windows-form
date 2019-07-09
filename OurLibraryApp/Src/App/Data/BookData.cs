﻿using OurLibrary.Models;
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

        public BookData()
        {
            Entity = typeof(book);
        }
        public BookData(string Name)
        {
            Entity = typeof(book);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit)
        {
            Dictionary<string, object> BookListInfo = Transaction.BookList(Offset, Limit);
            Books = (List<book>)BookListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Books);
            EntityTotalCount = (int)BookListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

       
    }
}
