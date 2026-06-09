using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Game.BuildTools.Deprecated;
using Graphics2026.Model.SceneManagement;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools
{
    internal class Floor : Grid
    {
        private Tile[][] floorTiles = new Tile[0][];

        public Floor() { 
            SetType(SurfaceType.Floor);
        }
        public Floor(string name) : base(name) { }
        public Floor(Vector2i size)
        {
            SetSize(size);
        }
        public Floor(string name, Vector2i size) : base(name)
        {
            SetSize(size);
        }

        private void GenFloorArray()
        {
            foreach (Tile[] tileRow in floorTiles)
            {
                foreach (Tile tile in tileRow)
                {
                    if (tile == null)
                        continue;

                    SceneManager.CurrentScene().renderables.Remove(tile);
                }
            }

            floorTiles = new Tile[GridSize().X][];
            for (int x = 0; x < floorTiles.Length; x++)
            {
                floorTiles[x] = new Tile[GridSize().Y];
            }
        }

        public override void SetSize(Vector2i size)
        {
            base.SetSize(size);
            GenFloorArray();
        }

        public bool PlaceFloorTile(Vector2i square)
        {
            int x = square.X + GridSize().X / 2 - (int)transform.localPosition.X,
                y = square.Y + GridSize().Y / 2 - (int)transform.localPosition.Z;

            if (floorTiles[x][y] != null)
                return false;

            Tile placedTile = new Tile();
            SceneManager.CurrentScene().renderables.Add(placedTile);
            placedTile.transform.localRotation = transform.localRotation;
            placedTile.transform.localPosition = MouseGridSelection.GridSpaceToWorldSpace(square, this);
            //placedTile.transform.position = new Vector3(square.X, 0, square.Y);
            floorTiles[x][y] = placedTile;
            placedTile.transform.SetParent(transform);

            return true;
        }
    }
}
