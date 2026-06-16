using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading;
using OpenTK.Graphics.OpenGL;


namespace Graphics2026.Model.Actors
{
    internal class SimpleActor : Renderable<Point>
    {
        public SimpleActor() : base(PrimitiveType.Triangles) { }
        public SimpleActor(string name) : base(name, PrimitiveType.Triangles) { }

        public override void Render()
        {
            ShaderProgram.DEFAULT.UseProgram();

            ShaderProgram.DEFAULT.ApplyTransform(transform.WorldTransform());

            base.Render();
        }
    }
}
