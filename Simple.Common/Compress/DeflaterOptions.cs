namespace Common.Compress
{
    public class DeflaterOptions
    {
        private int goodLength;
        private int maxLazy;
        private int niceLength;
        private int maxChain;
        private DeflaterFunction function;

        public DeflaterOptions(
            int goodLength,
            int maxLazy,
            int niceLength,
            int maxChain,
            DeflaterFunction function)
        {
            this.goodLength = goodLength;
            this.maxLazy = maxLazy;
            this.niceLength = niceLength;
            this.maxChain = maxChain;
            this.function = function;
        }

        public int GoodLength
        {
            get
            {
                return this.goodLength;
            }
            set
            {
                this.goodLength = value;
            }
        }

        public int MaxLazy
        {
            get
            {
                return this.maxLazy;
            }
            set
            {
                this.maxLazy = value;
            }
        }

        public int NiceLength
        {
            get
            {
                return this.niceLength;
            }
            set
            {
                this.niceLength = value;
            }
        }

        public int MaxChain
        {
            get
            {
                return this.maxChain;
            }
            set
            {
                this.maxChain = value;
            }
        }

        public DeflaterFunction Function
        {
            get
            {
                return this.function;
            }
            set
            {
                this.function = value;
            }
        }
    }

    public enum DeflaterFunction
    {
        Stored,
        Fast,
        Slow,
    }
}
