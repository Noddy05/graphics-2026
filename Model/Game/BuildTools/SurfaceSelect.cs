using Graphics2026.Model.Actors.Gizmos;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.Rays;
using Graphics2026.Model.SceneManagement;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Game.BuildTools
{
    internal static class SurfaceSelect
    {
        public static Surface? RaycastSelectSurface(out Vector3 point, out float rayLength)
            => RaycastFindSurface(GetMouseRay(), out point, out rayLength);

        public static Surface? RaycastFindSurface(Ray ray, out Vector3 point, out float rayLength)
        {
            Surface? selectedSurface = null;

            point = Vector3.Zero;
            rayLength = float.PositiveInfinity;

            foreach (Surface surface in SceneManager.CurrentScene().surfaces)
            {
                if (!PointOnSurface(ray, surface, out Vector3 p, out float length))
                    continue;

                if (length < rayLength)
                {
                    rayLength = length;
                    selectedSurface = surface;
                    point = p;
                }
            }

            return selectedSurface;
        }

        public static bool SelectPoint(Surface surface, out Vector3 point, out float rayLength)
            => PointOnSurface(GetMouseRay(), surface, out point, out rayLength);

        public static Ray GetMouseRay()
        {
            if (Camera.current == null)
                throw new Exception("No camera!");

            return Camera.current.MouseRayInWorld();
        }

        public static bool PointOnPlane(Ray ray, Surface surface, out Vector3 point, out float rayLength)
        {
            point = Vector3.Zero;
            rayLength = float.PositiveInfinity;

            Vector3 pointInPlane = surface.transform.WorldPosition();
            Vector3 planeNormal = surface.transform.Up();
            float denominator = Vector3.Dot(ray.direction, planeNormal);
            if (denominator == 0)
                return false;

            rayLength = Vector3.Dot(pointInPlane - ray.origin, planeNormal) / denominator;
            point = ray.origin + rayLength * ray.direction;

            return true;
        }

        public static bool PointOnSurface(Ray ray, Surface surface, out Vector3 point, out float rayLength)
        {
            if (!PointOnPlane(ray, surface, out point, out rayLength))
            {
                return false;
            }

            //Check if outside 
            if (!surface.IsPointContained(surface.PointToGridSpace(point)))
            {
                return false;
            }

            if (rayLength < 0)
            {
                rayLength = float.PositiveInfinity;
                return false;
            }

            return true;
        }
    }
}
