
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.View;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Model.Game.BuildTools
{
    internal abstract class BuildTool
    {
        public List<SurfaceType> selectableSurfaces = new List<SurfaceType>();
        protected readonly Window window;
        protected readonly Renderer renderer;
        protected bool isActive = false;

        public BuildTool(List<SurfaceType> selectableSurfaces)
        {
            this.selectableSurfaces = selectableSurfaces;
            window = Program.GetWindow();
            renderer = window.GetRenderer()!;

            window.UpdateFrame += Update;
        }

        private void Update(FrameEventArgs args)
        {
            if (isActive)
                Update((float)args.Time);
        }

        protected abstract void Update(float deltaTime);

        public void ActivateTool()
        {
            foreach (SurfaceType surfaceType in selectableSurfaces)
            {
                renderer.drawSurfaces[surfaceType] = true;
            }

            isActive = true;
        }

        public void DeactivateTool()
        {
            isActive = false;

            foreach (SurfaceType surfaceType in selectableSurfaces)
            {
                renderer.drawSurfaces[surfaceType] = false;
            }
        }
    }
}
