
using OpenTK.Windowing.Common;
using System.Diagnostics;

namespace Graphics2026
{
    internal static class Profiler
    {
        private static Dictionary<string, long> ticksPerType = new Dictionary<string, long>();
        private static Dictionary<string, long> prevTicksPerType = new Dictionary<string, long>();
        private static Window window;

        public static bool record = true;

        static Profiler()
        {
            window = Program.GetWindow();
            window.UpdateFrame += Update;
        }

        public static void AddTime(Type type, long ticks) => AddTime(type.ToString(), ticks);
        public static void AddTime(string type, long ticks)
        {
            if (ticksPerType.ContainsKey(type))
            {
                ticksPerType[type] += ticks;
            }
            else
            {
                ticksPerType.Add(type, ticks);
                prevTicksPerType.Add(type, ticks);
            }
        }

        public static void RecordTime(string name, Action function)
        {
            if (!record)
            {
                function();
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            function();
            sw.Stop();
            AddTime(name, sw.Elapsed.Ticks);
        }

        private static void Update(FrameEventArgs args)
        {
            foreach (string timings in ticksPerType.Keys)
            {
                ticksPerType[timings] -= prevTicksPerType[timings];
                prevTicksPerType[timings] = ticksPerType[timings];
            }
        }

        public static double GetTime(Type type) => GetTime(type.ToString());
        public static double GetTime(string type)
        {
            if (ticksPerType.ContainsKey(type))
                return ticksPerType[type] / (double)TimeSpan.TicksPerMillisecond;

            return 0;
        }

        public static KeyValuePair<string, long>[] GetTicksPerType() => [.. ticksPerType];
    }
}
