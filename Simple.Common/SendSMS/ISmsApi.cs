using System;
using System.Collections.Generic;

namespace Simple.Common.SendSMS
{
    public interface ISmsApi
    {
        /// <summary>
        /// 每条短信最大字符数
        /// </summary>
        /// <returns></returns>
        int MaxChars();
        /// <summary>
        /// 发送短信接口
        /// </summary>
        /// <param name="uid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="tos">接收号码(小灵通区号加号码)</param>
        /// <param name="msg">短信内容(最多70个字)</param>
        /// <param name="otime">定时发送，留空即时发送 格式是：2008-08-19 09:10:00</param>
        /// <returns></returns>
         SMSReturnCode SendMessages(string uid,string pwd,string tos,string msg, string otime="");
         /// <summary>
         /// 发送短信接口
         /// </summary>
         /// <param name="tos">接收号码(小灵通区号加号码)</param>
         /// <param name="msg">短信内容(最多70个字)</param>
         /// <param name="otime">定时发送，留空即时发送 格式是：2008-08-19 09:10:00</param>
         /// <returns></returns>
         SMSReturnCode SendMessages(string tos,string msg, string otime="");
        /// <summary>
        /// 获取短信回复记录 (示例：手机号码/内容/端口/时间|手机号码/内容/端口/时间|手机号码/内容/端口/时间)
        /// </summary>
        /// <param name="uid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="IDtype"></param>
        /// <returns></returns>
         SMSReturnCode GET_SMS_MO(string uid,string pwd,string IDtype);
         /// <summary>
         /// 获取短信回复记录_可扩展 (示例：手机号码/内容/端口/时间|手机号码/内容/端口/时间|手机号码/内容/端口/时间)
         /// </summary>
         /// <param name="uid">用户名</param>
         /// <param name="pwd">密码</param>
         /// <returns></returns>
         SMSReturnCode GET_SMS_MO_Ext(string uid,string pwd);
         /// <summary>
         /// 短信处理状态
         /// </summary>
         /// <param name="snum"></param>
         /// <returns></returns>
         SMSReturnCode GetMessageInfo(string snum);
         /// <summary>
         /// 短信发送记录
         /// </summary>
         /// <param name="uid">用户名</param>
         /// <param name="pwd">密码</param>
         /// <param name="num">信息编号</param>
         /// <param name="StartDate">开始时间</param>
         /// <param name="EndDate">结束时间</param>
         /// <param name="isday"></param>
         /// <returns></returns>
         SMSReturnCode GetMessageRecord(string uid,string pwd,string num,DateTime StartDate,DateTime EndDate,bool isday);
        /// <summary>
        /// 读取账户信息
        /// </summary>
        /// <param name="uid">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
         SMSReturnCode GetUserInfo(string uid,string pwd);
         /// <summary>
         /// 读取账户信息
         /// </summary>
         /// <returns></returns>
         SMSReturnCode GetUserInfo();
         /// <summary>
         /// 读取账户信息(带个人信息)
         /// </summary>
         /// <param name="uid">用户名</param>
         /// <param name="pwd">密码</param>
         /// <returns></returns>
         SMSReturnCode GetUserInfo_(string uid,string pwd);
         /// <summary>
         /// 短信发送报告(返回示例:编号/手机号/状态/发送时间|编号/手机号/状态/发送时间|编号/手机号/状态/发送时间|...)
         /// </summary>
         /// <param name="uid">用户名</param>
         /// <param name="pwd">密码</param>
         /// <returns></returns>
         SMSReturnCode SMS_Reports(string uid,string pwd);
         /// <summary>
         /// 短信发送报告(返回示例:编号/手机号/状态/发送时间|编号/手机号/状态/发送时间|编号/手机号/状态/发送时间|...)
         /// </summary>
         /// <returns></returns>
         SMSReturnCode SMS_Reports();
        /// <summary>
         /// 判断短信是否收到
         /// </summary>
         /// <returns></returns>
         bool? SMS_IS_Receive(string snum);
        /// <summary>
        /// 获得接收到的Reports
        /// </summary>
        /// <returns></returns>
         List<SMSReport> GetReceivedReports(bool pullData=true);
         /// <summary>
         /// 保存短信报告
         /// </summary>
         /// <param name="reports"></param>
         void SaveReports(params SMSReport[] reports);
         /// <summary>
         /// 移除短信报告
         /// </summary>
         /// <param name="reports"></param>
         void RemoveReports(params SMSReport[] reports);
    }
}