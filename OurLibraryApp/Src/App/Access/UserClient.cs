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
    class UserClient
    {
        public user UserLogin(string Username, string Password)
        {
            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"username", Username },
                  {"password", Password }
            };
            Dictionary<string, object> RespParams = Request.PostReq("http://localhost:64945/Web/API/CheckUser", Params);

            if (RespParams["login"] != null && (string)RespParams["login"].ToString() == "True")
            {
                return new user() { id = RespParams["id"].ToString(), username = (string)RespParams["username"].ToString(), name = (string)RespParams["name"].ToString(), password = (string)RespParams["password"].ToString() };

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
            Dictionary<string, object> RespParams = Request.PostReq("http://localhost:64945/Web/API/Info", Params);
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

        public static student StudentById(string Id, AppUser AppUser)
        {

            string Username = "";
            string Password = "";

            if (AppUser != null && AppUser.User != null)
            {
                Username = AppUser.User.username;
                Password = AppUser.User.password;
            }

            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"Action", "studentById" },
                 {"Id", Id }, {"u",Username }, {"p",Password }
            };

            Dictionary<string, object> RespParams = Request.PostReq("http://localhost:64945/Web/API/Info", Params);
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
    }
}
