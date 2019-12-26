namespace Common.CheckSum
{
    public interface IChecksumAlgorithm
    {
        void Add(byte data);

        void Add(byte[] data);

        void Add(byte[] data, int start, int length);

        void Reset();

        long Get();
    }
}
