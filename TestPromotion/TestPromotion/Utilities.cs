using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQ.Platform.PosPromotions.Model;
using IQ.Platform.PosPromotions.Utilities.Mappers;

namespace TestPromotion
{
    public class Utilities
    {
        public static void Compare(ActivePromotionsForNextDays expect, ActivePromotionsForNextDays actual)
        {
            Assert.AreEqual(expect.Id, actual.Id, "compare Id");
            CompareApplicablePromotionsForDays(expect.ApplicablePromotionsForDays.ToList(), actual.ApplicablePromotionsForDays.ToList());
            ComparePromotions(expect.Promotions.ToList(), actual.Promotions.ToList());
        }

        public static void CompareApplicablePromotionsForDays(List<ActivePromotionsForDay> expect, List<ActivePromotionsForDay> actual)
        {
            Assert.AreEqual(expect.Count, actual.Count, "compare length");
            for(int i=0; i<expect.Count; i++)
            {
                CompareActivePromotionsForDay(expect[i], actual[i]);
            }
        }

        public static void CompareActivePromotionsForDay(ActivePromotionsForDay expect, ActivePromotionsForDay actual)
        {
            Assert.AreEqual(expect.Date, actual.Date, "compare date");
            CompareActivePromotionIdsAndTimes(expect.PromotionIdsAndTimes.ToList(), actual.PromotionIdsAndTimes.ToList());
        }

        public static void CompareActivePromotionIdsAndTimes(List<ActivePromotionIdsAndTimes> expect, List<ActivePromotionIdsAndTimes> actual)
        {
            Assert.AreEqual(expect.Count, actual.Count);
            foreach(var expectIdAndTimes in expect)
            {
                var promotionId = expectIdAndTimes.PromotionId;
                bool isFound = false;
                foreach (var actualIdAndTimes in actual)
                {
                    if (!promotionId.Equals(actualIdAndTimes.PromotionId))
                    {
                        continue;
                    }

                    for(int i=0; i<expectIdAndTimes.Times.ToList().Count; i++)
                    {
                        Assert.AreEqual(expectIdAndTimes.Times.ToList()[i].StartTime, actualIdAndTimes.Times.ToList()[i].StartTime);
                        Assert.AreEqual(expectIdAndTimes.Times.ToList()[i].EndTime, actualIdAndTimes.Times.ToList()[i].EndTime);
                    }

                    isFound = true;
                    break;
                }
                Assert.IsTrue(isFound, "Failed to fnd the promotion with id "+promotionId);
            }
        }

        public static void ComparePromotions(List<ActivePromotion> expect, List<ActivePromotion> actual)
        {

        }

        public static List<ActivePromotion> ConvertPromotionsToActivePromotions(List<Promotion> promotions)
        {
            var mapper = new ActivePromotionMapper(new ActiveConditionMapper(new ActiveApplicableToMapper()));
            return promotions.Select(mapper.Map).ToList();
        }

    }
}
