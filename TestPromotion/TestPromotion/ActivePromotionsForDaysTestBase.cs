using System.Collections.Generic;
using IQ.Platform.PosPromotions.Model;
using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace TestPromotion
{
    public abstract class ActivePromotionsForDaysTestBase
    {
        protected const string UrlBase = "http://localhost:61284/v1/Companies(272002)";
//        protected const string UrlBase = "https://apiint.iqmetrix.net/pospromotions/v1/companies(272002)";
        protected const string Url = UrlBase + "/promotions";
        protected const string UrlGetActivePromotions = UrlBase + "/entities(337730)/ActivePromotionsForDays";

        protected Promotion InputProm1, InputProm2;
        protected Promotion OutputProm1, OutputProm2;
        protected string Id;
        protected ActivePromotionsForNextDays Actual;
        protected ActivePromotionsForNextDays Expect;

        protected Promotion CreatePromotion(Promotion promotion)
        {
            string ret = ServiceCaller.Invoke("POST", Url, JsonConvert.SerializeObject(promotion));
            return JsonConvert.DeserializeObject<Promotion>(ret);
        }

        protected void ClearPromotion()
        {
            List<Promotion> promotions = GetPromotions();
            foreach (var promotion in promotions)
            {
                ServiceCaller.Invoke("DELETE", $"{Url}({promotion.Id})", "");
            }
            Assert.AreEqual(GetPromotions().Count, 0, "All promtions are removed");
        }

        protected List<Promotion> GetPromotions()
        {
            var str = ServiceCaller.Invoke("GET", Url, "");
            List<Promotion> promotions = JsonConvert.DeserializeObject<List<Promotion>>(str);
            return promotions;
        }

        protected ActivePromotionsForNextDays GetActivePromotions(string id)
        {
            var str = ServiceCaller.Invoke("GET", $"{UrlGetActivePromotions}({id})", "");
            ActivePromotionsForNextDays promotions = JsonConvert.DeserializeObject<ActivePromotionsForNextDays>(str);
            return promotions;
        }
    }
}