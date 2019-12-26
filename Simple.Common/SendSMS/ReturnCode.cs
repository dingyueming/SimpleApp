using System;

namespace Simple.Common.SendSMS {
    public class SMSReturnCode {

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>       
        public bool IsSuccess { get; }
        /// <summary>
        /// 错误码
        /// </summary>
        /// <returns></returns>
        public int ErrCode { get; }
        /// <summary>
        /// 错误码
        /// </summary>
        /// <returns></returns>
        public IntCode ErrorCode { get; }
        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public string ReturnString { get; }
        /// <summary>
        /// 返回数组
        /// </summary>
        /// <returns></returns>
        public string[] ReturnArray { get; }

        public SMSReturnCode (string returnStr) {
            if (!string.IsNullOrEmpty (returnStr)) {
                ReturnString = returnStr;
                string[] tmp = returnStr.Split ('/');
                ReturnArray = new string[tmp.Length - 1];
                if (ReturnArray.Length > 0) {
                    Array.Copy (tmp, 1, ReturnArray, 0, ReturnArray.Length);
                }
                int code = -100;
                bool suc = CommonHelper.IsNumricForInt (tmp[0]);
                if (suc) {
                    if (int.TryParse (tmp[0], out code)) {
                        ErrCode = code;
                        ErrorCode = (IntCode) code;
                    }
                    IsSuccess = code == 0 || (suc && tmp.Length == 1);
                }
            }
        }

        public override string ToString () {
            return ReturnString;
        }
    }
    public enum IntCode {
        ///<summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 当前账号余额不足！
        /// </summary>
        NoMoney = -01,
        /// <summary>
        /// 当前用户ID错误！
        /// </summary>
        UIdError = -02,
        /// <summary>
        /// 当前密码错误！
        /// </summary>
        PwdError = -03,
        /// <summary>
        /// 参数不够或参数内容的类型错误!
        /// </summary>
        ParamError = -04,
        /// <summary>
        /// 手机号码格式不对！
        /// </summary>
        PhoneError = -05,
        /// <summary>
        /// 短信内容编码不对！
        /// </summary>
        EncodeError = -06,
        /// <summary>
        /// 短信内容含有敏感字符！
        /// </summary>
        WordError = -07,
        /// <summary>
        /// 无接收数据
        /// </summary>
        NoReceiveError = -08,
        /// <summary>
        /// 系统维护中..
        /// </summary>
        ServiceError = -09,
        /// <summary>
        /// 手机号码数量超长！
        /// </summary>
        OutPhoneRangeError = -10,
        /// <summary>
        /// 短信内容超长！（70个字符）
        /// </summary>
        OutSMSRangeError = -11,
        /// <summary>
        /// 其它错误！
        /// </summary>
        OtherError = -12,
        /// <summary>
        /// 文件传输错误
        /// </summary>
        FileTransferError = -13,
        ///<summary>
        /// 未知错误
        ///</summary>
        UnknowError = -100
    }
}