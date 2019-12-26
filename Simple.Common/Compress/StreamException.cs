using System.IO;

namespace Common.Compress
{
    public class StreamException : IOException
    {
        public StreamException()
        {
        }

        public StreamException(string msg)
            : base(msg)
        {
        }
    }
}
