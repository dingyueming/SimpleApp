using System;
using System.Collections.Generic;
using System.Text;
using Simple.Entity;

namespace Simple.IRepository.SM
{
    public interface IAuthRepository
    {
        void Add(AUTHEntity entity);
    }
}
