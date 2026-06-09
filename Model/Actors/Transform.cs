
using OpenTK.Mathematics;

namespace Graphics2026.Model.Actors
{
    internal class Transform
    {
        public Vector3 localPosition;
        public Vector3 localRotation;
        public Vector3 localSize = Vector3.One;

        private Transform? parent;
        private List<Transform> children = new List<Transform>();

        private Matrix4? cachedWorldTransform;
        private Vector3 cachedPosition;
        private Vector3 cachedRotation;
        private Vector3 cachedSize;

        private Matrix4 matrixMod = Matrix4.Identity;
        private readonly IRenderable renderable;

        public Transform(IRenderable renderable)
        {
            this.renderable = renderable;
            cachedPosition = localPosition;
            cachedRotation = localRotation;
            cachedSize = localSize;
        }

        public void SetParent(Transform? parent)
        {
            if (this.parent != null)
                this.parent.children.Remove(this);

            Matrix4 previousWorldTransform = WorldTransform();
            this.parent = parent;

            matrixMod = previousWorldTransform * (LocalTransformWithoutMod() * ParentTransform()).Inverted();

            if (parent == null)
                return;

            parent.children.Add(this);
        }

        public Transform? GetChild(int i)
        {
            if(i < children.Count)
                return children[i];

            return null;
        }
        public Transform? GetParent() => parent;
        public IRenderable GetRenderable() => renderable;

        public void ClearCache() => cachedWorldTransform = null;
        public Matrix4 LocalSize() => Matrix4.CreateScale(localSize);
        public Matrix4 LocalPosition() => Matrix4.CreateTranslation(localPosition);
        public Matrix4 LocalRotation() => Matrix4.CreateFromQuaternion(
            Quaternion.FromEulerAngles(localRotation * MathF.PI / 180f));
        public Matrix4 InvertedRotation() => LocalRotation().Inverted();
        public Matrix4 LocalTransform() => matrixMod * LocalSize() * LocalRotation() * LocalPosition();
        public Matrix4 LocalTransformWithoutMod() => LocalSize() * LocalRotation() * LocalPosition();

        /* over-engineered
        public Matrix4 WorldTransform()
        {
            //Check if cached
            if (cachedWorldTransform != null)
            {
                if(cachedPosition == position && cachedRotation == rotation && cachedSize == size)
                    return (Matrix4)cachedWorldTransform;

                foreach(Transform child in children)
                    child.ClearCache();

                cachedPosition = position;
                cachedRotation = rotation;
                cachedSize = size;
            }

            Matrix4 finalTransform = LocalTransform();

            if (parent != null)
                finalTransform *= parent.WorldTransform();

            cachedWorldTransform = finalTransform;
            return finalTransform;
        }*/
        public Vector3 WorldPosition() => (new Vector4(0, 0, 0, 1) * WorldTransform()).Xyz;

        public Matrix4 WorldTransform()
        {
            Matrix4 finalTransform = LocalTransform();

            if (parent != null)
                finalTransform = finalTransform * parent.WorldTransform();

            return finalTransform;
        }
        public Matrix4 ParentTransform()
        {
            if (parent != null)
                return parent.WorldTransform();

            return Matrix4.Identity;
        }

        /*
        public Vector3 Forward() => (WorldTransform() * new Vector4(0, 0, -1, 1) -
            WorldTransform() * new Vector4(0, 0, 0, 1)).Xyz;
        public Vector3 Backward() => -Forward();
        public Vector3 Up() => (WorldTransform() * new Vector4(0, 1, 0, 1) -
            WorldTransform() * new Vector4(0, 0, 0, 1)).Xyz;
        public Vector3 Down() => -Up();
        public Vector3 Right() => (WorldTransform() * new Vector4(1, 0, 0, 1) -
            WorldTransform() * new Vector4(1, 0, 0, 1)).Xyz;
        public Vector3 Left() => -Right();
        */

        public Vector3 Forward() => OrientVector(-Vector3.UnitZ);
        public Vector3 Backward() => -Forward();
        public Vector3 Up() => OrientVector(Vector3.UnitY);
        public Vector3 Down() => -Up();
        public Vector3 Right() => OrientVector(Vector3.UnitX);
        public Vector3 Left() => -Right();
        public Vector3 OrientVector(Vector3 vec) => Vector3.TransformNormal(vec, WorldTransform()).Normalized();
        public Vector3 OrientVectorWithoutNormalization(Vector3 vec) 
            => Vector3.TransformNormal(vec, WorldTransform());
    }
}
