
using Graphics2026.Model.Actors;

namespace Graphics2026.Shopping
{
    internal static class AllFloors
    {
        private static List<Prefab> floors = new();

        static AllFloors()
        {
            floors.Add(new Prefab("Tile", new Actor(), 15));
        }

        public static Prefab GetFloor(int index) => floors[index - 1];
    }
}
