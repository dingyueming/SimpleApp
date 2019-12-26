namespace Common.Compress
{
    internal class InflaterBlocksContext
    {
        private int temp;
        private int bitBuf;
        private int bits;
        private int pIn;
        private int bytesCount;
        private int qOut;
        private int endBytes;
        private bool isReturn;

        public void ShiftRight(int v)
        {
            this.BitBuf = Utils.ShiftRight(this.BitBuf, v);
            this.Bits -= v;
        }

        public int Temp
        {
            get
            {
                return this.temp;
            }
            set
            {
                this.temp = value;
            }
        }

        public int BitBuf
        {
            get
            {
                return this.bitBuf;
            }
            set
            {
                this.bitBuf = value;
            }
        }

        public int Bits
        {
            get
            {
                return this.bits;
            }
            set
            {
                this.bits = value;
            }
        }

        public int Input
        {
            get
            {
                return this.pIn;
            }
            set
            {
                this.pIn = value;
            }
        }

        public int BytesCount
        {
            get
            {
                return this.bytesCount;
            }
            set
            {
                this.bytesCount = value;
            }
        }

        public int Output
        {
            get
            {
                return this.qOut;
            }
            set
            {
                this.qOut = value;
            }
        }

        public int EndBytes
        {
            get
            {
                return this.endBytes;
            }
            set
            {
                this.endBytes = value;
            }
        }

        public bool IsReturn
        {
            get
            {
                return this.isReturn;
            }
            set
            {
                this.isReturn = value;
            }
        }
    }

}
