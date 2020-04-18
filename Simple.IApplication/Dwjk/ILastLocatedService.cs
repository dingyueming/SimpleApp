﻿using Simple.ExEntity.Map;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.Dwjk
{
    public interface ILastLocatedService
    {
        Task<LastLocatedExEntity> GetLastLocatedByMac(string mac);
        Task<LastLocatedExEntity> GetLastLocated(string keyword);
    }
}