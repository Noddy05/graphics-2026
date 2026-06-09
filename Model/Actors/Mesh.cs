
using Graphics2026.Model.BufferObjects;
using Graphics2026.Model.VertexData;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.Actors
{
    internal class Mesh<T> where T : struct, IVertex
    {
        private VAO vao;
        private VBO<T> vbo;
        private EBO ebo;

        public Mesh() : this([], []) { }
        public Mesh(T[] vertices) : this(vertices, []) { }
        public Mesh(T[] vertices, uint[] indices) : this(vertices, indices, BufferUsageHint.StaticCopy) { }
        public Mesh(T[] vertices, uint[] indices, BufferUsageHint hint)
        {
            vbo = new VBO<T>(vertices, hint);
            vao = new VAO(vbo);
            ebo = new EBO(indices, hint);
            T.BindVAOAttribPointers(vao);
        }
        public Mesh(VBO<T> vbo, EBO ebo, BufferUsageHint hint)
        {
            this.vbo = vbo;
            vao = new VAO(vbo);
            this.ebo = ebo;
            T.BindVAOAttribPointers(vao);
        }

        public VAO GetVAO() => vao;
        public VBO<T> GetVBO() => vbo;
        public EBO GetEBO() => ebo;

        public int NumVertices() => vbo.GetLengthOfData();
        public int NumIndices() => ebo.GetLengthOfData();

        public void PrepareForRendering()
        {
            vao.Bind();
            ebo.Bind();
        }

        public void FlipFaces()
        {
            FlipIndices();
            FlipNormals();
        }
        public void FlipIndices()
        {
            uint[] oldIndices = ebo.GetData();
            for (int i = 0; i < oldIndices.Length; i += 3)
            {
                uint tmp = oldIndices[i + 1];
                oldIndices[i + 1] = oldIndices[i + 2];
                oldIndices[i + 2] = tmp;
            }

            ebo.UpdateData(oldIndices);
        }
        public void FlipNormals()
        {
            T[] vertices = vbo.GetData();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Flip();
            }

            vbo.UpdateData(vertices);
        }
        public void BakeTransformation(Matrix4 transformation)
        {
            T[] vertices = vbo.GetData();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].BakeTransformation(transformation);
            }

            vbo.UpdateData(vertices);
        }

        public Mesh<T> Clone()
        {
            T[] vertexData = Array.ConvertAll(vbo.GetData(), v => (T)v.Clone());
            return new Mesh<T>(vertexData, ebo.GetData());
        }

        public static Mesh<T> Combine(Mesh<T> a, Mesh<T> b)
        {
            int numVertices = a.NumVertices() + b.NumVertices();
            int numIndices = a.NumIndices() + b.NumIndices();

            T[] newVertices = new T[numVertices];
            uint[] newIndices = new uint[numIndices];

            T[] verticesA = a.GetVBO().GetData();
            T[] verticesB = b.GetVBO().GetData();
            for (int i = 0; i < verticesA.Length; i++)
            {
                newVertices[i] = verticesA[i];
            }
            for (int i = 0; i < verticesB.Length; i++)
            {
                newVertices[i + verticesA.Length] = verticesB[i];
            }

            uint[] indicesA = a.GetEBO().GetData();
            uint[] indicesB = b.GetEBO().GetData();
            for (int i = 0; i < indicesA.Length; i++)
            {
                newIndices[i] = indicesA[i];
            }
            for (int i = 0; i < indicesB.Length; i++)
            {
                newIndices[i + indicesA.Length] = indicesB[i] + (uint)verticesA.Length;
            }

            return new Mesh<T>(newVertices, newIndices);
        }
        public static Mesh<T> Combine(params Mesh<T>[] meshes)
        {
            int numVertices = 0;
            int numIndices = 0;
            T[][] vertices = new T[meshes.Length][];
            uint[][] indices = new uint[meshes.Length][];
            for(int i = 0; i < meshes.Length; i++)
            {
                numVertices += meshes[i].NumVertices();
                numIndices += meshes[i].NumIndices();
                vertices[i] = meshes[i].GetVBO().GetData();
                indices[i] = meshes[i].GetEBO().GetData();
            }

            T[] newVertices = new T[numVertices];
            uint[] newIndices = new uint[numIndices];

            int offsetVerts = 0;
            int offsetTris = 0;
            for (int i = 0; i < meshes.Length; i++)
            {
                int numVerts = meshes[i].NumVertices();
                int numTris = meshes[i].NumIndices();

                for (int j = 0; j < numVerts; j++)
                {
                    newVertices[offsetVerts + j] = vertices[i][j];
                }
                for (int j = 0; j < numTris; j++)
                {
                    newIndices[offsetTris + j] = indices[i][j] + (uint)offsetVerts;
                }

                offsetTris += numTris;
                offsetVerts += numVerts;
            }

            return new Mesh<T>(newVertices, newIndices);
        }
    }
}
