using OpenTK.Mathematics;

namespace Graphics2026
{
    internal static class Helper
    {
        public static Vector3 ApplyMatrix(Vector3 vec, Matrix4 mat) => (new Vector4(vec, 1) * mat).Xyz;
        public static Vector3 Flatten(Vector3 v) => new Vector3(v.X, 0, v.Z);
        
        public static float[] Matrix4ToFloatArray(Matrix4 m) => [
                m.M11, m.M21, m.M31, m.M41,
                m.M12, m.M22, m.M32, m.M42,
                m.M13, m.M23, m.M33, m.M43,
                m.M14, m.M24, m.M34, m.M44,
        ];

        public static float[] Matrix4ToFloatArray(Matrix4[] m)
        {
            float[] output = new float[16 * m.Length];
            for(int i = 0; i < m.Length; i++)
            {
                for(int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        output[16 * i + 4 * x + y] = m[i][x, y];
                    }
                }
            }
            return output;
        }
    }
}
