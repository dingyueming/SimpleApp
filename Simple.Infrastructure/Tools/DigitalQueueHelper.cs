using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.Tools
{
    public class DigitalQueueHelper
    {
        private int msgSeq;
        public DigitalQueueHelper()
        {
            msgSeq = 0;
        }
        public int NextNumber()
        {
            if (msgSeq == 65535)
            {
                msgSeq = 0;
            }
            msgSeq++;
            return msgSeq;
        }
    }
}
