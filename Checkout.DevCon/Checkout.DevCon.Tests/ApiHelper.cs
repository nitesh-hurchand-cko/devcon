using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Checkout.DevCon.Tests
{
    public class ApiHelper
    {
        public static Tuple<bool, string, string> CreateLogin(string endpoint, string username, string password)
        {
            //uri: /login
            var json = CreateLoginJson(username, password);
            var result = JObject.Parse(ProcessRequestAndReturnResultAsString(endpoint, null, json, "POST", null));
            var valid = Convert.ToBoolean(result.GetValue("valid").ToString()); 
            var token = result.GetValue("token").ToString();
            var name = result.GetValue("name").ToString();
            return new Tuple<bool, string, string>(valid, token, name);

        }

        private static string CreateLoginJson(string username, string password)
        {
            var sb = new StringBuilder();
            sb.Append("{\"username\":\"" + username + "\",");
            sb.Append("\"password\":\"" + password + "\"}");
            return sb.ToString();
        }

        public static string ProcessRequestAndReturnResultAsString(string endPoint, 
            WebHeaderCollection headers, string body, string method, string header)
        {
            Console.WriteLine("URI:");
            Console.WriteLine(endPoint + " [" + method + "]");
            Console.WriteLine("");

            if (method == "POST")
            {
                Console.WriteLine("PAYLOAD:");
                Console.WriteLine(body);
                Console.WriteLine("");
            }

            var result = ExecuteJsonRequest(endPoint, headers, body, method, header);

            if (result.Item2 == HttpStatusCode.OK)
            {
                Console.WriteLine("RESPONSE:");
                Console.WriteLine(result.Item1);
                Console.WriteLine("");

                return result.Item1;
            }

            Console.WriteLine("ERROR:");
            Console.Write(result.Item1);
            return null;
        }

        public static Tuple<string, HttpStatusCode> ExecuteJsonRequest(string endPoint, 
            WebHeaderCollection headers, string body, string method, string header)
        {
            var request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.Method = method;
            request.ContentType = "application/json";

            if (headers != null)
                request.Headers.Add(headers);
            else if (header != null)
            {
                request.Headers.Add("X-AuthToken", header);
            }
            switch (method)
            {
                case "GET":
                    break;
                case "POST":
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(body);
                    }
                    break;
                case "PUT":
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(body);
                    }
                    break;

            }

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    string responseValue;

                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }

                    return new Tuple<string, HttpStatusCode>(responseValue, response.StatusCode);
                }
            }
            catch (WebException e)
            {
                string error;

                using (var stream = e.Response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        error = reader.ReadToEnd();
                    }
                }

                return new Tuple<string, HttpStatusCode>(error, ((HttpWebResponse)e.Response).StatusCode);
            }
        }
    }
}
