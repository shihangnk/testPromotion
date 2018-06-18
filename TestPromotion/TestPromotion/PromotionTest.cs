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
//      private const string url = "http://localhost:61284/v1/Companies(272002)/promotions";
        private const string _url = "https://apiint.iqmetrix.net/pospromotions/v1/companies(272002)/promotions";

        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine("..................................... step 1");

            ClearPromotion();

            var prom1 = TestData.GetAPromotionObject();
            Promotion p1 = CreatePromotion(prom1);

            Debug.WriteLine(".............. p1 " + p1.Id);

//            var promotions = getPromotions();

            //         Debug.WriteLine("......." + promotions.Result);
////            Assert.AreSame("aaaa", promotions.Result);


        }

        private Promotion CreatePromotion(Promotion promotion)
        {
            string ret = ServiceCaller.Invoke("POST", _url, JsonConvert.SerializeObject(promotion));
            return JsonConvert.DeserializeObject<Promotion>(ret);
        }

        private void ClearPromotion()
        {
            List<Promotion> promotions = GetPromotions();
            Assert.IsTrue(GetPromotions().Count>0, "Get all promotions");
            foreach (var promotion in promotions)
            {
                ServiceCaller.Invoke("DELETE", $"{_url}({promotion.Id})", "");
            }
            Assert.IsTrue(GetPromotions().Count==0, "All promtions are removed");
        }

        private List<Promotion> GetPromotions()
        {
            var str = ServiceCaller.Invoke("GET", _url, "");
            Debug.WriteLine("......." + str);

            List<Promotion> promotions = JsonConvert.DeserializeObject<List<Promotion>>(str);
            return promotions;
        }


        
    }

}
