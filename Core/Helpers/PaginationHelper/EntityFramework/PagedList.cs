namespace Core.Helpers.PaginationHelper.EntityFramework
{
    public class PagedList<T>
    {
        private readonly HashSet<T> _items = new();

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddItems(items);
            Items = _items;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public HashSet<T> Items { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        private void AddItems(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                _items.Add(item);
            }
        }

        public IEnumerable<T> GetItems()
        {
            return _items;
        }
    }
}
