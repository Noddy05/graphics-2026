using Graphics2026.Model.SceneManagement;
using Graphics2026.View;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Actors.Gizmos
{
    public enum SurfaceType
    {
        Wall,
        Floor
    }

    internal class Surface : Actor
    {
        protected Vector2 size;
        protected List<SurfaceType> surfaceType = new();

        public Surface() : base() { 
            SceneManager.CurrentScene().surfaces.Add(this);
        }
        public Surface(string name) : base(name) { 
            SceneManager.CurrentScene().surfaces.Add(this);
        }
        public Surface(Vector2 size) : this()
        {
            this.size = size;
        }
        public Surface(string name, Vector2 size) : this(size) 
        {
            this.name = name;
        }

        public void SetType(SurfaceType surfaceType)
        {
            if (IsType(surfaceType))
                return;

            this.surfaceType.Add(surfaceType);
        }
        public void RemoveType(SurfaceType surfaceType)
        {
            this.surfaceType.Remove(surfaceType);
        }

        public bool IsType(SurfaceType surfaceType)
            => this.surfaceType.Contains(surfaceType);

        public List<SurfaceType> GetTypes() => surfaceType;

        public virtual Vector2 GetSize() => size;
        public virtual void SetSize(Vector2 size)
        {
            this.size = size;
        }

        public override void Render()
        {
            WireRenderer.DrawInFront(true);
            WireRenderer.SetColor(Color4.White);
            WireRenderer.DrawSquare(Matrix4.CreateScale(size.X, 1, size.Y) * transform.WorldTransform());
            WireRenderer.DrawInFront(false);
        }

        public bool IsPointContained(Vector2 pointInGridSpace)
        {
            return 2 * MathF.Abs(pointInGridSpace.X) <= size.X
                && 2 * MathF.Abs(pointInGridSpace.Y) <= size.Y;
        }

        private Vector3 Right() => transform.OrientVectorWithoutNormalization(Vector3.UnitX);
        private Vector3 Forward() => transform.OrientVectorWithoutNormalization(Vector3.UnitZ);

        public Vector2 PointToGridSpace(Vector3 point)
        {
            float x = Vector3.Dot(point - transform.WorldPosition(), Right());
            float y = Vector3.Dot(point - transform.WorldPosition(), Forward());
            return new Vector2(x, y);
        }

        public Vector3 PointToWorldSpace(Vector2 point)
        {
            return transform.WorldPosition() + Right() * point.X + Forward() * point.Y;
        }

        public Vector3 RoundOffset3D() => (transform.Right() *
            ((1 + size.X) % 2 + 2 * MathF.Ceiling(transform.WorldPosition().X)) -
            transform.Forward() * ((1 + size.Y) % 2 + 2 * MathF.Ceiling(transform.WorldPosition().Z))) / 2f;
    }
}
