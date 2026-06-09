using Graphics2026.Model.SceneManagement;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.Actors
{
    internal class Renderable<T> : IRenderable where T : struct, IVertex
    {
        public string name;
        protected int actorNo;
        protected static int actorCounter = 1;

        public Mesh<T>? mesh;
        private readonly PrimitiveType primitiveType;
        protected bool shouldRender = true;

        public readonly Transform transform;

        public Renderable(PrimitiveType primitiveType) 
            : this($"New Renderable ({actorCounter})", primitiveType) { }
        public Renderable(string name, PrimitiveType primitiveType)
        {
            this.primitiveType = primitiveType;
            transform = new Transform(this);
            SceneManager.CurrentScene().renderables.Add(this);
            actorNo = actorCounter++;
            this.name = name;
        }

        public string GetName() => name;
        public Transform GetTransform() => transform;

        public virtual void Render()
        {
            if (mesh == null)
                return;

            mesh.PrepareForRendering();
            GL.DrawElements(primitiveType, mesh.NumIndices(), DrawElementsType.UnsignedInt, 0);
        }

        public void SetRenderStatus(bool shouldRender) => this.shouldRender = shouldRender;
        public bool GetRenderStatus() => shouldRender;
        public void SetParent(Transform? parent) => GetTransform().SetParent(parent);

        public static implicit operator Transform(Renderable<T> renderable) => renderable.GetTransform();
    }
}
