namespace Common.CheckSum
{
    public sealed class Adler32 : IChecksumAlgorithm
    {
        private const int crcBase = 65521;
        private const int crcMax = 5552;
        private uint value;

        public Adler32()
            : this(1U)
        {
        }

        public Adler32(uint value)
        {
            this.value = value;
        }

        public void Add(byte data)
        {
            uint num1 = this.value & (uint)ushort.MaxValue;
            uint num2 = this.value >> 16 & (uint)ushort.MaxValue;
            uint num3 = num1 + ((uint)data & (uint)byte.MaxValue);
            this.value = ((num2 + num3) % 65521U << 16) + num3 % 65521U;
        }

        public void Add(byte[] data)
        {
            this.Add(data, 0, data.Length);
        }

        public void Add(byte[] data, int start, int length)
        {
            uint num1 = this.value & (uint)ushort.MaxValue;
            uint num2 = this.value >> 16 & (uint)ushort.MaxValue;
            while (length > 0)
            {
                int num3 = 5552;
                if (num3 > length)
                    num3 = length;
                length -= num3;
                for (int index = 0; index < num3; ++index)
                {
                    num1 += (uint)data[start++] & (uint)byte.MaxValue;
                    num2 += num1;
                }
                num1 %= 65521U;
                num2 %= 65521U;
            }
            this.value = num2 << 16 | num1;
        }

        public void Reset()
        {
            this.value = 1U;
        }

        public long Get()
        {
            return (long)this.value;
        }

        public static long Get(byte[] data)
        {
            Adler32 adler32 = new Adler32();
            adler32.Add(data);
            return adler32.Get();
        }

        public static long Get(byte[] data, int start, int length)
        {
            Adler32 adler32 = new Adler32();
            adler32.Add(data, start, length);
            return adler32.Get();
        }
    }

}
