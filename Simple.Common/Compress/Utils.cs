using System;
using System.IO;

namespace Common.Compress
{
    public sealed class Utils
    {
        internal static long Identity(long literal)
        {
            return literal;
        }

        internal static int ShiftRight(int number, int bits)
        {
            return number >= 0 ? number >> bits : (int)((long)(number >> bits) + (2L << ~bits));
        }

        internal static int ShiftRight(int number, long bits)
        {
            return Utils.ShiftRight(number, (int)bits);
        }

        internal static long ShiftRight(long number, int bits)
        {
            return number >= 0L ? number >> bits : (number >> bits) + (2L << ~bits);
        }

        internal static long ShiftRight(long number, long bits)
        {
            return Utils.ShiftRight(number, (int)bits);
        }

        public static int ReadInput(Stream source, byte[] target, int start, int count)
        {
            if (target.Length == 0)
                return 0;
            byte[] buffer = new byte[target.Length];
            int num = source.Read(buffer, start, count);
            if (num == 0)
                return -1;
            for (int index = start; index < start + num; ++index)
                target[index] = buffer[index];
            return num;
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2000];
            int count;
            while ((count = input.Read(buffer, 0, 2000)) > 0)
                output.Write(buffer, 0, count);
            output.Flush();
        }

        public static byte[] Compress(byte[] input)
        {
            MemoryStream memoryStream1 = new MemoryStream(input);
            MemoryStream memoryStream2 = new MemoryStream();
            try
            {
                Utils.Compress((Stream)memoryStream1, (Stream)memoryStream2);
                byte[] buffer = memoryStream2.ToArray();
                byte[] numArray = new byte[memoryStream2.Length];
                Array.Copy((Array)buffer, 0, (Array)numArray, 0, (int)memoryStream2.Length);
                return numArray;
            }
            finally
            {
                memoryStream1.Dispose();
                memoryStream2.Dispose();
            }
        }

        public static void Compress(Stream input, Stream output)
        {
            OutputStream outputStream = new OutputStream(output, CompressionLevel.DefaultCompression);
            try
            {
                Utils.CopyStream(input, (Stream)outputStream);
            }
            finally
            {
                outputStream.Close();
            }
        }

        public static byte[] Decompress(byte[] input)
        {
            MemoryStream memoryStream1 = new MemoryStream(input);
            MemoryStream memoryStream2 = new MemoryStream();
            try
            {
                Utils.Decompress((Stream)memoryStream1, (Stream)memoryStream2);
                byte[] buffer = memoryStream2.ToArray();
                byte[] numArray = new byte[memoryStream2.Length];
                Array.Copy((Array)buffer, 0, (Array)numArray, 0, (int)memoryStream2.Length);
                return numArray;
            }
            finally
            {
                memoryStream1.Dispose();
                memoryStream2.Dispose();
            }
        }

        public static void Decompress(Stream input, Stream output)
        {
            OutputStream outputStream = new OutputStream(output);
            try
            {
                Utils.CopyStream(input, (Stream)outputStream);
            }
            finally
            {
                outputStream.Close();
            }
        }
    }

    public enum FlushOption
    {
        NoFlush,
        PartialFlush,
        SyncFlush,
        FullFlush,
        Finish,
    }

    public enum ErrorCode
    {
        VersionError = -6,
        BufferError = -5,
        MemoryError = -4,
        DataError = -3,
        StreamError = -2,
        ErrorNo = -1,
        Ok = 0,
        StreamEnd = 1,
        NeedsDictionary = 2,
    }

    public enum CompressionLevel
    {
        DefaultCompression = -1,
        NoCompression = 0,
        BestSpeed = 1,
        BestCompression = 9,
    }

    public enum CompressionMethod
    {
        Stored = 0,
        Deflated = 8,
        Deflate64 = 9,
        BZip2 = 11, // 0x0000000B
        WinZipAES = 99, // 0x00000063
    }
}
