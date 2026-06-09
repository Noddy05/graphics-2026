using Graphics2026.Model.Actors;
using OpenTK.Graphics.OpenGL;

namespace Graphics2026.Model.Game.BuildTools.Walls
{
    internal class Wall : Actor
    {
        private List<IRenderable> cutouts = new();

        public Wall() : this($"New Wall ({actorCounter})") { }
        public Wall(string name) : base(name) { }
        /*
        public Wall(string name)
        {
            SceneManager.CurrentScene().actors.Add(this);
            actorNo = actorCounter++;
            this.name = name;
        }*/

        public void AddCutout(IRenderable renderable)
        {
            renderable.SetRenderStatus(false);
            cutouts.Add(renderable);
        }
        public void ClearCutouts()
        {
            cutouts.Clear();
        }

        public override void Render()
        {
            //Disable rendering color
            GL.ColorMask(false, false, false, false);

            //Disable rendering depth
            GL.DepthMask(false);


            GL.StencilMask(0xFF); // Enable writing to stencil buffer
            GL.Clear(ClearBufferMask.StencilBufferBit);

            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);

            foreach(IRenderable cutout in cutouts)
            {
                cutout.Render();
            }

            GL.StencilFunc(StencilFunction.Notequal, 1, 0xFF);

            //Enable rendering color:
            GL.ColorMask(true, true, true, true);
            GL.DepthMask(true);
            base.Render();

            GL.StencilFunc(StencilFunction.Always, 1, 0xFF);
            //Enable rendering depth:
        }
    }
}
