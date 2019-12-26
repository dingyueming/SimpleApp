using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common
{
    public class Range
    {
        /// <summary>
        /// 最小索引
        /// </summary>
        public int MinIndex { get; set; }
        /// <summary>
        /// 最大索引
        /// </summary>
        public int MaxIndex { get; set; }

        public Range() { }

        public Range(int minIndex, int maxIndex)
        {
            this.MinIndex = minIndex;
            this.MaxIndex = maxIndex;
        }

        public Range(int minIndex, uint lenght)
        {
            this.MinIndex = minIndex;
            this.MaxIndex = minIndex + (int)lenght - 1;
        }

        public int Length { get { return MaxIndex - MinIndex + 1; } }
    }

    public class CRange<TDimension>
    {
        private TDimension fromValue;
        private TDimension toValue;

        public CRange(TDimension fromValue, TDimension toValue)
        {
            this.fromValue = fromValue;
            this.toValue = toValue;
        }

        public TDimension From
        {
            get
            {
                return this.fromValue;
            }
        }

        public TDimension To
        {
            get
            {
                return this.toValue;
            }
        }
    }
}
