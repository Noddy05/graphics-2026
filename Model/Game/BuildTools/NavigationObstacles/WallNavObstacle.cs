
using Graphics2026.Model.Game.BuildTools.Interior;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools.NavigationObstacles
{
    internal class WallNavObstacle : NavigationObstacle
    {
        public Vector2 A, B;
        public float thickness = 0.1f;

        public WallNavObstacle() : base() { }
        public WallNavObstacle(Vector2 A, Vector2 B, float thickness) : base()
        {
            this.A = A; 
            this.B = B;
            this.thickness = thickness;
        }

        public void SetA(Vector2 A) => this.A = A;
        public void SetB(Vector2 B) => this.B = B;
        public void SetThickness(float thickness) => this.thickness = thickness;

        public override float DistanceToObject(Vector2 position)
        {
            Vector2 pa = position - A, ba = B - A;
            if (ba.LengthSquared == 0)
                return pa.Length - thickness;

            float h = MathF.Min(MathF.Max(Vector2.Dot(pa, ba) / ba.LengthSquared, 0), 1);
            return (pa - ba * h).Length - thickness;
        }
    }
}
