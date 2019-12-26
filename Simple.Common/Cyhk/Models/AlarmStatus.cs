using System.Collections.Generic;
using System.Linq;

namespace CommLiby.Cyhk.Models
{
    public class AlarmStatus
    {
        private readonly List<string> _alarmStatusList = new List<string>();
        private bool _emergencyAlarm;
        private bool _overspeedAlarm;
        private bool _fatiguedrivingAlarm;
        private bool _dangerWarning;
        private bool _handwarefaultAlarm;
        private bool _gsmcutAlarm;
        private bool _shortcircuitAlarm;
        private bool _undervoltageAlarm;
        private bool _powerfailureAlarm;
        private bool _overspeedWarning;
        private bool _fatiguedrivingWarning;
        private bool _overdrivingtimeAlarm;
        private bool _stopovertimeAlarm;
        private bool _outareaAlarm;
        private bool _inareaAlarm;
        private bool _runtimeloworover;
        private bool _outwayAlarm;
        private bool _oilabnormalAlarm;
        private bool _carstolenAlarm;
        private bool _illegalfireonAlarm;
        private bool _illegalmoveAlarm;
        private bool _emergencybrakeAlarm;
        private bool _emergencystopAlarm;
        private bool _sharpturnAlarm;
        private bool _notinareaAlarm;
        private bool _noallowrunAlarm;
        private bool _inareaoverspeedAlarm;
        public uint _alarmStatus;
        private bool _lockoilleakAlarm;
        private bool _lostAlarm;
        private bool _outadministrativeareaAlarm;
        private bool _safetybeltAlarm;

        public AlarmStatus()
        {

        }
        public void RefreshAlarmStatus(uint alarmStatus)
        {
            if (_alarmStatus == alarmStatus) return;  //状态相同忽略

            _alarmStatusList.Clear();
            _alarmStatus = alarmStatus;
            emergency_alarm = (alarmStatus & 0x1) == 0x1;   //0
            overspeed_alarm = (alarmStatus & 0x2) == 0x2;
            fatiguedriving_alarm = (alarmStatus & 0x4) == 0x4;
            danger_warning = (alarmStatus & 0x8) == 0x8;
            handwarefault_alarm = (alarmStatus & 0x10) == 0x10;
            gsmcut_alarm = (alarmStatus & 0x20) == 0x20;    //5
            gsmshortcircuit_alarm = (alarmStatus & 0x40) == 0x40;
            undervoltage_alarm = (alarmStatus & 0x80) == 0x80;
            powerfailure_alarm = (alarmStatus & 0x100) == 0x100;
            overspeed_warning = (alarmStatus & 0x200) == 0x200;
            fatiguedriving_warning = (alarmStatus & 0x400) == 0x400;    //10
            overdrivingtime_alarm = (alarmStatus & 0x800) == 0x800;
            stopovertime_alarm = (alarmStatus & 0x1000) == 0x1000;
            outarea_alarm = (alarmStatus & 0x2000) == 0x2000;
            inarea_alarm = (alarmStatus & 0x4000) == 0x4000;
            runtimeloworover = (alarmStatus & 0x8000) == 0x8000;    //15
            outway_alarm = (alarmStatus & 0x10000) == 0x10000;
            oilabnormal_alarm = (alarmStatus & 0x20000) == 0x20000;
            carstolen_alarm = (alarmStatus & 0x40000) == 0x40000;
            illegalfireon_alarm = (alarmStatus & 0x80000) == 0x80000;
            illegalmove_alarm = (alarmStatus & 0x100000) == 0x100000;   //20
            emergencybrake_alarm = (alarmStatus & 0x200000) == 0x200000;
            emergencystop_alarm = (alarmStatus & 0x400000) == 0x400000;
            sharpturn_alarm = (alarmStatus & 0x800000) == 0x800000;
            lockoilleak_alarm = (alarmStatus & 0x1000000) == 0x1000000;
            notinarea_alarm = (alarmStatus & 0x2000000) == 0x2000000;   //25
            noallowrun_alarm = (alarmStatus & 0x4000000) == 0x4000000;
            inareaoverspeed_alarm = (alarmStatus & 0x8000000) == 0x8000000;
            lost_alarm = (alarmStatus & 0x10000000) == 0x10000000;
            outadministrativearea_alarm = (alarmStatus & 0x20000000) == 0x20000000;
            safetybelt_alarm = (alarmStatus & 0x40000000) == 0x40000000;    //30
        }

        private void AddStatusStr(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (!_alarmStatusList.Contains(str))
                    _alarmStatusList.Add(str);
            }
        }

        /// <summary>
        /// 紧急报警
        /// </summary>
        public bool emergency_alarm
        {
            get { return _emergencyAlarm; }
            set
            {
                _emergencyAlarm = value;
                if (_emergencyAlarm)
                {
                    AddStatusStr("紧急报警");
                }
            }
        }
        /// <summary>
        /// 超速报警
        /// </summary>
        public bool overspeed_alarm
        {
            get { return _overspeedAlarm; }
            set
            {
                _overspeedAlarm = value;
                if (_overspeedAlarm)
                {
                    AddStatusStr("超速");
                }
            }
        }

        /// <summary>
        /// 疲劳驾驶
        /// </summary>
        public bool fatiguedriving_alarm
        {
            get { return _fatiguedrivingAlarm; }
            set
            {
                _fatiguedrivingAlarm = value;
                if (_fatiguedrivingAlarm)
                {
                    AddStatusStr("疲劳驾驶");
                }
            }
        }

        /// <summary>
        /// 危险预警
        /// </summary>
        public bool danger_warning
        {
            get { return _dangerWarning; }
            set
            {
                _dangerWarning = value;
                if (_dangerWarning)
                {
                    AddStatusStr("危险预警");
                }
            }
        }

        /// <summary>
        /// 车机故障
        /// </summary>
        public bool handwarefault_alarm
        {
            get { return _handwarefaultAlarm; }
            set
            {
                _handwarefaultAlarm = value;
                if (_handwarefaultAlarm)
                {
                    AddStatusStr("车机故障");
                }
            }
        }

        /// <summary>
        /// 天线断开
        /// </summary>
        public bool gsmcut_alarm
        {
            get { return _gsmcutAlarm; }
            set
            {
                _gsmcutAlarm = value;
                if (_gsmcutAlarm)
                {
                    AddStatusStr("天线断开");
                }
            }
        }
        /// <summary>
        /// 天线短路
        /// </summary>
        public bool gsmshortcircuit_alarm
        {
            get { return _shortcircuitAlarm; }
            set
            {
                _shortcircuitAlarm = value;
                if (_shortcircuitAlarm)
                {
                    AddStatusStr("天线短路");
                }
            }
        }

        /// <summary>
        /// 终端欠压
        /// </summary>
        public bool undervoltage_alarm
        {
            get { return _undervoltageAlarm; }
            set
            {
                _undervoltageAlarm = value;
                if (_undervoltageAlarm)
                {
                    AddStatusStr("终端欠压");
                }
            }
        }

        /// <summary>
        /// 终端掉电
        /// </summary>
        public bool powerfailure_alarm
        {
            get { return _powerfailureAlarm; }
            set
            {
                _powerfailureAlarm = value;
                if (_powerfailureAlarm)
                {
                    AddStatusStr("终端掉电");
                }
            }
        }

        /// <summary>
        /// 超速预警
        /// </summary>
        public bool overspeed_warning
        {
            get { return _overspeedWarning; }
            set
            {
                _overspeedWarning = value;
                if (value)
                {
                    AddStatusStr("超速预警");
                }
            }
        }

        /// <summary>
        /// 疲劳驾驶预警
        /// </summary>
        public bool fatiguedriving_warning
        {
            get { return _fatiguedrivingWarning; }
            set
            {
                _fatiguedrivingWarning = value;
                if (value)
                {
                    AddStatusStr("疲劳预警");
                }
            }
        }

        /// <summary>
        /// 超时驾驶
        /// </summary>
        public bool overdrivingtime_alarm
        {
            get { return _overdrivingtimeAlarm; }
            set
            {
                _overdrivingtimeAlarm = value;
                if (value)
                {
                    AddStatusStr("超时驾驶");
                }
            }
        }

        /// <summary>
        /// 超时停车
        /// </summary>
        public bool stopovertime_alarm
        {
            get { return _stopovertimeAlarm; }
            set
            {
                _stopovertimeAlarm = value;
                if (value)
                {
                    AddStatusStr("超时停车");
                }
            }
        }

        /// <summary>
        /// 出区域
        /// </summary>
        public bool outarea_alarm
        {
            get { return _outareaAlarm; }
            set
            {
                _outareaAlarm = value;
                if (value)
                {
                    AddStatusStr("离开区域");
                }
            }
        }

        /// <summary>
        /// 进区域
        /// </summary>
        public bool inarea_alarm
        {
            get { return _inareaAlarm; }
            set
            {
                _inareaAlarm = value;
                if (value)
                {
                    AddStatusStr("进入区域");
                }
            }
        }

        /// <summary>
        /// 路段行驶时间不足/过长
        /// </summary>
        public bool runtimeloworover
        {
            get { return _runtimeloworover; }
            set
            {
                _runtimeloworover = value;
                if (value)
                {
                    AddStatusStr("路段行驶时间不足/过长");
                }
            }
        }

        /// <summary>
        /// 偏航报警
        /// </summary>
        public bool outway_alarm
        {
            get { return _outwayAlarm; }
            set
            {
                _outwayAlarm = value;
                if (value)
                {
                    AddStatusStr("偏航报警");
                }
            }
        }

        /// <summary>
        /// 油量异常
        /// </summary>
        public bool oilabnormal_alarm
        {
            get { return _oilabnormalAlarm; }
            set
            {
                _oilabnormalAlarm = value;
                if (value)
                {
                    AddStatusStr("油量异常");
                }
            }
        }

        /// <summary>
        /// 车辆被盗
        /// </summary>
        public bool carstolen_alarm
        {
            get { return _carstolenAlarm; }
            set
            {
                _carstolenAlarm = value;
                if (value)
                {
                    AddStatusStr("车辆被盗");
                }
            }
        }

        /// <summary>
        /// 非法点火
        /// </summary>
        public bool illegalfireon_alarm
        {
            get { return _illegalfireonAlarm; }
            set
            {
                _illegalfireonAlarm = value;
                if (value)
                {
                    AddStatusStr("非法点火");
                }
            }
        }

        /// <summary>
        /// 非法位移
        /// </summary>
        public bool illegalmove_alarm
        {
            get { return _illegalmoveAlarm; }
            set
            {
                _illegalmoveAlarm = value;
                if (value)
                {
                    AddStatusStr("非法位移");
                }
            }
        }

        /// <summary>
        /// 急刹车报警
        /// </summary>
        public bool emergencybrake_alarm
        {
            get { return _emergencybrakeAlarm; }
            set
            {
                _emergencybrakeAlarm = value;
                if (value)
                {
                    AddStatusStr("急刹车");
                }
            }
        }

        /// <summary>
        /// 急停车报警
        /// </summary>
        public bool emergencystop_alarm
        {
            get { return _emergencystopAlarm; }
            set
            {
                _emergencystopAlarm = value;
                if (value)
                {
                    AddStatusStr("急停车");
                }
            }
        }

        /// <summary>
        /// 急转弯报警
        /// </summary>
        public bool sharpturn_alarm
        {
            get { return _sharpturnAlarm; }
            set
            {
                _sharpturnAlarm = value;
                if (value)
                {
                    AddStatusStr("急转弯");
                }
            }
        }

        /// <summary>
        /// 施封状态液位降低
        /// </summary>
        public bool lockoilleak_alarm
        {
            get { return _lockoilleakAlarm; }
            set
            {
                _lockoilleakAlarm = value;
                if (value)
                    AddStatusStr("施封状态液位降低");
            }
        }

        /// <summary>
        /// 未入库报警 
        /// </summary>
        public bool notinarea_alarm
        {
            get { return _notinareaAlarm; }
            set
            {
                _notinareaAlarm = value;
                if (value)
                {
                    AddStatusStr("未入库");
                }
            }
        }

        /// <summary>
        /// 非法行车
        /// </summary>
        public bool noallowrun_alarm
        {
            get { return _noallowrunAlarm; }
            set
            {
                _noallowrunAlarm = value;
                if (value)
                {
                    AddStatusStr("非法行车");
                }
            }
        }

        /// <summary>
        /// 区域内超速
        /// </summary>
        public bool inareaoverspeed_alarm
        {
            get { return _inareaoverspeedAlarm; }
            set
            {
                _inareaoverspeedAlarm = value;
                if (value)
                {
                    AddStatusStr("区域超速");
                }
            }
        }

        /// <summary>
        /// 失联报警
        /// </summary>
        public bool lost_alarm
        {
            get { return _lostAlarm; }
            set
            {
                _lostAlarm = value;
                if (value)
                    AddStatusStr("失联");
            }
        }

        /// <summary>
        /// 出行政区域报警
        /// </summary>
        public bool outadministrativearea_alarm
        {
            get { return _outadministrativeareaAlarm; }
            set
            {
                _outadministrativeareaAlarm = value;
                if (value)
                    AddStatusStr("出行政区域");
            }
        }

        /// <summary>
        /// 未系安全带报警
        /// </summary>
        public bool safetybelt_alarm
        {
            get { return _safetybeltAlarm; }
            set
            {
                _safetybeltAlarm = value;
                if (value)
                {
                    AddStatusStr("未系安全带");
                }
            }
        }

        public override string ToString()
        {
            return string.Join(" ", _alarmStatusList);
        }
        /// <summary>
        /// 是否有报警
        /// </summary>
        public bool HasAlarm
        {
            get
            {
                return _alarmStatusList.Any();
            }
        }
    }
}
