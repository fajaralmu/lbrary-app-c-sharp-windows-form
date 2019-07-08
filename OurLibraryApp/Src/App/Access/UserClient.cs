using OurLibrary.Models;
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

            if(RespParams["login"] != null && (string)RespParams["login"].ToString()=="True")
            {
                return new user() { id = RespParams["id"].ToString(), username = (string)RespParams["username"].ToString(), name = (string)RespParams["name"].ToString(), password = (string)RespParams["password"].ToString() };

            }
            return null;
        }
    }
}
