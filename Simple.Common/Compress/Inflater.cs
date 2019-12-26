using Common.CheckSum;

namespace Common.Compress
{
    public sealed class Inflater
    {
        private static byte[] mark = new byte[4]
        {
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue
        };
        private long[] was = new long[1];
        private InflaterState state;
        private ErrorCode startError;
        private ErrorCode error;
        private int method;
        private uint need;
        private int marker;
        private int nowrap;
        private int wbits;
        private InflaterBlocks blocks;

        private int Reset(CompressionStream stream)
        {
            if (stream == null)
                return -2;
            stream.InputTotal = 0L;
            stream.OutputTotal = 0L;
            stream.ErrorMessage = (string)null;
            this.state = this.nowrap != 0 ? InflaterState.Blocks : InflaterState.Method;
            this.blocks.Reset(stream, (long[])null);
            return 0;
        }

        private int GetSyncPoint(CompressionStream stream)
        {
            if (stream == null || this.blocks == null)
                return -2;
            return this.blocks.GetSyncPoint();
        }

        private bool HandleMethodState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            if (((this.method = (int)stream.Input[stream.InputIndex++]) & 15) != 8)
            {
                this.state = InflaterState.Error;
                stream.ErrorMessage = "unknown compression method";
                this.marker = 5;
                return true;
            }
            if ((this.method >> 4) + 8 > this.wbits)
            {
                this.state = InflaterState.Error;
                stream.ErrorMessage = "invalid window size";
                this.marker = 5;
                return true;
            }
            this.state = InflaterState.Flag;
            return true;
        }

        private bool HandleFlagState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            int num = (int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue;
            if (((this.method << 8) + num) % 31 != 0)
            {
                this.state = InflaterState.Error;
                stream.ErrorMessage = "incorrect header check";
                this.marker = 5;
                return true;
            }
            if ((num & 32) == 0)
            {
                this.state = InflaterState.Blocks;
                return true;
            }
            this.state = InflaterState.DictionaryFour;
            return true;
        }

        private bool HandleDictionaryFourState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need = (uint)(((int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue) << 24 & -16777216);
            this.state = InflaterState.DictionaryThree;
            return true;
        }

        private bool HandleDictionaryThreeState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need += (uint)((ulong)(((int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue) << 16) & 16711680UL);
            this.state = InflaterState.DictionaryTwo;
            return true;
        }

        private bool HandleDictionaryTwoState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need += (uint)((ulong)(((int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue) << 8) & 65280UL);
            this.state = InflaterState.DictionaryOne;
            return true;
        }

        private bool HandleDictionaryOneState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need += (uint)((ulong)stream.Input[stream.InputIndex++] & (ulong)byte.MaxValue);
            stream.Checksum = new Adler32(this.need);
            this.state = InflaterState.DictionaryZero;
            result = ErrorCode.NeedsDictionary;
            return false;
        }

        private bool HandleDictionaryZeroState(CompressionStream stream, out ErrorCode result)
        {
            this.state = InflaterState.Error;
            stream.ErrorMessage = "need dictionary";
            this.marker = 0;
            result = ErrorCode.StreamError;
            return false;
        }

        private bool HandleBlocksState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            this.error = (ErrorCode)this.blocks.Process(stream, (int)this.error);
            if (this.error == ErrorCode.DataError)
            {
                this.state = InflaterState.Error;
                this.marker = 0;
                return true;
            }
            if (this.error == ErrorCode.Ok)
                this.error = this.startError;
            if (this.error != ErrorCode.StreamEnd)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            this.blocks.Reset(stream, this.was);
            if (this.nowrap != 0)
            {
                this.state = InflaterState.Done;
                return true;
            }
            this.state = InflaterState.CheckFour;
            return true;
        }

        private bool HandleCheckFourState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need = (uint)(((int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue) << 24 & -16777216);
            this.state = InflaterState.CheckThree;
            return true;
        }

        private bool HandleCheckThreeState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need += (uint)((ulong)(((int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue) << 16) & 16711680UL);
            this.state = InflaterState.CheckTwo;
            return true;
        }

        private bool HandleCheckTwoState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need += (uint)((ulong)(((int)stream.Input[stream.InputIndex++] & (int)byte.MaxValue) << 8) & 65280UL);
            this.state = InflaterState.CheckOne;
            return true;
        }

        private bool HandleCheckOneState(CompressionStream stream, out ErrorCode result)
        {
            result = ErrorCode.Ok;
            if (stream.InputCount == 0)
            {
                result = this.error;
                return false;
            }
            this.error = this.startError;
            --stream.InputCount;
            ++stream.InputTotal;
            this.need += (uint)((ulong)stream.Input[stream.InputIndex++] & (ulong)byte.MaxValue);
            if ((int)this.was[0] != (int)this.need)
            {
                this.state = InflaterState.Error;
                stream.ErrorMessage = "incorrect data check";
                this.marker = 5;
                return true;
            }
            this.state = InflaterState.Done;
            return true;
        }

        public int Initialize(CompressionStream stream, int windowSize)
        {
            stream.ErrorMessage = (string)null;
            this.blocks = (InflaterBlocks)null;
            this.nowrap = 0;
            if (windowSize < 0)
            {
                windowSize = -windowSize;
                this.nowrap = 1;
            }
            if (windowSize < 8 || windowSize > 15)
            {
                this.End(stream);
                return -2;
            }
            this.wbits = windowSize;
            Inflater inflater = this.nowrap != 0 ? (Inflater)null : this;
            this.blocks = new InflaterBlocks(stream, (object)inflater, 1 << windowSize);
            this.Reset(stream);
            return 0;
        }

        public int Inflate(CompressionStream stream, int flush)
        {
            if (stream == null || stream.Input == null)
                return -2;
            this.startError = flush == 4 ? ErrorCode.BufferError : ErrorCode.Ok;
            this.error = ErrorCode.BufferError;
            ErrorCode result = ErrorCode.Ok;
            while (true)
            {
                switch (this.state)
                {
                    case InflaterState.Method:
                        if (this.HandleMethodState(stream, out result))
                            break;
                        goto label_4;
                    case InflaterState.Flag:
                        if (this.HandleFlagState(stream, out result))
                            break;
                        goto label_6;
                    case InflaterState.DictionaryFour:
                        if (this.HandleDictionaryFourState(stream, out result))
                            break;
                        goto label_8;
                    case InflaterState.DictionaryThree:
                        if (this.HandleDictionaryThreeState(stream, out result))
                            break;
                        goto label_10;
                    case InflaterState.DictionaryTwo:
                        if (this.HandleDictionaryTwoState(stream, out result))
                            break;
                        goto label_12;
                    case InflaterState.DictionaryOne:
                        if (this.HandleDictionaryOneState(stream, out result))
                            break;
                        goto label_14;
                    case InflaterState.DictionaryZero:
                        if (this.HandleDictionaryZeroState(stream, out result))
                            break;
                        goto label_16;
                    case InflaterState.Blocks:
                        if (this.HandleBlocksState(stream, out result))
                            break;
                        goto label_18;
                    case InflaterState.CheckFour:
                        if (this.HandleCheckFourState(stream, out result))
                            break;
                        goto label_20;
                    case InflaterState.CheckThree:
                        if (this.HandleCheckThreeState(stream, out result))
                            break;
                        goto label_22;
                    case InflaterState.CheckTwo:
                        if (this.HandleCheckThreeState(stream, out result))
                            break;
                        goto label_24;
                    case InflaterState.CheckOne:
                        if (this.HandleCheckOneState(stream, out result))
                            break;
                        goto label_26;
                    case InflaterState.Done:
                        goto label_27;
                    case InflaterState.Error:
                        goto label_28;
                    default:
                        goto label_29;
                }
            }
            label_4:
            return (int)result;
            label_6:
            return (int)result;
            label_8:
            return (int)result;
            label_10:
            return (int)result;
            label_12:
            return (int)result;
            label_14:
            return (int)result;
            label_16:
            return (int)result;
            label_18:
            return (int)result;
            label_20:
            return (int)result;
            label_22:
            return (int)result;
            label_24:
            return (int)result;
            label_26:
            return (int)result;
            label_27:
            return 1;
            label_28:
            return -3;
            label_29:
            return -2;
        }

        public int SetDictionary(CompressionStream stream, byte[] dictionary, int dictLength)
        {
            int start = 0;
            int n = dictLength;
            if (stream == null || this.state != InflaterState.DictionaryZero)
                return -2;
            if (Adler32.Get(dictionary, 0, dictLength) != stream.Checksum.Get())
                return -3;
            stream.Checksum = new Adler32();
            if (n >= 1 << this.wbits)
            {
                n = (1 << this.wbits) - 1;
                start = dictLength - n;
            }
            this.blocks.SetDictionary(dictionary, start, n);
            this.state = InflaterState.Blocks;
            return 0;
        }

        public int Sync(CompressionStream stream)
        {
            if (stream == null)
                return -2;
            if (this.state != InflaterState.Error)
            {
                this.state = InflaterState.Error;
                this.marker = 0;
            }
            int inputCount = stream.InputCount;
            if (inputCount == 0)
                return -5;
            int inputIndex = stream.InputIndex;
            int index;
            for (index = this.marker; inputCount != 0 && index < 4; --inputCount)
            {
                if ((int)stream.Input[inputIndex] == (int)Inflater.mark[index])
                    ++index;
                else
                    index = stream.Input[inputIndex] == (byte)0 ? 4 - index : 0;
                ++inputIndex;
            }
            stream.InputTotal += (long)(inputIndex - stream.InputIndex);
            stream.InputIndex = inputIndex;
            stream.InputCount = inputCount;
            this.marker = index;
            if (index != 4)
                return -3;
            long inputTotal = stream.InputTotal;
            long outputTotal = stream.OutputTotal;
            this.Reset(stream);
            stream.InputTotal = inputTotal;
            stream.OutputTotal = outputTotal;
            this.state = InflaterState.Blocks;
            return 0;
        }

        public int End(CompressionStream stream)
        {
            if (this.blocks != null)
                this.blocks.Free(stream);
            this.blocks = (InflaterBlocks)null;
            return 0;
        }
    }

    internal enum InflaterState
    {
        Method,
        Flag,
        DictionaryFour,
        DictionaryThree,
        DictionaryTwo,
        DictionaryOne,
        DictionaryZero,
        Blocks,
        CheckFour,
        CheckThree,
        CheckTwo,
        CheckOne,
        Done,
        Error,
    }
}
