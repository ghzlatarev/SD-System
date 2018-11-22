namespace SD.Web.Models
{
    public class PaginationViewModel
    {
        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public string SortOrder { get; set; }

        public string SearchTerm { get; set; }

        public string AreaRoute { get; set; }

        public string ControllerRoute { get; set; }

        public string ActionRoute { get; set; }
    }
}
