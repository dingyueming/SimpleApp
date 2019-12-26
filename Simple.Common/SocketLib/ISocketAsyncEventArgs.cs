using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common.SocketLib
{
    public abstract class ISocketAsyncEventArgs : EventArgs, IDisposable
    {
        public abstract SocketProtocol SocketProtocol { get; set; }
        public event EventHandler<ISocketAsyncEventArgs> Completed;
  
        protected virtual void OnCompleted(ISocketAsyncEventArgs e)
        {
            Completed?.Invoke(e.CurrentSocket, e);
        }

        public abstract object SocketAsyncEventArgs { get; }

        public ISocket CurrentSocket { get; set; }

        public abstract SocketError SocketError { get; set; }

        public abstract SocketAsyncOperation LastOperation { get; }

        public abstract string RemoteIP { get; set; }
        public abstract int RemotePort { get; set; }

        public abstract object UserToken { get; set; }
        public abstract void SetBuffer(byte[] buffer, int offset, int count);
        public abstract int BytesTransferred { get; }
        public abstract void Dispose();              
    }
}
