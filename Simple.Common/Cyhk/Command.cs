using CommLiby.Cyhk.Models;
using Simple.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommLiby.Cyhk
{
    /// <summary>
    /// 命令反馈事件委托
    /// </summary>
    /// <param name="cmd">命令信息</param>
    /// <param name="extraData">附加信息</param>
    public delegate void CommandAckHandler(Command cmd, byte[] extraData);
    /// <summary>
    /// 命令对象
    /// </summary>
    public class Command
    {
        //当前运算序列号值
        private static ushort currentNo;

        /// <summary>
        /// 产生下一个消息序列号
        /// </summary>
        /// <returns></returns>
        public static Command Next()
        {
            if (cmdInfoList == null || cmdStatusList == null) throw new Exception("cmdInfoList和cmdStatusList为空");

            Command sn = new Command();
            ushort next = ++currentNo;
            if (next == ushort.MaxValue)  //最大值后循环
                currentNo = 0;
            sn.SequenceNumber = next;
            sn.SendStatus = CmdStatus.WaitForSend;
            sn.AckStatus = CmdStatus.Unknown;
            return sn;
        }

        public static List<DICT_CMD> cmdInfoList; //命令列表 
        public static List<DICT_ERROR> cmdStatusList; //命令反馈状态表 


        private Command() { }
        /// <summary>
        /// 序列号
        /// </summary>
        public ushort SequenceNumber { get; set; }
        /// <summary>
        /// 命令发送状态
        /// </summary>
        public CmdStatus SendStatus { get; set; }
        /// <summary>
        /// 命令反馈状态
        /// </summary>
        public CmdStatus AckStatus { get; private set; }

        private ushort _ackCode;

        /// <summary>
        /// 命令反馈码
        /// </summary>
        public ushort AckCode
        {
            get { return _ackCode; }
            set
            {
                _ackCode = value;
                AckStatus = _ackCode == 0 ? CmdStatus.Success : CmdStatus.Failed;
                try
                {
                    DICT_ERROR error = cmdStatusList.First(x => x.NO == _ackCode);
                    AckMessage = error.EDESC;
                }
                catch
                {
                    if (_ackCode == 60000) //发送失败
                        AckMessage = "发送失败";
                    if (_ackCode == 60001)  //超时
                        AckMessage = "超时";
                    else
                        AckMessage = "失败（未知）";
                }
                AckTime = DateTimeHelper.Now;
            }
        }

        /// <summary>
        /// 命令反馈消息
        /// </summary>
        public string AckMessage { get; private set; }

        private ushort _cmdId;
        /// <summary>
        /// 命令号
        /// </summary>
        public ushort CmdId
        {
            get { return _cmdId; }
            set
            {
                _cmdId = value;
                CmdName = GetCmdName(_cmdId);
            }
        }

        /// <summary>
        /// 命令名称
        /// </summary>
        public string CmdName { get; private set; }
        /// <summary>
        /// 发送(成功)时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 反馈时间
        /// </summary>
        public DateTime AckTime { get; private set; }
        /// <summary>
        /// 车机识别码
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// 得到命令名称
        /// </summary>
        /// <returns></returns>
        public static string GetCmdName(ushort cmdId)
        {
            try
            {
                DICT_CMD cmd = cmdInfoList.First(x => x.CMDID == cmdId);
                return cmd.CMDNAME;
            }
            catch
            {
                return "未知命令:" + cmdId;
            }
        }
        /// <summary>
        /// 发送命令的车辆信息
        /// </summary>
        public ICar Car { get; set; }
        /// <summary>
        /// 发送字节
        /// </summary>
        public byte[] SendBytes { get; set; }
    }

    /// <summary>
    /// 命令状态枚举
    /// </summary>
    public enum CmdStatus
    {
        WaitForSend,
        Unknown,
        Success,
        Failed
    }
}
