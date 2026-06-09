using Graphics2026.Model.Actors;
using Graphics2026.Model.VertexData;
using OpenTK.Mathematics;
using System.Globalization;

namespace Graphics2026.Model.MeshGeneration
{
    internal class MeshImport
    {
        public static Mesh<Vertex>[] OBJ(string filepath)
        {
            List<Mesh<Vertex>> meshes = new();
            string[] lines = File.ReadAllLines(filepath);

            int subOBJStart = -1;
            int pOffset = 0;
            int nOffset = 0;
            int tOffset = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line[0] == 'o')
                {
                    if(subOBJStart > 0)
                    {
                        Mesh<Vertex> nextMesh = SubOBJ(filepath, subOBJStart, i, pOffset, nOffset, tOffset,
                            out pOffset, out nOffset, out tOffset);
                        meshes.Add(nextMesh);
                    }
                    subOBJStart = i;
                }
            }
            meshes.Add(SubOBJ(filepath, subOBJStart, lines.Length, pOffset, nOffset, tOffset, out _, out _, out _));

            return meshes.ToArray();
        }

        private static Mesh<Vertex> SubOBJ(string filepath, int lineStartIndex, int lineEndIndex, int pOffset,
            int nOffset, int tOffset, out int newPOffset, out int newNOffset, out int newTOffset)
        {
            string[] lines = File.ReadAllLines(filepath);
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textureCoordinates = new List<Vector2>();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            newPOffset = pOffset;
            newNOffset = nOffset;
            newTOffset = tOffset;

            for (int i = lineStartIndex; i < lineEndIndex; i++)
            {
                string[] splitLine = lines[i].Split(' ');
                switch (splitLine[0])
                {
                    case "v":
                        float x = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        float y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        float z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);

                        positions.Add(new Vector3(x, y, z));
                        newPOffset++;
                        break;
                    case "vn":
                        x = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);

                        normals.Add(new Vector3(x, y, z));
                        newNOffset++;
                        break;
                    case "vt":
                        x = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);

                        textureCoordinates.Add(new Vector2(x, y));
                        newTOffset++;
                        break;
                    case "f":
                        for (int j = 0; j < 3; j++)
                        {
                            string[] index = splitLine[1 + j].Split('/');
                            int indexP = int.Parse(index[0]) - 1 - pOffset;
                            int indexT = int.Parse(index[1]) - 1 - tOffset;
                            int indexN = int.Parse(index[2]) - 1 - nOffset;
                            vertices.Add(new Vertex(
                                positions[indexP],
                                normals[indexN],
                                textureCoordinates[indexT]));
                        }
                        indices.AddRange([(uint)(indices.Count),
                            (uint)(indices.Count + 1),
                            (uint)(indices.Count + 2)]);

                        break;
                }
            }

            return new Mesh<Vertex>(vertices.ToArray(), indices.ToArray());
        }
    }
}
