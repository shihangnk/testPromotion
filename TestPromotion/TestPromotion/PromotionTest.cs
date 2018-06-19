using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IQ.Platform.PosPromotions.Model;
using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;
using IQ.Platform.PosPromotions.Model.Types.Conditions.Period;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestPromotion
{
    [TestClass]
    public class PromotionTest
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PromotionTest));
        //      private const string _urlBase = "http://localhost:61284/v1/Companies(272002)";
        private const string _urlBase = "https://apiint.iqmetrix.net/pospromotions/v1/companies(272002)";
        private const string _url = _urlBase+"/promotions";
        private const string _getActivePromotions = _urlBase + "/entities(337730)/ActivePromotionsForDays";

        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine("..................................... step 1");

            ClearPromotion();

            var prom1 = TestData.GetAPromotionObject(
                new List<DateRange> {
                        new DateRange()
                        {
                            StartDate = new DateTime(2018, 05, 01, 1, 10, 20),
                            EndDate = new DateTime(2018, 05, 10, 15, 30, 40)
                        }
                    });
            Promotion p1 = CreatePromotion(prom1);

            Debug.WriteLine(".............. p1 " + p1.Id);

            string id = "2018-05-10, 30";
            var actual = GetActivePromotions(id);

            var expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    new ActivePromotionsForDay
                    {
                        Date = new DateTime(2018, 05, 10),
                        PromotionIdsAndTimes = new List<ActivePromotionIdsAndTimes>
                        {
                            new ActivePromotionIdsAndTimes
                            {
                                PromotionId = p1.Id,
                                Times = new List<TimeSchedule>
                                {
                                    new TimeSchedule
                                    {
                                        StartTime = new TimeSpan(0, 0, 0),
                                        EndTime = new TimeSpan(15, 30, 40)
                                    }
                                }
                            }
                        }
                    }
                },
                Promotions = new List<ActivePromotion>()
            };

            Utilities.Compare(expect, actual);
        }

        private Promotion CreatePromotion(Promotion promotion)
        {
            string ret = ServiceCaller.Invoke("POST", _url, JsonConvert.SerializeObject(promotion));
            return JsonConvert.DeserializeObject<Promotion>(ret);
        }

        private void ClearPromotion()
        {
            List<Promotion> promotions = GetPromotions();
            foreach (var promotion in promotions)
            {
                ServiceCaller.Invoke("DELETE", $"{_url}({promotion.Id})", "");
            }
            Assert.IsTrue(GetPromotions().Count==0, "All promtions are removed");
        }

        private List<Promotion> GetPromotions()
        {
            var str = ServiceCaller.Invoke("GET", _url, "");
            List<Promotion> promotions = JsonConvert.DeserializeObject<List<Promotion>>(str);
            return promotions;
        }

        private ActivePromotionsForNextDays GetActivePromotions(string id)
        {
            var str = ServiceCaller.Invoke("GET", $"{_getActivePromotions}({id})", "");
            ActivePromotionsForNextDays promotions = JsonConvert.DeserializeObject<ActivePromotionsForNextDays>(str);
            return promotions;
        }
    }

}
