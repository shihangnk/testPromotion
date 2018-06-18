using System;
using System.Collections.Generic;
using TestPromotion.Model;

namespace TestPromotion
{
    public class TestData
    {
        public const string userName = "admin@automatedtestingcova.com";
        public const string password = "Abcd1234";

        public static Promotion GetAPromotionObject()
        {
            return new Promotion()
            {
                Name = "Prom1",
                Period = new Period()
                {
                    Tag = "Definite",
                    DateRanges = new List<DateRange> {
                        new DateRange()
                        {
                            StartDate = new DateTime(2019,05, 01, 1, 10, 30),
                            EndDate = new DateTime(2019,05, 10, 1, 10, 30)
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
                            Products = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
                            Classifications = new List<int> { 100, 200, 300 },
                            Categories = new List<int> { 400, 500, 600 }
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
        }
    }
}