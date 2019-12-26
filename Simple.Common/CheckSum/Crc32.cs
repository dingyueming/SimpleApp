namespace Common.CheckSum
{
    public sealed class Crc32 : IChecksumAlgorithm
    {
        private static uint[] _table = Crc32.CalculateCrcTable();
        private const uint poly = 3988292384;
        private uint value = 0U;

        private static uint[] CalculateCrcTable()
        {
            uint[] table = new uint[4096];
            for (uint index1 = 0; index1 < 256U; ++index1)
            {
                uint num = index1;
                for (int index2 = 0; index2 < 16; ++index2)
                {
                    for (int index3 = 0; index3 < 8; ++index3)
                        num = ((int)num & 1) == 1 ? poly ^ num >> 1 : num >> 1;
                    table[(long)(index2 * 256) + (long)index1] = num;
                }
            }
            return table;
        }

        public Crc32()
        {
            this.Reset();
        }

        public void Add(byte[] data, int start, int length)
        {
            uint num1 = uint.MaxValue ^ value;
            uint[] table = _table;
            for (; length >= 16; length -= 16)
            {
                uint num2 = table[768 + (int)data[start + 12]] ^ table[512 + (int)data[start + 13]] ^ table[256 + (int)data[start + 14]] ^ table[(int)data[start + 15]];
                uint num3 = table[1792 + (int)data[start + 8]] ^ table[1536 + (int)data[start + 9]] ^ table[1280 + (int)data[start + 10]] ^ table[1024 + (int)data[start + 11]];
                uint num4 = table[2816 + (int)data[start + 4]] ^ table[2560 + (int)data[start + 5]] ^ table[2304 + (int)data[start + 6]] ^ table[2048 + (int)data[start + 7]];
                num1 = table[(uint)(3840 + (((int)num1 ^ (int)data[start]) & (int)byte.MaxValue))] ^ table[(uint)(3584 + (((int)(num1 >> 8) ^ (int)data[start + 1]) & (int)byte.MaxValue))] ^ table[(uint)(3328 + (((int)(num1 >> 16) ^ (int)data[start + 2]) & (int)byte.MaxValue))] ^ table[(uint)(3072 + (((int)(num1 >> 24) ^ (int)data[start + 3]) & (int)byte.MaxValue))] ^ num4 ^ num3 ^ num2;
                start += 16;
            }
            while (--length >= 0)
                num1 = table[(uint)(((int)num1 ^ (int)data[start++]) & (int)byte.MaxValue)] ^ num1 >> 8;
            value = num1 ^ uint.MaxValue;
        }

        public void Add(byte data)
        {
            this.Add(new[] { data }, 0, 1);
        }

        public void Add(byte[] data)
        {
            this.Add(data, 0, data.Length);
        }

        public void Reset()
        {
            this.value = 0;
        }

        public long Get()
        {
            return (long)(this.value);
        }

        public static long Get(byte[] data)
        {
            Crc32 crc32 = new Crc32();
            crc32.Add(data);
            return crc32.Get();
        }

        public static long Get(byte[] data, int start, int length)
        {
            Crc32 crc32 = new Crc32();
            crc32.Add(data, start, length);
            return crc32.Get();
        }
    }

}
