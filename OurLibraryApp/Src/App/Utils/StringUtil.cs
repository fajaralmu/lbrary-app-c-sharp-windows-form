using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Src.App.Utils
{
    class StringUtil
    {
       
        public static string DictionatyToQueryString(Dictionary<string, object> Dict)
        {
            string QueryString = "";
            for (int i = 0; i < Dict.Keys.Count; i++)
            {
                string Key = Dict.Keys.ElementAt(i);
                QueryString += Key + "=" + Dict[Key];
                if (i < Dict.Keys.Count - 1)
                {
                    QueryString += "&";
                }
            }
            return QueryString;
        }

        public static Dictionary<string, object> JSONStringToMap(string JSONString)
        {
            object JObject = JsonConvert.DeserializeObject(JSONString);
            if (JObject.GetType().Equals(typeof(JArray)))
            {
                JArray ObjList = (JArray)JObject;
                Dictionary<string, object> ArrayMap = new Dictionary<string, object>();
                List<Dictionary<string, object>> Array = new List<Dictionary<string, object>>();
                for (int i = 0; i < ObjList.Count; i++)
                {
                    JToken Element = ObjList.ElementAt(i);
                    // Gui.App.Controls.CustomConsole.Write("element to String " + Element.ToString());
                    Dictionary<string, object> ElementMap = JSONStringToMap(Element.ToString());
                    Array.Add(ElementMap);
                }
                ArrayMap.Add("Array", Array);
                return ArrayMap;
            }
            else
            {
                JObject JSONObj = (JObject)JObject;
                Gui.App.Controls.CustomConsole.WriteLine("==========BEGIN::JSONObject to MAP=======");
                Dictionary<string, object> Dict = new Dictionary<string, object>();
                foreach (JProperty key in JSONObj.Properties())
                {
                    Gui.App.Controls.CustomConsole.WriteLine(key.Name.ToString() + "-> " + key.Value + " | " + key.Value.GetType());
                    Dict.Add(key.Name, key.Value);
                }
                Gui.App.Controls.CustomConsole.WriteLine("==========END::JSONObject to MAP=======");
                return Dict;
            }
        }
    }
}
