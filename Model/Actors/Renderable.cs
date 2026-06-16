using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.Actors
{
    internal class Renderable<T> : IRenderable where T : struct, IVertex
    {
        public string name;
        public PhysicalShader? shader;
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
            actorNo = actorCounter++;
            this.name = name;
        }

        public virtual Renderable<T> AddToScene()
        {
            SceneManager.AddToScene(this);
            return this;
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
        public void RenderFamilyWithShader(PhysicalShader shader)
        {
            foreach(Transform child in transform.GetChildren())
            {
                child.GetRenderable().RenderFamilyWithShader(shader);
            }

            RenderWithShader(shader);
        }

        public void SetRenderStatus(bool shouldRender) => this.shouldRender = shouldRender;
        public bool GetRenderStatus() => shouldRender;
        public void SetParent(Transform? parent) => GetTransform().SetParent(parent);

        public virtual void RenderWithShader(PhysicalShader shader)
        {
            if (mesh == null)
                return;

            shader.UseProgram();
            shader.ApplyTransform(transform.WorldTransform());

            mesh.PrepareForRendering();
            GL.DrawElements(primitiveType, mesh.NumIndices(), DrawElementsType.UnsignedInt, 0);
        }

        public virtual IRenderable Clone()
        {
            Renderable<T> clone = new Renderable<T>(name, primitiveType);
            clone.shader = shader;
            clone.mesh = mesh;
            clone.shouldRender = shouldRender;
            clone.transform.CopyTransform(transform);

            return clone;
        }

        public static implicit operator Transform(Renderable<T> renderable) => renderable.GetTransform();
    }
}
