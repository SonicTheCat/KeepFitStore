namespace KeepFitStore.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Linq.Dynamic.Core;

    using Microsoft.EntityFrameworkCore;

    public class PaginatedList<T> : List<T>
    {
        public PaginatedList()
        { }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, string sortBy)
        {
            this.PageIndex = pageIndex;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.PageSize = pageSize;
            this.SortBy = sortBy; 

            this.AddRange(items);
        }

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public string SortBy { get; private set; }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, string sortBy)
        {
            var countTask = source.CountAsync();
            var itemsTask = source
                .OrderBy(sortBy)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var count = await countTask;
            var items = await itemsTask;

            return new PaginatedList<T>(items, count, pageIndex, pageSize, sortBy);
        }

        public static void SwapValues<U>(PaginatedList<T> destination, PaginatedList<U> source)
        {
            destination.PageIndex = source.PageIndex;
            destination.TotalPages = source.TotalPages; 
            destination.PageSize = source.PageSize;
            destination.SortBy = source.SortBy; 
        }
    }
}