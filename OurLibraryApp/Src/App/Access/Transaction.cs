using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OurLibrary.Models;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Src.App.Access
{
    class Transaction
    {
        public static Dictionary<string, object> ObjectList(int Offset, int Limit, string Url, string Action)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"offset", Offset },{"limit", Limit }, {"Action", Action }
            };
            return Request.PostReq(Url, Params);

        }

        public static Dictionary<string, object> BookList(int Offset, int Limit)
        {

            int TotalCount = 0;
            Dictionary<string, object> RespParams = ObjectList(Offset, Limit, "http://localhost:64945/Web/API/Info", "bookList");
            List<book> Books = new List<book>();
            if (RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                TotalCount = int.Parse(RespParams["count"].ToString());
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                if (DataMap["Array"] != null && DataMap["Array"].GetType().Equals(typeof(List<Dictionary<string, object>>)))
                {
                    Console.WriteLine("type array");
                    List<Dictionary<string, object>> BookListMap = (List<Dictionary<string, object>>)DataMap["Array"];
                    foreach (Dictionary<string, object> BookMap in BookListMap)
                    {
                        string[] BookKeys =
                        {
                            "id","title","author_id", "publisher_id","category_id",
                            "isbn","page","img","review"
                        };
                        string[] AuthorKeys = { "id", "name" };
                        string[] PublisherKeys = { "id", "name" };
                        string[] CatKeys = { "id", "category_name" };

                        Dictionary<string, object> AuthorMap = StringUtil.JSONStringToMap(BookMap["author"].ToString());
                        Dictionary<string, object> PublisherMap = StringUtil.JSONStringToMap(BookMap["publisher"].ToString());
                        Dictionary<string, object> CatMap = StringUtil.JSONStringToMap(BookMap["category"].ToString());
                        Dictionary<string, object> RecordMap = StringUtil.JSONStringToMap(BookMap["book_record"].ToString());

                        List<Dictionary<string, object>> BookRecordsMap = (List<Dictionary<string, object>>)RecordMap["Array"];
                        List<book_record> BookRecords = new List<book_record>();

                        foreach (Dictionary<string, object> BookRecordMap in BookRecordsMap)
                        {
                            string[] RecordKeys = { "id", "book_code", "book_id", "available" };
                            BookRecords.Add((book_record)ObjectUtil.FillObjectWithMap(new book_record(), BookRecordMap,
                            RecordKeys));
                        }

                        book Book = (book)ObjectUtil.FillObjectWithMap(new book(), BookMap, BookKeys);
                        Book.category = (category)ObjectUtil.FillObjectWithMap(new category(), CatMap,
                            CatKeys);
                        Book.publisher = (publisher)ObjectUtil.FillObjectWithMap(new publisher(), PublisherMap,
                            PublisherKeys);
                        Book.author = (author)ObjectUtil.FillObjectWithMap(new author(), AuthorMap,
                            AuthorKeys);
                        Book.book_record = BookRecords;
                        Books.Add(Book);
                    }
                }

            }
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",Books }
            };
        }



    }
}
