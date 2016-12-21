using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockportWebapp.Utils
{
    public interface ITimeProvider
    {
        DateTime Now();
        DateTime Today();
    }

    public class TimeProvider : ITimeProvider
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}
