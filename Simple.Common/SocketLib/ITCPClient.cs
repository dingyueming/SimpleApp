using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Simple.Common.SocketLib
{
    public abstract class ITCPClient
    {
        private ISocketAsyncEventArgs _socketAsyncEventArgs;  //Socket异步连接参数
        private bool _connSuccess = false;   //是否成功连接服务器
        private ISocket _sokClient = null; //负责与服务端通信的套接字

        public event Action ConnectSuccess;
        public event Action SocketClosed;
        public event Action SocketDisconnected;
        public bool ConnSuccess { get { return _connSuccess; } }

        public ITCPClient()
        {

        }

        public abstract ISocketAsyncEventArgs GetISocketAsyncEventArgs();
        protected abstract ISocket GetISocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType);

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        /// <param name="state">异步状态对象</param>
        /// <returns></returns>
        public bool ConnectServer(string ip, int port, StateObject state)
        {
            try
            {
                if (_socketAsyncEventArgs != null)
                {
                    _socketAsyncEventArgs.Completed -= OnSocketAsyncCompleted;
                    _socketAsyncEventArgs.Dispose();
                    _socketAsyncEventArgs = null;
                }
                //连接 服务端监听套接字
                if (_socketAsyncEventArgs == null)
                {
                    _socketAsyncEventArgs = GetISocketAsyncEventArgs();
                }

                _socketAsyncEventArgs.SocketProtocol = SocketProtocol.Tcp;
                _socketAsyncEventArgs.Completed -= OnSocketAsyncCompleted;
                _socketAsyncEventArgs.Completed += OnSocketAsyncCompleted;
                _socketAsyncEventArgs.UserToken = state;

                // 包含 ip和port 
                _socketAsyncEventArgs.RemoteIP = ip;
                _socketAsyncEventArgs.RemotePort = port;

                //实例化 套接字
                _sokClient = GetISocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _sokClient.SendBufferSize = 1024 * 10;
                _sokClient.ReceiveBufferSize = 1024 * 100;
                _sokClient.Ttl = 64;
                _socketAsyncEventArgs.CurrentSocket = _sokClient;

                return _sokClient.ConnectAsync(_socketAsyncEventArgs);
            }
            catch (Exception ex)
            {
                CloseSocket();
                GC.Collect();
                return false;
            }
        }

        /// <summary>
        /// 连接完成回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSocketAsyncCompleted(object sender, ISocketAsyncEventArgs e)
        {
            bool stopReceive = false;  //是否停止数据接收
            StateObject state = e.UserToken as StateObject;
            try
            {
                if (e.SocketError == SocketError.Success)
                {
                    if (e.LastOperation == SocketAsyncOperation.Connect)
                    {
                        _connSuccess = true;
                        ConnectSuccess?.Invoke();
                    }
                    else if (e.LastOperation == SocketAsyncOperation.Receive)
                    {
                        if (state != null)
                        {
                            //服务器发来的消息
                            int length = e.BytesTransferred;
                            state.EnqueueBufferQueue(length); //存储接受过来的数据
                        }
                    }
                }
                else if (e.SocketError == SocketError.ConnectionReset)
                {
                    System.Diagnostics.Debug.WriteLine(e.SocketError);
                    stopReceive = true;
                    SocketDisconnected?.Invoke();
                }
                else if (e.SocketError == SocketError.ConnectionAborted || e.SocketError == SocketError.OperationAborted)
                {
                    stopReceive = true;
                }
                else
                {
                    stopReceive = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is ObjectDisposedException)
                {
                    stopReceive = true;
                    return; //停止接收数据
                }
            }
            finally
            {
                try
                {
                    if (!stopReceive)
                    {
                        if (state != null)
                        {
                            //继续接收消息
                            e.SetBuffer(state.Buffer, 0, state.BufferSize);
                            e.CurrentSocket.ReceiveAsync(e);
                        }
                    }
                    else
                    {
                        e.Dispose();
                        e = null;
                        state = null;
                        CloseSocket();
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is ObjectDisposedException))
                    {
                        CloseSocket();
                        GC.Collect();
                    }
                }

            }
        }

        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool SendAsync(ISocketAsyncEventArgs e)
        {
            e.CurrentSocket = _sokClient;
            return _sokClient.SendAsync(e);
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <returns></returns>
        public void CloseSocket()
        {
            lock (this)
            {
                try
                {
                    if (_sokClient != null)
                    {
                        _sokClient.Shutdown(SocketShutdown.Both);
                        _sokClient.Dispose();
                        _sokClient.Close();
                        _sokClient = null;
                    }
                }
                catch
                {
                    _sokClient = null;
                }

                _connSuccess = false;
                SocketClosed?.Invoke();
            }
        }
    }
}
