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
        private const string URL = "http://localhost:64945/Web/API/Info";

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
            List<book> Books = new List<book>();
            List<Dictionary<string, object>> BookListMap = MapList(Offset, Limit, URL, "bookList");

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
            
            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",Books }
            };
        }

        public static List<Dictionary<string, object>> MapList(int Offset, int Limit, string _URL, string Action)
        {
            int TotalCount = 0;
            Dictionary<string, object> RespParams = ObjectList(Offset, Limit, _URL, Action);
            if (RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                TotalCount = int.Parse(RespParams["count"].ToString());
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                if (DataMap["Array"] != null && DataMap["Array"].GetType().Equals(typeof(List<Dictionary<string, object>>)))
                {
                    Console.WriteLine("type array");
                    List<Dictionary<string, object>> ListMap = (List<Dictionary<string, object>>)DataMap["Array"];
                    ListMap.Add(new Dictionary<string, object> { { "count", TotalCount } });
                    return ListMap;
                }
            }
            return new List<Dictionary<string, object>>();
        }

        public static Dictionary<string, object> StudentList(int Offset, int Limit)
        {
            int TotalCount = 0;
            List<Dictionary<string, object>> StudentListMap = MapList(Offset, Limit, URL, "studentList");
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
                string[] StudentKeys =
                {
                            "id","name","class_id", "email","bod",
                            "address"
                        };
                string[] ClassKeys = { "id", "class_name" };

                Dictionary<string, object> ClassMap = StringUtil.JSONStringToMap(StudentMap["class"].ToString());


                student Student = (student)ObjectUtil.FillObjectWithMap(new student(), StudentMap, StudentKeys);
                Student.@class = (@class)ObjectUtil.FillObjectWithMap(new @class(), ClassMap,
                    ClassKeys);
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
