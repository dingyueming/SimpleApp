using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common.SocketLib
{
    public abstract class ISocket : IDisposable
    {
        public AddressFamily AddressFamily { get; }
        public SocketType SocketType { get; }
        public ProtocolType ProtocolType { get; }

        public ISocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            AddressFamily = addressFamily;
            SocketType = socketType;
            ProtocolType = protocolType;
        }

        //
        // 摘要:
        //     获取或设置一个值，该值指定 System.Net.Sockets.Socket 发送缓冲区的大小。
        //
        // 返回结果:
        //     System.Int32，它包含发送缓冲区的大小（以字节为单位）。默认值为 8192。
        //
        public abstract int SendBufferSize { get; set; }

        public abstract bool SendAsync(ISocketAsyncEventArgs e);
        //
        // 摘要:
        //     获取或设置一个值，它指定 System.Net.Sockets.Socket 接收缓冲区的大小。
        //
        // 返回结果:
        //     System.Int32，它包含接收缓冲区的大小（以字节为单位）。默认值为 8192。
        //
        public abstract int ReceiveBufferSize { get; set; }
        //
        // 摘要:
        //     获取或设置一个值，指定 System.Net.Sockets.Socket 发送的 Internet 协议 (IP) 数据包的生存时间 (TTL) 值。
        //
        // 返回结果:
        //     TTL 值。
        //
        public abstract short Ttl { get; set; }
        /// <summary>
        /// 开始一个对远程主机连接的异步请求。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public abstract bool ConnectAsync(ISocketAsyncEventArgs e);
        /// <summary>
        /// 开始一个异步请求以便从连接的 <see cref="T:System.Net.Sockets.Socket" /> 对象中接收数据。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public abstract bool ReceiveAsync(ISocketAsyncEventArgs e);

        /// <summary>
        /// 禁用某 System.Net.Sockets.Socket 上的发送和接收。
        /// </summary>
        /// <param name="how"></param>
        public abstract void Shutdown(SocketShutdown how);

        public abstract void Dispose();

        public abstract void Close();

    }
}
