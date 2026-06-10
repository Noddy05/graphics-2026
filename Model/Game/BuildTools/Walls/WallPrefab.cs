using Graphics2026.Model.Actors;
using Graphics2026.Model.Game.BuildTools.NavigationObstacles;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools.Walls
{
    internal class WallPrefab : Actor
    {
        private Wall? wallToCutout;
        private Vector2 A, B;
        public float thickness;

        private readonly WallNavObstacle obstacle = new();

        private List<IRenderable> cutouts = new();

        public WallPrefab(Vector2 A, Vector2 B, Wall wallToCutout) : this(A, B, $"New Wall ({actorCounter})", wallToCutout) { }
        public WallPrefab(Vector2 A, Vector2 B) : this(A, B, $"New Wall ({actorCounter})") { }
        public WallPrefab(Vector2 A, Vector2 B, string name) : base(name) { 
            this.A = A;
            this.B = B;
            UpdateNavObstacle();
        }
        public WallPrefab(Vector2 A, Vector2 B, string name, Wall wallToCutout) : base(name) { 
            this.wallToCutout = wallToCutout;
            this.A = A;
            this.B = B;
            UpdateNavObstacle();
        }

        public void UpdateNavObstacle()
        {
            obstacle.SetA(A);
            obstacle.SetB(B);
            obstacle.SetThickness(thickness);
        }

        public void AddCutout(IRenderable cutout)
        {
            if (wallToCutout != null)
                wallToCutout.AddCutout(cutout);
        }
        public void SetTemporaryCutout(List<IRenderable> cutouts)
        {
            if (wallToCutout != null)
                wallToCutout.SetTemporaryCutout(cutouts);
        }

        public void SetWallToCutout(Wall wall)
        {
            wallToCutout = wall;
            wallToCutout!.ClearCutouts();
            foreach (IRenderable cutout in cutouts)
                wallToCutout.AddCutout(cutout);
        }
    }
}
