using Graphics2026.View;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Controller;
using System.Diagnostics;
using Graphics2026.Model.Game.BuildTools.Walls;
using Graphics2026.Model.Game.BuildTools;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Mesh;

namespace Graphics2026
{
    internal class Window : GameWindow
    {
        private Renderer? renderer;

        private double timeSinceStart;
        private Builder builder;

        private Actor pivot = new Actor();
        private Actor child = new Actor();

        public Window() : base(GameWindowSettings.Default, new NativeWindowSettings
        {
            ClientSize = new Vector2i(1920, 1080),
            StartVisible = false,
            Title = "yes"
        })
        {
            CenterWindow();
        }

        public Renderer? GetRenderer() => renderer;

        public double GetTimeSinceStart() => timeSinceStart;

        protected override void OnLoad()
        {
            renderer = new Renderer();

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.CullFace(TriangleFace.Back);
            GL.Enable(EnableCap.StencilTest);

            GL.ClearColor(Color4.DarkGray);

            new Materials();
            builder = new Builder()
                .AddTool(new WallBuilder())
                .AddTool(new WallMod());

            new GridController();
            TestSceneFactory.InitializeScene();
            //BuildOnFloor gridTiler = new BuildOnFloor();

            IsVisible = true;


            child.transform.localPosition = new Vector3(1, 0, 0);
            child.mesh = MeshGenerator.Cube();
            child.transform.localSize *= 0.25f;
            child.shader = new DefaultLit();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        public float GetAspectRatio() => ClientSize.X / (float)ClientSize.Y;

        private Stopwatch stopwatch = new Stopwatch();
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            timeSinceStart += args.Time;

            Title = "Time spent Pathfinding: " + Profiler.GetTime(typeof(Window)).ToString("0.00")
                + "ms - FPS: " + (1 / args.Time).ToString("0.00");

            pivot.transform.localPosition.Z = MathF.Sin((float)timeSinceStart) - 4;
            pivot.transform.localRotation.Y = 4 * (float)timeSinceStart;

            WireRenderer.DrawTransform(pivot, 0.25f);
            if(Input.GetKeyDown(Keys.V))
            {
                if (child.transform.GetParent() == null)
                {
                    child.SetParent(pivot);
                } 
                else
                {
                    child.SetParent(null);
                }
            }

            if (Input.ToggleProfilerRecord())
                Profiler.record = !Profiler.record;

            if (Input.PrintProfilerStats())
            {
                foreach (KeyValuePair<string, long> val in Profiler.GetTicksPerType())
                {
                    Console.WriteLine("(" + val.Key + ") " + val.Value + "ns");
                }
            }

            if (!Profiler.record)
            {
                DrawFrame(args);
                return;
            }

            stopwatch.Restart();
            DrawFrame(args);
            stopwatch.Stop();

            Profiler.AddTime("Window - Render Frame", stopwatch.Elapsed.Ticks);
        }

        private void DrawFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            if (renderer == null)
                return;

            renderer.RenderScene();
            Context.SwapBuffers();

            GL.StencilMask(0xFF);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit
                | ClearBufferMask.StencilBufferBit);
            GL.StencilMask(0x00);
        }
    }
}
