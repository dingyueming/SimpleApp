using System;

namespace Common.Compress
{
    internal sealed class CompressionTree
    {
        private int maxCode;
        private CompressionStaticTree staticTree;
        private short[] dynamicTree;

        private void GenerateBitLengths(Deflater deflater)
        {
            short[] dynamicTree = this.dynamicTree;
            short[] staticTreeData = this.staticTree.StaticTreeData;
            int[] extraBits = this.staticTree.ExtraBits;
            int extraBase = this.staticTree.ExtraBase;
            int maxLength = this.staticTree.MaxLength;
            int num1 = 0;
            for (int index = 0; index <= 15; ++index)
                deflater.bl_count[index] = (short)0;
            dynamicTree[deflater.heap[deflater.heapMax] * 2 + 1] = (short)0;
            int index1;
            for (index1 = deflater.heapMax + 1; index1 < TreeConstants.HeapSize; ++index1)
            {
                int num2 = deflater.heap[index1];
                int index2 = (int)dynamicTree[(int)dynamicTree[num2 * 2 + 1] * 2 + 1] + 1;
                if (index2 > maxLength)
                {
                    index2 = maxLength;
                    ++num1;
                }
                dynamicTree[num2 * 2 + 1] = (short)index2;
                if (num2 <= this.maxCode)
                {
                    ++deflater.bl_count[index2];
                    int num3 = 0;
                    if (num2 >= extraBase)
                        num3 = extraBits[num2 - extraBase];
                    short num4 = dynamicTree[num2 * 2];
                    deflater.opt_len += (int)num4 * (index2 + num3);
                    if (staticTreeData != null)
                        deflater.static_len += (int)num4 * ((int)staticTreeData[num2 * 2 + 1] + num3);
                }
            }
            if (num1 == 0)
                return;
            do
            {
                int index2 = maxLength - 1;
                while (deflater.bl_count[index2] == (short)0)
                    --index2;
                --deflater.bl_count[index2];
                deflater.bl_count[index2 + 1] = (short)((int)deflater.bl_count[index2 + 1] + 2);
                --deflater.bl_count[maxLength];
                num1 -= 2;
            }
            while (num1 > 0);
            for (int index2 = maxLength; index2 != 0; --index2)
            {
                int num2 = (int)deflater.bl_count[index2];
                while (num2 != 0)
                {
                    int num3 = deflater.heap[--index1];
                    if (num3 <= this.maxCode)
                    {
                        if ((int)dynamicTree[num3 * 2 + 1] != index2)
                        {
                            deflater.opt_len = (int)((long)deflater.opt_len + ((long)index2 - (long)dynamicTree[num3 * 2 + 1]) * (long)dynamicTree[num3 * 2]);
                            dynamicTree[num3 * 2 + 1] = (short)index2;
                        }
                        --num2;
                    }
                }
            }
        }

        private static void GenerateCodes(short[] tree, int maxCode, short[] blCount)
        {
            short[] numArray = new short[16];
            short num = 0;
            for (int index = 1; index <= 15; ++index)
                numArray[index] = num = (short)((int)num + (int)blCount[index - 1] << 1);
            for (int index = 0; index <= maxCode; ++index)
            {
                int len = (int)tree[index * 2 + 1];
                if (len != 0)
                    tree[index * 2] = (short)CompressionTree.ReverseBits((int)numArray[len]++, len);
            }
        }

        private static int ReverseBits(int code, int len)
        {
            int number = 0;
            do
            {
                int num = number | code & 1;
                code = Utils.ShiftRight(code, 1);
                number = num << 1;
            }
            while (--len > 0);
            return Utils.ShiftRight(number, 1);
        }

        public static int GetDistanceCode(int distance)
        {
            if (distance < 256)
                return (int)TreeConstants.DistanceCode[distance];
            return (int)TreeConstants.DistanceCode[256 + Utils.ShiftRight(distance, 7)];
        }

        public void BuildTree(Deflater deflater)
        {
            short[] dynamicTree = this.dynamicTree;
            short[] staticTreeData = this.staticTree.StaticTreeData;
            int maxElements = this.staticTree.MaxElements;
            int maxCode = -1;
            deflater.heapLen = 0;
            deflater.heapMax = TreeConstants.HeapSize;
            for (int index = 0; index < maxElements; ++index)
            {
                if (dynamicTree[index * 2] != (short)0)
                {
                    deflater.heap[++deflater.heapLen] = maxCode = index;
                    deflater.depth[index] = (byte)0;
                }
                else
                    dynamicTree[index * 2 + 1] = (short)0;
            }
            while (deflater.heapLen < 2)
            {
                int[] heap = deflater.heap;
                int index1 = ++deflater.heapLen;
                int num1;
                if (maxCode >= 2)
                    num1 = 0;
                else
                    maxCode = num1 = maxCode + 1;
                int num2 = num1;
                heap[index1] = num1;
                int index2 = num2;
                dynamicTree[index2 * 2] = (short)1;
                deflater.depth[index2] = (byte)0;
                --deflater.opt_len;
                if (staticTreeData != null)
                    deflater.static_len -= (int)staticTreeData[index2 * 2 + 1];
            }
            this.maxCode = maxCode;
            for (int k = deflater.heapLen / 2; k >= 1; --k)
                deflater.RestoreHeapDown(dynamicTree, k);
            int index3 = maxElements;
            do
            {
                int index1 = deflater.heap[1];
                deflater.heap[1] = deflater.heap[deflater.heapLen--];
                deflater.RestoreHeapDown(dynamicTree, 1);
                int index2 = deflater.heap[1];
                deflater.heap[--deflater.heapMax] = index1;
                deflater.heap[--deflater.heapMax] = index2;
                dynamicTree[index3 * 2] = (short)((int)dynamicTree[index1 * 2] + (int)dynamicTree[index2 * 2]);
                deflater.depth[index3] = (byte)((uint)Math.Max(deflater.depth[index1], deflater.depth[index2]) + 1U);
                dynamicTree[index1 * 2 + 1] = dynamicTree[index2 * 2 + 1] = (short)index3;
                deflater.heap[1] = index3++;
                deflater.RestoreHeapDown(dynamicTree, 1);
            }
            while (deflater.heapLen >= 2);
            deflater.heap[--deflater.heapMax] = deflater.heap[1];
            this.GenerateBitLengths(deflater);
            CompressionTree.GenerateCodes(dynamicTree, maxCode, deflater.bl_count);
        }

        public int MaxCode
        {
            get
            {
                return this.maxCode;
            }
            set
            {
                this.maxCode = value;
            }
        }

        public CompressionStaticTree StaticTree
        {
            get
            {
                return this.staticTree;
            }
            set
            {
                this.staticTree = value;
            }
        }

        public short[] DynamicTree
        {
            get
            {
                return this.dynamicTree;
            }
            set
            {
                this.dynamicTree = value;
            }
        }
    }

}
