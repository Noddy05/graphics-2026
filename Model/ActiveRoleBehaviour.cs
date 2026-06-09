using OpenTK.Windowing.Common;
using System.Diagnostics;

namespace Graphics2026.Model
{
    internal abstract class ActiveRoleBehaviour
    {
        protected readonly Window window;
        public ActiveRoleBehaviour()
        {
            window = Program.GetWindow();

            Start();
            window.UpdateFrame += Update;
        }

        private Stopwatch stopwatch = new Stopwatch();
        private void Update(FrameEventArgs args)
        {
            if (!Profiler.record)
            {
                Update((float)args.Time);
                return;
            }

            stopwatch.Restart();
            Update((float)args.Time);
            stopwatch.Stop();

            Profiler.AddTime(GetType(), stopwatch.Elapsed.Ticks);
        }

        protected virtual void Start() { }
        protected virtual void Update(float deltaTime) { }
    }
}
