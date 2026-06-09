using Graphics2026.Model.Actors;
using Graphics2026.Model.VertexData;
using OpenTK.Mathematics;
using System.Globalization;

namespace Graphics2026.Model.MeshGeneration
{
    internal class MeshImport
    {
        public static Mesh<Vertex> OBJ(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textureCoordinates = new List<Vector2>();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitLine = lines[i].Split(' ');
                switch (splitLine[0])
                {
                    case "v":
                        float x = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        float y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        float z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);

                        positions.Add(new Vector3(x, y, z));
                        break;
                    case "vn":
                        x = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                        z = float.Parse(splitLine[3], CultureInfo.InvariantCulture.NumberFormat);

                        normals.Add(new Vector3(x, y, z));
                        break;
                    case "vt":
                        x = float.Parse(splitLine[1], CultureInfo.InvariantCulture.NumberFormat);
                        y = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);

                        textureCoordinates.Add(new Vector2(x, y));
                        break;
                    case "f":
                        for (int j = 0; j < 3; j++)
                        {
                            string[] index = splitLine[1 + j].Split('/');
                            int indexP = int.Parse(index[0]) - 1;
                            int indexT = int.Parse(index[1]) - 1;
                            int indexN = int.Parse(index[2]) - 1;
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
