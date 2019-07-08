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

        public static Dictionary<string, object> JSONObjectToMap(JObject JSONObj)
        {
            Console.WriteLine("==========BEGIN::JSONObject to MAP=======");
            Dictionary<string, object> Dict = new Dictionary<string, object>();
            foreach (JProperty key in JSONObj.Properties())
            {
                Console.WriteLine(key.Name.ToString() + "-> " + key.Value+" | "+ key.Value.GetType());
                Dict.Add(key.Name, key.Value);
            }
            Console.WriteLine("==========END::JSONObject to MAP=======");
            return Dict;
        }
    }
}
