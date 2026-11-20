using Graphics2026.Model.Actors;
using Graphics2026.Model.SceneManagement;

namespace Graphics2026.View
{
    internal class Renderer
    {
        public Renderer()
        {

        }

        public virtual void RenderScene()
        {
            Scene scene = SceneManager.CurrentScene();
            DrawFamily(scene);
        }

        protected virtual void DrawFamily(IParent parentObject)
        {
            foreach (Transform child in parentObject.GetChildren())
            {
                DrawRenderable(child.GetRenderable());
                DrawFamily(child);
            }
        }

        protected virtual void DrawRenderable(IRenderable renderable)
        {
            if (!renderable.ShouldRender())
                return;

            renderable.Render();
        }
    }
}
