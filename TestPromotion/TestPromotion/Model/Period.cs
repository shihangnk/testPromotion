using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPromotion.Model
{
    public class Period
    {
        public string Tag;
        public IEnumerable<DateRange> DateRanges;
        public PatternBase Pattern;
        public TimeSchedule TimeScheudle;
        public DateRange EffectiveDateRange;

    }
}
