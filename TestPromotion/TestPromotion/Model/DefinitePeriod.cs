using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPromotion.Model
{
    public class DefinitePeriod : PeriodBase
    {
        public IEnumerable<DateRange> DateRanges;
    }
}
