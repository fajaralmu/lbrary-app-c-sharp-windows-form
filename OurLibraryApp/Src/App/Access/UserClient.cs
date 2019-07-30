using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OurLibrary.Models;
using OurLibraryApp.Src.App.Data;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Src.App.Access
{
    class UserClient
    {

        public user UserLogin(string Username, string Password)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"username", Username },
                  {"password", Password }
            };
            Dictionary<string, object> RespParams = Request.PostReq("http://" + Transaction.Host + "/Web/API/CheckUser", Params);

            if (RespParams["login"] != null && (string)RespParams["login"].ToString() == "True")
            {
                return new user() { id = RespParams["id"].ToString(), username = (string)RespParams["username"].ToString(), name = (string)RespParams["name"].ToString(), password = (string)RespParams["password"].ToString() };

            }
            return null;
        }

        public static book_issue GetByStudentAndRecId(student Student, string RecId, AppUser AppUser)
        {
            if (Student == null)
            {
                return null;
            }
            Dictionary<string, object> Params = MixParamWithUnP(
               new Dictionary<string, object>()
               {
                    {"Action","getBookIssueByStudentId" },
                    {"student_id",Student.id },
                    {"rec_id",RecId }
               },
               AppUser);
            Dictionary<string, object> RespParams = Request.PostReq(Transaction.URL, Params);
            if (RespParams != null && RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                Dictionary<string, object> BookIssueMap = StringUtil.JSONStringToMap(DataMap["book_issue"].ToString());

                return (book_issue)ObjectUtil.FillObjectWithMap(new book_issue(), BookIssueMap);
            }
            return null;
        }

        public static issue SubmitIssue(List<book_issue> BookIssues, string StudentId, AppUser AppUser)
        {
            string BookRecs = ObjectUtil.ListToDelimitedString(BookIssues, ";", "-", "book_record_id");

            Dictionary<string, object> Params = MixParamWithUnP(
                new Dictionary<string, object>()
                {
                    {"Action","issueBook" },
                    {"student_id",StudentId },
                    {"book_recs",BookRecs }
                },
                AppUser);
            Dictionary<string, object> RespParams = Request.PostReq(Transaction.URL, Params);
            if (RespParams != null && RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                string IssueId = DataMap["issue_id"].ToString();
                string Date = DataMap["date"].ToString();
                string[] BookIssuesString = DataMap["items"].ToString().Split(';');
                List<book_issue> BookIssuesList = new List<book_issue>();
                if (BookIssuesString.Length > 0)
                    foreach (string Item in BookIssuesString)
                    {
                        string[] Ids = Item.Split('~');
                        book_issue BS = new book_issue()
                        {
                            id = Ids[1],
                            book_record_id = Ids[0]
                        };
                        BookIssuesList.Add(BS);
                    }
                issue Issue = new issue()
                {
                    id = IssueId,
                    date = DateTime.Parse(Date),
                    book_issue = BookIssuesList
                };
                return Issue;
            }
            return null;
        }


        public static issue SubmitReturn(List<book_issue> BookIssues, string StudentId, AppUser AppUser)
        {
            string BookRecs = ObjectUtil.ListToDelimitedString(BookIssues, ";", "-", "book_record_id","book_issue_id");

            Dictionary<string, object> Params = MixParamWithUnP(
                new Dictionary<string, object>()
                {
                    {"Action","returnBook" },
                    {"student_id",StudentId },
                    {"book_recs",BookRecs }
                },
                AppUser);
            Dictionary<string, object> RespParams = Request.PostReq(Transaction.URL, Params);
            if (RespParams != null && RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                string IssueId = DataMap["issue_id"].ToString();
                string Date = DataMap["date"].ToString();
                if (DataMap["items"] == null || DataMap["items"].ToString().Equals(""))
                    return null;
                string[] BookIssuesString = DataMap["items"].ToString().Split(';');
                List<book_issue> BookIssuesList = new List<book_issue>();
                if (BookIssuesString.Length > 0)
                    foreach (string Item in BookIssuesString)
                    {
                        string[] Ids = Item.Split('~');
                        book_issue BS = new book_issue()
                        {
                            id = Ids[1],
                            book_record_id = Ids[0]
                        };
                        BookIssuesList.Add(BS);
                    }
                issue Issue = new issue()
                {
                    id = IssueId,
                    date = DateTime.Parse(Date),
                    book_issue = BookIssuesList
                };
                return Issue;
            }
            return null;
        }

        public student StudentVisit(string Id)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"Action", "studentVisit" },
                 {"Id", Id }
            };
            Dictionary<string, object> RespParams = Request.PostReq("http://" + Transaction.Host + "/Web/API/Info", Params);
            if (RespParams != null && RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                Dictionary<string, object> StdMap = StringUtil.JSONStringToMap(DataMap["student"].ToString());
                Dictionary<string, object> VisitMap = StringUtil.JSONStringToMap(DataMap["visit"].ToString());
                ICollection<visit> Visits = new List<visit>();
                Visits.Add(new visit()
                {
                    id = int.Parse(VisitMap["id"].ToString()),
                    date = DateTime.Parse(VisitMap["date"].ToString())
                });
                return new student()
                {
                    id = Id,
                    name = StdMap["name"].ToString(),
                    bod = StdMap["bod"].ToString(),
                    class_id = StdMap["class_id"].ToString().Trim(),
                    visits = Visits
                };
            }
            return null;
        }

        public static Dictionary<string, object> MixParamWithUnP(Dictionary<string, object> InputParams,
            AppUser AppUser)
        {
            string Username = "";
            string Password = "";

            if (AppUser != null && AppUser.User != null)
            {
                Username = AppUser.User.username;
                Password = AppUser.User.password;
            }
            Dictionary<string, object> Params = new Dictionary<string, object>();
            Params.Add("u", Username);
            Params.Add("p", Password);
            foreach (string key in InputParams.Keys)
            {
                Params.Add(key, InputParams[key]);
            }
            return Params;
        }

        public static student StudentById(string Id, AppUser AppUser)
        {


            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"Action", "studentById" },
                 {"Id", Id }
            };
            Params = MixParamWithUnP(Params, AppUser);

            Dictionary<string, object> RespParams = Request.PostReq("http://" + Transaction.Host + "/Web/API/Info", Params);
            if (RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                Dictionary<string, object> StdMap = StringUtil.JSONStringToMap(DataMap["student"].ToString());

                return new student()
                {
                    id = Id,
                    name = StdMap["name"].ToString(),
                    bod = StdMap["bod"].ToString(),
                    class_id = StdMap["class_id"].ToString().Trim()
                };
            }
            return null;
        }

        public static book_record BookRecById(string Id, AppUser AppUser)
        {
            Dictionary<string, object> Params = MixParamWithUnP(new Dictionary<string, object>() { { "id", Id }, { "Action", "bookRecById" } }, AppUser);

            Dictionary<string, object> RespParams = Request.PostReq(Transaction.URL, Params);
            if (RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                Dictionary<string, object> BookRecMap = StringUtil.JSONStringToMap(DataMap["book_record"].ToString());

                return (book_record)ObjectUtil.FillObjectWithMap(new book_record(), BookRecMap);
            }
            return null;
        }

    }
}
