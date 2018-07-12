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

        public static Promotion BuildDefinitePromotion(List<DateRange> dateRanges)
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

        public static Promotion BuildRecurrentPromotion(BasePattern pattern, TimeSchedule timeSchedule, DateRange effectiveDateRange)
        {
            var promotion = BuildDefinitePromotion(null);
            promotion.Period = new Recurrent
            {
                Pattern = pattern,
                TimeSchedule = timeSchedule,
                EffectiveDateRange = effectiveDateRange
            };
            return promotion;
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

        public static ActivePromotionsForDay GetActivePromotionsForDay(DateTime date, Guid id, TimeSpan startTime, TimeSpan endTime)
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

        public static ActivePromotionsForDay GetActivePromotionsForDay(DateTime date, Guid id1, TimeSpan startTime1, TimeSpan endTime1, Guid id2, TimeSpan startTime2, TimeSpan endTime2)
        {
            return new ActivePromotionsForDay
            {
                Date = date,
                PromotionIdsAndTimes = new List<ActivePromotionIdsAndTimes>
                {
                    new ActivePromotionIdsAndTimes
                    {
                        PromotionId = id1,
                        Times = new List<TimeSchedule>
                        {
                            new TimeSchedule
                            {
                                StartTime = startTime1,
                                EndTime = endTime1
                            }
                        }
                    },
                    new ActivePromotionIdsAndTimes
                    {
                        PromotionId = id2,
                        Times = new List<TimeSchedule>
                        {
                            new TimeSchedule
                            {
                                StartTime = startTime2,
                                EndTime = endTime2
                            }
                        }
                    }

                }
            };
        }

    }
}