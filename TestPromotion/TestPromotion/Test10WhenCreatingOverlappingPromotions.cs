using System;
using System.Collections.Generic;
using IQ.Platform.PosPromotions.Model;
using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;
using IQ.Platform.PosPromotions.Model.Types.Conditions.Period;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPromotion
{
    [TestClass]
    public class Test10WhenCreatingOverlappingPromotions : ActivePromotionsForDaysTestBase
    {
        [TestMethod]
        public void Test_01_ShouldReturnOverlappingPromotions()
        {
            ClearPromotion();
            InputProm1 = TestData.BuildDefinitePromotion(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 01, 1, 10, 20),
                        EndDate = new DateTime(2018, 05, 10, 15, 30, 40)
                    }
                });

            InputProm2 = TestData.BuildDefinitePromotion(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 04, 30, 1, 10, 20),
                        EndDate = new DateTime(2018, 05, 10, 15, 30, 40)
                    }
                });

            OutputProm1 = CreatePromotion(InputProm1);
            OutputProm2 = CreatePromotion(InputProm2);

            Id = "2018-05-01, 1";
            Actual = GetActivePromotions(Id);
            Expect = new ActivePromotionsForNextDays
            {
                Id = Id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 01), OutputProm1.Id, new TimeSpan(1, 10, 20), new TimeSpan(23, 59, 59), OutputProm2.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = Utilities.ConvertPromotionsToActivePromotions(new List<Promotion> { OutputProm1, OutputProm2 })
            };
            Utilities.Compare(Expect, Actual);
        }
    }
}
