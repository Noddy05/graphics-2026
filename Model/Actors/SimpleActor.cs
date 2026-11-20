using Graphics2026.Model.VertexData;
using OpenTK.Graphics.OpenGL;


namespace Graphics2026.Model.Actors
{
    internal class SimpleActor : Renderable<Point>
    {
        public SimpleActor() : base(PrimitiveType.Triangles) { }
        public SimpleActor(string name) : base(name, PrimitiveType.Triangles) { }

        public override void Render()
        {
            base.RenderWithShader(shader);
        }
    }
}
