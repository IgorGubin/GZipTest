namespace GZipTest.Model
{
    internal class BufferInfo
    {
        public BufferInfo(decimal offset, byte[] buffer)
        {
            OrigOffset = offset;
            Buffer = buffer;
            OrigLength = buffer.Length;
        }

        public decimal OrigOffset { get; private set; }

        public int OrigLength { get; private set; }

        public byte[] Buffer { get; set; }
    }
}
