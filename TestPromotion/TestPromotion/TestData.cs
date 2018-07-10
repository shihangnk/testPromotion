using IQ.Platform.PosPromotions.Model;
using IQ.Platform.PosPromotions.Model.Types.Conditions;
using IQ.Platform.PosPromotions.Model.Types.Conditions.ApplicableTo;
using IQ.Platform.PosPromotions.Model.Types.Conditions.Customer;
using IQ.Platform.PosPromotions.Model.Types.Conditions.Period;
using IQ.Platform.PosPromotions.Model.Types.Discounts;
using IQ.Platform.PosPromotions.Model.Types.Rules;
using System;
using System.Collections.Generic;
using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;

namespace TestPromotion
{
    public class TestData
    {
        public const string userName = "admin@automatedtestingcova.com";
        public const string password = "Abcd1234";

        public static Promotion GetAPromotionObject()
        {
            return GetAPromotionObject(new List<DateRange> {
                        new DateRange()
                        {
                            StartDate = new DateTime(2019,05, 01, 1, 10, 30),
                            EndDate = new DateTime(2019,05, 10, 1, 10, 30)
                        }
                    });
        }

        public static Promotion GetAPromotionObject(List<DateRange> dateRanges)
        {
            return new Promotion()
            {
                Name = "Prom1",
                Period = new Definite()
                {
                    DateRanges = dateRanges
                },
                Status = "Active",
                Condition = new Condition
                {
                    MatchAll = new ApplicableTo
                    {
                        Products = new ProductsClassificationsAndCategories
                        {
                            Products = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                            Classifications = new List<int> { 100, 200, 300 },
                            Categories = new List<int> { 400, 500, 600 }
                        },
                        Locations = new AllLocations(),
                    },
                    Customers = new AllCustomers()
                },
                Rule = new MatchedLineItems
                {
                    Discount = new PercentageDiscount
                    {
                        Percentage = 10
                    }
                }
            };
        }

        public static ActivePromotionsForNextDays GetEmptyActivePromotionsForNextDays(string id)
        {
            var expect = new ActivePromotionsForNextDays()
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>(),
                Promotions = new List<ActivePromotion>()
            };
            return expect;
        }

        public static ActivePromotionsForDay GetActivePromotionsForDay(Guid id, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return new ActivePromotionsForDay
            {
                Date = date,
                PromotionIdsAndTimes = new List<ActivePromotionIdsAndTimes>
                {
                    new ActivePromotionIdsAndTimes
                    {
                        PromotionId = id,
                        Times = new List<TimeSchedule>
                        {
                            new TimeSchedule
                            {
                                StartTime = startTime,
                                EndTime = endTime
                            }
                        }
                    }
                }
            };
        }

    }
}