
using System.Reflection.Metadata;

namespace Graphics2026.Model.BufferObjects
{
    internal interface IBufferObject : IDisposable
    {
        public int GetHandle();
        public void Bind();
        public void Unbind();
        public int SizeOfData();
    }
}
