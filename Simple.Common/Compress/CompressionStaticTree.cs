namespace Common.Compress
{
    internal sealed class CompressionStaticTree
    {
        private static CompressionStaticTree staticLTreeDescription = new CompressionStaticTree(TreeConstants.StaticLTree, TreeConstants.ExtraLBits, 257, TreeConstants.LCodes, 15);
        private static CompressionStaticTree staticDTreeDescription = new CompressionStaticTree(TreeConstants.StaticDTree, TreeConstants.ExtraDBits, 0, 30, 15);
        private static CompressionStaticTree staticBLDescription = new CompressionStaticTree((short[])null, TreeConstants.ExtraBLBits, 0, 19, 7);
        private short[] staticTreeData;
        private int[] extraBits;
        private int extraBase;
        private int maxElements;
        private int maxLength;

        public CompressionStaticTree(
          short[] staticTreeData,
          int[] extraBits,
          int extraBase,
          int elements,
          int maxLength)
        {
            this.staticTreeData = staticTreeData;
            this.extraBits = extraBits;
            this.extraBase = extraBase;
            this.maxElements = elements;
            this.maxLength = maxLength;
        }

        public static CompressionStaticTree StaticLTreeDescription
        {
            get
            {
                return CompressionStaticTree.staticLTreeDescription;
            }
        }

        public static CompressionStaticTree StaticDTreeDescription
        {
            get
            {
                return CompressionStaticTree.staticDTreeDescription;
            }
        }

        public static CompressionStaticTree StaticBLDescription
        {
            get
            {
                return CompressionStaticTree.staticBLDescription;
            }
        }

        public short[] StaticTreeData
        {
            get
            {
                return this.staticTreeData;
            }
            set
            {
                this.staticTreeData = value;
            }
        }

        public int[] ExtraBits
        {
            get
            {
                return this.extraBits;
            }
            set
            {
                this.extraBits = value;
            }
        }

        public int ExtraBase
        {
            get
            {
                return this.extraBase;
            }
            set
            {
                this.extraBase = value;
            }
        }

        public int MaxElements
        {
            get
            {
                return this.maxElements;
            }
            set
            {
                this.maxElements = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                this.maxLength = value;
            }
        }
    }

}
