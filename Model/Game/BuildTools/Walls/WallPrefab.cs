using Graphics2026.Model.Actors;

namespace Graphics2026.Model.Game.BuildTools.Walls
{
    internal class WallPrefab : Actor
    {
        private Wall? wallToCutout;
        public float thickness;

        private List<IRenderable> cutouts = new();

        public WallPrefab(Wall wallToCutout) : this($"New Wall ({actorCounter})", wallToCutout) { }
        public WallPrefab() : this($"New Wall ({actorCounter})") { }
        public WallPrefab(string name) : base(name) { }
        public WallPrefab(string name, Wall wallToCutout) : base(name) { 
            this.wallToCutout = wallToCutout;
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
