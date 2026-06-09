using Graphics2026.Controller;
using Graphics2026.Model.Actors;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Graphics2026.Model.Attachments.CameraControls
{
    internal class MovementControls : ActorAttachment
    {
        protected float moveSpeed = 5f;
        protected float sprintSpeed = 20f;

        public MovementControls(Actor actor) : base(actor)
        {

        }

        protected override void Update(float deltaTime)
        {
            float stepSize = deltaTime * moveSpeed;
            if (Input.GetKey(Keys.LeftShift))
                stepSize *= sprintSpeed / moveSpeed;

            Matrix4 rotation = transform.LocalRotation();
            Vector3 right = (rotation * new Vector4(1, 0, 0, 0)).Xyz;
            Vector3 up = (rotation * new Vector4(0, 1, 0, 0)).Xyz;
            Vector3 forward = (rotation * new Vector4(0, 0, -1, 0)).Xyz;

            Vector2 direction = Input.Direction();
            transform.localPosition += (right * direction.X +
                forward * direction.Y) * stepSize;

            int y = 0;
            if (Input.GetKey(Keys.E))
                y++;
            if (Input.GetKey(Keys.Q))
                y--;

            transform.localPosition += up * y * stepSize;
        }
    }
}
