namespace UsersApi.Application.DTO
{
    public class RequestParams
    {
        private const int _maxPageSize = 50;
        private int _pageSize = 5;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > _maxPageSize ? _maxPageSize : value; }
        }
    }
}
