using System;
using Common.CheckSum;

namespace Common.Compress
{
    internal sealed class InflaterBlocks
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
        private static readonly int[] border = new int[19]
        {
      16,
      17,
      18,
      0,
      8,
      7,
      9,
      6,
      10,
      5,
      11,
      4,
      12,
      3,
      13,
      2,
      14,
      1,
      15
        };
        private int[] bitLengthTreeDepth = new int[1];
        private int[] bitLengthDecodeTree = new int[1];
        private int mode;
        private int left;
        private int table;
        private int index;
        private int[] blens;
        private InflaterCodes codes;
        private int last;
        private int[] hufts;
        private object checkfn;
        private long check;
        private int bitBufferLength;
        private int bitBuffer;
        private byte[] window;
        private int windowEnd;
        private int windowRead;
        private int windowWrite;

        internal InflaterBlocks(CompressionStream z, object checkfn, int w)
        {
            this.hufts = new int[4320];
            this.Window = new byte[w];
            this.WindowEnd = w;
            this.checkfn = checkfn;
            this.mode = 0;
            this.Reset(z, (long[])null);
        }

        private void ProcessTypeStored(InflaterBlocksContext state)
        {
            state.ShiftRight(3);
            state.Temp = state.Bits & 7;
            state.ShiftRight(state.Temp);
            this.mode = 1;
        }

        private void ProcessTypeFixed(CompressionStream zip, InflaterBlocksContext state)
        {
            int[] bl = new int[1];
            int[] bd = new int[1];
            int[][] tl = new int[1][];
            int[][] td = new int[1][];
            InflaterTree.InflateTreeFixed(bl, bd, tl, td, zip);
            this.codes = new InflaterCodes(bl[0], bd[0], tl[0], td[0], zip);
            state.ShiftRight(3);
            this.mode = 6;
        }

        private void ProcessTypeDynamic(InflaterBlocksContext state)
        {
            state.ShiftRight(3);
            this.mode = 3;
        }

        private void TransferBlocksState(CompressionStream zip, InflaterBlocksContext state)
        {
            this.BitBuffer = state.BitBuf;
            this.BitBufferLength = state.Bits;
            zip.InputCount = state.BytesCount;
            zip.InputTotal += (long)(state.Input - zip.InputIndex);
            zip.InputIndex = state.Input;
            this.WindowWrite = state.Output;
        }

        private int ProcessType(CompressionStream zip, InflaterBlocksContext state, int res)
        {
            for (; state.Bits < 3; state.Bits += 8)
            {
                if (state.BytesCount != 0)
                {
                    res = 0;
                    --state.BytesCount;
                    state.BitBuf |= ((int)zip.Input[state.Input++] & (int)byte.MaxValue) << state.Bits;
                }
                else
                {
                    this.TransferBlocksState(zip, state);
                    state.IsReturn = true;
                    return this.InflateFlush(zip, res);
                }
            }
            state.Temp = state.BitBuf & 7;
            this.last = state.Temp & 1;
            switch (Utils.ShiftRight(state.Temp, 1))
            {
                case 0:
                    this.ProcessTypeStored(state);
                    break;
                case 1:
                    this.ProcessTypeFixed(zip, state);
                    break;
                case 2:
                    this.ProcessTypeDynamic(state);
                    break;
                case 3:
                    state.ShiftRight(3);
                    this.mode = 9;
                    zip.ErrorMessage = "invalid block type";
                    res = -3;
                    this.TransferBlocksState(zip, state);
                    state.IsReturn = true;
                    return this.InflateFlush(zip, res);
            }
            return 0;
        }

        private int ProcessLens(CompressionStream zip, InflaterBlocksContext state, ref int res)
        {
            for (; state.Bits < 32; state.Bits += 8)
            {
                if (state.BytesCount != 0)
                {
                    res = 0;
                    --state.BytesCount;
                    state.BitBuf |= ((int)zip.Input[state.Input++] & (int)byte.MaxValue) << state.Bits;
                }
                else
                {
                    this.TransferBlocksState(zip, state);
                    state.IsReturn = true;
                    return this.InflateFlush(zip, res);
                }
            }
            if ((Utils.ShiftRight(~state.BitBuf, 16) & (int)ushort.MaxValue) != (state.BitBuf & (int)ushort.MaxValue))
            {
                this.mode = 9;
                zip.ErrorMessage = "invalid stored block lengths";
                res = -3;
                this.TransferBlocksState(zip, state);
                state.IsReturn = true;
                return this.InflateFlush(zip, res);
            }
            this.left = state.BitBuf & (int)ushort.MaxValue;
            state.BitBuf = state.Bits = 0;
            this.mode = this.left == 0 ? (this.last == 0 ? 0 : 7) : 2;
            return 0;
        }

        private int GetEndBytes(InflaterBlocksContext state)
        {
            return state.Output < this.WindowRead ? this.WindowRead - state.Output - 1 : this.WindowEnd - state.Output;
        }

        private int ProcessStored(CompressionStream zip, InflaterBlocksContext state, int res)
        {
            if (state.BytesCount == 0)
            {
                this.TransferBlocksState(zip, state);
                state.IsReturn = true;
                return this.InflateFlush(zip, res);
            }
            if (state.EndBytes == 0)
            {
                if (state.Output == this.WindowEnd && this.WindowRead != 0)
                {
                    state.Output = 0;
                    state.EndBytes = this.GetEndBytes(state);
                }
                if (state.EndBytes == 0)
                {
                    this.WindowWrite = state.Output;
                    res = this.InflateFlush(zip, res);
                    state.Output = this.WindowWrite;
                    state.EndBytes = this.GetEndBytes(state);
                    if (state.Output == this.WindowEnd && this.WindowRead != 0)
                    {
                        state.Output = 0;
                        state.EndBytes = this.GetEndBytes(state);
                    }
                    if (state.EndBytes == 0)
                    {
                        this.TransferBlocksState(zip, state);
                        state.IsReturn = true;
                        return this.InflateFlush(zip, res);
                    }
                }
            }
            res = 0;
            state.Temp = this.left;
            if (state.Temp > state.BytesCount)
                state.Temp = state.BytesCount;
            if (state.Temp > state.EndBytes)
                state.Temp = state.EndBytes;
            Array.Copy((Array)zip.Input, state.Input, (Array)this.Window, state.Output, state.Temp);
            state.Input += state.Temp;
            state.BytesCount -= state.Temp;
            state.Output += state.Temp;
            state.EndBytes -= state.Temp;
            return 0;
        }

        private int ProcessTable(CompressionStream zip, InflaterBlocksContext state, int res)
        {
            for (; state.Bits < 14; state.Bits += 8)
            {
                if (state.BytesCount != 0)
                {
                    res = 0;
                    --state.BytesCount;
                    state.BitBuf |= ((int)zip.Input[state.Input++] & (int)byte.MaxValue) << state.Bits;
                }
                else
                {
                    this.TransferBlocksState(zip, state);
                    state.IsReturn = true;
                    return this.InflateFlush(zip, res);
                }
            }
            this.table = state.Temp = state.BitBuf & 16383;
            if ((state.Temp & 31) > 29 || (state.Temp >> 5 & 31) > 29)
            {
                this.mode = 9;
                zip.ErrorMessage = "too many length or distance symbols";
                res = -3;
                this.TransferBlocksState(zip, state);
                state.IsReturn = true;
                return this.InflateFlush(zip, res);
            }
            state.Temp = 258 + (state.Temp & 31) + (state.Temp >> 5 & 31);
            this.blens = new int[state.Temp];
            if (this.blens == null || this.blens.Length < state.Temp)
            {
                this.blens = new int[state.Temp];
            }
            else
            {
                for (int index = 0; index < state.Temp; ++index)
                    this.blens[index] = 0;
            }
            state.ShiftRight(14);
            this.index = 0;
            this.mode = 4;
            return 0;
        }

        private int ProcessBTree(CompressionStream zip, InflaterBlocksContext state, int res)
        {
            while (this.index < 4 + Utils.ShiftRight(this.table, 10))
            {
                for (; state.Bits < 3; state.Bits += 8)
                {
                    if (state.BytesCount != 0)
                    {
                        res = 0;
                        --state.BytesCount;
                        state.BitBuf |= ((int)zip.Input[state.Input++] & (int)byte.MaxValue) << state.Bits;
                    }
                    else
                    {
                        this.TransferBlocksState(zip, state);
                        state.IsReturn = true;
                        return this.InflateFlush(zip, res);
                    }
                }
                this.blens[InflaterBlocks.border[this.index++]] = state.BitBuf & 7;
                state.ShiftRight(3);
            }
            while (this.index < 19)
                this.blens[InflaterBlocks.border[this.index++]] = 0;
            this.bitLengthTreeDepth[0] = 7;
            state.Temp = InflaterTree.InflateTreeBits(this.blens, this.bitLengthTreeDepth, this.bitLengthDecodeTree, this.hufts, zip);
            if (state.Temp != 0)
            {
                res = state.Temp;
                if (res == -3)
                {
                    this.blens = (int[])null;
                    this.mode = 9;
                }
                this.TransferBlocksState(zip, state);
                state.IsReturn = true;
                return this.InflateFlush(zip, res);
            }
            this.index = 0;
            this.mode = 5;
            return 0;
        }

        private int ProcessDTree(CompressionStream zip, InflaterBlocksContext state, int res)
        {
            while (true)
            {
                state.Temp = this.table;
                if (this.index < 258 + (state.Temp & 31) + (state.Temp >> 5 & 31))
                {
                    for (state.Temp = this.bitLengthTreeDepth[0]; state.Bits < state.Temp; state.Bits += 8)
                    {
                        if (state.BytesCount != 0)
                        {
                            res = 0;
                            --state.BytesCount;
                            state.BitBuf |= ((int)zip.Input[state.Input++] & (int)byte.MaxValue) << state.Bits;
                        }
                        else
                        {
                            this.TransferBlocksState(zip, state);
                            state.IsReturn = true;
                            return this.InflateFlush(zip, res);
                        }
                    }
                    state.Temp = this.hufts[(this.bitLengthDecodeTree[0] + (state.BitBuf & InflaterBlocks.inflateMask[state.Temp])) * 3 + 1];
                    int huft = this.hufts[(this.bitLengthDecodeTree[0] + (state.BitBuf & InflaterBlocks.inflateMask[state.Temp])) * 3 + 2];
                    if (huft < 16)
                    {
                        state.ShiftRight(state.Temp);
                        this.blens[this.index++] = huft;
                    }
                    else
                    {
                        int v = huft == 18 ? 7 : huft - 14;
                        int num1 = huft == 18 ? 11 : 3;
                        for (; state.Bits < state.Temp + v; state.Bits += 8)
                        {
                            if (state.BytesCount != 0)
                            {
                                res = 0;
                                --state.BytesCount;
                                state.BitBuf |= ((int)zip.Input[state.Input++] & (int)byte.MaxValue) << state.Bits;
                            }
                            else
                            {
                                this.TransferBlocksState(zip, state);
                                state.IsReturn = true;
                                return this.InflateFlush(zip, res);
                            }
                        }
                        state.ShiftRight(state.Temp);
                        int num2 = num1 + (state.BitBuf & InflaterBlocks.inflateMask[v]);
                        state.ShiftRight(v);
                        int index = this.index;
                        state.Temp = this.table;
                        if (index + num2 <= 258 + (state.Temp & 31) + (state.Temp >> 5 & 31) && (huft != 16 || index >= 1))
                        {
                            int num3 = huft == 16 ? this.blens[index - 1] : 0;
                            do
                            {
                                this.blens[index++] = num3;
                            }
                            while (--num2 != 0);
                            this.index = index;
                        }
                        else
                            break;
                    }
                }
                else
                    goto label_20;
            }
            this.blens = (int[])null;
            this.mode = 9;
            zip.ErrorMessage = "invalid bit length repeat";
            res = -3;
            this.TransferBlocksState(zip, state);
            state.IsReturn = true;
            return this.InflateFlush(zip, res);
            label_20:
            this.bitLengthDecodeTree[0] = -1;
            int[] bl = new int[1];
            int[] bd = new int[1];
            int[] tl = new int[1];
            int[] td = new int[1];
            bl[0] = 9;
            bd[0] = 6;
            state.Temp = this.table;
            state.Temp = InflaterTree.InflateTreeDynamic(257 + (state.Temp & 31), 1 + (state.Temp >> 5 & 31), this.blens, bl, bd, tl, td, this.hufts, zip);
            if (state.Temp != 0)
            {
                if (state.Temp == -3)
                {
                    this.blens = (int[])null;
                    this.mode = 9;
                }
                res = state.Temp;
                this.TransferBlocksState(zip, state);
                state.IsReturn = true;
                return this.InflateFlush(zip, res);
            }
            this.codes = new InflaterCodes(bl[0], bd[0], this.hufts, tl[0], this.hufts, td[0], zip);
            this.mode = 6;
            return 0;
        }

        internal void Reset(CompressionStream z, long[] checksum)
        {
            if (checksum != null)
                checksum[0] = this.check;
            if (this.mode == 4 || this.mode == 5)
                this.blens = (int[])null;
            if (this.mode == 6)
                this.codes.Free(z);
            this.mode = 0;
            this.BitBufferLength = 0;
            this.BitBuffer = 0;
            this.WindowRead = this.WindowWrite = 0;
            if (this.checkfn == null)
                return;
            z.Checksum = new Adler32();
            this.check = z.Checksum.Get();
        }

        internal int Process(CompressionStream zip, int res)
        {
            InflaterBlocksContext state = new InflaterBlocksContext()
            {
                Input = zip.InputIndex,
                BytesCount = zip.InputCount,
                BitBuf = this.BitBuffer,
                Bits = this.BitBufferLength,
                Output = this.WindowWrite
            };
            state.EndBytes = this.GetEndBytes(state);
            int num1;
            int num2;
            int num3;
            int num4;
            int num5;
            int num6;
            while (true)
            {
                switch (this.mode)
                {
                    case 0:
                        state.IsReturn = false;
                        num1 = this.ProcessType(zip, state, res);
                        if (!state.IsReturn)
                            break;
                        goto label_2;
                    case 1:
                        state.IsReturn = false;
                        num2 = this.ProcessLens(zip, state, ref res);
                        if (!state.IsReturn)
                            break;
                        goto label_4;
                    case 2:
                        state.IsReturn = false;
                        num3 = this.ProcessStored(zip, state, res);
                        if (!state.IsReturn)
                        {
                            if ((this.left -= state.Temp) == 0)
                            {
                                this.mode = this.last == 0 ? 0 : 7;
                                break;
                            }
                            break;
                        }
                        goto label_6;
                    case 3:
                        state.IsReturn = false;
                        num4 = this.ProcessTable(zip, state, res);
                        if (!state.IsReturn)
                            goto case 4;
                        else
                            goto label_10;
                    case 4:
                        state.IsReturn = false;
                        num5 = this.ProcessBTree(zip, state, res);
                        if (!state.IsReturn)
                            goto case 5;
                        else
                            goto label_12;
                    case 5:
                        state.IsReturn = false;
                        num6 = this.ProcessDTree(zip, state, res);
                        if (!state.IsReturn)
                            goto case 6;
                        else
                            goto label_14;
                    case 6:
                        this.TransferBlocksState(zip, state);
                        if ((res = this.codes.Process(this, zip, res)) == 1)
                        {
                            res = 0;
                            this.codes.Free(zip);
                            state.Input = zip.InputIndex;
                            state.BytesCount = zip.InputCount;
                            state.BitBuf = this.BitBuffer;
                            state.Bits = this.BitBufferLength;
                            state.Output = this.WindowWrite;
                            state.EndBytes = this.GetEndBytes(state);
                            if (this.last == 0)
                            {
                                this.mode = 0;
                                break;
                            }
                            goto label_19;
                        }
                        else
                            goto label_16;
                    case 7:
                        goto label_20;
                    case 8:
                        goto label_23;
                    case 9:
                        goto label_24;
                    default:
                        goto label_25;
                }
            }
            label_2:
            return num1;
            label_4:
            return num2;
            label_6:
            return num3;
            label_10:
            return num4;
            label_12:
            return num5;
            label_14:
            return num6;
            label_16:
            return this.InflateFlush(zip, res);
            label_19:
            this.mode = 7;
            label_20:
            this.WindowWrite = state.Output;
            res = this.InflateFlush(zip, res);
            state.Output = this.WindowWrite;
            state.EndBytes = this.GetEndBytes(state);
            if (this.WindowRead != this.WindowWrite)
            {
                this.TransferBlocksState(zip, state);
                return this.InflateFlush(zip, res);
            }
            this.mode = 8;
            label_23:
            res = 1;
            this.TransferBlocksState(zip, state);
            return this.InflateFlush(zip, res);
            label_24:
            res = -3;
            this.TransferBlocksState(zip, state);
            return this.InflateFlush(zip, res);
            label_25:
            res = -2;
            this.TransferBlocksState(zip, state);
            return this.InflateFlush(zip, res);
        }

        internal void Free(CompressionStream z)
        {
            this.Reset(z, (long[])null);
            this.Window = (byte[])null;
            this.hufts = (int[])null;
        }

        internal void SetDictionary(byte[] d, int start, int n)
        {
            Array.Copy((Array)d, start, (Array)this.Window, 0, n);
            this.WindowRead = this.WindowWrite = n;
        }

        internal int GetSyncPoint()
        {
            return this.mode == 1 ? 1 : 0;
        }

        internal int InflateFlush(CompressionStream z, int r)
        {
            int outputIndex = z.OutputIndex;
            int windowRead = this.WindowRead;
            int length1 = (windowRead <= this.WindowWrite ? this.WindowWrite : this.WindowEnd) - windowRead;
            if (length1 > z.OutputCount)
                length1 = z.OutputCount;
            if (length1 != 0 && r == -5)
                r = 0;
            z.OutputCount -= length1;
            z.OutputTotal += (long)length1;
            if (this.checkfn != null)
            {
                z.Checksum = new Adler32((uint)this.check);
                z.Checksum.Add(this.Window, windowRead, length1);
                this.check = z.Checksum.Get();
            }
            Array.Copy((Array)this.Window, windowRead, (Array)z.Output, outputIndex, length1);
            int destinationIndex = outputIndex + length1;
            int num1 = windowRead + length1;
            if (num1 == this.WindowEnd)
            {
                int num2 = 0;
                if (this.WindowWrite == this.WindowEnd)
                    this.WindowWrite = 0;
                int length2 = this.WindowWrite - num2;
                if (length2 > z.OutputCount)
                    length2 = z.OutputCount;
                if (length2 != 0 && r == -5)
                    r = 0;
                z.OutputCount -= length2;
                z.OutputTotal += (long)length2;
                if (this.checkfn != null)
                {
                    z.Checksum = new Adler32((uint)this.check);
                    z.Checksum.Add(this.Window, num2, length2);
                    this.check = z.Checksum.Get();
                }
                Array.Copy((Array)this.Window, num2, (Array)z.Output, destinationIndex, length2);
                destinationIndex += length2;
                num1 = num2 + length2;
            }
            z.OutputIndex = destinationIndex;
            this.WindowRead = num1;
            return r;
        }

        public int BitBufferLength
        {
            get
            {
                return this.bitBufferLength;
            }
            set
            {
                this.bitBufferLength = value;
            }
        }

        public int BitBuffer
        {
            get
            {
                return this.bitBuffer;
            }
            set
            {
                this.bitBuffer = value;
            }
        }

        public byte[] Window
        {
            get
            {
                return this.window;
            }
            set
            {
                this.window = value;
            }
        }

        public int WindowEnd
        {
            get
            {
                return this.windowEnd;
            }
            set
            {
                this.windowEnd = value;
            }
        }

        public int WindowRead
        {
            get
            {
                return this.windowRead;
            }
            set
            {
                this.windowRead = value;
            }
        }

        public int WindowWrite
        {
            get
            {
                return this.windowWrite;
            }
            set
            {
                this.windowWrite = value;
            }
        }
    }

}
