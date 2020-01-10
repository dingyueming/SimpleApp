using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel.Paionation
{
    public class Pagination<T>
    {
        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int Total { get; set; }

        public string Where { get; set; }

        public string OrderBy { get; set; }

        public List<T> Data { get; set; }
    }
}
