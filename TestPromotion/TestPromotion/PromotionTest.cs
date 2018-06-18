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

 //           ClearPromotion();

            var prom1 = new Promotion()
            {
                Name = "Prom1",
                Period = new DefinitePeriod()
                {
                    Tag = "Definite",
                    DateRanges = new List<DateRange> {
                        new DateRange()
                        {
                            StartDate = new DateTime(2018,05, 01, 1, 10, 30),
                            EndDate = new DateTime(2018,05, 10, 1, 10, 30)
                        }
                    }
                },
                Status = "Active",
                Condition = new Condition
                {
                    MatchAll = new MatchAll
                    {
                        Products = new ProductsClassificationsAndCategories
                        {
                            Tag = "ProductsClassificationsAndCategories",
                            Products = new List<Guid>{ Guid.NewGuid(), Guid.NewGuid() },
                            Classifications = new List<int> {100, 200, 300},
                            Categories = new List<int> { 400, 500, 600}
                        },
                        Locations = new Locations
                        {
                            Tag = "All"
                        }

                    },
                    Customers = new Customers
                    {
                        Tag = "All"
                    }
                },
                Rule = new Rule
                {
                    Tag = "MatchedLineItems",
                    Discount = new Discount
                    {
                        Tag = "Percentage",
                        Percentage = 10
                    }
                }
            };
            Promotion p1 = CreatePromotion(prom1);

            Debug.WriteLine(".............. p1 " + p1.Id);

//            var promotions = getPromotions();

            //         Debug.WriteLine("......." + promotions.Result);
////            Assert.AreSame("aaaa", promotions.Result);


        }

        private Promotion CreatePromotion(Promotion promotion)
        {
            string ret = ServiceCaller.Invoke("POST", url, JsonConvert.SerializeObject(promotion));
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
 //           Assert.IsTrue(GetPromotions().Count==0, "All promtions are removed");
        }

        private List<Promotion> GetPromotions()
        {
            var str = ServiceCaller.Invoke("GET", url, "");
            Debug.WriteLine("......." + str);

            List<Promotion> promotions = JsonConvert.DeserializeObject<List<Promotion>>(str);
            return promotions;
        }


    }

}
