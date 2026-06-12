using Graphics2026.Model.Actors;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.View.Shading.Shaders;

namespace Graphics2026.Model.SceneManagement
{
    internal class Scene : IRenderable
    {
        protected readonly Transform transform;
        private readonly static List<Surface> surfaces = new();
        protected string name;

        public Scene() : this("New Scene") { }
        public Scene(string name)
        {
            transform = new Transform(this);
            this.name = name;
        }
        public string GetName() => name;
        public Transform GetTransform() => transform;

        public void AddToScene(IRenderable renderable)
        {
            renderable.SetParent(transform);
        }
        public void AddToScene(Surface surface)
        {
            surfaces.Add(surface);
        }
        public List<Transform> GetChildren() => transform.GetChildren();
        public List<Surface> GetSurfaces() => surfaces;


        public IRenderable Clone()
        {
            throw new NotImplementedException();
        }

        public bool GetRenderStatus()
        {
            throw new NotImplementedException();
        }

        public void Render()
        {
            throw new NotImplementedException();
        }

        public void RenderFamilyWithShader(PhysicalShader shader)
        {
            throw new NotImplementedException();
        }

        public void RenderWithShader(PhysicalShader shader)
        {
            throw new NotImplementedException();
        }

        public void SetParent(Transform? parent)
        {
            throw new NotImplementedException();
        }

        public void SetRenderStatus(bool shouldRender)
        {
            throw new NotImplementedException();
        }
    }
}
