using System.Threading.Tasks;

namespace StockportWebappTests
{
    public static class AsyncTestHelper
    {
        private const int Timeout = 50;

        public static T Resolve<T>(Task<T> task)
        {
            task.Wait(Timeout);
            return task.Result;
        }
    }
}
