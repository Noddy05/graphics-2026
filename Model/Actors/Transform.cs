
using OpenTK.Mathematics;

namespace Graphics2026.Model.Actors
{
    internal class Transform : Parent
    {
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localSize = Vector3.One;

        private IParent? parent;
        private List<Transform> children = new List<Transform>();

        private Matrix4 matrixMod = Matrix4.Identity;
        private readonly IRenderable renderable;

        public Transform(IRenderable renderable)
        {
            this.renderable = renderable;
        }

        public void CopyTransform(Transform transform)
        {
            localPosition = transform.localPosition;
            localRotation = transform.localRotation;
            localSize = transform.localSize;

            matrixMod = transform.matrixMod;
        }

        public void SetMatrixMod(Matrix4 mod) => matrixMod = mod;
        public Matrix4 GetMatrixMod() => matrixMod;

        public void SetParent(IParent? newParent)
        {
            if (newParent != null && newParent is Transform && IsDescendentOfTransform((Transform)newParent))
            {
                Console.WriteLine("Cannot set parent: parent is a child of transform");
                return;
            }

            if (parent != null)
            {
                parent.GetChildren().Remove(this);
            }

            Matrix4 previousWorldTransform = WorldTransform();
            parent = newParent;

            matrixMod = previousWorldTransform * (LocalTransformWithoutMod() * ParentTransform()).Inverted();

            if (newParent == null)
                return;

            newParent.GetChildren().Add(this);
        }

        public IParent? GetParent() => parent;
        public IRenderable GetRenderable() => renderable;

        #region Matrices
        public Matrix4 LocalSize() => Matrix4.CreateScale(localSize);
        public Matrix4 LocalPosition() => Matrix4.CreateTranslation(localPosition);
        public Matrix4 LocalRotation() => Matrix4.CreateFromQuaternion(
            Quaternion.FromEulerAngles(localRotation * MathF.PI / 180f));
        public Matrix4 InvertedRotation() => LocalRotation().Inverted();
        public Matrix4 LocalTransform() => matrixMod * LocalSize() * LocalRotation() * LocalPosition();
        public Matrix4 LocalTransformWithoutMod() => LocalSize() * LocalRotation() * LocalPosition();

        public Vector3 WorldPosition() => (new Vector4(0, 0, 0, 1) * WorldTransform()).Xyz;

        public Matrix4 WorldTransform()
        {
            Matrix4 finalTransform = LocalTransform();

            if (parent != null && parent is Transform)
                finalTransform = finalTransform * ((Transform)parent).WorldTransform();

            return finalTransform;
        }
        public Matrix4 ParentTransform()
        {
            if (parent != null && parent is Transform)
                return ((Transform)parent).WorldTransform();

            return Matrix4.Identity;
        }

        public Vector3 Forward() => OrientVector(-Vector3.UnitZ);
        public Vector3 Backward() => -Forward();
        public Vector3 Up() => OrientVector(Vector3.UnitY);
        public Vector3 Down() => -Up();
        public Vector3 Right() => OrientVector(Vector3.UnitX);
        public Vector3 Left() => -Right();
        public Vector3 OrientVector(Vector3 vec) => Vector3.TransformNormal(vec, WorldTransform()).Normalized();
        public Vector3 OrientVectorWithoutNormalization(Vector3 vec) 
            => Vector3.TransformNormal(vec, WorldTransform());
        #endregion
    }
}
