using System;
using System.IO;

namespace Common.Compress
{
    public class OutputStream : Stream
    {
        private CompressionStream zip = new CompressionStream();
        private byte[] buffer = new byte[1];
        private int bufsize = 4096;
        private Stream stream;
        private FlushOption flushOption;
        private bool compress;

        private void InitializeStream(Stream stream, CompressionLevel level, bool compress)
        {
            this.flushOption = FlushOption.NoFlush;
            this.buffer = new byte[this.bufsize];
            this.stream = stream;
            this.compress = compress;
            if (compress)
                this.zip.InitializeDeflater(level);
            else
                this.zip.InitializeInflater();
        }

        public OutputStream(Stream stream)
        {
            this.InitializeStream(stream, 0, false);
        }

        public OutputStream(Stream stream, CompressionLevel level)
        {
            this.InitializeStream(stream, level, true);
        }

        public void WriteByte(int b)
        {
            this.WriteByte((int)(byte)b);
        }

        public override void WriteByte(byte b)
        {
            this.Write(new byte[1] { b }, 0, 1);
        }

        public override void Write(byte[] data, int start, int length)
        {
            if (length == 0)
                return;
            byte[] numArray = new byte[data.Length];
            Array.Copy((Array)data, 0, (Array)numArray, 0, data.Length);
            this.zip.Input = numArray;
            this.zip.InputIndex = start;
            this.zip.InputCount = length;
            int num;
            do
            {
                this.zip.Output = this.buffer;
                this.zip.OutputIndex = 0;
                this.zip.OutputCount = this.bufsize;
                num = !this.compress ? this.zip.Inflate((int)this.flushOption) : this.zip.Deflate((int)this.flushOption);
                if (num != 0 && num != 1)
                    throw new StreamException((this.compress ? "de" : "in") + "flating: " + this.zip.ErrorMessage);
                this.stream.Write(this.buffer, 0, this.bufsize - this.zip.OutputCount);
            }
            while (num != 1 && (this.zip.InputCount > 0 || this.zip.OutputCount == 0));
        }

        public virtual void Close()
        {
            try
            {
                this.Finish();
            }
            catch
            {
            }
            finally
            {
                this.End();
                this.stream = (Stream)null;
            }
        }

        public void Finish()
        {
            do
            {
                this.zip.Output = this.buffer;
                this.zip.OutputIndex = 0;
                this.zip.OutputCount = this.bufsize;
                int num = !this.compress ? this.zip.Inflate(4) : this.zip.Deflate(4);
                if (num != 1 && num != 0)
                    throw new StreamException((this.compress ? "de" : "in") + "flating: " + this.zip.ErrorMessage);
                if (this.bufsize - this.zip.OutputCount > 0)
                    this.stream.Write(this.buffer, 0, this.bufsize - this.zip.OutputCount);
                if (num == 1)
                    break;
            }
            while (this.zip.InputCount > 0 || this.zip.OutputCount == 0);
            try
            {
                this.Flush();
            }
            catch
            {
            }
        }

        public void End()
        {
            if (this.compress)
                this.zip.DeflateEnd();
            else
                this.zip.InflateEnd();
            this.zip.Free();
            this.zip = (CompressionStream)null;
        }

        public override void Flush()
        {
            this.stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.stream.Read(buffer, offset, count);
        }

        public override void SetLength(long value)
        {
            this.stream.SetLength(value);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.stream.Seek(offset, origin);
        }

        public FlushOption FlushMode
        {
            get
            {
                return this.flushOption;
            }
            set
            {
                this.flushOption = value;
            }
        }

        public virtual long TotalIn
        {
            get
            {
                return this.zip.InputTotal;
            }
        }

        public virtual long TotalOut
        {
            get
            {
                return this.zip.OutputTotal;
            }
        }

        public override bool CanRead
        {
            get
            {
                return this.stream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this.stream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this.stream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return this.stream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.stream.Position;
            }
            set
            {
                this.stream.Position = value;
            }
        }
    }
}
