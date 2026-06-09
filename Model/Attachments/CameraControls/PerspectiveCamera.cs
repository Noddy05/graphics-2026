using Graphics2026.Controller;
using Graphics2026.Model.Actors;
using Graphics2026.Model.Rays;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Model.Attachments.CameraControls
{
    internal class PerspectiveCamera : Camera
    {
        /// <summary>
        /// FOV in degrees
        /// </summary>
        private float fov = 70f;

        public float cameraSensitivity = 1f / (800 / 57);

        public PerspectiveCamera(Actor actor, float fov, float nearDepth, float farDepth) 
            : base(actor, nearDepth, farDepth)
        {
            this.fov = fov;

            window.Resize += OnResize;
            window.MouseMove += MouseMoved;

            UpdateProjection(); 
        }

        private void MouseMoved(MouseMoveEventArgs args)
        {
            if (!Input.GetMouseButton(MouseButton.Right))
            {
                window.CursorState = CursorState.Normal;
                return;
            }

            window.CursorState = CursorState.Grabbed;

            actor.transform.localRotation.X += args.DeltaY * cameraSensitivity;
            actor.transform.localRotation.X = MathF.Min(MathF.Max(
                actor.transform.localRotation.X, -90), 90);

            actor.transform.localRotation.Y += args.DeltaX * cameraSensitivity;
        }

        public void SetFOV(float fov)
        {
            this.fov = fov;
            UpdateProjection();
        }

        public override void SetNearDepth(float nearDepth)
        {
            base.SetNearDepth(nearDepth);
            UpdateProjection();
        }
        public override void SetFarDepth(float farDepth)
        {
            base.SetFarDepth(farDepth);
            UpdateProjection();
        }

        protected virtual void OnResize(ResizeEventArgs e) => UpdateProjection();

        private void UpdateProjection()
        {
            projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov * MathF.PI / 180,
                Program.GetWindow().GetAspectRatio(), nearDepth, farDepth);
        }

        public override Ray MouseRay()
        {
            Vector2 localPos = (window.MousePosition - window.ClientSize / 2)
                / window.ClientSize.Y * 2 * MathF.Tan(fov * MathF.PI / 360);

            return new Ray(Vector3.Zero, new Vector3(localPos.X, -localPos.Y, -1f));
        }

        public override Ray MouseRayInWorld()
        {
            Ray ray = MouseRay();
            Vector3 worldSpaceDirection = (transform.LocalRotation() * new Vector4(ray.direction, 1)).Xyz;
            return new Ray(transform.localPosition, worldSpaceDirection);
        }
    }
}
