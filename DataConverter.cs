using OpenTK.Mathematics;

namespace Graphics2026
{
    internal static class DataConverter
    {
        public static float[] ToFloatArray(params object[] toConvert)
        {
            List<float> data = new();
            foreach (object obj in toConvert) {
                if (obj is float) data.Add((float)obj);
                else if (obj is Vector2) data.AddRange(ToFloatArray((Vector2)obj));
                else if (obj is Vector3) data.AddRange(ToFloatArray((Vector3)obj));
                else if (obj is Vector4) data.AddRange(ToFloatArray((Vector4)obj));
                else if (obj is Color4) data.AddRange(ToFloatArray((Color4)obj));
                else if (obj is Matrix4) data.AddRange(ToFloatArray((Matrix4)obj));
            }


            return data.ToArray();
        }

        public static float[] ToFloatArray(Vector2 v) => [
            v.X, v.Y
        ];
        public static float[] ToFloatArray(Vector3 v) => [
            v.X, v.Y, v.Z
        ];
        public static float[] ToFloatArray(Vector4 v) => [
            v.X, v.Y, v.Z, v.W
        ];
        public static float[] ToFloatArray(Color4 c) => [
            c.R, c.G, c.B, c.A
        ];

        public static float[] ToFloatArray(Matrix4 m) => [
            m.M11, m.M12, m.M13, m.M14,
            m.M21, m.M22, m.M23, m.M24,
            m.M31, m.M32, m.M33, m.M34,
            m.M41, m.M42, m.M43, m.M44,
        ];

        public static float[] ToFloatArray(Matrix4[] m)
        {
            float[] output = new float[16 * m.Length];
            for (int i = 0; i < m.Length; i++)
            {
                for (int x = 0; x < 4; x++)
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
