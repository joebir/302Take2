namespace EFConnect.Models
{
    public class UserParams
    {
        public int PageNumber { get; set; } = 1;
        private const int MaxPageSize = 50;
        private int pageSize = 12;
        public int UserId { get; set; }
        public string Specialty { get; set; } = "All";
        public string OrderBy { get; set; }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }   // if requested value is greater than our max allowed - return the max
        }
    }
}