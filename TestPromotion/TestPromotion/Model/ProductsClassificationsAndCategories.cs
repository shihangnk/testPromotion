using System;
using System.Collections.Generic;

namespace TestPromotion.Model
{
    public class ProductsClassificationsAndCategories
    {
        public string Tag;
        public IEnumerable<Guid> Products;
        public IEnumerable<int> Classifications;
        public IEnumerable<int> Categories;
    }
}