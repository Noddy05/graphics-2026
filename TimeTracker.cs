using System.Diagnostics;

namespace Graphics2026
{
    internal class TimeTracker
    {
        private int runs = 0;
        private long totalMillis = 0;
        private Stopwatch stopwatch = new Stopwatch();

        public void Run(Action function, int numRuns = 1)
        {
            stopwatch.Restart();
            for(int i = 0; i < numRuns; i++) { 
                function();
            }
            totalMillis += stopwatch.ElapsedMilliseconds;

            runs += numRuns;
        }

        public (int runs, long totalMillis) GetStats() => (runs, totalMillis);
        public double AverageMillis() => totalMillis / (double)runs;
    }
}
