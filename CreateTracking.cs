using System;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Collections;

namespace net_demo {
    public class CreateTracking {

        /// <summary>
        ///  You can find your ApiKey on https://app.kd100.com/api-managment
        /// </summary>
        private string ApiKey = "";
        /// <summary>
        ///  You can find your Secret on https://app.kd100.com/api-managment 
        /// </summary>
        private string Secret = "";

        private string ReqURL = "https://www.kd100.com/api/v1/tracking/create";

         static void Main(string[] args)
        {
             CreateTracking createTracking = new CreateTracking();
             Console.WriteLine(createTracking.getResult());
        }

        private  string getResult () {
           
            
            Hashtable ht = new Hashtable();
            ht.Add("carrier_id", "dhlen");
            ht.Add("tracking_number", "9926933413");
            ht.Add("phone", "95279527");
            ht.Add("ship_from", "Toronto, Canada");
            ht.Add("ship_to", "Los Angeles, CA, United States");
            ht.Add("area_show", "1");
            ht.Add("webhook_url", "http://www.kd100.com/console/debug/callback/sandbox");
            string data =  JsonConvert.SerializeObject(ht,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
           
           
            string signature = GetMD5(data + ApiKey + Secret);

            string result = sendPost (ReqURL, data , signature);

            return result;
        }

     
        private  string sendPost (string url, string param , string signature) {
           string result = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.Method = "POST";

            req.Timeout = 15000;

            req.ContentType = "application/json";

            req.ServicePoint.Expect100Continue = false;

            req.Headers.Add("API-Key",ApiKey);

            req.Headers.Add("signature",signature);
    
            byte[] data = Encoding.UTF8.GetBytes(param);

            req.ContentLength = data.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);

                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            Stream stream = resp.GetResponseStream();

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        private  string GetMD5(string data)
        {
           byte[] buffer = System.Text.Encoding.GetEncoding ("UTF-8").GetBytes (data);
            try {
                System.Security.Cryptography.MD5CryptoServiceProvider check;
                check = new System.Security.Cryptography.MD5CryptoServiceProvider ();
                byte[] somme = check.ComputeHash (buffer);
                string ret = "";
                foreach (byte a in somme) {
                    if (a < 16)
                        ret += "0" + a.ToString ("X");
                    else
                        ret += a.ToString ("X");
                }
                return ret.ToUpper();
            } catch {
                throw;
            }
        }
        
    }
}