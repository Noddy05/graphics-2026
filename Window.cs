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
using Graphics2026.View.Shading;
using Graphics2026.Model.Game.BuildTools.Interior;
using Graphics2026.Shopping;
using Graphics2026.Model.BufferObjects.UniformBufferObject;
using Graphics2026.Model.Attachments.CameraControls;

namespace Graphics2026
{
    internal class Window : GameWindow
    {
        private Renderer? renderer;
        private UBO<CameraBlock> cameraUBO;

        private double timeSinceStart;

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
            GL.Enable(EnableCap.Blend);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(Color4.DarkGray);

            new Materials();
            Player.SetBalance(10_000);
            new CreateStore();
            cameraUBO = new UBO<CameraBlock>(1, BufferUsageHint.StreamCopy);
            //builder.AddTool(new WallBuilder())
            //builder.AddTool(new WallMod());

            //new GridController();
            //TestSceneFactory.InitializeScene();
            //BuildOnFloor gridTiler = new BuildOnFloor();

            IsVisible = true;
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

            if (Input.ToggleProfilerRecord())
                Profiler.record = !Profiler.record;

            if (Input.PrintProfilerStats())
            {
                foreach (KeyValuePair<string, long> val in Profiler.GetTicksPerType())
                {
                    Console.WriteLine("(" + val.Key + ") " + val.Value + "ns");
                }
            }

            cameraUBO.GetBlock().view = Camera.current!.GetCameraMatrix();
            cameraUBO.GetBlock().projection = Camera.current!.GetProjection();
            cameraUBO.BindData();

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

            SurfaceSelect.RaycastSelectSurface(out Vector3 point, out _);
            WireRenderer.DrawCircle(point, NavigationObstacle.SDF(point.Xz));

            Context.SwapBuffers();

            GL.StencilMask(0xFF);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit
                | ClearBufferMask.StencilBufferBit);
            GL.StencilMask(0x00);
        }
    }
}
