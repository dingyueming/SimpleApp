using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Simple.Common.SendSMS
{
    public class WebServiceClient : ServiceBase, ISmsApi {
        private readonly SmsSetting _smsSetting;
        private readonly object lockObj = new object ();
        /// <summary>
        /// 短信最大字符限制
        /// </summary>
        public readonly int MaxChars;
        public WebServiceClient () : base ("http://service2.winic.org/Service.asmx") {
            _smsSetting = ConfigurtaionHelper.Instance.GetAppSettings<SmsSetting> ("sms");
            SMSReturnCode rstr = GetUserInfo ();
            LogHelper.Warn ("smsInfo：" + rstr.ReturnString);
            if (rstr.IsSuccess) {
                MaxChars = Convert.ToInt32 (rstr.ReturnArray[3]);
            }
        }

        public SMSReturnCode GetMessageInfo (string snum) {
            return new SMSReturnCode (InvokeMethod (nameof (GetMessageInfo), snum) ? .ToString ());
        }

        public SMSReturnCode GetMessageRecord (string uid, string pwd, string num, DateTime StartDate, DateTime EndDate, bool isday) {
            return new SMSReturnCode (InvokeMethod (nameof (GetMessageRecord), uid, pwd, num, StartDate, EndDate, isday) ? .ToString ());
        }

        public SMSReturnCode GetMessageRecord (string num, DateTime StartDate, DateTime EndDate, bool isday) {
            return GetMessageRecord (_smsSetting.uid, _smsSetting.pwd, num, StartDate, EndDate, isday);
        }

        public SMSReturnCode GetUserInfo (string uid, string pwd) {
            return new SMSReturnCode (InvokeMethod (nameof (GetUserInfo), uid, pwd) ? .ToString ());
        }

        public SMSReturnCode GetUserInfo () {
            return GetUserInfo (_smsSetting.uid, _smsSetting.pwd);
        }

        public SMSReturnCode GetUserInfo_ (string uid, string pwd) {
            return new SMSReturnCode (InvokeMethod (nameof (GetUserInfo_), uid, pwd) ? .ToString ());
        }

        public SMSReturnCode GetUserInfo_ () {
            return GetUserInfo_ (_smsSetting.uid, _smsSetting.pwd);
        }

        public SMSReturnCode GET_SMS_MO (string uid, string pwd, string IDtype) {
            return new SMSReturnCode (InvokeMethod (nameof (GET_SMS_MO), uid, pwd, IDtype) ? .ToString ());
        }

        public SMSReturnCode GET_SMS_MO (string IDtype) {
            return GET_SMS_MO (_smsSetting.uid, _smsSetting.pwd, IDtype);
        }
        public SMSReturnCode GET_SMS_MO_Ext (string uid, string pwd) {
            return new SMSReturnCode (InvokeMethod (nameof (GET_SMS_MO_Ext), uid, pwd) ? .ToString ());
        }

        public SMSReturnCode GET_SMS_MO_Ext () {
            return GET_SMS_MO_Ext (_smsSetting.uid, _smsSetting.pwd);
        }

        public SMSReturnCode SendMessages (string uid, string pwd, string tos, string msg, string otime = "") {
            return new SMSReturnCode (InvokeMethod (nameof (SendMessages), uid, pwd, tos, msg, otime) ? .ToString ());
        }

        public SMSReturnCode SendMessages (string tos, string msg, string otime = "") {
            return SendMessages (_smsSetting.uid, _smsSetting.pwd, tos, msg, otime);
        }

        public SMSReturnCode SMS_Reports (string uid, string pwd) {
            return new SMSReturnCode (InvokeMethod (nameof (SMS_Reports), uid, pwd) ? .ToString ());
        }

        public SMSReturnCode SMS_Reports () {
            return SMS_Reports (_smsSetting.uid, _smsSetting.pwd);
        }

        int ISmsApi.MaxChars () {
            if (MaxChars == 0)
                return 70;
            return MaxChars;
        }

        private List<SMSReport> PullSMSReports () {
            List<SMSReport> list_tmp = new List<SMSReport> ();
            SMSReturnCode rcode = SMS_Reports ();
            if (rcode.IsSuccess) {
                try {
                    string[] tmps = rcode.ReturnString.Split ('|');
                    if (tmps != null) {
                        for (int i = 0; i < tmps.Length; i++) {
                            string tmp = tmps[i];
                            if (string.IsNullOrWhiteSpace (tmp)) continue;
                            string[] tmparr = tmp.Split ('/');
                            if (tmparr.Length == 5) {
                                SMSReport report = new SMSReport () {
                                snum = tmparr[0].Trim (),
                                uid = tmparr[1],
                                phone = tmparr[2],
                                state = tmparr[3],
                                time = DateTime.ParseExact (tmparr[4], "yyyy-M-d H:mm:ss", CultureInfo.InvariantCulture)
                                };
                                list_tmp.Add (report);
                            }
                        }
                    }
                } catch (Exception ex) {
                    LogHelper.Warn (rcode.ReturnString);
                    LogHelper.Error ("PullSMSReports", ex);
                }
            }
            return list_tmp;
        }

        private void PullAndSaveSMSReports () {
            List<SMSReport> list_tmp = PullSMSReports ();
            SaveReports (list_tmp.ToArray ());
        }

        public bool? SMS_IS_Receive (string snum) {
            List<SMSReport> list = GetReceivedReports ();
            bool? res = null;
            SMSReport r = list.FirstOrDefault (s => s.snum == snum);
            if (r != null) {
                res = r.IsSuccess ();
                RemoveReports (r);
            }
            return res;
        }

        public List<SMSReport> GetReceivedReports (bool pullData = true) {
            if (pullData)
                PullAndSaveSMSReports ();
            lock (lockObj) {
                List<SMSReport> list = new List<SMSReport> ();
                try {
                    if (File.Exists ("SMSReport")) {
                        string fstr = File.ReadAllText ("SMSReport");
                        if (!string.IsNullOrWhiteSpace (fstr))
                            list = JsonConvert.DeserializeObject<List<SMSReport>> (fstr);
                    }
                } catch (Exception ex) {
                    LogHelper.Error ("读取短信报告文件错误", ex);
                }
                return list;
            }
        }

        public void SaveReports (params SMSReport[] reports) {
            if (reports.Length == 0) return;
            List<SMSReport> list = GetReceivedReports (false);
            lock (lockObj) {
                try {
                    list.AddRange (reports);
                    list.RemoveAll (s => (DateTime.Now - s.time).TotalDays > 100);
                    File.WriteAllText ("SMSReport", JsonConvert.SerializeObject (list));
                } catch (Exception ex) {
                    LogHelper.Error ("保存短信报告文件错误", ex);
                }
            }
        }

        public void RemoveReports (params SMSReport[] reports) {
            if (reports.Length == 0) return;
            List<SMSReport> list = GetReceivedReports (false);
            lock (lockObj) {
                try {
                    for (int i = 0; i < reports.Length; i++) {
                        SMSReport r = reports[i];
                        if (list.Contains (r))
                            list.Remove (r);
                        else {
                            r = list.FirstOrDefault (s => s.snum == r.snum);
                            if (r != null) {
                                list.Remove (r);
                            }
                        }
                    }
                    list.RemoveAll (s => (DateTime.Now - s.time).TotalDays > 100);
                    File.WriteAllText ("SMSReport", JsonConvert.SerializeObject (list));
                } catch (Exception ex) {
                    LogHelper.Error ("移除短信报告文件错误", ex);
                }
            }
        }
    }

    public class SMSReport {
        /// <summary>
        /// 短信编码
        /// </summary>
        /// <returns></returns>
        public string snum { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        /// <returns></returns>
        public string uid { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        /// <returns></returns>
        public string phone { get; set; }
        /// <summary>
        /// 报告状态
        /// </summary>
        /// <returns></returns>
        public string state { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        /// <returns></returns>
        public DateTime time { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess () {
            return state?.Trim () == "DELIVRD";
        }
    }
}