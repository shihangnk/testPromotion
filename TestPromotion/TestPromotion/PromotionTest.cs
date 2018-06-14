using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestPromotion.Model;

namespace TestPromotion
{
    [TestClass]
    public class PromotionTest
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PromotionTest));
        private const string url = "http://localhost:61284/v1/Companies(272002)/promotions";


        [TestMethod]
        public void TestMethod1()
        {
            Trace.WriteLine("........................................Trace Trace the World");
            Debug.WriteLine(".....................................Debug Debug WOrld");

            Logger.Info("................................. this is log4net");

            ClearPromotion();



//            var promotions = getPromotions();

   //         Debug.WriteLine("......." + promotions.Result);
////            Assert.AreSame("aaaa", promotions.Result);


            var token = getToken();
            Trace.WriteLine(".................. "+token.Result);
        }

        private Promotion CreatePromotion(Promotion promotion)
        {
            string ret = ServiceCaller.Invoke("CREATE", url, JsonConvert.SerializeObject(promotion));
            return JsonConvert.DeserializeObject<Promotion>(ret);
        }

        private void ClearPromotion()
        {
            List<Promotion> promotions = GetPromotions();
            Assert.IsTrue(GetPromotions().Count>0, "Get all promotions");
            foreach (var promotion in promotions)
            {
                ServiceCaller.Invoke("DELETE", $"{url}({promotion.Id})", "");
            }
            Assert.IsTrue(GetPromotions().Count==0, "All promtions are removed");
        }

        private List<Promotion> GetPromotions()
        {
            var str = ServiceCaller.Invoke("GET", url, "");
            Debug.WriteLine("......." + str);

            List<Promotion> promotions = JsonConvert.DeserializeObject<List<Promotion>>(str);
            return promotions;
        }


        async Task<string> getPromotions()
        {
            return await getPromotionsAsync();
        }

        async Task<string> getPromotionsAsync()
        {
            {
                // ... Target page.
                string page = "http://en.wikipedia.org/";

                // ... Use HttpClient.
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(page))
                using (HttpContent content = response.Content)
                {
                    // ... Read the string.
                    string result = await content.ReadAsStringAsync();

                    // ... Display the result.
                    if (result != null &&
                        result.Length >= 50)
                    {
                        Console.WriteLine(result.Substring(0, 50) + "...");
                    }
                }
            }

          return "aaaaa";
        }

        private static string _token;
        async Task<String> getToken()
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

            Debug.WriteLine("token................."+_token);
            return _token;
        }
    }

}
