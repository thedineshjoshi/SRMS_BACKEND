using Application.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mapping
{
      public static class MappingExtensions
        {
            public static Task<PaginationList<TDestination>> PaginationListAsync<TDestination>(this IQueryable<TDestination> source, int pageNumber, int pageSize)
            => PaginationList<TDestination>.CreatePaginationAsync(source, pageNumber, pageSize);

        }
}

