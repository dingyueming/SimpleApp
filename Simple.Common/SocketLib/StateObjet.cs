using Simple.Common;
using System;

namespace Simple.Common.SocketLib
{
    /// <summary>
    /// Socket接收对象
    /// </summary>
    public class StateObject
    {
        public readonly object State;

        public StateObject()
        {
            _buffer = new byte[_bufferSize];
        }

        public StateObject(object state) : this()
        {
            this.State = state;
        }

        private readonly int _bufferSize = 1024 * 1024;
        // Size of receive buffer.
        public int BufferSize
        {
            get { return _bufferSize; }
            //set { _bufferSize = value; }
        }

        private readonly byte[] _buffer;
        // Receive buffer.
        public byte[] Buffer
        {
            get
            {
                //if (_buffer == null)
                //_buffer = new byte[_bufferSize];
                return _buffer;
            }
            //set { _buffer = value; }
        }

        private TQueue<byte> _bufferQueue;
        private TQueue<byte> _tempBufferQueue;

        public TQueue<byte> BufferQueue
        {
            get
            {
                if (_bufferQueue == null)
                    _bufferQueue = new TQueue<byte>();
                return _bufferQueue;
            }
        }

        public TQueue<byte> TempBufferQueue
        {
            get
            {
                if (_tempBufferQueue == null)
                    _tempBufferQueue = new TQueue<byte>();
                return _tempBufferQueue;
            }
        }

        public void EnqueueBufferQueue(int length, int index = 0)
        {
            if (index + length > BufferSize)
                throw new Exception("超过Buffer长度");

            BufferQueue.Lock();
            try
            {
                for (var i = index; i < length; i++)
                {
                    BufferQueue.Enqueue(Buffer[i], true);
                }
            }
            catch
            {

            }
            BufferQueue.UnLock();
            BufferQueue.EnqueueEvent.Set();
        }
        // Received data string.
        //	public StringBuilder sb = new StringBuilder();
    }


}
