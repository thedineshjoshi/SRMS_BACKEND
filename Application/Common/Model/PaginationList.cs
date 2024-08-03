using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Model
{
    public class PaginationList<T> //Generic
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginationList(List<T> items, int pageNumber, int totalPages)
        {
            this.Items = items;
            this.PageNumber = pageNumber;
            this.TotalPages = totalPages;
        }
        public static async Task<PaginationList<T>> CreatePaginationAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {

            var items = await source.Skip((pageNumber - 1)).Take(pageSize).ToListAsync();
            var totalCount = await source.CountAsync();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            return new PaginationList<T>(items, totalCount, totalPages);
        }
    }
}
