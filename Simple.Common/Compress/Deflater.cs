using System;
using Common.CheckSum;

namespace Common.Compress
{
    public sealed class Deflater
    {
        private CompressionTree l_desc = new CompressionTree();
        private CompressionTree d_desc = new CompressionTree();
        private CompressionTree bl_desc = new CompressionTree();
        internal short[] bl_count = new short[16];
        internal int[] heap = new int[2 * Deflater.LCodes + 1];
        internal byte[] depth = new byte[2 * Deflater.LCodes + 1];
        private static readonly string[] errmsg = new string[10]
        {
      "need dictionary",
      "stream end",
      string.Empty,
      "file error",
      "stream error",
      "data error",
      "insufficient memory",
      "buffer error",
      "incompatible version",
      string.Empty
        };
        private static readonly int MinLookahead = 262;
        private static readonly int LCodes = 286;
        private static readonly int HeapSize = 2 * Deflater.LCodes + 1;
        private static DeflaterOptions[] Options = new DeflaterOptions[10];
        private const int NeedMore = 0;
        private const int BlockDone = 1;
        private const int FinishStarted = 2;
        private const int FinishDone = 3;
        private const int BufferSize = 16;
        private const int Repeat3_6 = 16;
        private const int RepeatZero3_10 = 17;
        private const int RepeatZerp11_138 = 18;
        private const int MinMatch = 3;
        private const int MaxMatch = 258;
        private const int MaxBits = 15;
        private const int DCodes = 30;
        private const int BlCodes = 19;
        private const int LengthCodes = 29;
        private const int Literals = 256;
        private const int EndBlock = 256;
        private CompressionStream stream;
        private int status;
        private int pendingBufferSize;
        internal byte[] pendingBuffer;
        internal int pendingOutput;
        internal int pending;
        internal int noheader;
        private byte dataType;
        private byte method;
        private int lastFlush;
        private int wSize;
        private int wBits;
        private int wMask;
        private byte[] window;
        private int windowSize;
        private short[] prev;
        private short[] head;
        private int ins_h;
        private int hashSize;
        private int hashBits;
        private int hashMask;
        private int hashShift;
        private int blockStart;
        private int matchLength;
        private int prevMatch;
        private int matchAvailable;
        private int strstart;
        private int matchStart;
        private int lookahead;
        private int prevLength;
        private int maxChainLength;
        private int maxLazyMatch;
        private int level;
        private int strategy;
        private int goodMatch;
        private int niceMatch;
        private short[] dyn_ltree;
        private short[] dyn_dtree;
        private short[] bl_tree;
        internal int heapLen;
        internal int heapMax;
        private int litBuf;
        private int litBufsize;
        private int lastLit;
        private int d_buf;
        internal int opt_len;
        internal int static_len;
        internal int matches;
        internal int last_eob_len;
        private short biBuf;
        private int biValid;

        static Deflater()
        {
            Deflater.Options[0] = new DeflaterOptions(0, 0, 0, 0, DeflaterFunction.Stored);
            Deflater.Options[1] = new DeflaterOptions(4, 4, 8, 4, DeflaterFunction.Fast);
            Deflater.Options[2] = new DeflaterOptions(4, 5, 16, 8, DeflaterFunction.Fast);
            Deflater.Options[3] = new DeflaterOptions(4, 6, 32, 32, DeflaterFunction.Fast);
            Deflater.Options[4] = new DeflaterOptions(4, 4, 16, 16, DeflaterFunction.Slow);
            Deflater.Options[5] = new DeflaterOptions(8, 16, 32, 32, DeflaterFunction.Slow);
            Deflater.Options[6] = new DeflaterOptions(8, 16, 128, 128, DeflaterFunction.Slow);
            Deflater.Options[7] = new DeflaterOptions(8, 32, 128, 256, DeflaterFunction.Slow);
            Deflater.Options[8] = new DeflaterOptions(32, 128, 258, 1024, DeflaterFunction.Slow);
            Deflater.Options[9] = new DeflaterOptions(32, 258, 258, 4096, DeflaterFunction.Slow);
        }

        public Deflater()
        {
            this.dyn_ltree = new short[Deflater.HeapSize * 2];
            this.dyn_dtree = new short[122];
            this.bl_tree = new short[78];
        }

        private void InitializeMatch()
        {
            this.windowSize = 2 * this.wSize;
            this.head[this.hashSize - 1] = (short)0;
            for (int index = 0; index < this.hashSize - 1; ++index)
                this.head[index] = (short)0;
            this.maxLazyMatch = Deflater.Options[this.level].MaxLazy;
            this.goodMatch = Deflater.Options[this.level].GoodLength;
            this.niceMatch = Deflater.Options[this.level].NiceLength;
            this.maxChainLength = Deflater.Options[this.level].MaxChain;
            this.strstart = 0;
            this.blockStart = 0;
            this.lookahead = 0;
            this.matchLength = this.prevLength = 2;
            this.matchAvailable = 0;
            this.ins_h = 0;
        }

        private void InitializeTree()
        {
            this.l_desc.DynamicTree = this.dyn_ltree;
            this.l_desc.StaticTree = CompressionStaticTree.StaticLTreeDescription;
            this.d_desc.DynamicTree = this.dyn_dtree;
            this.d_desc.StaticTree = CompressionStaticTree.StaticDTreeDescription;
            this.bl_desc.DynamicTree = this.bl_tree;
            this.bl_desc.StaticTree = CompressionStaticTree.StaticBLDescription;
            this.biBuf = (short)0;
            this.biValid = 0;
            this.last_eob_len = 8;
            this.InitializeBlock();
        }

        private void InitializeBlock()
        {
            for (int index = 0; index < Deflater.LCodes; ++index)
                this.dyn_ltree[index * 2] = (short)0;
            for (int index = 0; index < 30; ++index)
                this.dyn_dtree[index * 2] = (short)0;
            for (int index = 0; index < 19; ++index)
                this.bl_tree[index * 2] = (short)0;
            this.dyn_ltree[512] = (short)1;
            this.opt_len = this.static_len = 0;
            this.lastLit = this.matches = 0;
        }

        private void ScanTree(short[] tree, int max_code)
        {
            int num1 = -1;
            int num2 = (int)tree[1];
            int num3 = 0;
            int num4 = 7;
            int num5 = 4;
            if (num2 == 0)
            {
                num4 = 138;
                num5 = 3;
            }
            tree[(max_code + 1) * 2 + 1] = (short)Utils.Identity((long)ushort.MaxValue);
            for (int index = 0; index <= max_code; ++index)
            {
                int num6 = num2;
                num2 = (int)tree[(index + 1) * 2 + 1];
                if (++num3 >= num4 || num6 != num2)
                {
                    if (num3 < num5)
                        this.bl_tree[num6 * 2] = (short)((int)this.bl_tree[num6 * 2] + num3);
                    else if (num6 != 0)
                    {
                        if (num6 != num1)
                            ++this.bl_tree[num6 * 2];
                        ++this.bl_tree[32];
                    }
                    else if (num3 <= 10)
                        ++this.bl_tree[34];
                    else
                        ++this.bl_tree[36];
                    num3 = 0;
                    num1 = num6;
                    if (num2 == 0)
                    {
                        num4 = 138;
                        num5 = 3;
                    }
                    else if (num6 == num2)
                    {
                        num4 = 6;
                        num5 = 3;
                    }
                    else
                    {
                        num4 = 7;
                        num5 = 4;
                    }
                }
            }
        }

        private int BuildBitLengthTree()
        {
            this.ScanTree(this.dyn_ltree, this.l_desc.MaxCode);
            this.ScanTree(this.dyn_dtree, this.d_desc.MaxCode);
            this.bl_desc.BuildTree(this);
            int index = 18;
            while (index >= 3 && this.bl_tree[(int)TreeConstants.BLOrder[index] * 2 + 1] == (short)0)
                --index;
            this.opt_len += 3 * (index + 1) + 5 + 5 + 4;
            return index;
        }

        private void SendAllTrees(int lcodes, int dcodes, int blcodes)
        {
            this.SendBits(lcodes - 257, 5);
            this.SendBits(dcodes - 1, 5);
            this.SendBits(blcodes - 4, 4);
            for (int index = 0; index < blcodes; ++index)
                this.SendBits((int)this.bl_tree[(int)TreeConstants.BLOrder[index] * 2 + 1], 3);
            this.SendTree(this.dyn_ltree, lcodes - 1);
            this.SendTree(this.dyn_dtree, dcodes - 1);
        }

        private void SendTree(short[] tree, int max_code)
        {
            int num1 = -1;
            int num2 = (int)tree[1];
            int num3 = 0;
            int num4 = 7;
            int num5 = 4;
            if (num2 == 0)
            {
                num4 = 138;
                num5 = 3;
            }
            for (int index = 0; index <= max_code; ++index)
            {
                int c = num2;
                num2 = (int)tree[(index + 1) * 2 + 1];
                if (++num3 >= num4 || c != num2)
                {
                    if (num3 < num5)
                    {
                        do
                        {
                            this.SendCode(c, this.bl_tree);
                        }
                        while (--num3 != 0);
                    }
                    else if (c != 0)
                    {
                        if (c != num1)
                        {
                            this.SendCode(c, this.bl_tree);
                            --num3;
                        }
                        this.SendCode(16, this.bl_tree);
                        this.SendBits(num3 - 3, 2);
                    }
                    else if (num3 <= 10)
                    {
                        this.SendCode(17, this.bl_tree);
                        this.SendBits(num3 - 3, 3);
                    }
                    else
                    {
                        this.SendCode(18, this.bl_tree);
                        this.SendBits(num3 - 11, 7);
                    }
                    num3 = 0;
                    num1 = c;
                    if (num2 == 0)
                    {
                        num4 = 138;
                        num5 = 3;
                    }
                    else if (c == num2)
                    {
                        num4 = 6;
                        num5 = 3;
                    }
                    else
                    {
                        num4 = 7;
                        num5 = 4;
                    }
                }
            }
        }

        private void WriteByte(byte[] p, int start, int len)
        {
            Array.Copy((Array)p, start, (Array)this.pendingBuffer, this.pending, len);
            this.pending += len;
        }

        private void WriteByte(byte c)
        {
            this.pendingBuffer[this.pending++] = c;
        }

        private void WriteShort(int w)
        {
            this.WriteByte((byte)w);
            this.WriteByte((byte)Utils.ShiftRight(w, 8));
        }

        private void WriteShortMSB(int b)
        {
            this.WriteByte((byte)(b >> 8));
            this.WriteByte((byte)b);
        }

        private void SendCode(int c, short[] tree)
        {
            int index = c * 2;
            this.SendBits((int)tree[index] & (int)ushort.MaxValue, (int)tree[index + 1] & (int)ushort.MaxValue);
        }

        private void AppendToBitBuffer(int v)
        {
            this.biBuf = (short)((int)(ushort)this.biBuf | (int)(ushort)(v << this.biValid & (int)ushort.MaxValue));
        }

        private void SendBits(int v, int length)
        {
            int num = length;
            if (this.biValid > 16 - num)
            {
                this.AppendToBitBuffer(v);
                this.WriteShort((int)this.biBuf);
                this.biBuf = (short)Utils.ShiftRight(v, 16 - this.biValid);
                this.biValid += num - 16;
            }
            else
            {
                this.AppendToBitBuffer(v);
                this.biValid += num;
            }
        }

        private void AlignTree()
        {
            this.SendBits(2, 3);
            this.SendCode(256, TreeConstants.StaticLTree);
            this.FlushBitBuffer();
            if (1 + this.last_eob_len + 10 - this.biValid < 9)
            {
                this.SendBits(2, 3);
                this.SendCode(256, TreeConstants.StaticLTree);
                this.FlushBitBuffer();
            }
            this.last_eob_len = 7;
        }

        private bool TallyTree(int dist, int lc)
        {
            this.pendingBuffer[this.d_buf + this.lastLit * 2] = (byte)Utils.ShiftRight(dist, 8);
            this.pendingBuffer[this.d_buf + this.lastLit * 2 + 1] = (byte)dist;
            this.pendingBuffer[this.litBuf + this.lastLit] = (byte)lc;
            ++this.lastLit;
            if (dist == 0)
            {
                ++this.dyn_ltree[lc * 2];
            }
            else
            {
                ++this.matches;
                --dist;
                ++this.dyn_ltree[((int)TreeConstants.LengthCode[lc] + 256 + 1) * 2];
                ++this.dyn_dtree[CompressionTree.GetDistanceCode(dist) * 2];
            }
            if ((this.lastLit & 8191) == 0 && this.level > 2)
            {
                int number = this.lastLit * 8;
                int num1 = this.strstart - this.blockStart;
                for (int index = 0; index < 30; ++index)
                    number = (int)((long)number + (long)this.dyn_dtree[index * 2] * (5L + (long)TreeConstants.ExtraDBits[index]));
                int num2 = Utils.ShiftRight(number, 3);
                if (this.matches < this.lastLit / 2 && num2 < num1 / 2)
                    return true;
            }
            return this.lastLit == this.litBufsize - 1;
        }

        private void CompressBlock(short[] ltree, short[] dtree)
        {
            int num1 = 0;
            if (this.lastLit != 0)
            {
                do
                {
                    int num2 = (int)this.pendingBuffer[this.d_buf + num1 * 2] << 8 & 65280 | (int)this.pendingBuffer[this.d_buf + num1 * 2 + 1] & (int)byte.MaxValue;
                    int c = (int)this.pendingBuffer[this.litBuf + num1] & (int)byte.MaxValue;
                    ++num1;
                    if (num2 == 0)
                    {
                        this.SendCode(c, ltree);
                    }
                    else
                    {
                        int index = (int)TreeConstants.LengthCode[c];
                        this.SendCode(index + 256 + 1, ltree);
                        int extraLbit = TreeConstants.ExtraLBits[index];
                        if (extraLbit != 0)
                            this.SendBits(c - TreeConstants.BaseLength[index], extraLbit);
                        int distance = num2 - 1;
                        int distanceCode = CompressionTree.GetDistanceCode(distance);
                        this.SendCode(distanceCode, dtree);
                        int extraDbit = TreeConstants.ExtraDBits[distanceCode];
                        if (extraDbit != 0)
                            this.SendBits(distance - TreeConstants.BaseDistance[distanceCode], extraDbit);
                    }
                }
                while (num1 < this.lastLit);
            }
            this.SendCode(256, ltree);
            this.last_eob_len = (int)ltree[513];
        }

        internal void SetDataType()
        {
            int num1 = 0;
            int number = 0;
            int num2 = 0;
            for (; num1 < 7; ++num1)
                num2 += (int)this.dyn_ltree[num1 * 2];
            for (; num1 < 128; ++num1)
                number += (int)this.dyn_ltree[num1 * 2];
            for (; num1 < 256; ++num1)
                num2 += (int)this.dyn_ltree[num1 * 2];
            this.dataType = num2 > Utils.ShiftRight(number, 2) ? (byte)0 : (byte)1;
        }

        private void FlushBitBuffer()
        {
            if (this.biValid == 16)
            {
                this.WriteShort((int)this.biBuf);
                this.biBuf = (short)0;
                this.biValid = 0;
            }
            else
            {
                if (this.biValid < 8)
                    return;
                this.WriteByte((byte)this.biBuf);
                this.biBuf = (short)Utils.ShiftRight((int)this.biBuf, 8);
                this.biValid -= 8;
            }
        }

        private void WindupBitBuffer()
        {
            if (this.biValid > 8)
                this.WriteShort((int)this.biBuf);
            else if (this.biValid > 0)
                this.WriteByte((byte)this.biBuf);
            this.biBuf = (short)0;
            this.biValid = 0;
        }

        private void CopyBlock(int buf, int len, bool header)
        {
            this.WindupBitBuffer();
            this.last_eob_len = 8;
            if (header)
            {
                this.WriteShort((int)(short)len);
                this.WriteShort((int)(short)~len);
            }
            this.WriteByte(this.window, buf, len);
        }

        private void FlushBlockOnly(bool eof)
        {
            this.TreeFlushBlock(this.blockStart >= 0 ? this.blockStart : -1, this.strstart - this.blockStart, eof);
            this.blockStart = this.strstart;
            this.stream.FlushPending();
        }

        private int DeflateStored(int flush)
        {
            int num1 = (int)ushort.MaxValue;
            if (num1 > this.pendingBufferSize - 5)
                num1 = this.pendingBufferSize - 5;
            while (true)
            {
                if (this.lookahead <= 1)
                {
                    this.FillWindow();
                    if (this.lookahead != 0 || flush != 0)
                    {
                        if (this.lookahead == 0)
                            goto label_13;
                    }
                    else
                        break;
                }
                this.strstart += this.lookahead;
                this.lookahead = 0;
                int num2 = this.blockStart + num1;
                if (this.strstart == 0 || this.strstart >= num2)
                {
                    this.lookahead = this.strstart - num2;
                    this.strstart = num2;
                    this.FlushBlockOnly(false);
                    if (this.stream.OutputCount == 0)
                        goto label_7;
                }
                if (this.strstart - this.blockStart >= this.wSize - Deflater.MinLookahead)
                {
                    this.FlushBlockOnly(false);
                    if (this.stream.OutputCount == 0)
                        goto label_10;
                }
            }
            return 0;
            label_7:
            return 0;
            label_10:
            return 0;
            label_13:
            this.FlushBlockOnly(flush == 4);
            if (this.stream.OutputCount == 0)
                return flush == 4 ? 2 : 0;
            return flush == 4 ? 3 : 1;
        }

        private void TreeSendStoredBlock(int buf, int stored_len, bool eof)
        {
            this.SendBits(eof ? 1 : 0, 3);
            this.CopyBlock(buf, stored_len, true);
        }

        private void TreeFlushBlock(int buf, int stored_len, bool eof)
        {
            int num1 = 0;
            int num2;
            int num3;
            if (this.level > 0)
            {
                if (this.dataType == (byte)2)
                    this.SetDataType();
                this.l_desc.BuildTree(this);
                this.d_desc.BuildTree(this);
                num1 = this.BuildBitLengthTree();
                num2 = Utils.ShiftRight(this.opt_len + 3 + 7, 3);
                num3 = Utils.ShiftRight(this.static_len + 3 + 7, 3);
                if (num3 <= num2)
                    num2 = num3;
            }
            else
                num2 = num3 = stored_len + 5;
            if (stored_len + 4 <= num2 && buf != -1)
                this.TreeSendStoredBlock(buf, stored_len, eof);
            else if (num3 == num2)
            {
                this.SendBits(2 + (eof ? 1 : 0), 3);
                this.CompressBlock(TreeConstants.StaticLTree, TreeConstants.StaticDTree);
            }
            else
            {
                this.SendBits(4 + (eof ? 1 : 0), 3);
                this.SendAllTrees(this.l_desc.MaxCode + 1, this.d_desc.MaxCode + 1, num1 + 1);
                this.CompressBlock(this.dyn_ltree, this.dyn_dtree);
            }
            this.InitializeBlock();
            if (!eof)
                return;
            this.WindupBitBuffer();
        }

        private void FillWindow()
        {
            do
            {
                int size = this.windowSize - this.lookahead - this.strstart;
                if (size == 0 && this.strstart == 0 && this.lookahead == 0)
                    size = this.wSize;
                else if (size == -1)
                    --size;
                else if (this.strstart >= this.wSize + this.wSize - Deflater.MinLookahead)
                {
                    Array.Copy((Array)this.window, this.wSize, (Array)this.window, 0, this.wSize);
                    this.matchStart -= this.wSize;
                    this.strstart -= this.wSize;
                    this.blockStart -= this.wSize;
                    int hashSize = this.hashSize;
                    int index1 = hashSize;
                    do
                    {
                        int num = (int)this.head[--index1] & (int)ushort.MaxValue;
                        this.head[index1] = num >= this.wSize ? (short)(num - this.wSize) : (short)0;
                    }
                    while (--hashSize != 0);
                    int wSize = this.wSize;
                    int index2 = wSize;
                    do
                    {
                        int num = (int)this.prev[--index2] & (int)ushort.MaxValue;
                        this.prev[index2] = num >= this.wSize ? (short)(num - this.wSize) : (short)0;
                    }
                    while (--wSize != 0);
                    size += this.wSize;
                }
                if (this.stream.InputCount != 0)
                {
                    this.lookahead += this.stream.Read(this.window, this.strstart + this.lookahead, size);
                    if (this.lookahead >= 3)
                    {
                        this.ins_h = (int)this.window[this.strstart] & (int)byte.MaxValue;
                        this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[this.strstart + 1] & (int)byte.MaxValue) & this.hashMask;
                    }
                }
                else
                    goto label_16;
            }
            while (this.lookahead < Deflater.MinLookahead && this.stream.InputCount != 0);
            goto label_12;
            label_16:
            return;
            label_12:;
        }

        private int DeflateFast(int flush)
        {
            int cur_match = 0;
            while (true)
            {
                if (this.lookahead < Deflater.MinLookahead)
                {
                    this.FillWindow();
                    if (this.lookahead >= Deflater.MinLookahead || flush != 0)
                    {
                        if (this.lookahead == 0)
                            goto label_20;
                    }
                    else
                        break;
                }
                if (this.lookahead >= 3)
                {
                    this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[this.strstart + 2] & (int)byte.MaxValue) & this.hashMask;
                    cur_match = (int)this.head[this.ins_h] & (int)ushort.MaxValue;
                    this.prev[this.strstart & this.wMask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short)this.strstart;
                }
                if (cur_match != 0 && (this.strstart - cur_match & (int)ushort.MaxValue) <= this.wSize - Deflater.MinLookahead && this.strategy != 2)
                    this.matchLength = this.LongestMatch(cur_match);
                bool flag;
                if (this.matchLength >= 3)
                {
                    flag = this.TallyTree(this.strstart - this.matchStart, this.matchLength - 3);
                    this.lookahead -= this.matchLength;
                    if (this.matchLength <= this.maxLazyMatch && this.lookahead >= 3)
                    {
                        --this.matchLength;
                        do
                        {
                            ++this.strstart;
                            this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[this.strstart + 2] & (int)byte.MaxValue) & this.hashMask;
                            cur_match = (int)this.head[this.ins_h] & (int)ushort.MaxValue;
                            this.prev[this.strstart & this.wMask] = this.head[this.ins_h];
                            this.head[this.ins_h] = (short)this.strstart;
                        }
                        while (--this.matchLength != 0);
                        ++this.strstart;
                    }
                    else
                    {
                        this.strstart += this.matchLength;
                        this.matchLength = 0;
                        this.ins_h = (int)this.window[this.strstart] & (int)byte.MaxValue;
                        this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[this.strstart + 1] & (int)byte.MaxValue) & this.hashMask;
                    }
                }
                else
                {
                    flag = this.TallyTree(0, (int)this.window[this.strstart] & (int)byte.MaxValue);
                    --this.lookahead;
                    ++this.strstart;
                }
                if (flag)
                {
                    this.FlushBlockOnly(false);
                    if (this.stream.OutputCount == 0)
                        goto label_17;
                }
            }
            return 0;
            label_17:
            return 0;
            label_20:
            this.FlushBlockOnly(flush == 4);
            if (this.stream.OutputCount != 0)
                return flush == 4 ? 3 : 1;
            return flush == 4 ? 2 : 0;
        }

        private int DeflateSlow(int flush)
        {
            int cur_match = 0;
            while (true)
            {
                if (this.lookahead < Deflater.MinLookahead)
                {
                    this.FillWindow();
                    if (this.lookahead >= Deflater.MinLookahead || flush != 0)
                    {
                        if (this.lookahead == 0)
                            goto label_28;
                    }
                    else
                        break;
                }
                if (this.lookahead >= 3)
                {
                    this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[this.strstart + 2] & (int)byte.MaxValue) & this.hashMask;
                    cur_match = (int)this.head[this.ins_h] & (int)ushort.MaxValue;
                    this.prev[this.strstart & this.wMask] = this.head[this.ins_h];
                    this.head[this.ins_h] = (short)this.strstart;
                }
                this.prevLength = this.matchLength;
                this.prevMatch = this.matchStart;
                this.matchLength = 2;
                if (cur_match != 0 && this.prevLength < this.maxLazyMatch && (this.strstart - cur_match & (int)ushort.MaxValue) <= this.wSize - Deflater.MinLookahead)
                {
                    if (this.strategy != 2)
                        this.matchLength = this.LongestMatch(cur_match);
                    if (this.matchLength <= 5 && (this.strategy == 1 || this.matchLength == 3 && this.strstart - this.matchStart > 4096))
                        this.matchLength = 2;
                }
                if (this.prevLength >= 3 && this.matchLength <= this.prevLength)
                {
                    int num = this.strstart + this.lookahead - 3;
                    bool flag = this.TallyTree(this.strstart - 1 - this.prevMatch, this.prevLength - 3);
                    this.lookahead -= this.prevLength - 1;
                    this.prevLength -= 2;
                    do
                    {
                        if (++this.strstart <= num)
                        {
                            this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[this.strstart + 2] & (int)byte.MaxValue) & this.hashMask;
                            cur_match = (int)this.head[this.ins_h] & (int)ushort.MaxValue;
                            this.prev[this.strstart & this.wMask] = this.head[this.ins_h];
                            this.head[this.ins_h] = (short)this.strstart;
                        }
                    }
                    while (--this.prevLength != 0);
                    this.matchAvailable = 0;
                    this.matchLength = 2;
                    ++this.strstart;
                    if (flag)
                    {
                        this.FlushBlockOnly(false);
                        if (this.stream.OutputCount == 0)
                            goto label_19;
                    }
                }
                else if (this.matchAvailable != 0)
                {
                    if (this.TallyTree(0, (int)this.window[this.strstart - 1] & (int)byte.MaxValue))
                        this.FlushBlockOnly(false);
                    ++this.strstart;
                    --this.lookahead;
                    if (this.stream.OutputCount == 0)
                        goto label_24;
                }
                else
                {
                    this.matchAvailable = 1;
                    ++this.strstart;
                    --this.lookahead;
                }
            }
            return 0;
            label_19:
            return 0;
            label_24:
            return 0;
            label_28:
            if (this.matchAvailable != 0)
            {
                this.TallyTree(0, (int)this.window[this.strstart - 1] & (int)byte.MaxValue);
                this.matchAvailable = 0;
            }
            this.FlushBlockOnly(flush == 4);
            if (this.stream.OutputCount != 0)
                return flush == 4 ? 3 : 1;
            return flush == 4 ? 2 : 0;
        }

        private int LongestMatch(int cur_match)
        {
            int maxChainLength = this.maxChainLength;
            int index1 = this.strstart;
            int num1 = this.prevLength;
            int num2 = this.strstart > this.wSize - Deflater.MinLookahead ? this.strstart - (this.wSize - Deflater.MinLookahead) : 0;
            int num3 = this.niceMatch;
            int wMask = this.wMask;
            int num4 = this.strstart + 258;
            byte num5 = this.window[index1 + num1 - 1];
            byte num6 = this.window[index1 + num1];
            if (this.prevLength >= this.goodMatch)
                maxChainLength >>= 2;
            if (num3 > this.lookahead)
                num3 = this.lookahead;
            do
            {
                int index2 = cur_match;
                if ((int)this.window[index2 + num1] == (int)num6 && (int)this.window[index2 + num1 - 1] == (int)num5 && (int)this.window[index2] == (int)this.window[index1] && (int)this.window[++index2] == (int)this.window[index1 + 1])
                {
                    int num7 = index1 + 2;
                    int num8 = index2 + 1;
                    do
                        ;
                    while ((int)this.window[++num7] == (int)this.window[++num8] && (int)this.window[++num7] == (int)this.window[++num8] && ((int)this.window[++num7] == (int)this.window[++num8] && (int)this.window[++num7] == (int)this.window[++num8]) && ((int)this.window[++num7] == (int)this.window[++num8] && (int)this.window[++num7] == (int)this.window[++num8] && ((int)this.window[++num7] == (int)this.window[++num8] && (int)this.window[++num7] == (int)this.window[++num8])) && num7 < num4);
                    int num9 = 258 - (num4 - num7);
                    index1 = num4 - 258;
                    if (num9 > num1)
                    {
                        this.matchStart = cur_match;
                        num1 = num9;
                        if (num9 < num3)
                        {
                            num5 = this.window[index1 + num1 - 1];
                            num6 = this.window[index1 + num1];
                        }
                        else
                            break;
                    }
                }
            }
            while ((cur_match = (int)this.prev[cur_match & wMask] & (int)ushort.MaxValue) > num2 && --maxChainLength != 0);
            if (num1 <= this.lookahead)
                return num1;
            return this.lookahead;
        }

        private int Initialize(
          CompressionStream strm,
          int level,
          int method,
          int windowBits,
          int memLevel,
          int strategy)
        {
            int num = 0;
            strm.ErrorMessage = (string)null;
            if (level == -1)
                level = 6;
            if (windowBits < 0)
            {
                num = 1;
                windowBits = -windowBits;
            }
            if (memLevel < 1 || memLevel > 9 || (method != 8 || windowBits < 9) || (windowBits > 15 || level < 0 || (level > 9 || strategy < 0)) || strategy > 2)
                return -2;
            strm.Deflater = this;
            this.noheader = num;
            this.wBits = windowBits;
            this.wSize = 1 << this.wBits;
            this.wMask = this.wSize - 1;
            this.hashBits = memLevel + 7;
            this.hashSize = 1 << this.hashBits;
            this.hashMask = this.hashSize - 1;
            this.hashShift = (this.hashBits + 3 - 1) / 3;
            this.window = new byte[this.wSize * 2];
            this.prev = new short[this.wSize];
            this.head = new short[this.hashSize];
            this.litBufsize = 1 << memLevel + 6;
            this.pendingBuffer = new byte[this.litBufsize * 4];
            this.pendingBufferSize = this.litBufsize * 4;
            this.d_buf = this.litBufsize;
            this.litBuf = 3 * this.litBufsize;
            this.level = (int)level;
            this.strategy = strategy;
            this.method = (byte)method;
            return this.Reset(strm);
        }

        private int Reset(CompressionStream strm)
        {
            strm.InputTotal = strm.OutputTotal = 0L;
            strm.ErrorMessage = (string)null;
            strm.DataType = 2;
            this.pending = 0;
            this.pendingOutput = 0;
            if (this.noheader < 0)
                this.noheader = 0;
            this.status = this.noheader != 0 ? 113 : 42;
            strm.Checksum = new Adler32();
            this.lastFlush = 0;
            this.InitializeTree();
            this.InitializeMatch();
            return 0;
        }

        private static bool IsSmaller(short[] tree, int n, int m, byte[] depth)
        {
            short num1 = tree[n * 2];
            short num2 = tree[m * 2];
            return (int)num1 < (int)num2 || (int)num1 == (int)num2 && (int)depth[n] <= (int)depth[m];
        }

        public void RestoreHeapDown(short[] tree, int k)
        {
            int n = this.heap[k];
            for (int index = k << 1; index <= this.heapLen; index <<= 1)
            {
                if (index < this.heapLen && Deflater.IsSmaller(tree, this.heap[index + 1], this.heap[index], this.depth))
                    ++index;
                if (!Deflater.IsSmaller(tree, n, this.heap[index], this.depth))
                {
                    this.heap[k] = this.heap[index];
                    k = index;
                }
                else
                    break;
            }
            this.heap[k] = n;
        }

        public int Initialize(CompressionStream strm, CompressionLevel level, int bits)
        {
            return this.Initialize(strm, (int)level, (int)CompressionMethod.Deflated, bits, 8, 0);
        }

        public int Initialize(CompressionStream strm, CompressionLevel level)
        {
            return this.Initialize(strm, level, 15);
        }

        public int End()
        {
            if (this.status != 42 && this.status != 113 && this.status != 666)
                return -2;
            this.pendingBuffer = (byte[])null;
            this.head = (short[])null;
            this.prev = (short[])null;
            this.window = (byte[])null;
            return this.status == 113 ? -3 : 0;
        }

        public int SetParameters(CompressionStream strm, int level, int strategy)
        {
            int num = 0;
            if (level == -1)
                level = 6;
            if (level < 0 || level > 9 || strategy < 0 || strategy > 2)
                return -2;
            if (Deflater.Options[this.level].Function != Deflater.Options[level].Function && strm.InputTotal != 0L)
                num = strm.Deflate(1);
            if (this.level != level)
            {
                this.level = level;
                this.maxLazyMatch = Deflater.Options[this.level].MaxLazy;
                this.goodMatch = Deflater.Options[this.level].GoodLength;
                this.niceMatch = Deflater.Options[this.level].NiceLength;
                this.maxChainLength = Deflater.Options[this.level].MaxChain;
            }
            this.strategy = strategy;
            return num;
        }

        public int SetDictionary(CompressionStream strm, byte[] dictionary, int dictLength)
        {
            int length = dictLength;
            int sourceIndex = 0;
            if (dictionary == null || this.status != 42)
                return -2;
            strm.Checksum.Add(dictionary, 0, dictLength);
            if (length < 3)
                return 0;
            if (length > this.wSize - Deflater.MinLookahead)
            {
                length = this.wSize - Deflater.MinLookahead;
                sourceIndex = dictLength - length;
            }
            Array.Copy((Array)dictionary, sourceIndex, (Array)this.window, 0, length);
            this.strstart = length;
            this.blockStart = length;
            this.ins_h = (int)this.window[0] & (int)byte.MaxValue;
            this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[1] & (int)byte.MaxValue) & this.hashMask;
            for (int index = 0; index <= length - 3; ++index)
            {
                this.ins_h = (this.ins_h << this.hashShift ^ (int)this.window[index + 2] & (int)byte.MaxValue) & this.hashMask;
                this.prev[index & this.wMask] = this.head[this.ins_h];
                this.head[this.ins_h] = (short)index;
            }
            return 0;
        }

        public int Deflate(CompressionStream strm, int flush)
        {
            if (flush > 4 || flush < 0)
                return -2;
            if (strm.Output == null || strm.Input == null && strm.InputCount != 0 || this.status == 666 && flush != 4)
            {
                strm.ErrorMessage = Deflater.errmsg[4];
                return -2;
            }
            if (strm.OutputCount == 0)
            {
                strm.ErrorMessage = Deflater.errmsg[7];
                return -5;
            }
            this.stream = strm;
            int lastFlush = this.lastFlush;
            this.lastFlush = flush;
            if (this.status == 42)
            {
                int num1 = 8 + (this.wBits - 8 << 4) << 8;
                int num2 = (this.level - 1 & (int)byte.MaxValue) >> 1;
                if (num2 > 3)
                    num2 = 3;
                int num3 = num1 | num2 << 6;
                if (this.strstart != 0)
                    num3 |= 32;
                int b = num3 + (31 - num3 % 31);
                this.status = 113;
                this.WriteShortMSB(b);
                if (this.strstart != 0)
                {
                    this.WriteShortMSB((int)Utils.ShiftRight(strm.Checksum.Get(), 16));
                    this.WriteShortMSB((int)(strm.Checksum.Get() & (long)ushort.MaxValue));
                }
                strm.Checksum = new Adler32();
            }
            if (this.pending != 0)
            {
                strm.FlushPending();
                if (strm.OutputCount == 0)
                {
                    this.lastFlush = -1;
                    return 0;
                }
            }
            else if (strm.InputCount == 0 && flush <= lastFlush && flush != 4)
            {
                strm.ErrorMessage = Deflater.errmsg[7];
                return -5;
            }
            if (this.status == 666 && strm.InputCount != 0)
            {
                strm.ErrorMessage = Deflater.errmsg[7];
                return -5;
            }
            if (strm.InputCount != 0 || this.lookahead != 0 || flush != 0 && this.status != 666)
            {
                int num = -1;
                switch (Deflater.Options[this.level].Function)
                {
                    case DeflaterFunction.Stored:
                        num = this.DeflateStored(flush);
                        break;
                    case DeflaterFunction.Fast:
                        num = this.DeflateFast(flush);
                        break;
                    case DeflaterFunction.Slow:
                        num = this.DeflateSlow(flush);
                        break;
                }
                if (num == 2 || num == 3)
                    this.status = 666;
                if (num == 0 || num == 2)
                {
                    if (strm.OutputCount == 0)
                        this.lastFlush = -1;
                    return 0;
                }
                if (num == 1)
                {
                    if (flush == 1)
                    {
                        this.AlignTree();
                    }
                    else
                    {
                        this.TreeSendStoredBlock(0, 0, false);
                        if (flush == 3)
                        {
                            for (int index = 0; index < this.hashSize; ++index)
                                this.head[index] = (short)0;
                        }
                    }
                    strm.FlushPending();
                    if (strm.OutputCount == 0)
                    {
                        this.lastFlush = -1;
                        return 0;
                    }
                }
            }
            if (flush != 4)
                return 0;
            if (this.noheader != 0)
                return 1;
            this.WriteShortMSB((int)Utils.ShiftRight(strm.Checksum.Get(), 16));
            this.WriteShortMSB((int)(strm.Checksum.Get() & (long)ushort.MaxValue));
            strm.FlushPending();
            this.noheader = -1;
            return this.pending != 0 ? 0 : 1;
        }
    }
}
