using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools;
using Graphics2026.Model.SceneManagement;

namespace Graphics2026.View
{
    internal class Renderer
    {
        public Dictionary<SurfaceType, bool> drawSurfaces = new();

        public Renderer()
        {
            foreach(SurfaceType surfaceType in Enum.GetValues(typeof(SurfaceType)))
            {
                drawSurfaces.Add(surfaceType, false);
            }
        }

        public virtual void RenderScene()
        {
            Scene scene = SceneManager.CurrentScene();

            /*
            foreach (Wall wall in scene.walls)
            {
                DrawActor(wall);
            }*/
            foreach (IRenderable renderable in scene.renderables)
            {
                DrawActor(renderable);
            }
        }

        protected virtual void DrawActor(IRenderable renderable)
        {
            if (!renderable.GetRenderStatus())
                return;

            if (!DrawSurface(renderable))
                return;

            renderable.Render();
        }

        private bool DrawSurface(IRenderable renderable)
        {
            if(!(renderable is Surface))
                return true;

            Surface surface = (Surface)renderable;

            bool draw = false;
            foreach (SurfaceType surfaceType in surface.GetTypes())
            {
                if (drawSurfaces[surfaceType])
                    draw = true;
            }

            return draw;
        }
    }
}
