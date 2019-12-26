using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simple.Common.SocketLib
{
    public static class NetTools
    {
        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static short HostToNetworkOrder(short host)
        {
            if (BitConverter.IsLittleEndian)
                return (short) (((host & 0xff) << 8) | ((host >> 8) & 0xff));
            else
                return host;
        }

        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <returns></returns>
        public static byte[] HostToNetworkOrderToBytes(short _short)
        {
            _short = HostToNetworkOrder(_short);
            return BitConverter.GetBytes(_short);
        }

        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static ushort HostToNetworkOrder(ushort host)
        {
            if (BitConverter.IsLittleEndian)
                return (ushort)(((host & 0xff) << 8) | ((host >> 8) & 0xff));
            else
                return host;
        }
        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <returns></returns>
        public static byte[] HostToNetworkOrderToBytes(ushort _ushort)
        {
            _ushort = HostToNetworkOrder(_ushort);
            return BitConverter.GetBytes(_ushort);
        }

        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static int HostToNetworkOrder(int host)
        {
            if (BitConverter.IsLittleEndian)
                return (((HostToNetworkOrder((short) host) & 0xffff) << 0x10) |
                        (HostToNetworkOrder((short) (host >> 0x10)) & 0xffff));
            else
                return host;
        }
        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <returns></returns>
        public static byte[] HostToNetworkOrderToBytes(int _int)
        {
            _int = HostToNetworkOrder(_int);
            return BitConverter.GetBytes(_int);
        }

        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static uint HostToNetworkOrder(uint host)
        {
            if (BitConverter.IsLittleEndian)
                return (uint)(((HostToNetworkOrder((short)host) & 0xffff) << 0x10) |
                        (HostToNetworkOrder((short)(host >> 0x10)) & 0xffff));
            else
                return host;
        }
        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <returns></returns>
        public static byte[] HostToNetworkOrderToBytes(uint _uint)
        {
            _uint = HostToNetworkOrder(_uint);
            return BitConverter.GetBytes(_uint);
        }
        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static long HostToNetworkOrder(long host)
        {
            if(BitConverter.IsLittleEndian)
            return (long)(((HostToNetworkOrder((int)host) & 0xffffffffL) << 0x20) | (HostToNetworkOrder((int)(host >> 0x20)) & 0xffffffffL));
            else
                return host;
        }

        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static ulong HostToNetworkOrder(ulong host)
        {
            if (BitConverter.IsLittleEndian)
                return (ulong)(((HostToNetworkOrder((int)host) & 0xffffffffL) << 0x20) | (HostToNetworkOrder((int)(host >> 0x20)) & 0xffffffffL));
            else
                return host;
        }

        /// <summary>
        /// 转换为网络字节序
        /// </summary>
        /// <returns></returns>
        public static byte[] HostToNetworkOrderToBytes(string _string, int length)
        {
            if (length < -1) throw new ArgumentOutOfRangeException(nameof(length), "长度不正确");

            byte[] strBytes = new byte[length > -1 ? length : 0];
            if (string.IsNullOrEmpty(_string))
            {
                return strBytes;
            }
            byte[] array = GbUnicode.GetGB2312Array(_string);
            if (length > -1)
            {
                int tmplength = array.Length > length ? length : array.Length;
                for (var i = 0; i < tmplength; i++)
                {
                    strBytes[i] = array[i];
                }
                return strBytes;
            }
            else
            {
                return array;
            }
        }

        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static short NetworkToHostOrder(short network)
        {
            return HostToNetworkOrder(network);
        }

        /// <summary>
        /// 装换为主机字节序short
        /// </summary>
        /// <returns></returns>
        public static short NetworkToHostOrderFromBytesToShort(byte[] value, int startIndex = 0)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "数组不能为null");
            if (startIndex < 0) throw new ArgumentNullException(nameof(startIndex), "开始索引无效");

            try
            {
                short _short = BitConverter.ToInt16(value, startIndex);
                _short = NetworkToHostOrder(_short);
                return _short;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static ushort NetworkToHostOrder(ushort network)
        {
            return HostToNetworkOrder(network);
        }

        /// <summary>
        /// 装换为主机字节序ushort
        /// </summary>
        /// <returns></returns>
        public static ushort NetworkToHostOrderFromBytesToUshort(byte[] value, int startIndex = 0)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "数组不能为null");
            if (startIndex < 0) throw new ArgumentNullException(nameof(startIndex), "开始索引无效");
            try
            {
                ushort _ushort = BitConverter.ToUInt16(value, startIndex);
                _ushort = NetworkToHostOrder(_ushort);
                return _ushort;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static int NetworkToHostOrder(int network)
        {
            return HostToNetworkOrder(network);
        }
        /// <summary>
        /// 装换为主机字节序int
        /// </summary>
        /// <returns></returns>
        public static int NetworkToHostOrderFromBytesToInt(byte[] value, int startIndex = 0)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "数组不能为null");
            if (startIndex < 0) throw new ArgumentNullException(nameof(startIndex), "开始索引无效");

            int _int = BitConverter.ToInt32(value, startIndex);
            _int = NetworkToHostOrder(_int);
            return _int;
        }

        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static uint NetworkToHostOrder(uint network)
        {
            return HostToNetworkOrder(network);
        }
        /// <summary>
        /// 装换为主机字节序uint
        /// </summary>
        /// <returns></returns>
        public static uint NetworkToHostOrderFromBytesToUint(byte[] value, int startIndex = 0)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "数组不能为null");
            if (startIndex < 0) throw new ArgumentNullException(nameof(startIndex), "开始索引无效");

            uint _uint = BitConverter.ToUInt32(value, startIndex);
            _uint = NetworkToHostOrder(_uint);
            return _uint;
        }

        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static long NetworkToHostOrder(long network)
        {
            return HostToNetworkOrder(network);
        }
        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public static ulong NetworkToHostOrder(ulong network)
        {
            return HostToNetworkOrder(network);
        }

        /// <summary>
        /// 转换为主机字节序
        /// </summary>
        /// <returns></returns>
        public static string NetworkToHostOrderFromBytesToString(byte[] value, int startIndex, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value), "数组不能为null");
            if (startIndex < 0) throw new ArgumentNullException(nameof(startIndex), "开始索引无效");
            if (length < -1) return string.Empty;
            if (length == -1)
            {//自动查找结尾\0结束
                for (int i = startIndex; i < value.Length; i++)
                {
                    byte b = value[i];
                    if (b == '\0')
                    {
                        length = i + 1 - startIndex;
                        break;
                    }
                }
            }
            //string str=Encoding.UTF8.GetString(value, startIndex, length);
            //return str;
            //List<char> charList = new List<char>();
            //for (int i = startIndex; i < startIndex + length; i++)
            //{
            //    charList.Add((char)value[i]);
            //}
            //return new string(charList.ToArray());
            byte[] tmpbs = new byte[length];
            Array.Copy(value, startIndex, tmpbs, 0, length);
            return GbUnicode.GetUnicodeString(tmpbs);
        }

    }
}
