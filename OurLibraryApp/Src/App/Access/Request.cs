using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Src.App.Access
{
    class Request
    {

        public static Dictionary<string, object> PostReq(string Url, Dictionary<string, object> Param)
        {
            string postData = StringUtil.DictionatyToQueryString(Param);
            ASCIIEncoding ascii = new ASCIIEncoding();
            Console.WriteLine("Post Data: " + postData);
            byte[] postBytes = Encoding.UTF8.GetBytes(postData);

            Console.WriteLine("Begin post " + Url);

            WebRequest request = WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;

            // add post data to request
            Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Flush();
            postStream.Close();
                        
            WebResponse response = request.GetResponse();
            string responseFromServer = "";
            using (postStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(postStream);
                // Read the content.  
                 responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
            }

            // Close the response.  
            response.Close();
            Console.WriteLine("response " + ((HttpWebResponse)response).StatusDescription);
            JObject obj =  (JObject) JsonConvert.DeserializeObject(responseFromServer);
           
            Console.WriteLine(obj["login"]);
            return StringUtil.JSONObjectToMap(obj);
        }
    }
}
