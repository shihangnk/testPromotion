using System;
using System.Collections.Generic;
using System.Diagnostics;
using IQ.Platform.PosPromotions.Model;
using IQ.Platform.PosPromotions.Model.Types.ActivePromotion;
using IQ.Platform.PosPromotions.Model.Types.Conditions.Period;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static System.DayOfWeek;

namespace TestPromotion
{
    [TestClass]
    public class PromotionTest
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PromotionTest));
        private const string _urlBase = "http://localhost:61284/v1/Companies(272002)";
    //    private const string _urlBase = "https://apiint.iqmetrix.net/pospromotions/v1/companies(272002)";
        private const string _url = _urlBase+"/promotions";
        private const string _getActivePromotions = _urlBase + "/entities(337730)/ActivePromotionsForDays";

        [TestMethod]
        public void Test_01_DefinitePromotionWithOneDateRange()
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

            // --------- out of range, no active promotion ------
            string id = "2018-04-10, 10";
            var actual = GetActivePromotions(id);
            var expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- one active promotion -----------
            id = "2018-04-30, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 01), p1.Id, new TimeSpan(1, 10, 20), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- one active promotion -----------
            id = "2018-05-01, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 01), p1.Id, new TimeSpan(1, 10, 20), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 02), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- in date range -----------
            id = "2018-05-02, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 02), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 03), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- one active promotion -----------
            id = "2018-05-10, 30";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 10), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(15, 30, 40))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // --------- out of range, no active promotion ------
            id = "2018-05-11, 10";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);
        }

        [TestMethod]
        public void Test_02_DefinitePromotionWithTwoDateRanges()
        {
            Debug.WriteLine("..................................... step 1");

            ClearPromotion();

            var prom1 = TestData.GetAPromotionObject(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 01, 01, 10, 20),
                        EndDate = new DateTime(2018, 05, 10, 15, 30, 40)
                    },
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 06, 01, 12, 01, 02),
                        EndDate = new DateTime(2018, 06, 10, 18, 03, 04)
                    }
                });
            Promotion p1 = CreatePromotion(prom1);

            Debug.WriteLine(".............. p1 " + p1.Id);

            // ----------- earlier than the first start date, no promotion
            string id = "2018-04-10, 10";
            var actual = GetActivePromotions(id);
            var expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- right next to the first start date, no promotion
            id = "2018-04-30, 1";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- the first start date, one promotion
            id = "2018-05-01, 1";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 01), p1.Id, new TimeSpan(01, 10, 20), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- in the first date range, one promotion
            id = "2018-05-02, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 02), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 03), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- end date is out of the first date ranges -----------
            id = "2018-05-10, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 10), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(15, 30, 40))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- across two date ranges -----------
            id = "2018-05-10, 24";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 10), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(15, 30, 40)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 01), p1.Id, new TimeSpan(12, 1, 2), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 02), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- between two date ranges, no promotion
            id = "2018-05-11, 10";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- right next to the second start date, no promotion
            id = "2018-05-31, 1";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- the second start date
            id = "2018-05-31, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 01), p1.Id, new TimeSpan(12, 1, 2), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- the second start date
            id = "2018-06-01, 1";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 01), p1.Id, new TimeSpan(12, 1, 2), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- the second end date
            id = "2018-06-09, 3";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 09), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 10), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(18, 03, 04))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- later than the last end date, no promotion
            id = "2018-06-11, 10";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);
        }

        [TestMethod]
        public void Test_03_TwoDefinitePromotions()
        {
            Debug.WriteLine("..................................... step 1");

            ClearPromotion();

            var prom1 = TestData.GetAPromotionObject(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 01, 01, 10, 20),
                        EndDate = new DateTime(2018, 05, 10, 15, 30, 40)
                    }
                });
            Promotion p1 = CreatePromotion(prom1);

            var prom2 = TestData.GetAPromotionObject(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 06, 01, 12, 01, 02),
                        EndDate = new DateTime(2018, 06, 10, 18, 03, 04)
                    }
                });
            Promotion p2 = CreatePromotion(prom2);

            Debug.WriteLine(".............. p1 " + p1.Id +"... p2 "+p2.Id);

            // ----------- earlier than the first start date, no promotion
            string id = "2018-04-10, 10";
            var actual = GetActivePromotions(id);
            var expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- right next to the first start date, no promotion
            id = "2018-04-30, 1";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- the first start date, one promotion
            id = "2018-05-01, 1";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 01), p1.Id, new TimeSpan(01, 10, 20), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- in the first date range, one promotion
            id = "2018-05-02, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 02), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 03), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- end date is out of the first date ranges -----------
            id = "2018-05-10, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 10), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(15, 30, 40))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- across two date ranges -----------
            id = "2018-05-10, 24";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 10), p1.Id, new TimeSpan(0, 0, 0), new TimeSpan(15, 30, 40)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 01), p2.Id, new TimeSpan(12, 1, 2), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 02), p2.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- between two date ranges, no promotion
            id = "2018-05-11, 10";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- right next to the second start date, no promotion
            id = "2018-05-31, 1";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);

            // ----------- the second start date, no promotion
            id = "2018-05-31, 2";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 01), p2.Id, new TimeSpan(12, 1, 2), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- the second start date, no promotion
            id = "2018-06-01, 1";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 01), p2.Id, new TimeSpan(12, 1, 2), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- the second end date, no promotion
            id = "2018-06-09, 3";
            actual = GetActivePromotions(id);
            expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 09), p2.Id, new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 06, 10), p2.Id, new TimeSpan(0, 0, 0), new TimeSpan(18, 03, 04))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

            // ----------- later than the last end date, no promotion
            id = "2018-06-11, 10";
            actual = GetActivePromotions(id);
            expect = TestData.GetEmptyActivePromotionsForNextDays(id);
            Utilities.Compare(expect, actual);
        }

        [TestMethod]
        public void Test_04_TwoDefinitePromotionsOnTheSameDay()
        {
            Debug.WriteLine("..................................... step 1");

            ClearPromotion();

            var prom1 = TestData.GetAPromotionObject(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 01, 01, 10, 20),
                        EndDate = new DateTime(2018, 05, 10, 15, 30, 40)
                    },
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 15, 10, 10, 20),
                        EndDate = new DateTime(2018, 05, 15, 12, 13, 14)
                    }
                });
            Promotion p1 = CreatePromotion(prom1);

            var prom2 = TestData.GetAPromotionObject(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 15, 14, 01, 02),
                        EndDate = new DateTime(2018, 06, 10, 18, 03, 04)
                    }
                });
            Promotion p2 = CreatePromotion(prom2);

            Debug.WriteLine(".............. p1 " + p1.Id + "... p2 " + p2.Id);

            string id = "2018-05-14, 3";
            var actual = GetActivePromotions(id);
            var expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 15), p1.Id, new TimeSpan(10, 10, 20), new TimeSpan(12, 13, 14), p2.Id, new TimeSpan(14, 01, 02), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 16), p2.Id, new TimeSpan(00, 00, 00), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);
        }

        [TestMethod]
        public void Test_05_RecurrentPromotion()
        {
            ClearPromotion();

            var prom1 = TestData.GetRecurrentPromotionObject(
                new Weekly
                {
                    DaysOfTheWeek = new List<DayOfWeek>{Monday, Friday}
                },
                new TimeSchedule
                {
                    StartTime = new TimeSpan(10, 11, 12),
                    EndTime = new TimeSpan(20, 21, 22)
                },
                new DateRange
                {
                    StartDate = new DateTime(2018, 05, 1, 10, 10, 20),
                    EndDate = new DateTime(2018, 05, 31, 12, 13, 14)
                }
            );
            Promotion p1 = CreatePromotion(prom1);

            var prom2 = TestData.GetAPromotionObject(
                new List<DateRange> {
                    new DateRange()
                    {
                        StartDate = new DateTime(2018, 05, 15, 14, 01, 02),
                        EndDate = new DateTime(2018, 06, 10, 18, 03, 04)
                    }
                });
            Promotion p2 = CreatePromotion(prom2);

            Debug.WriteLine(".............. p1 " + p1.Id + "... p2 " + p2.Id);

            string id = "2018-05-14, 3";
            var actual = GetActivePromotions(id);
            var expect = new ActivePromotionsForNextDays
            {
                Id = id,
                ApplicablePromotionsForDays = new List<ActivePromotionsForDay>
                {
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 15), p1.Id, new TimeSpan(10, 10, 20), new TimeSpan(12, 13, 14), p2.Id, new TimeSpan(14, 01, 02), new TimeSpan(23, 59, 59)),
                    TestData.GetActivePromotionsForDay(new DateTime(2018, 05, 16), p2.Id, new TimeSpan(00, 00, 00), new TimeSpan(23, 59, 59))
                },
                Promotions = new List<ActivePromotion>()
            };
            Utilities.Compare(expect, actual);

        }

        [TestMethod]
        public void Test_06_MixedPromotions() { }


        [TestMethod]
        public void Test_16_TestLocations() { }


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
