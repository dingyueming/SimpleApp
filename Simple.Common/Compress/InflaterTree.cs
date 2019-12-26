using System;

namespace Common.Compress
{
    internal sealed class InflaterTree
    {
        private static int BuildHuffmanTree(
          int[] b,
          int bindex,
          int n,
          int s,
          int[] d,
          int[] e,
          int[] t,
          int[] m,
          int[] hp,
          int[] hn,
          int[] v)
        {
            int[] numArray1 = new int[16];
            int[] numArray2 = new int[3];
            int[] numArray3 = new int[15];
            int[] numArray4 = new int[16];
            int num1 = 0;
            int num2 = n;
            do
            {
                ++numArray1[b[bindex + num1]];
                ++num1;
                --num2;
            }
            while (num2 != 0);
            if (numArray1[0] == n)
            {
                t[0] = -1;
                m[0] = 0;
                return 0;
            }
            int num3 = m[0];
            int index1 = 1;
            while (index1 <= 15 && numArray1[index1] == 0)
                ++index1;
            int index2 = index1;
            if (num3 < index1)
                num3 = index1;
            int index3 = 15;
            while (index3 != 0 && numArray1[index3] == 0)
                --index3;
            int index4 = index3;
            if (num3 > index3)
                num3 = index3;
            m[0] = num3;
            int num4 = 1 << index1;
            while (index1 < index3)
            {
                int num5;
                if ((num5 = num4 - numArray1[index1]) < 0)
                    return -3;
                ++index1;
                num4 = num5 << 1;
            }
            int num6;
            if ((num6 = num4 - numArray1[index3]) < 0)
                return -3;
            numArray1[index3] += num6;
            int num7;
            numArray4[1] = num7 = 0;
            int index5 = 1;
            int index6 = 2;
            while (--index3 != 0)
            {
                numArray4[index6] = (num7 += numArray1[index5]);
                ++index6;
                ++index5;
            }
            int num8 = 0;
            int num9 = 0;
            do
            {
                int index7;
                if ((index7 = b[bindex + num9]) != 0)
                    v[numArray4[index7]++] = num8;
                ++num9;
            }
            while (++num8 < n);
            n = numArray4[index4];
            int number1;
            numArray4[0] = number1 = 0;
            int index8 = 0;
            int index9 = -1;
            int bits = -num3;
            numArray3[0] = 0;
            int num10 = 0;
            int num11 = 0;
            for (; index2 <= index4; ++index2)
            {
                int num5 = numArray1[index2];
                while (num5-- != 0)
                {
                    while (index2 > bits + num3)
                    {
                        ++index9;
                        bits += num3;
                        int num12 = index4 - bits;
                        int num13 = num12 > num3 ? num3 : num12;
                        int num14;
                        int num15;
                        if ((num15 = 1 << (num14 = index2 - bits)) > num5 + 1)
                        {
                            int num16 = num15 - (num5 + 1);
                            int index7 = index2;
                            if (num14 < num13)
                            {
                                int num17;
                                while (++num14 < num13 && (num17 = num16 << 1) > numArray1[++index7])
                                    num16 = num17 - numArray1[index7];
                            }
                        }
                        num11 = 1 << num14;
                        if (hn[0] + num11 > 1440)
                            return -3;
                        numArray3[index9] = num10 = hn[0];
                        hn[0] += num11;
                        if (index9 != 0)
                        {
                            numArray4[index9] = number1;
                            numArray2[0] = (int)(byte)num14;
                            numArray2[1] = (int)(byte)num3;
                            int num16 = Utils.ShiftRight(number1, bits - num3);
                            numArray2[2] = num10 - numArray3[index9 - 1] - num16;
                            Array.Copy((Array)numArray2, 0, (Array)hp, (numArray3[index9 - 1] + num16) * 3, 3);
                        }
                        else
                            t[0] = num10;
                    }
                    numArray2[1] = (int)(byte)(index2 - bits);
                    if (index8 >= n)
                        numArray2[0] = 192;
                    else if (v[index8] < s)
                    {
                        numArray2[0] = v[index8] < 256 ? 0 : 96;
                        numArray2[2] = v[index8++];
                    }
                    else
                    {
                        numArray2[0] = (int)(byte)(e[v[index8] - s] + 16 + 64);
                        numArray2[2] = d[v[index8++] - s];
                    }
                    int num18 = 1 << index2 - bits;
                    for (int index7 = Utils.ShiftRight(number1, bits); index7 < num11; index7 += num18)
                        Array.Copy((Array)numArray2, 0, (Array)hp, (num10 + index7) * 3, 3);
                    int number2;
                    for (number2 = 1 << index2 - 1; (number1 & number2) != 0; number2 = Utils.ShiftRight(number2, 1))
                        number1 ^= number2;
                    number1 ^= number2;
                    for (int index7 = (1 << bits) - 1; (number1 & index7) != numArray4[index9]; index7 = (1 << bits) - 1)
                    {
                        --index9;
                        bits -= num3;
                    }
                }
            }
            return num6 == 0 || index4 == 1 ? 0 : -5;
        }

        internal static int InflateTreeBits(
          int[] c,
          int[] bb,
          int[] tb,
          int[] hp,
          CompressionStream z)
        {
            int[] hn = new int[1];
            int[] v = new int[19];
            int num1 = InflaterTree.BuildHuffmanTree(c, 0, 19, 19, (int[])null, (int[])null, tb, bb, hp, hn, v);
            int num2;
            switch (num1)
            {
                case -5:
                    num2 = 0;
                    break;
                case -3:
                    z.ErrorMessage = "oversubscribed dynamic bit lengths tree";
                    goto label_6;
                default:
                    num2 = bb[0] != 0 ? 1 : 0;
                    break;
            }
            if (num2 == 0)
            {
                z.ErrorMessage = "incomplete dynamic bit lengths tree";
                num1 = -3;
            }
            label_6:
            return num1;
        }

        internal static int InflateTreeDynamic(
          int nl,
          int nd,
          int[] c,
          int[] bl,
          int[] bd,
          int[] tl,
          int[] td,
          int[] hp,
          CompressionStream z)
        {
            int[] v = new int[288];
            int[] hn = new int[1];
            int num1 = InflaterTree.BuildHuffmanTree(c, 0, nl, 257, InflaterTreeConstants.CpLens, InflaterTreeConstants.CpLext, tl, bl, hp, hn, v);
            if (num1 != 0 || bl[0] == 0)
            {
                switch (num1)
                {
                    case -4:
                        return num1;
                    case -3:
                        z.ErrorMessage = "oversubscribed literal/length tree";
                        goto case -4;
                    default:
                        z.ErrorMessage = "incomplete literal/length tree";
                        num1 = -3;
                        goto case -4;
                }
            }
            else
            {
                int num2 = InflaterTree.BuildHuffmanTree(c, nl, nd, 0, InflaterTreeConstants.CpDist, InflaterTreeConstants.CpDext, td, bd, hp, hn, v);
                if (num2 == 0 && (bd[0] != 0 || nl <= 257))
                    return 0;
                switch (num2)
                {
                    case -5:
                        z.ErrorMessage = "incomplete distance tree";
                        num2 = -3;
                        goto case -4;
                    case -4:
                        return num2;
                    case -3:
                        z.ErrorMessage = "oversubscribed distance tree";
                        goto case -4;
                    default:
                        z.ErrorMessage = "empty distance tree with lengths";
                        num2 = -3;
                        goto case -4;
                }
            }
        }

        internal static int InflateTreeFixed(
          int[] bl,
          int[] bd,
          int[][] tl,
          int[][] td,
          CompressionStream z)
        {
            bl[0] = 9;
            bd[0] = 5;
            tl[0] = InflaterTreeConstants.FixedTl;
            td[0] = InflaterTreeConstants.FixedTd;
            return 0;
        }
    }

}
