using Graphics2026.Model.Actors;
using Graphics2026.Model.VertexData;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Mesh
{
    internal static class MeshGenerator
    {
        private static Point[] RegularPolygonVertices(int numVertices)
        {
            if (numVertices < 3)
                throw new Exception("numVertices must >= 3");

            Point[] points = new Point[numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                float angle = i / (float)numVertices * MathF.Tau;
                points[i] = new Point(MathF.Cos(angle), 0, MathF.Sin(angle));
            }

            return points;
        }
        private static uint[] RegularFilledPolygonIndices(int numVertices)
        {
            if (numVertices < 3)
                throw new Exception("numVertices must >= 3");

            uint[] indices = new uint[3 * (numVertices - 2)];

            for (uint i = 0; i < numVertices - 2; i++)
            {
                indices[3 * i] = 0;
                indices[3 * i + 1] = i + 2;
                indices[3 * i + 2] = i + 1;
            }

            return indices;
        }
        public static Mesh<Point> RegularPolygon(int numVertices)
        {
            if (numVertices < 3)
                throw new Exception("numVertices must >= 3");

            Point[] points = RegularPolygonVertices(numVertices);
            uint[] indices = new uint[2 * numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                indices[2 * i] = (uint)i;
                indices[2 * i + 1] = (uint)((i + 1) % numVertices);
            }

            return new Mesh<Point>(points, indices);
        }
        public static Mesh<Vertex> RegularFilledPolygon(int numVertices)
        {
            if (numVertices < 3)
                throw new Exception("numVertices must >= 3");

            Point[] points = RegularPolygonVertices(numVertices);
            Vertex[] vertices = new Vertex[numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                Vector2 textureCoordinate = new Vector2(points[i].x, points[i].z);
                vertices[i] = new Vertex(new Vector3(textureCoordinate.X, 0, textureCoordinate.Y), Vector3.UnitY,
                    (textureCoordinate + Vector2.One) / 2);
            }

            return new Mesh<Vertex>(vertices, RegularFilledPolygonIndices(numVertices));
        }
        public static Mesh<Vertex> Quad()
        {
            //   0------1
            //   |      |
            //   |      |
            //   3------2
            Vertex[] vertices =
            {
                new Vertex(new Vector3(-0.5f, 0,  0.5f), Vector3.UnitY, new Vector2(0, 1)),
                new Vertex(new Vector3( 0.5f, 0,  0.5f), Vector3.UnitY, new Vector2(1, 1)),
                new Vertex(new Vector3( 0.5f, 0, -0.5f), Vector3.UnitY, new Vector2(1, 0)),
                new Vertex(new Vector3(-0.5f, 0, -0.5f), Vector3.UnitY, new Vector2(0, 0)),
            };
            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };

            return new Mesh<Vertex>(vertices, indices);
        }


        public static Mesh<Point> SimpleQuad()
        {
            //   0------1
            //   |      |
            //   |      |
            //   3------2
            Point[] points =
            {
                new Point(-0.5f, 0,  0.5f),
                new Point( 0.5f, 0,  0.5f),
                new Point( 0.5f, 0, -0.5f),
                new Point(-0.5f, 0, -0.5f),
            };
            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };

            return new Mesh<Point>(points, indices);
        }

        /// <summary>
        /// A simple quad with 2-sided faces
        /// </summary>
        /// <returns></returns>
        public static Mesh<Point> SimpleBillboard()
        {
            //   0------1
            //   |      |
            //   |      |
            //   3------2
            Point[] points =
            {
                new Point(-0.5f, 0,  0.5f),
                new Point( 0.5f, 0,  0.5f),
                new Point( 0.5f, 0, -0.5f),
                new Point(-0.5f, 0, -0.5f),
            };
            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3,

                0, 3, 1,
                1, 3, 2
            };

            return new Mesh<Point>(points, indices);
        }
        public static Mesh<Point> SimpleCube()
        {
            //    7--------6
            //   /|       /|
            //  3-+------2 |
            //  | |      | |
            //  | 4------+-5
            //  |/       |/
            //  0--------1

            Mesh<Point> quad = SimpleQuad();
            quad.BakeTransformation(Matrix4.CreateTranslation(0, 0.5f, 0));

            Mesh<Point> bottomPlane = quad.Clone();
            bottomPlane.BakeTransformation(Matrix4.CreateRotationX(MathF.PI));

            Mesh<Point> rightPlane = quad.Clone();
            rightPlane.BakeTransformation(Matrix4.CreateRotationZ(MathF.PI / 2));

            Mesh<Point> leftPlane = quad.Clone();
            leftPlane.BakeTransformation(Matrix4.CreateRotationZ(-MathF.PI / 2));

            Mesh<Point> frontPlane = quad.Clone();
            frontPlane.BakeTransformation(Matrix4.CreateRotationX(MathF.PI / 2));

            Mesh<Point> backPlane = quad.Clone();
            backPlane.BakeTransformation(Matrix4.CreateRotationX(-MathF.PI / 2));

            return Mesh<Point>.Combine(quad, bottomPlane, rightPlane, leftPlane, frontPlane, backPlane);
        }
        public static Mesh<Point> Cutout()
        {
            //    7--------6
            //   /|       /|
            //  3-+------2 |
            //  | |      | |
            //  | 4------+-5
            //  |/       |/
            //  0--------1

            Mesh<Point> quad = SimpleQuad();
            quad.BakeTransformation(Matrix4.CreateTranslation(0, 0.5f, 0) * Matrix4.CreateRotationX(MathF.PI / 2));

            Mesh<Point> bottomPlane = quad.Clone();
            bottomPlane.BakeTransformation(Matrix4.CreateRotationX(MathF.PI));

            return Mesh<Point>.Combine(quad, bottomPlane);
        }

        public static Mesh<Vertex> Cube()
        {
            //    7--------6
            //   /|       /|
            //  3-+------2 |
            //  | |      | |
            //  | 4------+-5
            //  |/       |/
            //  0--------1

            Mesh<Vertex> quad = Quad();
            quad.BakeTransformation(Matrix4.CreateScale(2) * Matrix4.CreateTranslation(0, 1f, 0));

            Mesh<Vertex> bottomPlane = quad.Clone();
            bottomPlane.BakeTransformation(Matrix4.CreateRotationX(MathF.PI));

            Mesh<Vertex> rightPlane = quad.Clone();
            rightPlane.BakeTransformation(Matrix4.CreateRotationZ(MathF.PI / 2));

            Mesh<Vertex> leftPlane = quad.Clone();
            leftPlane.BakeTransformation(Matrix4.CreateRotationZ(-MathF.PI / 2));

            Mesh<Vertex> frontPlane = quad.Clone();
            frontPlane.BakeTransformation(Matrix4.CreateRotationX(MathF.PI / 2));

            Mesh<Vertex> backPlane = quad.Clone();
            backPlane.BakeTransformation(Matrix4.CreateRotationX(-MathF.PI / 2));

            return Mesh<Vertex>.Combine(quad, bottomPlane, rightPlane, leftPlane, frontPlane, backPlane);
        }

        public static Mesh<Vertex> Cylinder(int numVertices)
        {
            if (numVertices < 3)
                throw new Exception("numVertices must >= 3");

            Point[] circlePoints = RegularPolygonVertices(numVertices);
            Vertex[] vertices = new Vertex[4 * numVertices];
            for (int i = 0; i < numVertices; i++)
            {
                vertices[i] = new Vertex(circlePoints[i]);
                vertices[i].position -= Vector3.UnitY;
                vertices[i].normal *= -1;
            }
            for (int i = 0; i < numVertices; i++)
            {
                vertices[numVertices + i] = new Vertex(circlePoints[i]);
                vertices[numVertices + i].position += Vector3.UnitY;
            }
            for (int i = 0; i < numVertices; i++)
            {
                vertices[2 * numVertices + i] = new Vertex(circlePoints[i]);
                vertices[2 * numVertices + i].normal = vertices[2 * numVertices + i].position;
                vertices[2 * numVertices + i].position -= Vector3.UnitY;
            }
            for (int i = 0; i < numVertices; i++)
            {
                vertices[3 * numVertices + i] = new Vertex(circlePoints[i]);
                vertices[3 * numVertices + i].normal = vertices[3 * numVertices + i].position;
                vertices[3 * numVertices + i].position += Vector3.UnitY;
            }

            uint[] circleIndices = RegularFilledPolygonIndices(numVertices);
            uint[] indices = new uint[12 * (numVertices - 1)];

            for (int i = 0; i < circleIndices.Length; i += 3)
            {
                indices[i] = circleIndices[i];
                indices[i + 1] = circleIndices[i + 2];
                indices[i + 2] = circleIndices[i + 1];
            }
            for (int i = 0; i < circleIndices.Length; i += 3)
            {
                indices[i + circleIndices.Length] = circleIndices[i] + (uint)numVertices;
                indices[i + circleIndices.Length + 1] = circleIndices[i + 1] + (uint)numVertices;
                indices[i + circleIndices.Length + 2] = circleIndices[i + 2] + (uint)numVertices;
            }
            for (uint i = 0; i < numVertices; i++)
            {
                indices[3 * i     + circleIndices.Length * 2] = 3 * (uint)numVertices + i;
                indices[3 * i + 1 + circleIndices.Length * 2] = 2 * (uint)numVertices + (i + 1) % (uint)numVertices;
                indices[3 * i + 2 + circleIndices.Length * 2] = 2 * (uint)numVertices + i;
            }
            for (uint i = 0; i < numVertices; i++)
            {
                indices[3 * i     + circleIndices.Length * 2 
                    + 3 * numVertices] = 3 * (uint)numVertices + i;
                indices[3 * i + 1 + circleIndices.Length * 2 
                    + 3 * numVertices] = 3 * (uint)numVertices + (i + 1) % (uint)numVertices;
                indices[3 * i + 2 + circleIndices.Length * 2 
                    + 3 * numVertices] = 2 * (uint)numVertices + (i + 1) % (uint)numVertices;
            }

            return new Mesh<Vertex>(vertices, indices);
        }
    }
}