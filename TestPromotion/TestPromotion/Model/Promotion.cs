using System;

namespace TestPromotion.Model
{
    public class Promotion
    {
        public Guid Id;
        public string Name;
        public string Status;
        public PeriodBase Period;
        public Condition Condition;
        public Rule Rule;
    }
}