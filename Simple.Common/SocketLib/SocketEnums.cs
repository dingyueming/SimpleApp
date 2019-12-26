using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common.SocketLib
{
    public enum SocketProtocol
    {
        Tcp,
        Http
    }
    public enum SocketError
    {
        AccessDenied = 0x271d,

        AddressAlreadyInUse = 0x2740,

        AddressFamilyNotSupported = 0x273f,

        AddressNotAvailable = 0x2741,

        AlreadyInProgress = 0x2735,

        ConnectionAborted = 0x2745,

        ConnectionRefused = 0x274d,

        ConnectionReset = 0x2746,

        DestinationAddressRequired = 0x2737,

        Disconnecting = 0x2775,

        Fault = 0x271e,

        HostDown = 0x2750,

        HostNotFound = 0x2af9,

        HostUnreachable = 0x2751,

        InProgress = 0x2734,

        Interrupted = 0x2714,

        InvalidArgument = 0x2726,

        IOPending = 0x3e5,

        IsConnected = 0x2748,

        MessageSize = 0x2738,

        NetworkDown = 0x2742,

        NetworkReset = 0x2744,

        NetworkUnreachable = 0x2743,

        NoBufferSpaceAvailable = 0x2747,

        NoData = 0x2afc,

        NoRecovery = 0x2afb,

        NotConnected = 0x2749,

        NotInitialized = 0x276d,

        NotSocket = 0x2736,

        OperationAborted = 0x3e3,

        OperationNotSupported = 0x273d,

        ProcessLimit = 0x2753,

        ProtocolFamilyNotSupported = 0x273e,

        ProtocolNotSupported = 0x273b,

        ProtocolOption = 0x273a,

        ProtocolType = 0x2739,

        Shutdown = 0x274a,

        SocketError = -1,

        SocketNotSupported = 0x273c,

        Success = 0,

        SystemNotReady = 0x276b,

        TimedOut = 0x274c,

        TooManyOpenSockets = 0x2728,

        TryAgain = 0x2afa,

        TypeNotFound = 0x277d,

        VersionNotSupported = 0x276c,

        WouldBlock = 0x2733
    }

    public enum SocketAsyncOperation
    {
        None,
        Accept,
        Connect,
        Disconnect,
        Receive,
        ReceiveFrom,
        ReceiveMessageFrom,
        Send,
        SendPackets,
        SendTo
    }

    public enum AddressFamily
    {

        AppleTalk = 0x10,

        Atm = 0x16,

        Banyan = 0x15,

        Ccitt = 10,

        Chaos = 5,

        Cluster = 0x18,

        DataKit = 9,

        DataLink = 13,

        DecNet = 12,

        Ecma = 8,

        FireFox = 0x13,

        HyperChannel = 15,

        Ieee12844 = 0x19,

        ImpLink = 3,

        InterNetwork = 2,

        InterNetworkV6 = 0x17,

        Ipx = 6,

        Irda = 0x1a,

        Iso = 7,

        Lat = 14,
        Max = 0x1d,

        NetBios = 0x11,

        NetworkDesigners = 0x1c,

        NS = 6,

        Osi = 7,

        Pup = 4,

        Sna = 11,

        Unix = 1,

        Unknown = -1,

        Unspecified = 0,

        VoiceView = 0x12
    }

    public enum SocketType
    {
        Dgram = 2,
        Raw = 3,
        Rdm = 4,
        Seqpacket = 5,
        Stream = 1,
        Unknown = -1
    }


    public enum ProtocolType
    {
        Ggp = 3,
        Icmp = 1,
        IcmpV6 = 0x3a,
        Idp = 0x16,
        Igmp = 2,
        IP = 0,
        IPSecAuthenticationHeader = 0x33,
        IPSecEncapsulatingSecurityPayload = 50,
        IPv4 = 4,
        IPv6 = 0x29,
        IPv6DestinationOptions = 60,
        IPv6FragmentHeader = 0x2c,
        IPv6HopByHopOptions = 0,
        IPv6NoNextHeader = 0x3b,
        IPv6RoutingHeader = 0x2b,
        Ipx = 0x3e8,
        ND = 0x4d,
        Pup = 12,
        Raw = 0xff,
        Spx = 0x4e8,
        SpxII = 0x4e9,
        Tcp = 6,
        Udp = 0x11,
        Unknown = -1,
        Unspecified = 0
    }

    public enum SocketShutdown
    {
        Receive,
        Send,
        Both
    }


}
