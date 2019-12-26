namespace Simple.Common.SocketLib
{
    public class TCPClient : ITCPClient
    {
        public override ISocketAsyncEventArgs GetISocketAsyncEventArgs()
        {
            return new WinSocketAsyncEventArgs();
        }

        protected override ISocket GetISocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            return new WinSocket(addressFamily, socketType, protocolType);
        }
    }
}
