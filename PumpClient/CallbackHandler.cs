using PumpService;
using System;

namespace PumpClient
{
    public class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(StatisticsService statistics)
        {
            Console.Clear();
            Console.WriteLine($"All: {statistics.AllTacts}");
            Console.WriteLine($"Success: {statistics.SuccessTacts}");
            Console.WriteLine($"Error: {statistics.ErrorTacts}");
        }
    }
}
