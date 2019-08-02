using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;

namespace Simple.IRepository.SM
{
    public interface IUserRepository
    {
        Task<int> Add(UsersEntity entity);

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        Task<List<UsersEntity>> GetAll();
    }
}
