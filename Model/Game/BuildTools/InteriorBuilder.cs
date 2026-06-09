using Graphics2026.Controller;
using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Game.BuildTools.Walls;
using Graphics2026.Model.Mesh;
using Graphics2026.Model.SceneManagement;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading.Shaders;
using Graphics2026.View;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics2026.Model.Game.BuildTools
{
    internal class InteriorBuilder : BuildTool
    {
        public InteriorBuilder() : base([ SurfaceType.Floor ])
        {

        }

        protected override void Update(float deltaTime)
        {
            Surface? surface = SurfaceSelect.RaycastSelectSurface(out Vector3 point, out float length);
            if (surface == null)
                return;

            if (!surface.IsType(SurfaceType.Floor))
                return;

            Floor floor = (Floor)surface;
            Vector3 snappedPoint = floor.SnapToGrid(point);

            WireRenderer.DrawInFront(true);
            WireRenderer.DrawSphere(snappedPoint, 0.1f);
            WireRenderer.DrawInFront(false);


            if (Input.GetMouseButtonDown(MouseButton.Left))
            {

            }
        }
    }
}
