using System;
using Common.CheckSum;

namespace Common.Compress
{
    public sealed class CompressionStream
    {
        private static readonly int DefaultWBits = 15;
        private const int MaxWBits = 15;
        private byte[] input;
        private int inputIndex;
        private int inputCount;
        private long inputTotal;
        private byte[] output;
        private int outputIndex;
        private int outputCount;
        private long outputTotal;
        private string errorMessage;
        private Deflater deflater;
        private Inflater inflater;
        private int dataType;
        private Adler32 checksum;

        internal void FlushPending()
        {
            int length = this.deflater.pending;
            if (length > this.outputCount)
                length = this.outputCount;
            if (length == 0)
                return;
            Array.Copy((Array)this.deflater.pendingBuffer, this.deflater.pendingOutput, (Array)this.output, this.outputIndex, length);
            this.outputIndex += length;
            this.deflater.pendingOutput += length;
            this.outputTotal += (long)length;
            this.outputCount -= length;
            this.deflater.pending -= length;
            if (this.deflater.pending != 0)
                return;
            this.deflater.pendingOutput = 0;
        }

        internal int Read(byte[] buffer, int start, int size)
        {
            int length = this.inputCount;
            if (length > size)
                length = size;
            if (length == 0)
                return 0;
            this.inputCount -= length;
            if (this.deflater.noheader == 0)
                this.checksum.Add(this.input, this.InputIndex, length);
            Array.Copy((Array)this.input, this.InputIndex, (Array)buffer, start, length);
            this.InputIndex += length;
            this.inputTotal += (long)length;
            return length;
        }

        public int InitializeInflater()
        {
            return this.InitializeInflater(CompressionStream.DefaultWBits);
        }

        public int InitializeInflater(int w)
        {
            this.inflater = new Inflater();
            return this.inflater.Initialize(this, w);
        }

        public int Inflate(int f)
        {
            if (this.inflater == null)
                return -2;
            return this.inflater.Inflate(this, f);
        }

        public int InflateEnd()
        {
            if (this.inflater == null)
                return -2;
            int num = this.inflater.End(this);
            this.inflater = (Inflater)null;
            return num;
        }

        public int InflateSync()
        {
            if (this.inflater == null)
                return -2;
            return this.inflater.Sync(this);
        }

        public int SetInflaterDictionary(byte[] dictionary, int dictLength)
        {
            if (this.inflater == null)
                return -2;
            return this.inflater.SetDictionary(this, dictionary, dictLength);
        }

        public int InitializeDeflater(CompressionLevel level)
        {
            return this.InitializeDeflater(level, 15);
        }

        public int InitializeDeflater(CompressionLevel level, int bits)
        {
            this.deflater = new Deflater();
            return this.deflater.Initialize(this, level, bits);
        }

        public int Deflate(int flush)
        {
            if (this.deflater == null)
                return -2;
            return this.deflater.Deflate(this, flush);
        }

        public int DeflateEnd()
        {
            if (this.deflater == null)
                return -2;
            int num = this.deflater.End();
            this.deflater = (Deflater)null;
            return num;
        }

        public int SetDeflaterParameters(int level, int strategy)
        {
            if (this.deflater == null)
                return -2;
            return this.deflater.SetParameters(this, level, strategy);
        }

        public int SetDeflaterDictionary(byte[] dictionary, int dictLength)
        {
            if (this.deflater == null)
                return -2;
            return this.deflater.SetDictionary(this, dictionary, dictLength);
        }

        public void Free()
        {
            this.input = (byte[])null;
            this.output = (byte[])null;
            this.ErrorMessage = (string)null;
        }

        internal Deflater Deflater
        {
            get
            {
                return this.deflater;
            }
            set
            {
                this.deflater = value;
            }
        }

        internal int DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
            }
        }

        public byte[] Input
        {
            get
            {
                return this.input;
            }
            set
            {
                this.input = value;
            }
        }

        public int InputIndex
        {
            get
            {
                return this.inputIndex;
            }
            set
            {
                this.inputIndex = value;
            }
        }

        public int InputCount
        {
            get
            {
                return this.inputCount;
            }
            set
            {
                this.inputCount = value;
            }
        }

        public long InputTotal
        {
            get
            {
                return this.inputTotal;
            }
            set
            {
                this.inputTotal = value;
            }
        }

        public byte[] Output
        {
            get
            {
                return this.output;
            }
            set
            {
                this.output = value;
            }
        }

        public int OutputIndex
        {
            get
            {
                return this.outputIndex;
            }
            set
            {
                this.outputIndex = value;
            }
        }

        public int OutputCount
        {
            get
            {
                return this.outputCount;
            }
            set
            {
                this.outputCount = value;
            }
        }

        public long OutputTotal
        {
            get
            {
                return this.outputTotal;
            }
            set
            {
                this.outputTotal = value;
            }
        }

        public Adler32 Checksum
        {
            get
            {
                return this.checksum;
            }
            set
            {
                this.checksum = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                this.errorMessage = value;
            }
        }
    }
}
