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
            Gui.App.Controls.CustomConsole.WriteLine("Post Data: " + postData);
            byte[] postBytes = Encoding.UTF8.GetBytes(postData);

            Gui.App.Controls.CustomConsole.WriteLine("Begin post " + Url);

            WebRequest request = WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;

            // add post data to request
            Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Flush();
            postStream.Close();
            try
            {
                WebResponse response = request.GetResponse();
                string responseFromServer = "";
                using (postStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.  
                    StreamReader reader = new StreamReader(postStream);
                    // Read the content.  
                    responseFromServer = reader.ReadToEnd();
                    // Display the content.  
                    Gui.App.Controls.CustomConsole.WriteLine(responseFromServer);
                }

                // Close the response.  
                response.Close();
                Gui.App.Controls.CustomConsole.WriteLine("response " + ((HttpWebResponse)response).StatusDescription);


                return StringUtil.JSONStringToMap(responseFromServer);
            }catch(WebException webEx)
            {
                return null;
            }
        }
    }
}
