using OpenTK.Mathematics;

namespace Graphics2026
{
    internal static class Helper
    {
        public static Vector3 ApplyMatrix(Vector3 vec, Matrix4 mat) => (new Vector4(vec, 1) * mat).Xyz;
        public static Vector3 Flatten(Vector3 v) => new Vector3(v.X, 0, v.Z);
    }
}
