using Graphics2026.Model.Actors;
using Graphics2026.Model.Attachments.CameraControls;
using Graphics2026.Model.BufferObjects;
using Graphics2026.Model.Mesh;
using Graphics2026.Model.VertexData;
using Graphics2026.View.Shading.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.View
{
    internal static class WireRenderer
    {
        private static VBO<Point> vbo;
        private static VAO vao;

        private static DefaultShader defaultShader;
        private static GizmoShader gizmoShader;
        private static Mesh<Point> circle = MeshGenerator.RegularPolygon(100);
        private static Mesh<Point> square = MeshGenerator.RegularPolygon(4);
        private static bool drawInFront = false;

        static WireRenderer() {
            Point[] vertices =
            {
                new Point(0f, 0.5f, 0),
                new Point(-0.5f, 0f, 0)
            };
            vbo = new VBO<Point>(vertices, BufferUsageHint.StreamCopy);
            vao = new VAO(vbo);

            vao.BindAttribPointer(0, 3, VertexAttribPointerType.Float, 0);
            square.BakeTransformation(Matrix4.CreateScale(MathF.Sqrt(2) / 2) * Matrix4.CreateRotationY(MathF.PI / 4));

            defaultShader = new DefaultShader(Color4.White);
            gizmoShader = new GizmoShader(Color4.White);
        }

        public static void SetColor(Color4 color)
        {
            defaultShader.color = color;
            gizmoShader.color = color;
        }
        public static void DrawInFront(bool doDrawInFront) => drawInFront = doDrawInFront;
        public static void DrawRay(Vector3 start, Vector3 direction) => DrawLine(start, start + direction, Matrix4.Identity);
        public static void DrawLine(Vector3 A, Vector3 B) => DrawLine(A, B, Matrix4.Identity);
        public static void DrawLine(Vector3 A, Vector3 B, Matrix4 transformationMatrix)
        {
            Point[] vertices =
            {
                new Point(A.X, A.Y, A.Z),
                new Point(B.X, B.Y, B.Z)
            };
            vbo.UpdateData(vertices);

            ApplyShader(transformationMatrix);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        }
        public static void DrawPointMesh(Mesh<Point> mesh, Matrix4 transformationMatrix)
        {
            mesh.PrepareForRendering();
            ApplyShader(transformationMatrix);

            GL.DrawElements(PrimitiveType.Lines, mesh.NumIndices(), DrawElementsType.UnsignedInt, 0);
        }
        public static void DrawSphere(Vector3 position, float radius)
        {
            Matrix4 matrix = Matrix4.CreateScale(radius) * Matrix4.CreateTranslation(position);
            DrawPointMesh(circle, matrix);

            matrix = Matrix4.CreateScale(radius) * Matrix4.CreateRotationX(MathF.PI / 2) * Matrix4.CreateTranslation(position);
            DrawPointMesh(circle, matrix);

            matrix = Matrix4.CreateScale(radius) * Matrix4.CreateRotationZ(MathF.PI / 2) * Matrix4.CreateTranslation(position);
            DrawPointMesh(circle, matrix);
        }
        public static void DrawCircle(Vector3 position, float radius)
        {
            Matrix4 matrix = Matrix4.CreateScale(radius) * Matrix4.CreateTranslation(position);
            DrawPointMesh(circle, matrix);
        }
        public static void DrawSquare(Matrix4 transformation)
        {
            DrawPointMesh(square, transformation);
        }
        public static void DrawSquare(Vector3 position, Vector2 size)
        {
            Matrix4 matrix = Matrix4.CreateScale(size.X, 1, size.Y) * Matrix4.CreateTranslation(position);
            DrawSquare(matrix);
        }

        public static void DrawTransform(Transform transform, float size = 1f)
        {
            SetColor(Color4.Red);
            DrawRay(transform.WorldPosition(), transform.Right() * size);
            SetColor(Color4.Lime);
            DrawRay(transform.WorldPosition(), transform.Up() * size);
            SetColor(Color4.Blue);
            DrawRay(transform.WorldPosition(), transform.Forward() * size);
            SetColor(Color4.White);
        }

        private static void ApplyShader(Matrix4 transformationMatrix)
        {
            if (drawInFront)
            {
                ApplyShader(gizmoShader, transformationMatrix);
            }
            else
            {
                ApplyShader(defaultShader, transformationMatrix);
            }
        }
        private static void ApplyShader(PhysicalShader shader, Matrix4 transformationMatrix)
        {
            shader.UseProgram();
            shader.ApplyTransform(transformationMatrix);
        }
    }
}
