using Graphics2026.Model.Attachments;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.Actors
{
    internal class Actor : Renderable<Vertex>
    {
        public readonly List<ActorAttachment> attachments = new List<ActorAttachment>();

        public Actor() : base(PrimitiveType.Triangles) { }
        public Actor(string name) : base(name, PrimitiveType.Triangles) { }

        public override Actor Clone()
        {
            Actor clone = new Actor(name);
            clone.shader = shader;
            clone.mesh = mesh;
            clone.shouldRender = shouldRender;
            clone.transform.CopyTransform(transform);
            clone.attachments.AddRange(attachments);

            return clone;
        }

        public override void Render()
        {
            if (mesh == null)
                return;

            PhysicalShader? shader = this.shader;

            if(shader == null)
                shader = ShaderProgram.DEFAULT;

            shader.UseProgram();

            shader.ApplyTransform(transform.WorldTransform());
            shader.ApplyCameraAndProjection(Camera.current);

            base.Render();
        }

        public virtual T AddAttachment<T>(params object?[]? args) where T : ActorAttachment
        {
            int numArgs = 1;
            if (args != null)
                numArgs = args.Length + 1;

            object?[]? arguments = new object?[numArgs];
            arguments[0] = this;
            for (int i = 1; i < arguments.Length; i++)
                arguments[i] = args![i - 1];

            T instance = (T)Activator.CreateInstance(typeof(T), arguments)!;
            attachments.Add(instance);
            return instance;
        }
    }
}
