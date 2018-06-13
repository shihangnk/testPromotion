using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestPromotion
{
    [TestClass]
    public class PromotionTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var promotions = getPromotions();

            Console.Out.WriteLine("......." + promotions.Result);

            Assert.AreSame("aaaaa", promotions.Result);
        }

        async Task<string> getPromotions()
        {
            return await getPromotionsAsync();
        }

        async Task<string> getPromotionsAsync()
        {
            await Task.Delay(1000);
            return "aaaaa";
        }
    }
}
