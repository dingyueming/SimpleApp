using System.Collections.Generic;

namespace CommLiby.Cyhk.Models
{
    public class CarStatus
    {
        private readonly List<string> _statusList = new List<string>();
        private bool _accStatus;
        private bool _latitudeStatus;
        private bool _longitudeStatus;
        private bool _workingStatus;
        private bool _lolaencryptionStatus;
        private byte _rotateStatus;
        private byte _carryingStatus;
        private bool _oilwayStatus;
        private bool _circuitStatus;
        private bool _doorlockStatus;
        private bool _oillockStatus;
        private bool _ledStatus;
        private bool _ttsStatus;
        private bool _cameraStatus;
        private bool _iccardStatus;
        private bool _vssStatus;
        private bool _areaoutStatus;
        private bool _areainStatus;
        private bool _overspeedStatus;
        private bool _fatiguedrivingStatus;
        private bool _powerfailureStatus;
        private bool _undervoltageStatus;
        private bool _blindareaStatus;
        private bool _online;
        private bool _offline;
        private bool _inflectionPoint;
        private bool _safetybeltState;

        public CarStatus()
        {

        }

        /// <summary>
        /// 刷新车辆状态
        /// </summary>
        /// <param name="carStatus"></param>
        /// <param name="stopTime"></param>
        public void RefreshStatus(uint carStatus, ushort stopTime)
        {
            _statusList.Clear();
            acc_status = (carStatus & 0x1) == 0x1;  //0
            latitude_status = (carStatus & 0x2) == 0x2;
            longitude_status = (carStatus & 0x4) == 0x4;
            working_status = (carStatus & 0x8) == 0x8;
            lolaencryption_status = (carStatus & 0x10) == 0x10; //4
            rotate_status = (byte)(carStatus >> 5 & 0x3);   //5 6
            carrying_status = (byte)(carStatus >> 7 & 0x3); // 7 8
            oilway_status = (carStatus & 0x200) == 0x200;
            circuit_status = (carStatus & 0x400) == 0x400;
            doorlock_status = (carStatus & 0x800) == 0x800;
            oillock_status = (carStatus & 0x1000) == 0x1000;    //12
            led_status = (carStatus & 0x2000) == 0x2000;
            tts_status = (carStatus & 0x4000) == 0x4000;
            camera_status = (carStatus & 0x8000) == 0x8000;
            iccard_status = (carStatus & 0x10000) == 0x10000;   //16
            vss_status = (carStatus & 0x20000) == 0x20000;
            areaout_status = (carStatus & 0x40000) == 0x40000;
            areain_status = (carStatus & 0x80000) == 0x80000;
            overspeed_status = (carStatus & 0x100000) == 0x100000;  //20
            fatiguedriving_status = (carStatus & 0x200000) == 0x200000;
            powerfailure_status = (carStatus & 0x400000) == 0x400000;
            undervoltage_status = (carStatus & 0x800000) == 0x800000;
            blindarea_status = (carStatus & 0x1000000) == 0x1000000;    //24
            online = (carStatus & 0x2000000) == 0x2000000;
            offline = (carStatus & 0x4000000) == 0x4000000;
            inflection_point = (carStatus & 0x8000000) == 0x8000000;
            safetybelt_state = (carStatus & 0x10000000) == 0x10000000;  //28
            has_ins = (carStatus & 0x40000000) == 0x40000000;   //30
            inertial_navigation = (carStatus & 0x20000000) == 0x20000000;   //29

            if (stopTime > 0)
                _statusList.Add("停车:" + stopTime + "分");
        }

        private void AddStatusStr(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                if (!_statusList.Contains(str))
                    _statusList.Add(str);
            }
        }

        /// <summary>
        /// Acc状态
        /// </summary>
        public bool acc_status
        {
            get { return _accStatus; }
            set
            {
                _accStatus = value;
                if (value)
                {
                    AddStatusStr("ACC开");
                }
                else
                {
                    AddStatusStr("ACC关");
                }
            }
        }
        /// <summary>
        /// 纬度状态 true-南纬 false-北纬
        /// </summary>
        public bool latitude_status
        {
            get { return _latitudeStatus; }
            set
            {
                _latitudeStatus = value;
                //if (value)
                //{
                //    _carStr += "南纬 ";
                //}
                //else
                //{
                //    _carStr += "北纬 ";
                //}
            }
        }

        /// <summary>
        /// 经度状态 true-西经 false-东经
        /// </summary>
        public bool longitude_status
        {
            get { return _longitudeStatus; }
            set
            {
                _longitudeStatus = value;
                //if (value)
                //{
                //    _carStr += "西经 ";
                //}
                //else
                //{
                //    _carStr += "东经 ";
                //}
            }
        }

        /// <summary>
        /// 运营状态
        /// </summary>
        public bool working_status
        {
            get { return _workingStatus; }
            set
            {
                _workingStatus = value;
                /*
                if (value)
                {
                    _carStr += "运营 ";
                }
                else
                {
                    _carStr += "停运 ";
                }*/
            }
        }

        /// <summary>
        /// 经纬度加密状态
        /// </summary>
        public bool lolaencryption_status
        {
            get { return _lolaencryptionStatus; }
            set
            {
                _lolaencryptionStatus = value;
            }
        }

        /// <summary>
        /// 混凝土罐车旋转状态
        /// </summary>
        public byte rotate_status
        {
            get { return _rotateStatus; }
            set
            {
                _rotateStatus = value;
                /*
               if (value == 2)
               {
                   _carStr += "正转 ";
               }
               else if (value == 1)
               {
                   _carStr += "反转 ";
               }
               else if (value == 0)
               {
                   _carStr += "不转 ";
               }
               else if (value == 3)
               {
                   _carStr += "不转 ";
               }
               */
            }
        }

        /// <summary>
        /// 车辆载重状态
        /// </summary>
        public byte carrying_status
        {
            get { return _carryingStatus; }
            set
            {
                _carryingStatus = value;
                /*
                if (value == 0)
                {
                    _carStr += "空车 ";
                }
                else if (value == 1)
                {
                    _carStr += "半载 ";
                }
                else if (value == 2)
                {//保留

                }
                else if (value == 3)
                {
                    _carStr += "满载 ";
                }
                 */
            }
        }

        /// <summary>
        /// 车辆油路状态
        /// </summary>
        public bool oilway_status
        {
            get { return _oilwayStatus; }
            set
            {
                _oilwayStatus = value;
                if (value)
                {
                    AddStatusStr("油路断开");
                }
                /*
                else
                {
                    _carStr += "油路正常 ";
                }*/
            }
        }

        /// <summary>
        /// 车辆电路状态
        /// </summary>
        public bool circuit_status
        {
            get { return _circuitStatus; }
            set
            {
                _circuitStatus = value;

                if (value)
                {
                    AddStatusStr("电路断开");
                }
                /*else
                {
                    _carStr += "电路正常 ";
                }*/
            }
        }

        /// <summary>
        /// 车门锁状态
        /// </summary>
        public bool doorlock_status
        {
            get { return _doorlockStatus; }
            set
            {
                _doorlockStatus = value;
                if (value)
                {
                    AddStatusStr("车门加锁");
                }
                /*else
                {
                    _carStr += "车门解锁 ";
                }*/
            }
        }

        /// <summary>
        /// 电子锁硬件故障
        /// </summary>
        public bool oillock_status
        {
            get { return _oillockStatus; }
            set
            {
                _oillockStatus = value;
                if (value)
                {
                    AddStatusStr("锁故障");
                }
            }
        }

        /// <summary>
        /// 终端LED或显示器故障
        /// </summary>
        public bool led_status
        {
            get { return _ledStatus; }
            set
            {
                _ledStatus = value;
                if (value)
                {
                    AddStatusStr("显示屏故障");
                }
            }
        }

        /// <summary>
        /// TTS模块故障
        /// </summary>
        public bool tts_status
        {
            get { return _ttsStatus; }
            set
            {
                _ttsStatus = value;
                if (value)
                {
                    AddStatusStr("TTS故障");
                }
            }
        }

        /// <summary>
        /// 摄像头故障
        /// </summary>
        public bool camera_status
        {
            get { return _cameraStatus; }
            set
            {
                _cameraStatus = value;
                if (value)
                {
                    AddStatusStr("摄像头故障");
                }
            }
        }

        /// <summary>
        /// 道路运输证IC卡模块故障
        /// </summary>
        public bool iccard_status
        {
            get { return _iccardStatus; }
            set
            {
                _iccardStatus = value;
                if (value)
                {
                    AddStatusStr("运输证IC卡故障");
                }
            }
        }

        /// <summary>
        /// 车辆VSS故障
        /// </summary>
        public bool vss_status
        {
            get { return _vssStatus; }
            set
            {
                _vssStatus = value;
                if (value)
                {
                    AddStatusStr("VSS故障");
                }
            }
        }

        /// <summary>
        /// 区域外状态
        /// </summary>
        public bool areaout_status
        {
            get { return _areaoutStatus; }
            set
            {
                _areaoutStatus = value;
                if (value)
                {
                    AddStatusStr("区域外");
                }
            }
        }

        /// <summary>
        /// 区域内状态
        /// </summary>
        public bool areain_status
        {
            get { return _areainStatus; }
            set
            {
                _areainStatus = value;
                if (value)
                {
                    AddStatusStr("区域内");
                }
                else
                {
                    // _carStr += "无区域 ";
                }
            }
        }

        /// <summary>
        /// 超速状态
        /// </summary>
        public bool overspeed_status
        {
            get { return _overspeedStatus; }
            set
            {
                _overspeedStatus = value;
                if (value)
                {
                    AddStatusStr("超速中");
                }
            }
        }

        /// <summary>
        /// 疲劳驾驶状态
        /// </summary>
        public bool fatiguedriving_status
        {
            get { return _fatiguedrivingStatus; }
            set
            {
                _fatiguedrivingStatus = value;
                if (value)
                {
                    AddStatusStr("疲劳驾驶中");
                }
            }
        }

        /// <summary>
        /// 断电状态
        /// </summary>
        public bool powerfailure_status
        {
            get { return _powerfailureStatus; }
            set
            {
                _powerfailureStatus = value;
                if (value)
                {
                    AddStatusStr("断电中(电池供电)");
                }
            }
        }

        /// <summary>
        /// 欠压状态
        /// </summary>
        public bool undervoltage_status
        {
            get { return _undervoltageStatus; }
            set
            {
                _undervoltageStatus = value;
                if (value)
                {
                    AddStatusStr("欠压中");
                }
            }
        }

        /// <summary>
        /// 盲区补偿状态 
        /// </summary>
        public bool blindarea_status
        {
            get { return _blindareaStatus; }
            set
            {
                _blindareaStatus = value;
                if (value)
                {
                    AddStatusStr("盲区补传");
                }
            }
        }

        /// <summary>
        /// 上线
        /// </summary>
        public bool online
        {
            get { return _online; }
            set
            {
                _online = value;
                if (value)
                    AddStatusStr("上线");
            }
        }

        /// <summary>
        /// 下线
        /// </summary>
        public bool offline
        {
            get { return _offline; }
            set
            {
                _offline = value;
                if (value)
                    AddStatusStr("下线");
            }
        }

        /// <summary>
        /// 拐点
        /// </summary>
        public bool inflection_point
        {
            get { return _inflectionPoint; }
            set
            {
                _inflectionPoint = value;
                if (value)
                    AddStatusStr("拐点");
            }
        }

        /// <summary>
        /// 隐藏安全带状态
        /// </summary>
        public static bool hide_safetybelt_state=true;

        /// <summary>
        /// 安全带状态
        /// </summary>
        public bool safetybelt_state
        {
            get { return _safetybeltState; }
            set
            {
                _safetybeltState = value;
                if (!acc_status) return;
                if (value)
                {
                    if (!hide_safetybelt_state)
                        AddStatusStr("已系安全带");
                }
                else
                {
                    if (!hide_safetybelt_state)
                        AddStatusStr("未系安全带");
                }
            }
        }


        bool _inertial_navigation;
        public bool inertial_navigation
        {
            get
            {
                return this._inertial_navigation;
            }
            set
            {
                this._inertial_navigation = value;
                if (has_ins)
                {
                    if (value)
                    {
                        AddStatusStr("静止");
                    }
                    else
                    {
                        AddStatusStr("运动");
                    }
                }
            }
        }

        bool _has_ins;
        public bool has_ins
        {
            get { return this._has_ins; }
            set { this._has_ins = value; }
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(" ", _statusList);
        }
    }
}
