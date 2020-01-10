using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.Tools
{
    public static class DataBaseHelper
    {

        public static string GetPaginationSql(string sql, int pageSize, int pageIndex)
        {
            var pageSql = $@"select *

                              from (select aaaaa.*, rownum rn
        
                                      from ({sql}) aaaaa
        
                                     where rownum <= {pageSize * pageIndex})

                             where rn >= {(pageIndex - 1) * pageSize}";
            return pageSql;
        }
    }
}
