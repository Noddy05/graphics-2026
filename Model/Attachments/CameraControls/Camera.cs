using Graphics2026.Model.Actors;
using Graphics2026.Model.Rays;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Attachments.CameraControls
{
    internal abstract class Camera : ActorAttachment
    {
        public static Camera? main;
        public static Camera? current;
        protected Matrix4 projectionMatrix;

        protected float nearDepth = 0.1f;
        protected float farDepth = 1000f;

        protected Camera(Actor actor, float nearDepth, float farDepth) : base(actor) {
            this.nearDepth = nearDepth;
            this.farDepth = farDepth;
        }


        public virtual void SetNearDepth(float nearDepth)
        {
            this.nearDepth = nearDepth;
        }
        public virtual void SetFarDepth(float farDepth)
        {
            this.farDepth = farDepth;
        }

        /// <summary>
        /// Returns the mouse position projected to the nearplane (without applying the camera transform)
        /// </summary>
        /// <returns></returns>
        public abstract Ray MouseRay();
        public virtual Ray MouseRayInWorld()
        {
            Vector3 start = actor.transform.localPosition;
            Vector3 direction = MouseRay().direction.X * transform.Right() +
                MouseRay().direction.Y * transform.Up() + transform.Forward();

            return new Ray(start, direction);
        }

        public void SetAsMain()
        {
            main = this;
        }

        public void SetAsCurrent()
        {
            current = this;
        }

        public Matrix4 GetProjection() => projectionMatrix;
        public Matrix4 GetCameraMatrix() => actor.transform.LocalPosition().Inverted() * actor.transform.LocalRotation();
    }
}
