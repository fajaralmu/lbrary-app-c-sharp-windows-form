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
        public const string URL = "http://localhost:64945/Web/API/Info";

        private static Dictionary<string, object> FILTER_PARAM = new Dictionary<string, object>(); 

        public static Dictionary<string, object> FetchObj(int Offset, int Limit, string Url, string Action, AppUser AppUser, Dictionary<string, object> FilterParams = null)
        {
            if(FilterParams == null)
            {
                FilterParams = new Dictionary<string, object>();
            }

            //auth
            string Username = "";
            string Password = "";

            if(AppUser!=null && AppUser.User != null)
            {
                Username = AppUser.User.username;
                Password = AppUser.User.password;
            }

            string ParamValue = StringUtil.DictionatyToQueryString(FilterParams);
            ParamValue = ParamValue.Replace("&", ";");
            Dictionary<string, object> Params = new Dictionary<string, object>
            {
                {"offset", Offset },{"limit", Limit },
                { "Action", Action },{"search_param", "${"+ParamValue+"}$" },
                {"u",Username }, {"p",Password }
            };
            return Request.PostReq(Url, Params);

        }
       
        public static List<Dictionary<string, object>> MapList(int Offset, int Limit, string _URL, string Action, AppUser AppUser, Dictionary<string, object> FilterParams)
        {
            int TotalCount = 0;
            Dictionary<string, object> RespParams = FetchObj(Offset, Limit, _URL, Action, AppUser, FilterParams);
            if(RespParams == null)
            {
                return null;
            }
            if (RespParams["result"] != null && RespParams["result"].ToString() == "0")
            {
                TotalCount = int.Parse(RespParams["count"].ToString());
                Dictionary<string, object> DataMap = StringUtil.JSONStringToMap(RespParams["data"].ToString());
                if (DataMap["Array"] != null && DataMap["Array"].GetType().Equals(typeof(List<Dictionary<string, object>>)))
                {
                    Gui.App.Controls.CustomConsole.WriteLine("type array");
                    List<Dictionary<string, object>> ListMap = (List<Dictionary<string, object>>)DataMap["Array"];
                    ListMap.Add(new Dictionary<string, object> { { "count", TotalCount } });
                    return ListMap;
                }
            }
            return null;//new List<Dictionary<string, object>>();
        }

       
    }
}
