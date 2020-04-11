﻿using Simple.ExEntity.Map;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.Dwjk
{
    public interface ISjgx110AlarmService
    {
        Task<List<Sjgx110AlarmExEntity>> GetAlarmPositionList(DateTime startTime, DateTime endTime);
    }
}
