using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestPromotion
{
    public class ServiceCaller
    {
        const string _contentType = "application/json";

        public static string Invoke(string method, string uri, string body)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            client.Timeout = new TimeSpan(0, 0, 90);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + getToken().Result);

            // You get the following exception when trying to set the "Content-Type" header like this:
            // // cl.DefaultRequestHeaders.Add("Content-Type", _contentType);
            // "Misused header name. Make sure request headers are used with HttpRequestMessage, response headers with HttpResponseMessage, and content headers with HttpContent objects."

            HttpResponseMessage response;
            switch (method.ToUpper())
            {
                case "GET":
                case "HEAD":
                    // synchronous request without the need for .ContinueWith() or await
                    response = client.GetAsync(uri).Result;
                    break;
                case "POST":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _Body = new StringContent(body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _Body.Headers.ContentType = new MediaTypeHeaderValue(_contentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = client.PostAsync(uri, _Body).Result;
                    }
                    break;
                case "PUT":
                    {
                        // Construct an HttpContent from a StringContent
                        HttpContent _Body = new StringContent(body);
                        // and add the header to this object instance
                        // optional: add a formatter option to it as well
                        _Body.Headers.ContentType = new MediaTypeHeaderValue(_contentType);
                        // synchronous request without the need for .ContinueWith() or await
                        response = client.PutAsync(uri, _Body).Result;
                    }
                    break;
                case "DELETE":
                    response = client.DeleteAsync(uri).Result;
                    break;
                default:
                    throw new NotImplementedException();
            }
            // either this - or check the status to retrieve more information
            response.EnsureSuccessStatusCode();
            // get the rest/content of the response in a synchronous way
            string content = response.Content.ReadAsStringAsync().Result;

            return content;
        }


        private static string _token;
        async static Task<String> getToken()
        {
            return await getTokenAsync();
        }

        async static Task<string> getTokenAsync()
        {
            if (_token != null)
            {
                return _token;
            }

            string accountEndpoint = "https://accountsint.iqmetrix.net/v1/oauth2/token";
            string resultContent;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountEndpoint);
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", TestData.userName),
                    new KeyValuePair<string, string>("password", TestData.password),
                    new KeyValuePair<string, string>("client_id", "iQPOS"),
                    new KeyValuePair<string, string>("client_secret", "FXSDWCy3qYXUgXnpP9uRAhG4")
                });
                var result = await client.PostAsync("", content);
                resultContent = await result.Content.ReadAsStringAsync();
                Debug.WriteLine(resultContent);
            }

            JObject jobject = JObject.Parse(resultContent);
            _token = jobject.GetValue("access_token").ToString();

            Debug.WriteLine("token................." + _token);
            return _token;
        }
    }

}

