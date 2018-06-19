using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPromotion
{
    public class Utilities
    {
        public static void Compare(ActivePromotionsForNextDays expect, ActivePromotionsForNextDays actual)
        {
            Assert.AreEqual(expect.Id, actual.Id, "compare Id");
            CompareApplicablePromotionsForDays(expect.ApplicablePromotionsForDays.ToList(), actual.ApplicablePromotionsForDays.ToList());

        }

        public static void CompareApplicablePromotionsForDays(List<ActivePromotionsForDay> expect, List<ActivePromotionsForDay> actual)
        {
            Assert.AreEqual(expect.Count, actual.Count, "compare length");

        }
    }
}
