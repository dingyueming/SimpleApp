using System;

namespace Common.Compress
{
    internal sealed class InflaterCodes
    {
        private static readonly int[] inflateMask = new int[17]
        {
      0,
      1,
      3,
      7,
      15,
      31,
      63,
      (int) sbyte.MaxValue,
      (int) byte.MaxValue,
      511,
      1023,
      2047,
      4095,
      8191,
      16383,
      (int) short.MaxValue,
      (int) ushort.MaxValue
        };
        private int treeIndex = 0;
        private int mode;
        private int len;
        private int[] tree;
        private int need;
        private int lit;
        private int getBits;
        private int dist;
        private byte lbits;
        private byte dbits;
        private int[] ltree;
        private int ltreeIndex;
        private int[] dtree;
        private int dtreeIndex;

        internal InflaterCodes(
          int bl,
          int bd,
          int[] tl,
          int tlIndex,
          int[] td,
          int tdIndex,
          CompressionStream z)
        {
            this.mode = 0;
            this.lbits = (byte)bl;
            this.dbits = (byte)bd;
            this.ltree = tl;
            this.ltreeIndex = tlIndex;
            this.dtree = td;
            this.dtreeIndex = tdIndex;
        }

        internal InflaterCodes(int bl, int bd, int[] tl, int[] td, CompressionStream z)
        {
            this.mode = 0;
            this.lbits = (byte)bl;
            this.dbits = (byte)bd;
            this.ltree = tl;
            this.ltreeIndex = 0;
            this.dtree = td;
            this.dtreeIndex = 0;
        }

        internal int Process(InflaterBlocks s, CompressionStream z, int r)
        {
            int inputIndex = z.InputIndex;
            int inputCount = z.InputCount;
            int number = s.BitBuffer;
            int bitBufferLength = s.BitBufferLength;
            int num1 = s.WindowWrite;
            int num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
            while (true)
            {
                switch (this.mode)
                {
                    case 0:
                        if (num2 >= 258 && inputCount >= 10)
                        {
                            s.BitBuffer = number;
                            s.BitBufferLength = bitBufferLength;
                            z.InputCount = inputCount;
                            z.InputTotal += (long)(inputIndex - z.InputIndex);
                            z.InputIndex = inputIndex;
                            s.WindowWrite = num1;
                            r = this.InflateFast((int)this.lbits, (int)this.dbits, this.ltree, this.ltreeIndex, this.dtree, this.dtreeIndex, s, z);
                            inputIndex = z.InputIndex;
                            inputCount = z.InputCount;
                            number = s.BitBuffer;
                            bitBufferLength = s.BitBufferLength;
                            num1 = s.WindowWrite;
                            num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                            int num3;
                            switch (r)
                            {
                                case 0:
                                    goto label_6;
                                case 1:
                                    num3 = 7;
                                    break;
                                default:
                                    num3 = 9;
                                    break;
                            }
                            this.mode = num3;
                            break;
                        }
                        label_6:
                        this.need = (int)this.lbits;
                        this.tree = this.ltree;
                        this.treeIndex = this.ltreeIndex;
                        this.mode = 1;
                        goto case 1;
                    case 1:
                        int need1;
                        for (need1 = this.need; bitBufferLength < need1; bitBufferLength += 8)
                        {
                            if (inputCount != 0)
                            {
                                r = 0;
                                --inputCount;
                                number |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << bitBufferLength;
                            }
                            else
                            {
                                s.BitBuffer = number;
                                s.BitBufferLength = bitBufferLength;
                                z.InputCount = inputCount;
                                z.InputTotal += (long)(inputIndex - z.InputIndex);
                                z.InputIndex = inputIndex;
                                s.WindowWrite = num1;
                                return s.InflateFlush(z, r);
                            }
                        }
                        int index1 = (this.treeIndex + (number & InflaterCodes.inflateMask[need1])) * 3;
                        number = Utils.ShiftRight(number, this.tree[index1 + 1]);
                        bitBufferLength -= this.tree[index1 + 1];
                        int num4 = this.tree[index1];
                        if (num4 == 0)
                        {
                            this.lit = this.tree[index1 + 2];
                            this.mode = 6;
                            break;
                        }
                        if ((num4 & 16) != 0)
                        {
                            this.getBits = num4 & 15;
                            this.len = this.tree[index1 + 2];
                            this.mode = 2;
                            break;
                        }
                        if ((num4 & 64) == 0)
                        {
                            this.need = num4;
                            this.treeIndex = index1 / 3 + this.tree[index1 + 2];
                            break;
                        }
                        if ((num4 & 32) != 0)
                        {
                            this.mode = 7;
                            break;
                        }
                        goto label_20;
                    case 2:
                        int getBits1;
                        for (getBits1 = this.getBits; bitBufferLength < getBits1; bitBufferLength += 8)
                        {
                            if (inputCount != 0)
                            {
                                r = 0;
                                --inputCount;
                                number |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << bitBufferLength;
                            }
                            else
                            {
                                s.BitBuffer = number;
                                s.BitBufferLength = bitBufferLength;
                                z.InputCount = inputCount;
                                z.InputTotal += (long)(inputIndex - z.InputIndex);
                                z.InputIndex = inputIndex;
                                s.WindowWrite = num1;
                                return s.InflateFlush(z, r);
                            }
                        }
                        this.len += number & InflaterCodes.inflateMask[getBits1];
                        number >>= getBits1;
                        bitBufferLength -= getBits1;
                        this.need = (int)this.dbits;
                        this.tree = this.dtree;
                        this.treeIndex = this.dtreeIndex;
                        this.mode = 3;
                        goto case 3;
                    case 3:
                        int need2;
                        for (need2 = this.need; bitBufferLength < need2; bitBufferLength += 8)
                        {
                            if (inputCount != 0)
                            {
                                r = 0;
                                --inputCount;
                                number |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << bitBufferLength;
                            }
                            else
                            {
                                s.BitBuffer = number;
                                s.BitBufferLength = bitBufferLength;
                                z.InputCount = inputCount;
                                z.InputTotal += (long)(inputIndex - z.InputIndex);
                                z.InputIndex = inputIndex;
                                s.WindowWrite = num1;
                                return s.InflateFlush(z, r);
                            }
                        }
                        int index2 = (this.treeIndex + (number & InflaterCodes.inflateMask[need2])) * 3;
                        number >>= this.tree[index2 + 1];
                        bitBufferLength -= this.tree[index2 + 1];
                        int num5 = this.tree[index2];
                        if ((num5 & 16) != 0)
                        {
                            this.getBits = num5 & 15;
                            this.dist = this.tree[index2 + 2];
                            this.mode = 4;
                            break;
                        }
                        if ((num5 & 64) == 0)
                        {
                            this.need = num5;
                            this.treeIndex = index2 / 3 + this.tree[index2 + 2];
                            break;
                        }
                        goto label_36;
                    case 4:
                        int getBits2;
                        for (getBits2 = this.getBits; bitBufferLength < getBits2; bitBufferLength += 8)
                        {
                            if (inputCount != 0)
                            {
                                r = 0;
                                --inputCount;
                                number |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << bitBufferLength;
                            }
                            else
                            {
                                s.BitBuffer = number;
                                s.BitBufferLength = bitBufferLength;
                                z.InputCount = inputCount;
                                z.InputTotal += (long)(inputIndex - z.InputIndex);
                                z.InputIndex = inputIndex;
                                s.WindowWrite = num1;
                                return s.InflateFlush(z, r);
                            }
                        }
                        this.dist += number & InflaterCodes.inflateMask[getBits2];
                        number >>= getBits2;
                        bitBufferLength -= getBits2;
                        this.mode = 5;
                        goto case 5;
                    case 5:
                        int num6 = num1 - this.dist;
                        while (num6 < 0)
                            num6 += s.WindowEnd;
                        for (; this.len != 0; --this.len)
                        {
                            if (num2 == 0)
                            {
                                if (num1 == s.WindowEnd && s.WindowRead != 0)
                                {
                                    num1 = 0;
                                    num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                                }
                                if (num2 == 0)
                                {
                                    s.WindowWrite = num1;
                                    r = s.InflateFlush(z, r);
                                    num1 = s.WindowWrite;
                                    num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                                    if (num1 == s.WindowEnd && s.WindowRead != 0)
                                    {
                                        num1 = 0;
                                        num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                                    }
                                    if (num2 == 0)
                                    {
                                        s.BitBuffer = number;
                                        s.BitBufferLength = bitBufferLength;
                                        z.InputCount = inputCount;
                                        z.InputTotal += (long)(inputIndex - z.InputIndex);
                                        z.InputIndex = inputIndex;
                                        s.WindowWrite = num1;
                                        return s.InflateFlush(z, r);
                                    }
                                }
                            }
                            s.Window[num1++] = s.Window[num6++];
                            --num2;
                            if (num6 == s.WindowEnd)
                                num6 = 0;
                        }
                        this.mode = 0;
                        break;
                    case 6:
                        if (num2 == 0)
                        {
                            if (num1 == s.WindowEnd && s.WindowRead != 0)
                            {
                                num1 = 0;
                                num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                            }
                            if (num2 == 0)
                            {
                                s.WindowWrite = num1;
                                r = s.InflateFlush(z, r);
                                num1 = s.WindowWrite;
                                num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                                if (num1 == s.WindowEnd && s.WindowRead != 0)
                                {
                                    num1 = 0;
                                    num2 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
                                }
                                if (num2 == 0)
                                    goto label_67;
                            }
                        }
                        r = 0;
                        s.Window[num1++] = (byte)this.lit;
                        --num2;
                        this.mode = 0;
                        break;
                    case 7:
                        goto label_70;
                    case 8:
                        goto label_75;
                    case 9:
                        goto label_76;
                    default:
                        goto label_77;
                }
            }
            label_20:
            this.mode = 9;
            z.ErrorMessage = "invalid literal/length code";
            r = -3;
            s.BitBuffer = number;
            s.BitBufferLength = bitBufferLength;
            z.InputCount = inputCount;
            z.InputTotal += (long)(inputIndex - z.InputIndex);
            z.InputIndex = inputIndex;
            s.WindowWrite = num1;
            return s.InflateFlush(z, r);
            label_36:
            this.mode = 9;
            z.ErrorMessage = "invalid distance code";
            r = -3;
            s.BitBuffer = number;
            s.BitBufferLength = bitBufferLength;
            z.InputCount = inputCount;
            z.InputTotal += (long)(inputIndex - z.InputIndex);
            z.InputIndex = inputIndex;
            s.WindowWrite = num1;
            return s.InflateFlush(z, r);
            label_67:
            s.BitBuffer = number;
            s.BitBufferLength = bitBufferLength;
            z.InputCount = inputCount;
            z.InputTotal += (long)(inputIndex - z.InputIndex);
            z.InputIndex = inputIndex;
            s.WindowWrite = num1;
            return s.InflateFlush(z, r);
            label_70:
            if (bitBufferLength > 7)
            {
                bitBufferLength -= 8;
                ++inputCount;
                --inputIndex;
            }
            s.WindowWrite = num1;
            r = s.InflateFlush(z, r);
            num1 = s.WindowWrite;
            int num7 = num1 < s.WindowRead ? s.WindowRead - num1 - 1 : s.WindowEnd - num1;
            if (s.WindowRead != s.WindowWrite)
            {
                s.BitBuffer = number;
                s.BitBufferLength = bitBufferLength;
                z.InputCount = inputCount;
                z.InputTotal += (long)(inputIndex - z.InputIndex);
                z.InputIndex = inputIndex;
                s.WindowWrite = num1;
                return s.InflateFlush(z, r);
            }
            this.mode = 8;
            label_75:
            r = 1;
            s.BitBuffer = number;
            s.BitBufferLength = bitBufferLength;
            z.InputCount = inputCount;
            z.InputTotal += (long)(inputIndex - z.InputIndex);
            z.InputIndex = inputIndex;
            s.WindowWrite = num1;
            return s.InflateFlush(z, r);
            label_76:
            r = -3;
            s.BitBuffer = number;
            s.BitBufferLength = bitBufferLength;
            z.InputCount = inputCount;
            z.InputTotal += (long)(inputIndex - z.InputIndex);
            z.InputIndex = inputIndex;
            s.WindowWrite = num1;
            return s.InflateFlush(z, r);
            label_77:
            r = -2;
            s.BitBuffer = number;
            s.BitBufferLength = bitBufferLength;
            z.InputCount = inputCount;
            z.InputTotal += (long)(inputIndex - z.InputIndex);
            z.InputIndex = inputIndex;
            s.WindowWrite = num1;
            return s.InflateFlush(z, r);
        }

        internal void Free(CompressionStream z)
        {
        }

        internal int InflateFast(
          int bl,
          int bd,
          int[] tl,
          int tlIndex,
          int[] td,
          int tdIndex,
          InflaterBlocks s,
          CompressionStream z)
        {
            int inputIndex = z.InputIndex;
            int inputCount = z.InputCount;
            int num1 = s.BitBuffer;
            int num2 = s.BitBufferLength;
            int destinationIndex = s.WindowWrite;
            int num3 = destinationIndex < s.WindowRead ? s.WindowRead - destinationIndex - 1 : s.WindowEnd - destinationIndex;
            int num4 = InflaterCodes.inflateMask[bl];
            int num5 = InflaterCodes.inflateMask[bd];
            do
            {
                for (; num2 < 20; num2 += 8)
                {
                    --inputCount;
                    num1 |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << num2;
                }
                int num6 = num1 & num4;
                int[] numArray1 = tl;
                int num7 = tlIndex;
                int index1 = (num7 + num6) * 3;
                int index2;
                if ((index2 = numArray1[index1]) == 0)
                {
                    num1 >>= numArray1[index1 + 1];
                    num2 -= numArray1[index1 + 1];
                    s.Window[destinationIndex++] = (byte)numArray1[index1 + 2];
                    --num3;
                }
                else
                {
                    bool flag;
                    while (true)
                    {
                        num1 >>= numArray1[index1 + 1];
                        num2 -= numArray1[index1 + 1];
                        if ((index2 & 16) == 0)
                        {
                            if ((index2 & 64) == 0)
                            {
                                num6 = num6 + numArray1[index1 + 2] + (num1 & InflaterCodes.inflateMask[index2]);
                                index1 = (num7 + num6) * 3;
                                if ((index2 = numArray1[index1]) != 0)
                                    flag = true;
                                else
                                    goto label_34;
                            }
                            else
                                goto label_35;
                        }
                        else
                            break;
                    }
                    int index3 = index2 & 15;
                    int length1 = numArray1[index1 + 2] + (num1 & InflaterCodes.inflateMask[index3]);
                    int num8 = num1 >> index3;
                    int num9;
                    for (num9 = num2 - index3; num9 < 15; num9 += 8)
                    {
                        --inputCount;
                        num8 |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << num9;
                    }
                    int num10 = num8 & num5;
                    int[] numArray2 = td;
                    int num11 = tdIndex;
                    int index4 = (num11 + num10) * 3;
                    int index5 = numArray2[index4];
                    while (true)
                    {
                        num8 >>= numArray2[index4 + 1];
                        num9 -= numArray2[index4 + 1];
                        if ((index5 & 16) == 0)
                        {
                            if ((index5 & 64) == 0)
                            {
                                num10 = num10 + numArray2[index4 + 2] + (num8 & InflaterCodes.inflateMask[index5]);
                                index4 = (num11 + num10) * 3;
                                index5 = numArray2[index4];
                                flag = true;
                            }
                            else
                                goto label_31;
                        }
                        else
                            break;
                    }
                    int index6;
                    for (index6 = index5 & 15; num9 < index6; num9 += 8)
                    {
                        --inputCount;
                        num8 |= ((int)z.Input[inputIndex++] & (int)byte.MaxValue) << num9;
                    }
                    int num12 = numArray2[index4 + 2] + (num8 & InflaterCodes.inflateMask[index6]);
                    num1 = num8 >> index6;
                    num2 = num9 - index6;
                    num3 -= length1;
                    int sourceIndex1;
                    int num13;
                    if (destinationIndex >= num12)
                    {
                        int sourceIndex2 = destinationIndex - num12;
                        if (destinationIndex - sourceIndex2 > 0 && 2 > destinationIndex - sourceIndex2)
                        {
                            byte[] window1 = s.Window;
                            int index7 = destinationIndex;
                            int num14 = index7 + 1;
                            byte[] window2 = s.Window;
                            int index8 = sourceIndex2;
                            int num15 = index8 + 1;
                            int num16 = (int)window2[index8];
                            window1[index7] = (byte)num16;
                            byte[] window3 = s.Window;
                            int index9 = num14;
                            destinationIndex = index9 + 1;
                            byte[] window4 = s.Window;
                            int index10 = num15;
                            sourceIndex1 = index10 + 1;
                            int num17 = (int)window4[index10];
                            window3[index9] = (byte)num17;
                            length1 -= 2;
                        }
                        else
                        {
                            Array.Copy((Array)s.Window, sourceIndex2, (Array)s.Window, destinationIndex, 2);
                            destinationIndex += 2;
                            sourceIndex1 = sourceIndex2 + 2;
                            length1 -= 2;
                        }
                    }
                    else
                    {
                        sourceIndex1 = destinationIndex - num12;
                        do
                        {
                            sourceIndex1 += s.WindowEnd;
                        }
                        while (sourceIndex1 < 0);
                        int length2 = s.WindowEnd - sourceIndex1;
                        if (length1 > length2)
                        {
                            length1 -= length2;
                            if (destinationIndex - sourceIndex1 > 0 && length2 > destinationIndex - sourceIndex1)
                            {
                                do
                                {
                                    s.Window[destinationIndex++] = s.Window[sourceIndex1++];
                                }
                                while (--length2 != 0);
                            }
                            else
                            {
                                Array.Copy((Array)s.Window, sourceIndex1, (Array)s.Window, destinationIndex, length2);
                                destinationIndex += length2;
                                num13 = sourceIndex1 + length2;
                            }
                            sourceIndex1 = 0;
                        }
                    }
                    if (destinationIndex - sourceIndex1 > 0 && length1 > destinationIndex - sourceIndex1)
                    {
                        do
                        {
                            s.Window[destinationIndex++] = s.Window[sourceIndex1++];
                        }
                        while (--length1 != 0);
                        goto label_39;
                    }
                    else
                    {
                        Array.Copy((Array)s.Window, sourceIndex1, (Array)s.Window, destinationIndex, length1);
                        destinationIndex += length1;
                        num13 = sourceIndex1 + length1;
                        goto label_39;
                    }
                    label_31:
                    z.ErrorMessage = "invalid distance code";
                    int num18 = z.InputCount - inputCount;
                    int num19 = num9 >> 3 < num18 ? num9 >> 3 : num18;
                    int num20 = inputCount + num19;
                    int num21 = inputIndex - num19;
                    int num22 = num9 - (num19 << 3);
                    s.BitBuffer = num8;
                    s.BitBufferLength = num22;
                    z.InputCount = num20;
                    z.InputTotal += (long)(num21 - z.InputIndex);
                    z.InputIndex = num21;
                    s.WindowWrite = destinationIndex;
                    return -3;
                    label_34:
                    num1 >>= numArray1[index1 + 1];
                    num2 -= numArray1[index1 + 1];
                    s.Window[destinationIndex++] = (byte)numArray1[index1 + 2];
                    --num3;
                    goto label_39;
                    label_35:
                    if ((index2 & 32) != 0)
                    {
                        int num14 = z.InputCount - inputCount;
                        int num15 = num2 >> 3 < num14 ? num2 >> 3 : num14;
                        int num16 = inputCount + num15;
                        int num17 = inputIndex - num15;
                        int num23 = num2 - (num15 << 3);
                        s.BitBuffer = num1;
                        s.BitBufferLength = num23;
                        z.InputCount = num16;
                        z.InputTotal += (long)(num17 - z.InputIndex);
                        z.InputIndex = num17;
                        s.WindowWrite = destinationIndex;
                        return 1;
                    }
                    z.ErrorMessage = "invalid literal/length code";
                    int num24 = z.InputCount - inputCount;
                    int num25 = num2 >> 3 < num24 ? num2 >> 3 : num24;
                    int num26 = inputCount + num25;
                    int num27 = inputIndex - num25;
                    int num28 = num2 - (num25 << 3);
                    s.BitBuffer = num1;
                    s.BitBufferLength = num28;
                    z.InputCount = num26;
                    z.InputTotal += (long)(num27 - z.InputIndex);
                    z.InputIndex = num27;
                    s.WindowWrite = destinationIndex;
                    return -3;
                    label_39:;
                }
            }
            while (num3 >= 258 && inputCount >= 10);
            int num29 = z.InputCount - inputCount;
            int num30 = num2 >> 3 < num29 ? num2 >> 3 : num29;
            int num31 = inputCount + num30;
            int num32 = inputIndex - num30;
            int num33 = num2 - (num30 << 3);
            s.BitBuffer = num1;
            s.BitBufferLength = num33;
            z.InputCount = num31;
            z.InputTotal += (long)(num32 - z.InputIndex);
            z.InputIndex = num32;
            s.WindowWrite = destinationIndex;
            return 0;
        }
    }

}
